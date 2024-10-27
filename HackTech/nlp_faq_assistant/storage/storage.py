import os
import time
import logging
import pandas as pd
from psycopg2 import pool
from langchain_community.document_loaders import DataFrameLoader
from langchain_community.embeddings import OllamaEmbeddings
from langchain_community.vectorstores import PGVector
from langchain.vectorstores.pgvector import DistanceStrategy
from langchain.docstore.document import Document

# Configure logging
logging.basicConfig(level=logging.INFO)

# Environment variables
PGVECTOR_DRIVER = os.getenv("PGVECTOR_DRIVER", "psycopg2")
PGVECTOR_HOST = os.getenv("PGVECTOR_HOST", "localhost")
PGVECTOR_PORT = os.getenv("PGVECTOR_PORT", "5432")
PGVECTOR_DATABASE = os.getenv("PGVECTOR_DATABASE", "faqdb")
PGVECTOR_USER = os.getenv("PGVECTOR_USER", "postgres")
PGVECTOR_PASSWORD = os.getenv("PGVECTOR_PASSWORD", "postgres")

OLLAMA_HOST = os.getenv("OLLAMA_HOST", "localhost")
OLLAMA_MODEL = os.getenv("OLLAMA_MODEL", "orca-mini")


class Database:
    def __init__(self, collection_name=None, distance_strategy=DistanceStrategy.COSINE, csv_path='./storage/FAQ.csv'):
        self.COLLECTION_NAME = collection_name or f"questions_{OLLAMA_MODEL.replace('-', '_')}"
        self.distance_strategy = distance_strategy
        self.csv_path = csv_path

        # Connection pool setup
        self.connection_pool = pool.SimpleConnectionPool(
            1, 10,
            dbname='postgres',
            user=PGVECTOR_USER,
            password=PGVECTOR_PASSWORD,
            host=PGVECTOR_HOST,
            port=PGVECTOR_PORT
        )

        self.CONNECTION_STRING = PGVector.connection_string_from_db_params(
            driver=PGVECTOR_DRIVER,
            host=PGVECTOR_HOST,
            port=int(PGVECTOR_PORT),
            database=PGVECTOR_DATABASE,
            user=PGVECTOR_USER,
            password=PGVECTOR_PASSWORD
        )

        self.embedding_model = OllamaEmbeddings(model=OLLAMA_MODEL, base_url=f"http://{OLLAMA_HOST}:11434")
        self.db = None  # Initialize self.db as None first
        self.wait_for_db_to_start()
        self.create_database_if_not_exists()
        self.create_extension_if_not_exists()
        self.populate_db_if_not_populated()

    def wait_for_db_to_start(self):
        """Wait for the database to be ready by attempting to acquire a connection."""
        conn = None
        while not conn:
            try:
                conn = self.get_connection()
                logging.info("Database connection successful")
            except Exception as e:
                logging.warning(f"Database connection failed: {e}. Retrying in 5 seconds.")
                time.sleep(5)
            finally:
                if conn:
                    self.release_connection(conn)

    def get_connection(self):
        return self.connection_pool.getconn()

    def release_connection(self, conn):
        if conn:
            self.connection_pool.putconn(conn)

    def initialize_db(self):
        """ Initialize the PGVector instance if it's not already initialized. """
        if not self.db:
            try:
                self.db = PGVector(
                    embedding_function=self.embedding_model,
                    collection_name=self.COLLECTION_NAME,
                    connection_string=self.CONNECTION_STRING
                )
                logging.info("Successfully initialized PGVector.")
            except Exception as e:
                logging.error(f"Failed to initialize PGVector: {e}")
                self.db = None  # Explicitly set self.db to None if initialization fails

    def create_database_if_not_exists(self):
        conn = self.get_connection()
        try:
            conn.autocommit = True
            cur = conn.cursor()
            cur.execute(f"SELECT 1 FROM pg_catalog.pg_database WHERE datname = '{PGVECTOR_DATABASE}'")
            if not cur.fetchone():
                cur.execute(f'CREATE DATABASE {PGVECTOR_DATABASE}')
                logging.info(f"Successfully created database {PGVECTOR_DATABASE}")
            else:
                logging.info(f"Database {PGVECTOR_DATABASE} already exists. Skipping creation.")
            cur.close()
        except Exception as e:
            logging.error(f"Error creating database: {e}")
        finally:
            self.release_connection(conn)

    def create_extension_if_not_exists(self):
        conn = self.get_connection()
        try:
            cur = conn.cursor()
            cur.execute("CREATE EXTENSION IF NOT EXISTS vector")
            conn.commit()
            logging.info("Successfully ensured vector extension exists.")
            cur.close()
        except Exception as e:
            logging.error(f"Error creating vector extension: {e}")
        finally:
            self.release_connection(conn)

    def populate_db_if_not_populated(self):
        self.initialize_db()  # Ensure db is initialized
        if self.db:
            try:
                # Use similarity_search to check if there are any documents
                try:
                    _ = self.db.similarity_search("Test", 1)
                    logging.info(f"Database {PGVECTOR_DATABASE}:{self.COLLECTION_NAME} already populated.")
                except IndexError:
                    # No documents found, so we proceed with population
                    self.create_db()
                    logging.info(f"Successfully populated database {PGVECTOR_DATABASE}:{self.COLLECTION_NAME}")
            except Exception as e:
                logging.error(f"Failed to check or populate database: {e}")


    def create_db(self):
        self.initialize_db()  # Ensure db is initialized
        if not self.db:
            logging.error("Database connection is not initialized. Cannot populate the database.")
            return
        
        logging.info("Populating database. This may take some time.")
        try:
            for chunk in pd.read_csv(self.csv_path, delimiter=';', chunksize=1000):
                loader = DataFrameLoader(chunk, page_content_column="question")
                docs = loader.load()
                self.db.add_documents(docs)
            logging.info("Finished populating database.")
        except Exception as e:
            logging.error(f"Error populating database: {e}")

    def query_by_similarity(self, query: str, top_k: int = 1):
        self.initialize_db()  # Ensure db is initialized
        if not self.db:
            logging.error("Database connection is not initialized. Aborting query.")
            return []
        
        try:
            results = self.db.similarity_search_with_score(query, top_k)
            if results:
                return results
            else:
                logging.warning("No results found for the query.")
                return []
        except Exception as e:
            logging.error(f"Failed to execute similarity query: {e}")
            return []

    def insert(self, question: str, answer: str):
        self.initialize_db()  # Ensure db is initialized
        if not self.db:
            logging.error("Database connection is not initialized. Aborting insert operation.")
            return
        
        logging.info("Inserting new question and answer into database")
        try:
            entry = Document(page_content=question, metadata={"answer": answer})
            self.db.add_documents([entry])
            logging.info("Successfully inserted new document.")
        except Exception as e:
            logging.error(f"Failed to insert document: {e}")

    def close(self):
        if self.connection_pool:
            self.connection_pool.closeall()
            logging.info("Closed all database connections.")

    def __del__(self):
        self.close()

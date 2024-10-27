# test_openai.py

import os
from dotenv import load_dotenv
import openai
import logging

# Configure logging
logging.basicConfig(level=logging.INFO)
logger = logging.getLogger(__name__)

# Load environment variables
load_dotenv()
openai_api_key = os.getenv("API_KEY")
if not openai_api_key:
    logger.error("API_KEY not found in environment variables.")
    exit(1)

openai.api_key = openai_api_key

def test_openai_api():
    try:
        response = openai.ChatCompletion.create(
            model="gpt-4o",
            messages=[
                {"role": "system", "content": "You are a helpful assistant."},
                {"role": "user", "content": "Hello, how are you?"}
            ],
            temperature=0.5,
            max_tokens=100
        )
        content = response.choices[0].message['content'].strip()
        logger.info("API Response: %s", content)
    except Exception as e:
        logger.error(f"Error: {e}")

if __name__ == "__main__":
    test_openai_api()

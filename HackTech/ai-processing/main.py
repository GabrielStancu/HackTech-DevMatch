# main.py

from flask import Flask, request, jsonify
from flask_cors import CORS
from llm.gpt import get_structured_text_for_cv, get_structured_text_for_job, get_general_score, get_tehnical_score, get_domain_score
import logging

# Configure logging
logging.basicConfig(level=logging.DEBUG)  # Set to DEBUG to capture all log levels
logger = logging.getLogger(__name__)
logging.disable(logging.CRITICAL)

app = Flask(__name__)
CORS(app)

@app.route("/structure-cv", methods=["POST"])
def structure_cv():
    # Log the incoming JSON payload
    incoming_json = request.get_json()
    logger.debug(f"Incoming JSON Payload: {incoming_json}")
    
    if not incoming_json:
        logger.error("No JSON payload received.")
        return jsonify({"structured_cv": None, "error": "No data provided."}), 400

    text_cv = incoming_json.get("text_cv")

    if not text_cv:
        logger.error("Missing 'text_cv' in request data.")
        return jsonify({"structured_cv": None, "error": "Missing 'text_cv' in request data."}), 400

    logger.info("Received /structure-cv request.")
    logger.debug(f"text_cv: {text_cv}")

    try:
        # Get structured CV
        logger.info("Calling get_structured_text_for_cv.")
        structured_text = get_structured_text_for_cv(text_cv)
        if not structured_text:
            logger.error("Failed to structure CV text.")
            return jsonify({"structured_cv": None, "error": "Failed to process CV text."}), 500
        logger.info("Structured CV obtained.")
        logger.debug(f"Structured CV: {structured_text}")

        return jsonify({"structured_cv": structured_text}), 200

    except Exception as e:
        logger.error(f"Error in /structure-cv: {e}")
        return jsonify({"structured_cv": None, "error": str(e)}), 500

@app.route("/structure-job", methods=["POST"])
def structure_job():
    # Log the incoming JSON payload
    incoming_json = request.get_json()
    logger.debug(f"Incoming JSON Payload: {incoming_json}")
    
    if not incoming_json:
        logger.error("No JSON payload received.")
        return jsonify({"structured_job": None, "error": "No data provided."}), 400

    text_job = incoming_json.get("text_job")

    if not text_job:
        logger.error("Missing 'text_job' in request data.")
        return jsonify({"structured_job": None, "error": "Missing 'text_job' in request data."}), 400

    logger.info("Received /structure-job request.")
    logger.debug(f"text_job: {text_job}")

    try:
        # Get structured Job Description
        logger.info("Calling get_structured_text_for_job.")
        structured_job = get_structured_text_for_job(text_job)
        if not structured_job:
            logger.error("Failed to structure job description.")
            return jsonify({"structured_job": None, "error": "Failed to process job description."}), 500
        logger.info("Structured job description obtained.")
        logger.debug(f"Structured Job: {structured_job}")

        return jsonify({"structured_job": structured_job}), 200

    except Exception as e:
        logger.error(f"Error in /structure-job: {e}")
        return jsonify({"structured_job": None, "error": str(e)}), 500

@app.route("/match", methods=["POST"])
def match():

    # Log the incoming JSON payload
    incoming_json = request.get_json()

    logger.debug(f"Incoming JSON Payload: {incoming_json}")
    
    if not incoming_json:
        logger.error("No JSON payload received.")
        return jsonify({"score": None, "error": "No data provided."}), 400

    structured_job = incoming_json.get("structured_job")
    structured_cv = incoming_json.get("structured_cv")

    if not structured_job or not structured_cv:
        logger.error("Missing 'structured_job' or 'structured_cv' in request data.")
        return jsonify({"score": None, "error": "Missing 'structured_job' or 'structured_cv' in request data."}), 400

    logger.info("Received /match-cv request.")
    logger.debug(f"structured_job: {structured_job}")
    logger.debug(f"structured_cv: {structured_cv}")

    try:
        # Generate score
        logger.info("Calling get_general_score.")

        domain_score_result = get_domain_score(structured_cv, structured_job)    
        tehnical_score_result = get_tehnical_score(structured_cv, structured_job)
        general_score_result = get_general_score(structured_cv, structured_job)

        # print(domain_score_result, "Domain")
        # print(tehnical_score_result, "tehnical_score_result")
        # print(general_score_result, "general_score_result")

        cv_id = domain_score_result.get("cv_id")
        job_id = domain_score_result.get("job_id")

        domain_reasoning = domain_score_result.get("reasoning")
        tehnical_reasoning = tehnical_score_result.get("reasoning")
        general_reasoning = general_score_result.get("reasoning")

        domain_score = domain_score_result.get("score")
        tehnical_score = tehnical_score_result.get("score")
        general_score = general_score_result.get("score")

        score = (10*domain_score + 30*tehnical_score + 60*general_score)/100

        # Validate score
        if not (0 <= score <= 100):
            logger.error("Invalid score format received from OpenAI API.")
            return jsonify({"score": None, "error": "Invalid score format received from OpenAI API."}), 500

        logger.info("Score obtained successfully.")

        # Construct the response JSON
        result = {
            "cv_id": cv_id,
            "job_id": job_id,
            "domain_reasoning": domain_reasoning,
            "tehnical_reasoning": tehnical_reasoning,
            "general_reasoning": general_reasoning,
            "score": score,
            "domain_score": domain_score,
            "tehnical_score": tehnical_score,
            "general_score": general_score,
        }

        return jsonify(result), 200

    except Exception as e:
        logger.error(f"Error in /match: {e}")
        return jsonify({"score": None, "error": str(e)}), 500 
    
if __name__ == "__main__":
    app.run(debug=True, host="0.0.0.0", port=5000)

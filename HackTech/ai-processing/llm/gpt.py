import os
import json
from dotenv import load_dotenv
import openai
from typing import Dict
from llm.prompts import cv_structuring_context, tehnical_skills_2, job_structuring_context, general_match_prompt_3, domain_1
import logging
import re 

# Configure logging for this module
logging.basicConfig(level=logging.DEBUG)  # Set to DEBUG to capture all log levels
logger = logging.getLogger(__name__)
logging.disable(logging.CRITICAL)

# Load environment variables once at startup
load_dotenv()
openai_api_key = os.getenv("API_KEY")
if not openai_api_key:
    logger.error("API_KEY not found in environment variables.")
    raise EnvironmentError("API_KEY not found in environment variables.")
openai.api_key = openai_api_key

# Define model and other constants
MODEL_NAME = "gpt-4o"  # Correct model name as per your setup
TEMPERATURE = 0.1      # Lower temperature for more deterministic results
MAX_TOKENS = 2000      # Adjust based on your needs
TIMEOUT = 60           # Timeout for API calls in seconds

########################## FETCH #########################################

def fetch_openai_response(messages: list, model: str = MODEL_NAME) -> Dict:
    try:
        # logger.debug(f"Sending request to OpenAI API with model: {model}")
        response = openai.ChatCompletion.create(
            model=model,
            messages=messages,
            temperature=TEMPERATURE,
            max_tokens=MAX_TOKENS,
            timeout=TIMEOUT
        )

        # logger.debug(f"Full API Response: {response}")  # Log the entire response for debugging

        # Validate response structure
        if not response.choices:
            # logger.error("No choices found in OpenAI API response.")
            return {}
        if 'message' not in response.choices[0]:
            # logger.error("No message found in the first choice of OpenAI API response.")
            return {}
        
        content = response.choices[0].message.get('content', '').strip()

        if not content:
            # logger.warning("Empty content received from OpenAI API.")
            return {}
        
        logger.debug(f"Raw GPT Response Content: {content}")  # Log the raw GPT response

        # Remove code fences if present
        # Pattern to match ```json ... ```
        json_pattern = re.compile(r'^```json\s*(.*?)\s*```$', re.DOTALL)
        match = json_pattern.match(content)
        if match:
            content = match.group(1)
            # logger.debug("Code fences detected and removed from GPT response.")

        # Attempt to parse JSON
        try:
            json_content = json.loads(content)
            # logger.debug(f"Parsed JSON Content: {json_content}")  # Log the parsed JSON
            return json_content
        except json.JSONDecodeError as e:
            logger.error(f"JSON decoding failed: {e}")
            # logger.debug(f"Response content after stripping code fences: {content}")
            # return {}
        
    except openai.error.OpenAIError as e:
        # Handle specific OpenAI errors
        logger.error(f"OpenAI API error: {e}")
        return {}
    except Exception as e:
        # Handle other exceptions
        logger.error(f"Unexpected error in fetch_openai_response: {e}")
        return {}

######################### SCORES #########################################

def get_tehnical_score(structured_cv: Dict, structured_job: Dict) -> Dict:
    messages = [
        {"role": "system", "content": tehnical_skills_2},
        {"role": "user", "content": f"The CV is: {json.dumps(structured_cv)}. The job is: {json.dumps(structured_job)}"}
    ]
    logger.info("Generating score for the job description.")
    score = fetch_openai_response(messages)
    if not score:
        logger.error("Failed to generate score.")
    return score

def get_general_score(structured_cv: Dict, structured_job: Dict) -> Dict:
    messages = [
        {"role": "system", "content": general_match_prompt_3},
        {"role": "user", "content": f"The CV is: {json.dumps(structured_cv)}. The job is: {json.dumps(structured_job)}"}
    ]
    logger.info("Generating score for the job description.")
    score = fetch_openai_response(messages)
    if not score:
        logger.error("Failed to generate score.")
    return score

def get_domain_score(structured_cv: Dict, structured_job: Dict) -> Dict:
    messages = [
        {"role": "system", "content": domain_1},
        {"role": "user", "content": f"The CV is: {json.dumps(structured_cv)}. The job is: {json.dumps(structured_job)}"}
    ]
    logger.info("Generating score for the job description.")
    score = fetch_openai_response(messages)

    # if not score:
    #     logger.error("Failed to generate score.")
    return score

########### STRUCTURE DATA #########################

def get_structured_text_for_cv(text: str) -> Dict:
    messages = [
        {"role": "system", "content": cv_structuring_context},
        {"role": "user", "content": text}
    ]
    logger.info("Structuring CV text.")
    structured_cv = fetch_openai_response(messages)
    
    return structured_cv

def get_structured_text_for_job(text: str) -> Dict:
    messages = [
        {"role": "system", "content": job_structuring_context},
        {"role": "user", "content": text}
    ]
    logger.info("Structuring job description.")
    structured_job = fetch_openai_response(messages)
    
    return structured_job

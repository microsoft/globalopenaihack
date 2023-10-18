import openai

# Set your OpenAI API key
openai.api_key = "YOUR_API_KEY"

# Generate Python code from natural language
def generate_code(prompt):
    response = openai.create(
        engine="davinci-codex",
        prompt=prompt,
        temperature=0.7,
        top_p=0.9,
        frequency_penalty=0.0,
        presence_penalty=0.0,
    )
    return response.choices[0].text

# Example usage
print(generate_code("Create a function that takes two numbers as input and returns their sum"))

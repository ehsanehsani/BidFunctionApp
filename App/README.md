# BidFunctionApp

# K6 Load Testing

This README describes how to use the K6 tool to test your API.

## Prerequisites
- Make sure you have K6 installed on your system.
- Ensure that your API is running locally or accessible for testing.

## Running the Script
To execute the K6 script, use the following command:

```bash
k6 run script.js
```

### Explanation
1. **`k6 run script.js`**: This command runs the `script.js` file using K6.
2. The script performs load testing on your API and checks for specific conditions (e.g., response status and timing). Modify `script.js` as needed for your testing scenarios.

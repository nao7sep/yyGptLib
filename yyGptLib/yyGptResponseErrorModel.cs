﻿using System.Text.Json.Serialization;

namespace yyGptLib
{
    public class yyGptResponseErrorModel
    {
        // Chat and Images both seem to use this model.
        // At this moment (meaning February 2022), I havent found any official document regarding this model (and ChatGPT couldnt find one either).
        // So, the following code is based on this specific response that was returned when I intentionally violated the content policy for testing purposes.

        // {
        //   "error": {
        //     "code": "content_policy_violation",
        //     "message": "Your request was rejected as a result of our safety system. Your prompt may contain text that is not allowed by our safety system.",
        //     "param": null,
        //     "type": "invalid_request_error"
        //   }
        // }

        [JsonPropertyName ("code")]
        public string? Code { get; set; }

        [JsonPropertyName ("message")]
        public string? Message { get; set; }

        [JsonPropertyName ("param")]
        public string? Param { get; set; }

        [JsonPropertyName ("type")]
        public string? Type { get; set; }
    }
}
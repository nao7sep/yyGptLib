﻿using System.Text.Json.Serialization;

namespace yyGptLib
{
    public class yyGptImagesResponseImage
    {
        [JsonPropertyName ("b64_json")]
        public string? B64Json { get; set; }

        [JsonPropertyName ("url")]
        public string? Url { get; set; }

        [JsonPropertyName ("revised_prompt")]
        public string? RevisedPrompt { get; set; }
    }
}

using System.Text.Json.Serialization;

namespace yyGptLib
{
    public class yyGptChatResponse
    {
        public static yyGptChatResponse Empty { get; } = new yyGptChatResponse ();

        [JsonPropertyName ("id")]
        public string? Id { get; set; }

        [JsonPropertyName ("object")]
        public string? Object { get; set; }

        [JsonPropertyName ("created")]
        public int? Created { get; set; }

        [JsonPropertyName ("model")]
        public string? Model { get; set; }

        [JsonPropertyName ("system_fingerprint")]
        public string? SystemFingerprint { get; set; }

        [JsonPropertyName ("choices")]
        public IList <yyGptChatResponseChoice>? Choices { get; set; }

        [JsonPropertyName ("usage")]
        public yyGptChatResponseUsage? Usage { get; set; }

        [JsonPropertyName ("error")]
        public yyGptResponseError? Error { get; set; }
    }
}

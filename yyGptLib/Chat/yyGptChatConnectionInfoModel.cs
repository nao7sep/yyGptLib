using System.Text.Json.Serialization;

namespace yyGptLib
{
    public class yyGptChatConnectionInfoModel
    {
        [JsonPropertyName ("api_key")]
        public string? ApiKey { get; set; }

        [JsonPropertyName ("organization")]
        public string? Organization { get; set; }

        [JsonPropertyName ("project")]
        public string? Project { get; set; }

        [JsonPropertyName ("endpoint")]
        public string? Endpoint { get; set; }

        public yyGptChatConnectionInfoModel ()
        {
            ApiKey = yyGpt.DefaultApiKey;
            Endpoint = yyGptChat.DefaultEndpoint;
        }
    }
}

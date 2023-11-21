using System.Text.Json.Serialization;

namespace yyGptLib
{
    public class yyGptChatConnectionInfoModel
    {
        [JsonPropertyName ("api_key")]
        public string? ApiKey { get; set; }

        [JsonPropertyName ("endpoint")]
        public string? Endpoint { get; set; }

        public yyGptChatConnectionInfoModel () => Endpoint = yyGptChat.DefaultEndpoint;
    }
}

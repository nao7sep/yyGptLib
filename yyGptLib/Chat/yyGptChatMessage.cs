using System.Text.Json.Serialization;

namespace yyGptLib
{
    public class yyGptChatMessage
    {
        [JsonPropertyName ("role")]
        [JsonConverter (typeof (yyGptChatMessageRoleJsonConverter))]
        public yyGptChatMessageRole? Role { get; set; }

        [JsonPropertyName ("content")]
        public string? Content { get; set; }

        [JsonPropertyName ("name")]
        public string? Name { get; set; }
    }
}

using System.Text.Json.Serialization;

namespace yyGptLib
{
    public class yyGptChatRequestResponseFormatModel
    {
        [JsonPropertyName ("type")]
        public string? Type { get; set; }
    }
}

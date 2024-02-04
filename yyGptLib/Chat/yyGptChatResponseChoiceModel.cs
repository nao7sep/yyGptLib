using System.Text.Json.Serialization;

namespace yyGptLib
{
    public class yyGptChatResponseChoiceModel
    {
        [JsonPropertyName ("index")]
        public int? Index { get; set; }

        [JsonPropertyName ("message")]
        public yyGptChatMessageModel? Message { get; set; }

        [JsonPropertyName ("delta")]
        public yyGptChatMessageModel? Delta { get; set; }

        [JsonPropertyName ("finish_reason")]
        public string? FinishReason { get; set; }
    }
}

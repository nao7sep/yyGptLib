using System.Text.Json.Serialization;

namespace yyGptLib
{
    public class yyGptChatRequestModel
    {
        [JsonPropertyName ("model")]
        public string Model { get; set; }

        [JsonPropertyName ("messages")]
        public IList <yyGptChatMessageModel> Messages { get; private set; } = new List <yyGptChatMessageModel> ();

        [JsonPropertyName ("frequency_penalty")]
        public double? FrequencyPenalty { get; set; }

        [JsonPropertyName ("max_tokens")]
        public int? MaxTokens { get; set; }

        [JsonPropertyName ("n")]
        public int? N { get; set; }

        [JsonPropertyName ("presence_penalty")]
        public double? PresencePenalty { get; set; }

        [JsonPropertyName ("stop")]
        public IList <string> Stop { get; private set; } = new List <string> ();

        [JsonPropertyName ("temperature")]
        public double? Temperature { get; set; }

        [JsonPropertyName ("top_p")]
        public double? TopP { get; set; }

        [JsonPropertyName ("stream")]
        public bool? Stream { get; set; }

        [JsonPropertyName ("user")]
        public string? User { get; set; }

        public yyGptChatRequestModel (string? model = null) => Model = model ?? yyGptChatDefaultValues.Model;
    }
}

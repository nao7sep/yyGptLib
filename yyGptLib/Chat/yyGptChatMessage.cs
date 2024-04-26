using System.Text.Json.Serialization;

namespace yyGptLib
{
    public class yyGptChatMessage
    {
        [JsonPropertyName ("role")]
        public string? RoleString
        {
            get => Role?.ToString ();
            set => Role = yyGptChatMessageRole.Parse (value);
        }

        [JsonIgnore]
        public yyGptChatMessageRole? Role { get; set; }

        [JsonPropertyName ("content")]
        public string? Content { get; set; }

        [JsonPropertyName ("name")]
        public string? Name { get; set; }
    }
}

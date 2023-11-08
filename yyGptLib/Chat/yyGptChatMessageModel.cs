using System.Text.Json.Serialization;

namespace yyGptLib
{
    public partial class yyGptChatMessageModel
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

        /// <summary>
        /// Optional. Name of the speaker.
        /// </summary>
        [JsonPropertyName ("name")]
        public string? Name { get; set; }
    }
}

using System.Text.Json.Serialization;

namespace yyGptLib
{
    public class yyGptChatMessageModel
    {
        /// <summary>
        /// For serialization only. Use Role instead.
        /// </summary>
        [JsonPropertyName ("role")]
        public string RoleString => Role.Value;

        [JsonIgnore]
        public yyGptChatRole Role { get; set; }

        [JsonPropertyName ("content")]
        public string Content { get; set; }

        /// <summary>
        /// Optional. Name of the speaker.
        /// </summary>
        [JsonPropertyName ("name")]
        public string? Name { get; set; }

        public yyGptChatMessageModel (yyGptChatRole role, string content)
        {
            Role = role;
            Content = content;
        }
    }
}

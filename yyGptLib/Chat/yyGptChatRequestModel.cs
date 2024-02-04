﻿using System.Text.Json.Serialization;

namespace yyGptLib
{
    public class yyGptChatRequestModel
    {
        [JsonPropertyName ("model")]
        public string? Model { get; set; }

        [JsonPropertyName ("messages")]
        public IList <yyGptChatMessageModel>? Messages { get; set; }

        // Optional parameters are in alphabetical order.

        [JsonPropertyName ("frequency_penalty")]
        public double? FrequencyPenalty { get; set; }

        [JsonPropertyName ("logit_bias")]
        public IDictionary <int, double>? LogitBias { get; set; }

        [JsonPropertyName ("max_tokens")]
        public int? MaxTokens { get; set; }

        [JsonPropertyName ("n")]
        public int? N { get; set; }

        [JsonPropertyName ("presence_penalty")]
        public double? PresencePenalty { get; set; }

        [JsonPropertyName ("response_format")]
        public yyGptChatRequestResponseFormatModel? ResponseFormat { get; set; }

        [JsonPropertyName ("seed")]
        public int? Seed { get; set; }

        [JsonPropertyName ("stop")]
        public IList <string>? Stop { get; set; }

        [JsonPropertyName ("stream")]
        public bool? Stream { get; set; }

        [JsonPropertyName ("temperature")]
        public double? Temperature { get; set; }

        [JsonPropertyName ("top_p")]
        public double? TopP { get; set; }

        [JsonPropertyName ("user")]
        public string? User { get; set; }

        public yyGptChatRequestModel () => Model = yyGptChat.DefaultModel;

        public void AddMessage (yyGptChatMessageRole role, string content, string? name = null)
        {
            Messages ??= new List <yyGptChatMessageModel> ();

            Messages.Add (new yyGptChatMessageModel
            {
                Role = role,
                Content = content,
                Name = name
            });
        }

        public string? GetLastMessage ()
        {
            if (Messages != null && Messages.Count > 0)
                return Messages [Messages.Count - 1].Content;

            return null;
        }

        public void RemoveLastMessage ()
        {
            if (Messages != null && Messages.Count > 0)
                Messages.RemoveAt (Messages.Count - 1);
        }
    }
}

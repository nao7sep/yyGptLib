using System.Text.Json.Serialization;

namespace yyGptLib
{
    public class yyGptImagesConnectionInfoModel
    {
        [JsonPropertyName ("api_key")]
        public string? ApiKey { get; set; }

        [JsonPropertyName ("endpoint")]
        public string? Endpoint { get; set; }

        public yyGptImagesConnectionInfoModel ()
        {
            ApiKey = yyGpt.DefaultApiKey;
            Endpoint = yyGptImages.DefaultEndpoint;
        }
    }
}

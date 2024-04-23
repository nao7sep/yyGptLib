using System.Text.Json.Serialization;

namespace yyGptLib
{
    public class yyGptImagesConnectionInfoModel
    {
        [JsonPropertyName ("api_key")]
        public string? ApiKey { get; set; }

        [JsonPropertyName ("organization")]
        public string? Organization { get; set; }

        [JsonPropertyName ("project")]
        public string? Project { get; set; }

        [JsonPropertyName ("endpoint")]
        public string? Endpoint { get; set; }

        public yyGptImagesConnectionInfoModel ()
        {
            ApiKey = yyGpt.DefaultApiKey;
            Organization = yyGpt.DefaultOrganization;
            Project = yyGpt.DefaultProject;
            Endpoint = yyGptImages.DefaultEndpoint;
        }
    }
}

using System.Text.Json.Serialization;

namespace yyGptLib
{
    public class yyGptImagesResponse
    {
        [JsonPropertyName ("created")]
        public int? Created { get; set; }

        [JsonPropertyName ("data")]
        public IList <yyGptImagesResponseImage>? Data { get; set; }

        [JsonPropertyName ("error")]
        public yyGptResponseError? Error { get; set; }
    }
}

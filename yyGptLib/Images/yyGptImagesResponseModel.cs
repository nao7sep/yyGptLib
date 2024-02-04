using System.Text.Json.Serialization;

namespace yyGptLib
{
    public class yyGptImagesResponseModel
    {
        [JsonPropertyName ("created")]
        public int? Created { get; set; }

        [JsonPropertyName ("data")]
        public IList <yyGptImagesResponseImageModel>? Data { get; set; }

        [JsonPropertyName ("error")]
        public yyGptResponseErrorModel? Error { get; set; }
    }
}

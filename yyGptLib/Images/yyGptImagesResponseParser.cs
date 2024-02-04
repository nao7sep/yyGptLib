using System.Text.Json;
using yyLib;

namespace yyGptLib
{
    public static class yyGptImagesResponseParser
    {
        public static yyGptImagesResponseModel Parse (string? str)
        {
            if (string.IsNullOrWhiteSpace (str))
                throw new yyArgumentException ($"'{nameof (str)}' is invalid: {str.GetVisibleString ()}");

            var xResponse = (yyGptImagesResponseModel?) JsonSerializer.Deserialize (str, typeof (yyGptImagesResponseModel), yyJson.DefaultDeserializationOptions);

            if (xResponse == null)
                throw new yyFormatException ($"Failed to deserialize JSON: {str.GetVisibleString ()}");

            return xResponse;
        }
    }
}

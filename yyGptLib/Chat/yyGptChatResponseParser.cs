using System.Text.Json;
using yyLib;

namespace yyGptLib
{
    public static class yyGptChatResponseParser
    {
        public static yyGptChatResponseModel Parse (string? str)
        {
            if (string.IsNullOrWhiteSpace (str))
                throw new yyArgumentException ($"'{nameof (str)}' is invalid: {str.GetVisibleString ()}");

            var xResponse = (yyGptChatResponseModel?) JsonSerializer.Deserialize (str, typeof (yyGptChatResponseModel), yyJson.DefaultDeserializationOptions);

            if (xResponse == null)
                throw new yyFormatException ($"Failed to deserialize JSON: {str.GetVisibleString ()}");

            return xResponse;
        }

        public static yyGptChatResponseModel ParseChunk (string? str)
        {
            if (string.IsNullOrWhiteSpace (str))
                throw new yyArgumentException ($"'{nameof (str)}' is invalid: {str.GetVisibleString ()}");

            if (str.StartsWith ("data: {", StringComparison.OrdinalIgnoreCase))
            {
                string xJson = str.Substring ("data: ".Length);

                var xResponse = (yyGptChatResponseModel?) JsonSerializer.Deserialize (xJson,
                    typeof (yyGptChatResponseModel), yyJson.DefaultDeserializationOptions);

                if (xResponse == null)
                    throw new yyFormatException ($"Failed to deserialize JSON: {xJson.GetVisibleString ()}");

                return xResponse;
            }

            if (str.Equals ("data: [DONE]", StringComparison.OrdinalIgnoreCase))
                return yyGptChatResponseModel.Empty;

            throw new yyFormatException ($"Failed to parse chunk: {str.GetVisibleString ()}");
        }
    }
}

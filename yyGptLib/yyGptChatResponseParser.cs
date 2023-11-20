using System.Text.Json;
using System.Text.Json.Serialization;
using yyLib;

namespace yyGptLib
{
    public class yyGptChatResponseParser
    {
        public JsonSerializerOptions JsonSerializerOptions { get; private set; }

        public yyGptChatResponseParser () => JsonSerializerOptions = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNameCaseInsensitive = true
        };

        public yyGptChatResponseModel Parse (string? str)
        {
            if (string.IsNullOrWhiteSpace (str))
                throw new yyArgumentException ($"'{nameof (str)}' is invalid.");

            var xResponse = (yyGptChatResponseModel?) JsonSerializer.Deserialize (str, typeof (yyGptChatResponseModel), JsonSerializerOptions);

            if (xResponse == null)
                throw new yyFormatException ("Failed to deserialize JSON.");

            return xResponse;
        }

        public yyGptChatResponseModel ParseChunk (string? str)
        {
            if (string.IsNullOrWhiteSpace (str))
                throw new yyArgumentException ($"'{nameof (str)}' is invalid.");

            if (str.StartsWith ("data: {", StringComparison.OrdinalIgnoreCase))
            {
                var xResponse = (yyGptChatResponseModel?) JsonSerializer.Deserialize (str.AsSpan ("data: ".Length), typeof (yyGptChatResponseModel), JsonSerializerOptions);

                if (xResponse == null)
                    throw new yyFormatException ("Failed to deserialize JSON.");

                return xResponse;
            }

            if (str.Equals ("data: [DONE]", StringComparison.OrdinalIgnoreCase))
                return yyGptChatResponseModel.Empty;

            throw new yyFormatException ("Failed to parse chunk.");
        }
    }
}

namespace yyGptLib
{
    public class yyGptChatConnectionInfo
    {
        public string ApiKey { get; private set; }

        public string Endpoint { get; private set; }

        public yyGptChatConnectionInfo (string apiKey, string? endpoint = null)
        {
            ApiKey = apiKey;
            Endpoint = endpoint ?? yyGptChatDefaultValues.Endpoint;
        }
    }
}

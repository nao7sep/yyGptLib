using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using yyLib;

namespace yyGptLib
{
    public class yyGptChatClient: IDisposable
    {
        public yyGptChatConnectionInfo ConnectionInfo { get; private set; }

        public HttpClient? HttpClient { get; private set; } = new HttpClient ();

        public JsonSerializerOptions JsonSerializerOptions { get; private set; } = new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };

        public Stream? ResponseStream { get; private set; }

        public StreamReader? ResponseStreamReader { get; private set; }

        public yyGptChatClient (yyGptChatConnectionInfo connectionInfo)
        {
            ConnectionInfo = connectionInfo;
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue ("Bearer", ConnectionInfo.ApiKey);
        }

        public async Task SendAsync (yyGptChatRequestModel request)
        {
            if (HttpClient == null)
                throw new yyObjectDisposedException ("HttpClient is disposed.");

            var xJson = JsonSerializer.Serialize (request, JsonSerializerOptions);

            using (var xContent = new StringContent (xJson, Encoding.UTF8, "application/json"))
            using (var xMessage = new HttpRequestMessage (HttpMethod.Post, ConnectionInfo.Endpoint) { Content = xContent })
            {
                var xResponse = await HttpClient.SendAsync (xMessage, HttpCompletionOption.ResponseHeadersRead);
                xResponse.EnsureSuccessStatusCode ();
                ResponseStream = await xResponse.Content.ReadAsStreamAsync ();
                ResponseStreamReader = new StreamReader (ResponseStream);
            }
        }

        public async Task <string?> ReadLineAsync ()
        {
            if (ResponseStreamReader == null)
                throw new yyObjectDisposedException ("ResponseStreamReader is disposed.");

            if (ResponseStreamReader.EndOfStream)
                return await Task.FromResult <string?> (null);

            return await ResponseStreamReader.ReadLineAsync ();
        }

        public void Dispose ()
        {
            if (HttpClient != null)
            {
                HttpClient.Dispose ();
                HttpClient = null;
            }

            if (ResponseStream != null)
            {
                ResponseStream.Dispose ();
                ResponseStream = null;
            }

            if (ResponseStreamReader != null)
            {
                ResponseStreamReader.Dispose ();
                ResponseStreamReader = null;
            }

            GC.SuppressFinalize (this);
        }
    }
}

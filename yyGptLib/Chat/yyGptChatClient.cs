using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using yyLib;

namespace yyGptLib
{
    public class yyGptChatClient: IDisposable
    {
        public yyGptChatConnectionInfoModel ConnectionInfo { get; private set; }

        public HttpClient? HttpClient { get; private set; }

        public JsonSerializerOptions JsonSerializerOptions { get; private set; }

        public HttpResponseMessage? ResponseMessage { get; private set; }

        public Stream? ResponseStream { get; private set; }

        public StreamReader? ResponseStreamReader { get; private set; }

        public yyGptChatClient (yyGptChatConnectionInfoModel connectionInfo)
        {
            ConnectionInfo = connectionInfo;
            HttpClient = new HttpClient ();
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue ("Bearer", ConnectionInfo.ApiKey);

            JsonSerializerOptions = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task <(HttpResponseMessage HttpResponseMessage, Stream Stream)> SendAsync (yyGptChatRequestModel request,
            CancellationToken? cancellationTokenForSendAsync = null, CancellationToken? cancellationTokenForReadAsStreamAsync = null)
        {
            if (HttpClient == null)
                throw new yyObjectDisposedException ($"'{nameof (HttpClient)}' is disposed.");

            var xJson = JsonSerializer.Serialize (request, JsonSerializerOptions);

            using (var xContent = new StringContent (xJson, Encoding.UTF8, "application/json"))
            using (var xMessage = new HttpRequestMessage (HttpMethod.Post, ConnectionInfo.Endpoint) { Content = xContent })
            {
                var xResponse = await HttpClient.SendAsync (xMessage, HttpCompletionOption.ResponseHeadersRead, cancellationTokenForSendAsync ?? CancellationToken.None);

                // Commented out to receive error messages.
                // xResponse.EnsureSuccessStatusCode ();

                ResponseMessage?.Dispose ();
                ResponseMessage = xResponse;

                ResponseStream?.Dispose ();
                ResponseStream = await xResponse.Content.ReadAsStreamAsync (cancellationTokenForReadAsStreamAsync ?? CancellationToken.None);

                ResponseStreamReader?.Dispose ();
                ResponseStreamReader = new StreamReader (ResponseStream);

                return (xResponse, ResponseStream);
            }
        }

        public async Task <string?> ReadToEndAsync (CancellationToken? cancellationToken = null)
        {
            if (ResponseStreamReader == null)
                throw new yyObjectDisposedException ($"'{nameof (ResponseStreamReader)}' is disposed.");

            if (ResponseStreamReader.EndOfStream)
                return await Task.FromResult <string?> (null);

            return await ResponseStreamReader.ReadToEndAsync (cancellationToken ?? CancellationToken.None);
        }

        public async ValueTask <string?> ReadLineAsync (CancellationToken? cancellationToken = null)
        {
            if (ResponseStreamReader == null)
                throw new yyObjectDisposedException ($"'{nameof (ResponseStreamReader)}' is disposed.");

            if (ResponseStreamReader.EndOfStream)
                return await ValueTask.FromResult <string?> (null);

            return await ResponseStreamReader.ReadLineAsync (cancellationToken ?? CancellationToken.None);
        }

        public void Dispose ()
        {
            HttpClient?.Dispose ();
            HttpClient = null;

            ResponseMessage?.Dispose ();
            ResponseMessage = null;

            ResponseStream?.Dispose ();
            ResponseStream = null;

            ResponseStreamReader?.Dispose ();
            ResponseStreamReader = null;

            GC.SuppressFinalize (this);
        }
    }
}

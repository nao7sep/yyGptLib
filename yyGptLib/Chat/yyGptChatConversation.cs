namespace yyGptLib
{
    public class yyGptChatConversation: IDisposable
    {
        public yyGptChatClient Client { get; private set; }

        public yyGptChatRequestModel Request { get; private set; }

        public yyGptChatResponseParser ResponseParser { get; private set; }

        public yyGptChatConversation (yyGptChatConnectionInfoModel connectionInfo)
        {
            Client = new yyGptChatClient (connectionInfo);
            Request = new yyGptChatRequestModel ();
            ResponseParser = new yyGptChatResponseParser ();
        }

        public async Task SendAsync (CancellationToken? cancellationTokenForSendAsync = null, CancellationToken? cancellationTokenForReadAsStreamAsync = null) =>
            await Client.SendAsync (Request, cancellationTokenForSendAsync, cancellationTokenForReadAsStreamAsync);

        // As a result of sugar coating, with or without a model class for the returned value, we anyway have to implement some conditioning.

        // If IsSuccess is true, Messages should contain one or more messages.
        // If IsSuccess is false, if an error message was retrieved, Messages should have only one element, which is the error message.
        // If IsSuccess is false and Messages is empty, an Exception instance should be available.

        /// <summary>
        /// Add one of the returned messages to the request to continue the conversation.
        /// </summary>
        public async Task <(bool IsSuccess, IList <string> Messages, Exception? Exception)> TryReadAndParseAsync (CancellationToken? cancellationToken = null)
        {
            try
            {
                string? xJson = await Client.ReadToEndAsync (cancellationToken);
                var xResponse = ResponseParser.Parse (xJson);

                if (Client.ResponseMessage!.IsSuccessStatusCode)
                    return (true, xResponse.Choices!.Select (x => x.Message!.Content!).ToList (), null);

                else return (false, new [] { xResponse.Error!.Message! }, null);
            }

            catch (Exception xException)
            {
                return (false, new List <string> (), xException);
            }
        }

        // If IsSuccess is true,
            // if PartialMessage isnt null, use it and continue reading.
            // if PartialMessage is null, discard it and stop reading.

        // If IsSuccess is false,
            // if PartialMessage isnt null, it should be an error message returned from the server.
            // if PartialMessage is null, an Exception instance should be available.

        /// <summary>
        /// Add one of the returned messages to the request to continue the conversation.
        /// </summary>
        public async Task <(bool IsSuccess, int Index, string? PartialMessage, Exception? Exception)> TryReadAndParseChunkAsync (CancellationToken? cancellationToken = null)
        {
            try
            {
                // Feels a little redundant, but the cost is negligible.
                // I dont want to move this check to SendAsync sacrificing the consistency.
                // An error, if it arises, should be returned from the reading code.

                if (Client.ResponseMessage!.IsSuccessStatusCode)
                {
                    string? xLine = await Client.ReadLineAsync (cancellationToken);

                    if (xLine == null)
                        return (true, default, null, null); // End of stream.
                        // We usually dont get here as "data: [DONE]" is detected before.

                    // If a returned line is empty and doesnt contain the "data: " part,
                    // we consider an empty partial message has been retrieved and continue for "data: [DONE]" or the end of stream.

                    if (string.IsNullOrWhiteSpace (xLine))
                        return (true, default, string.Empty, null);

                    var xResponse = ResponseParser.ParseChunk (xLine);

                    if (xResponse == yyGptChatResponseModel.Empty)
                        return (true, default, null, null); // "data: [DONE]" is detected.

                    int xIndex = xResponse.Choices! [0].Index!.Value;
                    string? xContent = xResponse.Choices! [0].Delta!.Content;

                    return (true, xIndex, xContent, null);
                }

                else
                {
                    string? xJson = await Client.ReadToEndAsync (cancellationToken);
                    var xResponse = ResponseParser.Parse (xJson);

                    return (false, default, xResponse.Error!.Message, null);
                }
            }

            catch (Exception xException)
            {
                return (false, default, null, xException);
            }
        }

        public void Dispose ()
        {
            Client.Dispose (); // Could be called a number of times.
            GC.SuppressFinalize (this);
        }
    }
}

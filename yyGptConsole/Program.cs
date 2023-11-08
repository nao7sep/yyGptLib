using System.Text;
using yyGptLib;
using yyLib;

namespace yyGptConsole
{
    internal class Program
    {
#pragma warning disable IDE0060 // Remove unused parameter
        static async Task Main (string [] args)
#pragma warning restore IDE0060
        {
            yyUserSecretsModel xUserSecrets = new yyUserSecretsLoader ().Load ();
            yyGptChatConnectionInfo xConnectionInfo = new (xUserSecrets.OpenAi!.ApiKey!);

            yyGptChatRequestModel xRequest = new ();
            xRequest.AddMessage (yyGptChatMessageRole.System, "You are a helpful assistant.");
            xRequest.N = 3;

            yyGptChatResponseParser xParser = new ();

            using (yyGptChatClient xClient = new (xConnectionInfo))
            {
                try
                {
                    xRequest.AddMessage (yyGptChatMessageRole.User, "A riddle in one line that has multiple answers, please.");
                    var xSendingTask1 = xClient.SendAsync (xRequest);

                    // Various tasks...

                    xSendingTask1.Wait ();

                    var xResponse1 = xParser.Parse (await xClient.ReadToEndAsync ());

                    foreach (var xChoice in xResponse1.Choices!)
                        Console.WriteLine (xChoice.Message!.Content);

                    xRequest.AddMessage (yyGptChatMessageRole.Assistant, xResponse1.Choices [Random.Shared.Next (0, xResponse1.Choices.Count)].Message!.Content!);
                }

                catch (Exception xException)
                {
                    Console.WriteLine (xException);
                }

                try
                {
                    xRequest.Stream = true;
                    xRequest.AddMessage (yyGptChatMessageRole.User, "What's the answer?");
                    var xSendingTask2 = xClient.SendAsync (xRequest);

                    // Various tasks...

                    xSendingTask2.Wait ();

                    yyAutoExpandingList <StringBuilder> xBuilders = new ();
                    string? xLine;

                    while ((xLine = await xClient.ReadLineAsync ()) != null)
                    {
                        if (string.IsNullOrWhiteSpace (xLine) == false)
                        {
                            var xResponse2 = xParser.ParseChunk (xLine);

                            if (xResponse2 != yyGptChatResponseModel.Empty)
                            {
                                string? xContent = xResponse2.Choices! [0].Delta!.Content;

                                if (string.IsNullOrWhiteSpace (xContent) == false)
                                {
                                    int xIndex = xResponse2.Choices [0].Index!.Value;

                                    xBuilders [xIndex].Append (xContent);
                                    Console.WriteLine (FormattableString.Invariant ($"[{xIndex}] {xContent}"));
                                }
                            }
                        }
                    }

                    foreach (StringBuilder xBuilder in xBuilders)
                        Console.WriteLine (xBuilder.ToString ());
                }

                catch (Exception xException)
                {
                    Console.WriteLine (xException);
                }
            }
        }
    }
}

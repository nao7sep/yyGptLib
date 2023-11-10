using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using yyGptLib;
using yyLib;

namespace yyGptLibConsole
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

                    string? xJson = await xClient.ReadToEndAsync ();
                    var xResponse1 = xParser.Parse (xJson);

                    if (xSendingTask1.Result.HttpResponseMessage.IsSuccessStatusCode)
                    {
                        foreach (var xChoice in xResponse1.Choices!)
                            Console.WriteLine (xChoice.Message!.Content);

                        xRequest.AddMessage (yyGptChatMessageRole.Assistant, xResponse1.Choices [Random.Shared.Next (0, xResponse1.Choices.Count)].Message!.Content!);
                    }

                    else
                    {
                        // I havent found any official documentation on the error model.
                        // A roundtrip is tried to make sure all the properties are covered.

                        // I might also cover the moderation model, but I couldnt be evil enough to get one.
                        // https://platform.openai.com/docs/api-reference/moderations/object
                        // Maybe, the API's priority is to refuse to respond and apologize.

                        Console.WriteLine (xJson);

                        Console.WriteLine (JsonSerializer.Serialize (xResponse1, new JsonSerializerOptions
                        {
                            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                            WriteIndented = true
                        }));
                    }
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

                    if (xSendingTask2.Result.HttpResponseMessage.IsSuccessStatusCode)
                    {
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

                    else
                    {
                        string? xJson = await xClient.ReadToEndAsync ();
                        var xResponse2 = xParser.Parse (xJson);

                        // Again, to make sure all the properties are covered.

                        Console.WriteLine (xJson);

                        Console.WriteLine (JsonSerializer.Serialize (xResponse2, new JsonSerializerOptions
                        {
                            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                            WriteIndented = true
                        }));
                    }
                }

                catch (Exception xException)
                {
                    Console.WriteLine (xException);
                }
            }
        }
    }
}

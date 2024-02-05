using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Text.Json;
using yyGptLib;
using yyLib;

namespace yyGptLibConsole
{
    public static class Tester3
    {
        public static void Test (int imageCount) // For the user to think how many she/he needs and how much she/he is willing to pay.
        {
            var xConnectionInfo = new yyGptImagesConnectionInfoModel { ApiKey = yyUserSecretsModel.Default.OpenAi!.ApiKey! };

            for (int temp = 0; temp < imageCount; temp ++) // If the AI is faster than usual, the rate limit might be reached.
            {
                var xRequest = new yyGptImagesRequestModel
                {
                    Prompt = $"A beautiful person in a beautiful place, please.", // Changed to a more general prompt to see the tendency of the results.
                    Model = "dall-e-3",
                    Quality = "hd",
                    Size = "1792x1024"
                };

                if (temp % 2 != 0)
                {
                    xRequest.ResponseFormat = "b64_json";
                    // xRequest.Style = "natural"; doesnt seem to produce good results.
                }

                using (yyGptImagesClient xClient = new (xConnectionInfo))
                {
                    try
                    {
                        var xSendingTask = xClient.SendAsync (xRequest);

                        // Various tasks...

                        xSendingTask.Wait ();

                        var xReadingTask = xClient.ReadToEndAsync ();

                        // Various tasks...

                        xReadingTask.Wait ();

                        string? xJson = xReadingTask.Result;
                        var xResponse1 = yyGptImagesResponseParser.Parse (xJson);

                        if (xSendingTask.Result.HttpResponseMessage.IsSuccessStatusCode)
                        {
                            byte [] xBytes;

                            if (temp % 2 == 0)
                            {
                                using (HttpClient xHttpClient = new ())
                                {
                                    var xResponse2 = xHttpClient.GetAsync (xResponse1.Data! [0].Url).Result;

                                    // Just to make sure.
                                    xResponse2.EnsureSuccessStatusCode ();

                                    xBytes = xResponse2.Content.ReadAsByteArrayAsync ().Result;
                                }
                            }

                            else xBytes = Convert.FromBase64String (xResponse1.Data! [0].B64Json!);

                            // Based on the time when the image becomes available locally.
                            string xFilePathWithoutExtension = yyApplicationDirectory.MapPath ($"Images{Path.DirectorySeparatorChar}{yyFormatter.ToRoundtripFileNameString (DateTime.UtcNow)}");

                            yyDirectory.CreateParent (xFilePathWithoutExtension);

                            File.WriteAllText (xFilePathWithoutExtension + ".txt", xResponse1.Data [0].RevisedPrompt);
                            File.WriteAllBytes (xFilePathWithoutExtension + ".png", xBytes); // There seems to be no official document on the format of the image, though.

                            // For content distribution, JPEG is more convenient.

#pragma warning disable CA1416 // Validate platform compatibility
                            using Image xImage = Image.FromFile (xFilePathWithoutExtension + ".png");
                            xImage.Save (xFilePathWithoutExtension + ".jpg", ImageFormat.Jpeg); // Default quality.
#pragma warning restore CA1416

                            Console.WriteLine ("Generated image: " + xFilePathWithoutExtension + ".png");
                        }

                        else
                        {
                            // The following code displays 2 similar portions of text for testing purposes.
                            // The second one should display the string representation of the content of the error model that was successfully deserialized.

                            // If IsSuccessStatusCode is false, refer to the Error property.

                            Console.WriteLine (xJson.GetVisibleString ());

                            Console.WriteLine (JsonSerializer.Serialize (xResponse1, yyJson.DefaultSerializationOptions));
                        }
                    }

                    catch (Exception xException)
                    {
                        yySimpleLogger.Default.TryWriteException (xException);
                        Console.WriteLine (xException.ToString ());
                    }
                }
            }
        }

        public static void GeneratePage (string directoryPath, string summary)
        {
            try
            {
                string xPageTitle = Path.GetFileNameWithoutExtension (directoryPath);

                var xFiles = Directory.EnumerateFiles (directoryPath).Where (x =>
                {
                    // Making sure the .md file is not included to later cause an out-of-range exception.
                    string xExtension = Path.GetExtension (x);

                    return xExtension.Equals (".jpg", StringComparison.OrdinalIgnoreCase) ||
                        xExtension.Equals (".txt", StringComparison.OrdinalIgnoreCase);
                }).
                Order (StringComparer.OrdinalIgnoreCase).ToArray (); // Ordered and finalized.

                string xPageFilePath = Path.Join (directoryPath, $"{xPageTitle}.md");

                StringBuilder xPageFileContents = new ();
                xPageFileContents.AppendLine ($"# {xPageTitle}");
                xPageFileContents.AppendLine ();
                xPageFileContents.AppendLine (summary);

                // Loads the API key from the .yyUserSecrets file.
                yyGptChatConnectionInfoModel xConnectionInfo = new ();

                var xRequest = new yyGptChatRequestModel
                {
                    Model = "gpt-4"
                };

                xRequest.AddMessage (yyGptChatMessageRole.System, "You are a helpful assistant.");

                using (yyGptChatClient xClient = new (xConnectionInfo))
                {
                    for (int temp = 0; temp < xFiles.Length; temp += 2)
                    {
                        string xImageFileName = Path.GetFileName (xFiles [temp]),
                            xPrompt = File.ReadAllText (xFiles [temp + 1], Encoding.UTF8).Trim ();

                        xRequest.AddMessage (yyGptChatMessageRole.User, $"Please generate a title without quotation marks or punctuation marks for an image generated with the following prompt: {xPrompt}");

                        var xSendingTask = xClient.SendAsync (xRequest);
                        xSendingTask.Wait ();

                        string? xJson = xClient.ReadToEndAsync ().Result;
                        var xResponse = yyGptChatResponseParser.Parse (xJson);

                        if (xSendingTask.Result.HttpResponseMessage.IsSuccessStatusCode)
                        {
                            // Just to make sure.
                            // Punctuation marks still do appear, but it's not a major problem.
                            string xTitle = xResponse.Choices! [0].Message!.Content.GetVisibleString ().Trim ().Trim ('"').TrimEnd ('.');

                            xRequest.RemoveLastMessage ();

                            xPageFileContents.AppendLine ();
                            xPageFileContents.AppendLine ($"## {xTitle}");
                            xPageFileContents.AppendLine ();
                            xPageFileContents.AppendLine (xPrompt);
                            xPageFileContents.AppendLine ();
                            xPageFileContents.AppendLine ($"![{xTitle}]({xImageFileName})");

                            Console.WriteLine ($"Added to page: {temp / 2 + 1}) {xTitle}");
                        }

                        else
                        {
                            Console.WriteLine (xJson.GetVisibleString ());

                            // Console.WriteLine (JsonSerializer.Serialize (xResponse, yyJson.DefaultSerializationOptions));

                            // No point in continuing.
                            return;
                        }
                    }
                }

                File.WriteAllText (xPageFilePath, xPageFileContents.ToString (), Encoding.UTF8);
            }

            catch (Exception xException)
            {
                yySimpleLogger.Default.TryWriteException (xException);
                Console.WriteLine (xException.ToString ());
            }
        }

        public static void TranslatePage (string invariantLanguagePageFilePath, string languageCode, string languageName)
        {
            try
            {
                string xInvariantLanguagePageTitle = Path.GetFileNameWithoutExtension (invariantLanguagePageFilePath),
                    xInvariantLanguagePageFileContents = File.ReadAllText (invariantLanguagePageFilePath, Encoding.UTF8);

                string xNewPageFilePath = Path.Join (Path.GetDirectoryName (invariantLanguagePageFilePath), $"{xInvariantLanguagePageTitle}.{languageCode}.md");

                StringBuilder xNewPageFileContents = new ();

                // Loads the API key from the .yyUserSecrets file.
                yyGptChatConnectionInfoModel xConnectionInfo = new ();

                var xRequest = new yyGptChatRequestModel
                {
                    Model = "gpt-4"
                };

                xRequest.AddMessage (yyGptChatMessageRole.System, "You are a helpful assistant.");

                using (yyGptChatClient xClient = new (xConnectionInfo))
                {
                    string? xLastTranslatedImageTitle = null;

                    foreach (string xLine in yyStringLines.EnumerateLines (xInvariantLanguagePageFileContents))
                    {
                        string Translate (string str)
                        {
                            xRequest!.AddMessage (yyGptChatMessageRole.User, $"Please translate the following text into {languageName} and return only the translated text: {str}");

                            var xSendingTask = xClient.SendAsync (xRequest);
                            xSendingTask.Wait ();

                            // Just making sure.
                            // If an error (such as the too-many-requests error) occurs,
                            //     only the corresponding thread (within the parallel loop) ends through the catch block and the file will not be written.
                            xSendingTask.Result.HttpResponseMessage.EnsureSuccessStatusCode ();

                            string? xJson = xClient.ReadToEndAsync ().Result;
                            var xResponse = yyGptChatResponseParser.Parse (xJson);

                            // During the initial tests, some titles got a period at the end.
                            string xTranslatedText = xResponse.Choices! [0].Message!.Content.GetVisibleString ().Trim ().Trim ('"').TrimEnd ('.');

                            xRequest.RemoveLastMessage ();

                            return xTranslatedText;
                        }

                        if (xLine.StartsWith ("# "))
                            xNewPageFileContents.AppendLine ($"# {Translate (xLine.Substring (2))}");

                        else if (xLine.StartsWith ("## "))
                        {
                            string xOriginalImageTitle = xLine.Substring (3);
                            xLastTranslatedImageTitle = Translate (xOriginalImageTitle);
                            xNewPageFileContents.AppendLine ($"## {xLastTranslatedImageTitle}");

                            Console.WriteLine ($"Translated image title: {xOriginalImageTitle} => {xLastTranslatedImageTitle}");
                        }

                        else if (xLine.StartsWith ("!["))
                            xNewPageFileContents.AppendLine ($"![{xLastTranslatedImageTitle}{xLine.AsSpan (xLine.IndexOf ("]("))}");

                        else if (xLine.Length > 0)
                        {
                            if (xLine.StartsWith ("https://"))
                                xNewPageFileContents.AppendLine (xLine);

                            else xNewPageFileContents.AppendLine (Translate (xLine));
                        }

                        else xNewPageFileContents.AppendLine (xLine);
                    }
                }

                File.WriteAllText (xNewPageFilePath, xNewPageFileContents.ToString (), Encoding.UTF8);
            }

            catch (Exception xException)
            {
                yySimpleLogger.Default.TryWriteException (xException);
                Console.WriteLine (xException.ToString ());
            }
        }
    }
}

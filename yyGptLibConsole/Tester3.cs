using System.Drawing;
using System.Drawing.Imaging;
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
    }
}

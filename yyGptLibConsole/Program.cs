using yyLib;

namespace yyGptLibConsole
{
    internal class Program
    {
#pragma warning disable IDE0060 // Remove unused parameter
        static void Main (string [] args)
#pragma warning restore IDE0060
        {
            try
            {
                // Tester1.Test ();
                // Tester2.Test ();
                // Tester3.Test (100); // Do not run this test casually as it might cost a lot of money.

                Tester3.GeneratePage (@"C:\Repositories\Resources\Static\Beautiful People and Places",
                    "100 images generated in February 2024 with https://github.com/nao7sep/yyGpt/blob/main/yyGptLibConsole/Tester3.cs for testing purposes.");
            }

            catch (Exception xException)
            {
                yySimpleLogger.Default.TryWriteException (xException);
                Console.WriteLine (xException.ToString ());
            }
        }
    }
}

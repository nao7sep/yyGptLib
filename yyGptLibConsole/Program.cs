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
                    "100 images generated with the following code for testing purposes in February 2024:" +
                    Environment.NewLine +
                    Environment.NewLine +
                    "https://github.com/nao7sep/yyGpt/blob/main/yyGptLibConsole/Tester3.cs");
            }

            catch (Exception xException)
            {
                yySimpleLogger.Default.TryWriteException (xException);
                Console.WriteLine (xException.ToString ());
            }
        }
    }
}

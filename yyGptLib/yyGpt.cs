using yyLib;

namespace yyGptLib
{
    public static class yyGpt
    {
        /// <summary>
        /// Returns null if the .yyUserSecrets file doesnt exist or contains no API key.
        /// </summary>
        public static string? DefaultApiKey { get; } = yyUserSecretsModel.Default.OpenAi?.ApiKey.WhiteSpaceToNull ();
    }
}

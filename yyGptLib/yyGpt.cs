using yyLib;

namespace yyGptLib
{
    public static class yyGpt
    {
        /// <summary>
        /// Returns null if the .yyUserSecrets.json file doesnt exist or contains no API key.
        /// </summary>
        public static string? DefaultApiKey { get; } = yyUserSecretsModel.Default.OpenAi?.ApiKey.WhiteSpaceToNull ();

        public static string? DefaultOrganization { get; } = yyUserSecretsModel.Default.OpenAi?.Organization.WhiteSpaceToNull ();

        public static string? DefaultProject { get; } = yyUserSecretsModel.Default.OpenAi?.Project.WhiteSpaceToNull ();
    }
}

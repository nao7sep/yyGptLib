using yyLib;

namespace yyGptLib
{
    public static class yyGptImages
    {
        public static string DefaultEndpoint { get; } = yyUserSecretsModel.Default.OpenAi?.ImagesEndpoint.WhiteSpaceToNull () ?? "https://api.openai.com/v1/images/generations";
    }
}

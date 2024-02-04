using yyLib;

namespace yyGptLib
{
    public static class yyGptChat
    {
        public static string DefaultEndpoint { get; } = yyUserSecretsModel.Default.OpenAi?.ChatEndpoint.WhiteSpaceToNull () ?? "https://api.openai.com/v1/chat/completions";

        public static string DefaultModel { get; } = yyUserSecretsModel.Default.OpenAi?.ChatModel.WhiteSpaceToNull () ?? "gpt-3.5-turbo";
    }
}

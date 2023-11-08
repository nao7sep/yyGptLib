using yyLib;

namespace yyGptLib
{
    public partial class yyGptChatMessageRole: IEquatable <yyGptChatMessageRole>
    {
        public static yyGptChatMessageRole System { get; } = new yyGptChatMessageRole ("system");

        public static yyGptChatMessageRole User { get; } = new yyGptChatMessageRole ("user");

        public static yyGptChatMessageRole Assistant { get; } = new yyGptChatMessageRole ("assistant");

        public static yyGptChatMessageRole Parse (string? value)
        {
            if ("system".Equals (value, StringComparison.OrdinalIgnoreCase))
                return System;

            else if ("user".Equals (value, StringComparison.OrdinalIgnoreCase))
                return User;

            else if ("assistant".Equals (value, StringComparison.OrdinalIgnoreCase))
                return Assistant;

            else throw new yyArgumentException ($"@{nameof (value)} is invalid: \"{value}\"");
        }
    }
}

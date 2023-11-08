using yyLib;

namespace yyGptLib
{
    public partial class yyGptChatRole: IEquatable <yyGptChatRole>
    {
        public static yyGptChatRole System { get; } = new yyGptChatRole ("system");

        public static yyGptChatRole User { get; } = new yyGptChatRole ("user");

        public static yyGptChatRole Assistant { get; } = new yyGptChatRole ("assistant");

        public static yyGptChatRole Parse (string value)
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

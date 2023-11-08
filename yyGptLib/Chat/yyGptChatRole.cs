namespace yyGptLib
{
    public partial class yyGptChatRole: IEquatable <yyGptChatRole>
    {
        public string Value { get; private set; }

        public yyGptChatRole (string value) => Value = value;

        public bool Equals (yyGptChatRole? role) => Value.Equals (role?.Value, StringComparison.OrdinalIgnoreCase);

        public override bool Equals (object? obj) => Equals (obj as yyGptChatRole);

        public override int GetHashCode () => Value.GetHashCode ();
    }
}

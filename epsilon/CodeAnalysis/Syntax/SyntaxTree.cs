public sealed class SyntaxTree {
    //  1 + 2 * 3          Expression
    //     +                   |   to
    //    / \            The Syntax Tree
    //   1   *
    //      / \
    //     2   3
    public SyntaxTree(IEnumerable<string> diagnostics, ExpressionSyntax root, SyntaxToken endOfFileToken) {
        Diagnostics = diagnostics.ToArray();
        Root = root;
        EndOfFileToken = endOfFileToken;
    }

    public IReadOnlyList<string> Diagnostics { get; }
    public ExpressionSyntax Root { get; }
    public SyntaxToken EndOfFileToken { get; }

    public static SyntaxTree Parse(string text){
        var parser = new Parser(text);
        return parser.Parse();
    }
}

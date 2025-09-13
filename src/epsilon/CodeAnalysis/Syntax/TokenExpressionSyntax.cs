namespace epsilon.CodeAnalysis.Syntax;

public sealed partial class TokenExpressionSyntax : ExpressionSyntax {
    public TokenExpressionSyntax(SyntaxTree syntaxTree, SyntaxToken token) : base(syntaxTree) {
        Token = token;
    }

    public override SyntaxKind Kind => SyntaxKind.TokenExpression;
    public SyntaxToken Token { get; }
}

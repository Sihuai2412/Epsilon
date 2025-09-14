namespace epsilon.CodeAnalysis.Syntax;

public sealed partial class CallExpressionSyntax : ExpressionSyntax {
    public CallExpressionSyntax(SyntaxTree syntaxTree, SyntaxToken identifier, SyntaxToken openParenthesisToken, SeparatedSyntaxList<ExpressionSyntax> arguments, SyntaxToken closeParenthesisToken, SyntaxToken? semicolon) : base(syntaxTree) {
        Identifier = identifier;
        OpenParenthesisToken = openParenthesisToken;
        Arguments = arguments;
        CloseParenthesisToken = closeParenthesisToken;
        Semicolon = semicolon;
    }

    public override SyntaxKind Kind => SyntaxKind.CallExpression;
    public SyntaxToken Identifier { get; }
    public SyntaxToken OpenParenthesisToken { get; }
    public SeparatedSyntaxList<ExpressionSyntax> Arguments { get; }
    public SyntaxToken CloseParenthesisToken { get; }
    public SyntaxToken? Semicolon { get; }
}

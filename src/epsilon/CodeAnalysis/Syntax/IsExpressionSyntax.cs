namespace epsilon.CodeAnalysis.Syntax;

public sealed partial class IsExpressionSyntax : ExpressionSyntax {
    public IsExpressionSyntax(SyntaxTree syntaxTree, ExpressionSyntax expression, SyntaxToken isKeyword, SyntaxToken type) : base(syntaxTree) {
        Expression = expression;
        IsKeyword = isKeyword;
        Type = type;
    }

    public override SyntaxKind Kind => SyntaxKind.IsExpression;
    public ExpressionSyntax Expression { get; }
    public SyntaxToken IsKeyword { get; }
    public SyntaxToken Type { get; }
}

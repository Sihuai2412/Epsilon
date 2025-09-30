namespace epsilon.CodeAnalysis.Syntax;

public sealed partial class InitializerSyntax : SyntaxNode {
    public InitializerSyntax(SyntaxTree syntaxTree, SyntaxToken equalsKeyword, ExpressionSyntax expression) : base(syntaxTree) {
        EqualsKeyword = equalsKeyword;
        Expression = expression;
    }

    public override SyntaxKind Kind => SyntaxKind.Initializer;
    public SyntaxToken EqualsKeyword { get; }
    public ExpressionSyntax Expression { get; }
}
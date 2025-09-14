namespace epsilon.CodeAnalysis.Syntax;

public sealed partial class ExpressionStatementSyntax : StatementSyntax {
    public ExpressionStatementSyntax(SyntaxTree syntaxTree, ExpressionSyntax expression, SyntaxToken semicolon) : base(syntaxTree) {
        Expression = expression;
        Semicolon = semicolon;
    }

    public override SyntaxKind Kind => SyntaxKind.ExpressionStatement;
    public ExpressionSyntax Expression { get; }
    public SyntaxToken Semicolon { get; }
}
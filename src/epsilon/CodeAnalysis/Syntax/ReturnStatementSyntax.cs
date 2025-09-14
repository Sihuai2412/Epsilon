namespace epsilon.CodeAnalysis.Syntax;

public sealed partial class ReturnStatementSyntax : StatementSyntax {
    public ReturnStatementSyntax(SyntaxTree syntaxTree, SyntaxToken returnKeyword, ExpressionSyntax? expression, SyntaxToken semicolon) : base(syntaxTree) {
        ReturnKeyword = returnKeyword;
        Expression = expression;
        Semicolon = semicolon;
    }

    public override SyntaxKind Kind => SyntaxKind.ReturnStatement;
    public SyntaxToken ReturnKeyword { get; }
    public ExpressionSyntax? Expression { get; }
    public SyntaxToken Semicolon { get; }
}

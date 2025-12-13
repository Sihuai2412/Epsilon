namespace epsilon.CodeAnalysis.Syntax;

public sealed partial class DoWhileStatementSyntax : StatementSyntax {
    public DoWhileStatementSyntax(SyntaxTree syntaxTree, SyntaxToken doKeyword, StatementSyntax body, SyntaxToken whileKeyword, ExpressionSyntax condition, SyntaxToken? semicolon) : base(syntaxTree) {
        DoKeyword = doKeyword;
        Body = body;
        WhileKeyword = whileKeyword;
        Condition = condition;
        Semicolon = semicolon;
    }

    public override SyntaxKind Kind => SyntaxKind.DoWhileStatement;
    public SyntaxToken DoKeyword { get; }
    public StatementSyntax Body { get; }
    public SyntaxToken WhileKeyword { get; }
    public ExpressionSyntax Condition { get; }
    public SyntaxToken? Semicolon { get; }
}
namespace epsilon.CodeAnalysis.Syntax;

public sealed partial class AssignmentExpressionSyntax : ExpressionSyntax {
    public AssignmentExpressionSyntax(SyntaxTree syntaxTree, SyntaxToken identifierToken, SyntaxToken assignmentToken, ExpressionSyntax expression, SyntaxToken? semicolon) : base(syntaxTree) {
        IdentifierToken = identifierToken;
        AssignmentToken = assignmentToken;
        Expression = expression;
        Semicolon = semicolon;
    }

    public override SyntaxKind Kind => SyntaxKind.AssignmentExpression;
    public SyntaxToken IdentifierToken { get; }
    public SyntaxToken AssignmentToken { get; }
    public ExpressionSyntax Expression { get; }
    public SyntaxToken? Semicolon { get; }
}

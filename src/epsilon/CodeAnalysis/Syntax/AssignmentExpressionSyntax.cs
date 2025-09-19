namespace epsilon.CodeAnalysis.Syntax;

public sealed partial class AssignmentExpressionSyntax : ExpressionSyntax {
    public AssignmentExpressionSyntax(SyntaxTree syntaxTree, SyntaxToken identifierToken, SyntaxToken assignmentToken, ExpressionSyntax expression) : base(syntaxTree) {
        IdentifierToken = identifierToken;
        AssignmentToken = assignmentToken;
        Expression = expression;
    }

    public override SyntaxKind Kind => SyntaxKind.AssignmentExpression;
    public SyntaxToken IdentifierToken { get; }
    public SyntaxToken AssignmentToken { get; }
    public ExpressionSyntax Expression { get; }
}

public sealed partial class IsExpressionSyntax : ExpressionSyntax {
    public IsExpressionSyntax(SyntaxTree syntaxTree, SyntaxToken identifierToken, SyntaxToken isKeyword, SyntaxToken type) : base(syntaxTree) {
        IdentifierToken = identifierToken;
        IsKeyword = isKeyword;
        Type = type;
    }

    public override SyntaxKind Kind => SyntaxKind.IsExpression;
    public SyntaxToken IdentifierToken { get; }
    public SyntaxToken IsKeyword { get; }
    public SyntaxToken Type { get; }
}

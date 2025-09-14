namespace epsilon.CodeAnalysis.Syntax;

public sealed partial class VariableDeclarationSyntax : StatementSyntax {
    public VariableDeclarationSyntax(SyntaxTree syntaxTree, SyntaxToken keyword, SyntaxToken identifier, TypeClauseSyntax? typeClause, SyntaxToken equalsToken, ExpressionSyntax initializer, SyntaxToken semicolon) : base(syntaxTree) {
        Keyword = keyword;
        Identifier = identifier;
        TypeClause = typeClause;
        EqualsToken = equalsToken;
        Initializer = initializer;
        Semicolon = semicolon;
    }

    public override SyntaxKind Kind => SyntaxKind.VariableDeclaration;
    public SyntaxToken Keyword { get; }
    public SyntaxToken Identifier { get; }
    public TypeClauseSyntax? TypeClause { get; }
    public SyntaxToken EqualsToken { get; }
    public ExpressionSyntax Initializer { get; }
    public SyntaxToken Semicolon { get; }
}
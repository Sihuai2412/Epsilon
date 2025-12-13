namespace epsilon.CodeAnalysis.Syntax;

public sealed partial class VariableDeclarationSyntax : StatementSyntax {
    public VariableDeclarationSyntax(SyntaxTree syntaxTree, SyntaxToken keyword, SyntaxToken identifier, TypeClauseSyntax? typeClause, InitializerSyntax? initializer, SyntaxToken? semicolon) : base(syntaxTree) {
        Keyword = keyword;
        Identifier = identifier;
        TypeClause = typeClause;
        Initializer = initializer;
        Semicolon = semicolon;
    }

    public override SyntaxKind Kind => SyntaxKind.VariableDeclaration;
    public SyntaxToken Keyword { get; }
    public SyntaxToken Identifier { get; }
    public TypeClauseSyntax? TypeClause { get; }
    public InitializerSyntax? Initializer { get; }
    public SyntaxToken? Semicolon { get; }
}
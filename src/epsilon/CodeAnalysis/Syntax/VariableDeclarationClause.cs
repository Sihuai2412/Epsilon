namespace epsilon.CodeAnalysis.Syntax;

public sealed partial class VariableDeclarationClause : SyntaxNode {
    public VariableDeclarationClause(SyntaxToken? comma, SyntaxTree syntaxTree, SyntaxToken identifier, TypeClauseSyntax? typeClause, InitializerSyntax? initializer) : base(syntaxTree) {
        Comma = comma;
        Identifier = identifier;
        TypeClause = typeClause;
        Initializer = initializer;
    }

    public override SyntaxKind Kind => SyntaxKind.VariableDeclarationClause;

    public SyntaxToken? Comma { get; }
    public SyntaxToken Identifier { get; }
    public TypeClauseSyntax? TypeClause { get; }
    public InitializerSyntax? Initializer { get; }
}
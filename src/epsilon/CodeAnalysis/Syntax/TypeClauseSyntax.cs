namespace epsilon.CodeAnalysis.Syntax;

public sealed partial class TypeClauseSyntax : SyntaxNode {
    public TypeClauseSyntax(SyntaxTree syntaxTree, SyntaxToken asKeyword, SyntaxToken identifier) : base(syntaxTree) {
        AsKeyword = asKeyword;
        Identifier = identifier;
    }

    public override SyntaxKind Kind => SyntaxKind.TypeClause;
    public SyntaxToken AsKeyword { get; }
    public SyntaxToken Identifier { get; }
}

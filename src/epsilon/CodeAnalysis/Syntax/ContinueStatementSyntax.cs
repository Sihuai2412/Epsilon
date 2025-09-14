namespace epsilon.CodeAnalysis.Syntax;

internal sealed partial class ContinueStatementSyntax : StatementSyntax {
    public ContinueStatementSyntax(SyntaxTree syntaxTree, SyntaxToken keyword, SyntaxToken semicolon) : base(syntaxTree) {
        Keyword = keyword;
        Semicolon = semicolon;
    }

    public override SyntaxKind Kind => SyntaxKind.ContinueStatement;
    public SyntaxToken Keyword { get; }
    public SyntaxToken Semicolon { get; }
}
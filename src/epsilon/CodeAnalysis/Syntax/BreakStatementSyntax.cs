namespace epsilon.CodeAnalysis.Syntax;

internal sealed partial class BreakStatementSyntax : StatementSyntax {
    public BreakStatementSyntax(SyntaxTree syntaxTree, SyntaxToken keyword, SyntaxToken semicolon) : base(syntaxTree) {
        Keyword = keyword;
        Semicolon = semicolon;
    }

    public override SyntaxKind Kind => SyntaxKind.BreakStatement;
    public SyntaxToken Keyword { get; }
    public SyntaxToken Semicolon { get; }
}
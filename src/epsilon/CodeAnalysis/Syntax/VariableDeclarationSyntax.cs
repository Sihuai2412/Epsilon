using System.Collections.Immutable;

namespace epsilon.CodeAnalysis.Syntax;

public sealed partial class VariableDeclarationSyntax : StatementSyntax {
    public VariableDeclarationSyntax(SyntaxTree syntaxTree, SyntaxToken keyword, ImmutableArray<VariableDeclarationClause> clauses, SyntaxToken? semicolon) : base(syntaxTree) {
        Keyword = keyword;
        Clauses = clauses;
        Semicolon = semicolon;
    }

    public override SyntaxKind Kind => SyntaxKind.VariableDeclarationStatement;
    public SyntaxToken Keyword { get; }
    public ImmutableArray<VariableDeclarationClause> Clauses { get; }
    public SyntaxToken? Semicolon { get; }
}

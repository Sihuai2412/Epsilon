using System.Collections.Immutable;
using epsilon.CodeAnalysis.Symbols;

namespace epsilon.CodeAnalysis.Binding;

internal sealed class BoundVariableDeclaration : BoundStatement {
    public BoundVariableDeclaration(ImmutableArray<(VariableSymbol variable, BoundExpression? initializer)> declarations) {
        Declarations = declarations;
    }

    public override BoundNodeKind Kind => BoundNodeKind.VariableDeclaration;
    public ImmutableArray<(VariableSymbol variable, BoundExpression? initializer)> Declarations { get; }
}

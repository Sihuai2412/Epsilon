using System.Collections.Immutable;
using epsilon.CodeAnalysis.Symbols;

namespace epsilon.CodeAnalysis.Binding;

internal sealed class BoundCallExpression : BoundExpression {
    public BoundCallExpression(FunctionSymbol function, ImmutableArray<BoundExpression> arguments) {
        Function = function;
        Arguments = arguments;
    }

    public override BoundNodeKind Kind => BoundNodeKind.CallExpression;
    public override TypeSymbol Type => Function.Type;
    public FunctionSymbol Function { get; }
    public ImmutableArray<BoundExpression> Arguments { get; }
}

internal sealed class BoundIsExpression : BoundExpression {
    public BoundIsExpression(VariableSymbol variable, TypeSymbol typeSymbol) {
        Variable = variable;
        TypeSymbol = typeSymbol;
    }

    public override BoundNodeKind Kind => BoundNodeKind.IsExpression;
    public override TypeSymbol Type => TypeSymbol.Bool;
    public VariableSymbol Variable { get; }
    public TypeSymbol TypeSymbol { get; }
}

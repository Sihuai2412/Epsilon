using epsilon.CodeAnalysis.Symbols;

namespace epsilon.CodeAnalysis.Binding;

internal sealed class BoundIsExpression : BoundExpression {
    public BoundIsExpression(BoundExpression expression, TypeSymbol typeSymbol) {
        Expression = expression;
        TypeSymbol = typeSymbol;
    }

    public override BoundNodeKind Kind => BoundNodeKind.IsExpression;
    public override TypeSymbol Type => TypeSymbol.Bool;
    public BoundExpression Expression { get; }
    public TypeSymbol TypeSymbol { get; }
}

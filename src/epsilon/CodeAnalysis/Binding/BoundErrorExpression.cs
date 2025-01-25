using epsilon.CodeAnalysis.Symbols;

namespace epsilon.CodeAnalysis.Binding;

internal sealed class BoundErrorExpression : BoundExpression {
    public override BoundNodeKind Kind => BoundNodeKind.ErrorExpression;
    public override TypeSymbol Type => TypeSymbol.Error;
}

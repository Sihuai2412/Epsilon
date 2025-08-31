using epsilon.CodeAnalysis.Symbols;

namespace epsilon.CodeAnalysis.Binding;

internal abstract class BoundExpression : BoundNode {
    public abstract TypeSymbol Type { get; }

    public virtual BoundConstant? ConstantValue => null;
}

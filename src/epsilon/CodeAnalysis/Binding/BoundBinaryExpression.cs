using epsilon.CodeAnalysis.Symbols;

namespace epsilon.CodeAnalysis.Binding;

internal sealed class BoundBinaryExpression : BoundExpression {
    public BoundBinaryExpression(BoundExpression left,
                                 BoundBinaryOperator op,
                                 BoundExpression right){
        Left = left;
        Op = op;
        Right = right;
    }

    public override BoundNodeKind Kind => BoundNodeKind.BinaryExpression;
    public override TypeSymbol Type => Op.ResultType;
    public BoundExpression Left { get; }
    public BoundBinaryOperator Op { get; }
    public BoundExpression Right { get; }
}

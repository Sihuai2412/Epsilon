using epsilon.CodeAnalysis.Symbols;

namespace epsilon.CodeAnalysis.Binding;

internal static class ConstantFolding {
    public static BoundConstant? Fold(BoundUnaryOperator op, BoundExpression operand) {
        if (operand.ConstantValue != null) {
            switch (op.Kind) {
                case BoundUnaryOperatorKind.Identity:
                    if (op.Type == TypeSymbol.Int) {
                        return new BoundConstant(+(int)operand.ConstantValue.Value);
                    } else if (op.Type == TypeSymbol.Float) {
                        return new BoundConstant(+(float)operand.ConstantValue.Value);
                    }
                    return null;
                case BoundUnaryOperatorKind.Negation:
                    if (op.Type == TypeSymbol.Int) {
                        return new BoundConstant(-(int)operand.ConstantValue.Value);
                    } else if (op.Type == TypeSymbol.Float) {
                        return new BoundConstant(-(float)operand.ConstantValue.Value);
                    }
                    return null;
                case BoundUnaryOperatorKind.LogicalNegation:
                    return new BoundConstant(!(bool)operand.ConstantValue.Value);
                case BoundUnaryOperatorKind.OnesComplement:
                    return new BoundConstant(~(int)operand.ConstantValue.Value);
                default:
                    throw new Exception($"Unexpected unary operator {op.Kind}");
            }
        }

        return null;
    }

    public static BoundConstant? Fold(BoundExpression left, BoundBinaryOperator op, BoundExpression right) {
        var leftConstant = left.ConstantValue;
        var rightConstant = right.ConstantValue;

        if (op.Kind == BoundBinaryOperatorKind.LogicalAnd) {
            if (leftConstant != null && !(bool)leftConstant.Value ||
                rightConstant != null && !(bool)rightConstant.Value) {
                return new BoundConstant(false);
            }
        }

        if (op.Kind == BoundBinaryOperatorKind.LogicalOr) {
            if (leftConstant != null && (bool)leftConstant.Value ||
                rightConstant != null && (bool)rightConstant.Value) {
                return new BoundConstant(true);
            }
        }

        if (leftConstant == null || rightConstant == null) {
            return null;
        }

        var l = leftConstant.Value;
        var r = rightConstant.Value;

        switch (op.Kind) {
            case BoundBinaryOperatorKind.Addition:
                if (left.Type == TypeSymbol.Int) {
                    return new BoundConstant((int)l + (int)r);
                } else if (left.Type == TypeSymbol.Float) {
                    return new BoundConstant((float)l + (float)r);
                } else if (left.Type == TypeSymbol.String) {
                    return new BoundConstant((string)l + (string)r);
                }
                return null;
            case BoundBinaryOperatorKind.Subtraction:
                if (left.Type == TypeSymbol.Int) {
                    return new BoundConstant((int)l - (int)r);
                } else if (left.Type == TypeSymbol.Float) {
                    return new BoundConstant((float)l - (float)r);
                }
                return null;
            case BoundBinaryOperatorKind.Multiplication:
                if (left.Type == TypeSymbol.Int) {
                    return new BoundConstant((int)l * (int)r);
                } else if (left.Type == TypeSymbol.Float) {
                    return new BoundConstant((float)l * (float)r);
                }
                return null;
            case BoundBinaryOperatorKind.Division:
                if (left.Type == TypeSymbol.Int) {
                    return new BoundConstant((int)l / (int)r);
                } else if (left.Type == TypeSymbol.Float) {
                    return new BoundConstant((float)l / (float)r);
                }
                return null;
            case BoundBinaryOperatorKind.BitwiseAnd: {
                    if (left.Type == TypeSymbol.Int) {
                        return new BoundConstant((int)l & (int)r);
                    } else {
                        return new BoundConstant((bool)l & (bool)r);
                    }
                }
            case BoundBinaryOperatorKind.BitwiseOr: {
                    if (left.Type == TypeSymbol.Int) {
                        return new BoundConstant((int)l | (int)r);
                    } else {
                        return new BoundConstant((bool)l | (bool)r);
                    }
                }
            case BoundBinaryOperatorKind.BitwiseXOr: {
                    if (left.Type == TypeSymbol.Int) {
                        return new BoundConstant((int)l ^ (int)r);
                    } else {
                        return new BoundConstant((bool)l ^ (bool)r);
                    }
                }
            case BoundBinaryOperatorKind.LogicalAnd:
                return new BoundConstant((bool)l && (bool)r);
            case BoundBinaryOperatorKind.LogicalOr:
                return new BoundConstant((bool)l || (bool)r);
            case BoundBinaryOperatorKind.Equals:
                return new BoundConstant(Equals(l, r));
            case BoundBinaryOperatorKind.NotEquals:
                return new BoundConstant(!Equals(l, r));
            case BoundBinaryOperatorKind.Less:
                if (left.Type == TypeSymbol.Int) {
                    return new BoundConstant((int)l < (int)r);
                } else if (left.Type == TypeSymbol.Float) {
                    return new BoundConstant((float)l < (float)r);
                }
                return null;
            case BoundBinaryOperatorKind.LessOrEquals:
                if (left.Type == TypeSymbol.Int) {
                    return new BoundConstant((int)l <= (int)r);
                } else if (left.Type == TypeSymbol.Float) {
                    return new BoundConstant((float)l <= (float)r);
                }
                return null;
            case BoundBinaryOperatorKind.Greater:
                if (left.Type == TypeSymbol.Int) {
                    return new BoundConstant((int)l > (int)r);
                } else if (left.Type == TypeSymbol.Float) {
                    return new BoundConstant((float)l > (float)r);
                }
                return null;
            case BoundBinaryOperatorKind.GreaterOrEquals:
                if (left.Type == TypeSymbol.Int) {
                    return new BoundConstant((int)l >= (int)r);
                } else if (left.Type == TypeSymbol.Float) {
                    return new BoundConstant((float)l >= (float)r);
                }
                return null;
            default:
                throw new Exception($"Unexpected binary operator {op.Kind}");
        }
    }
}
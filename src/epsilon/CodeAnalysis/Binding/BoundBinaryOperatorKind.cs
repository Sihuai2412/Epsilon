namespace epsilon.CodeAnalysis.Binding;

internal enum BoundBinaryOperatorKind {
    Addition,
    Subtraction,
    Multiplication,
    Exponentiation,
    Division,
    Modulo,
    BitwiseAnd,
    LogicalAnd,
    BitwiseOr,
    LogicalOr,
    BitwiseXOr,
    Equals,
    NotEquals,
    Less,
    LessOrEquals,
    Greater,
    GreaterOrEquals,
}

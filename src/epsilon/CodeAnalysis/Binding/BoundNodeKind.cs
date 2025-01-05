namespace epsilon.CodeAnalysis.Binding;

internal enum BoundNodeKind {
    // Statements
    BlockStatement,
    VariableDeclaration,
    IfStatement,
    WhileStatement,
    ExpressionStatement,
    
    // Expressions
    LiteralExpression,
    VariableExpression,
    AssignmentExpression,
    UnaryExpression,
    BinaryExpression
}

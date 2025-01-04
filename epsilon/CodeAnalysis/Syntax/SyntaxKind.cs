public enum SyntaxKind {
    // Tokens
    NumberToken,
    WhitespaceToken,
    PlusToken,
    MinusToken,
    StarToken,
    SlashToken,
    BangToken,
    EqualsToken,
    AmpersandAmpersandToken,
    PipePipeToken,
    EqualsEqualsToken,
    BangEqualsToken,
    OpenParenthesisToken,
    CloseParenthesisToken,
    OpenBraceToken,
    CloseBraceToken,
    BadToken,
    EndOfFileToken,
    IdentifierToken,

    // Keywords
    FalseKeyword,
    TrueKeyword,

    // Nodes
    CompilationUnit,

    // Statements
    BlockStatement,
    ExpressionStatement,

    // Expressions
    LiteralExpression,
    NameExpression,
    UnaryExpression,
    BinaryExpression,
    ParenthesizedExpression,
    AssignmentExpression
}

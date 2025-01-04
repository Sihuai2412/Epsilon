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
    LetKeyword,
    TrueKeyword,
    VarKeyword,

    // Nodes
    CompilationUnit,

    // Statements
    BlockStatement,
    VariableDeclaration,
    ExpressionStatement,

    // Expressions
    LiteralExpression,
    NameExpression,
    UnaryExpression,
    BinaryExpression,
    ParenthesizedExpression,
    AssignmentExpression
}

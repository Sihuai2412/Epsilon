namespace epsilon.CodeAnalysis.Syntax;

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
    LessToken,
    LessOrEqualsToken,
    GreaterToken,
    GreaterOrEqualsToken,
    OpenParenthesisToken,
    CloseParenthesisToken,
    OpenBraceToken,
    CloseBraceToken,
    BadToken,
    EndOfFileToken,
    IdentifierToken,

    // Keywords
    ElseKeyword,
    FalseKeyword,
    IfKeyword,
    LetKeyword,
    TrueKeyword,
    VarKeyword,
    WhileKeyword,

    // Nodes
    CompilationUnit,
    ElseClause,

    // Statements
    BlockStatement,
    VariableDeclaration,
    IfStatement,
    WhileStatement,
    ExpressionStatement,

    // Expressions
    LiteralExpression,
    NameExpression,
    UnaryExpression,
    BinaryExpression,
    ParenthesizedExpression,
    AssignmentExpression
}

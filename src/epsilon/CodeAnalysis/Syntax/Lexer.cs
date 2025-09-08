using System.Collections.Immutable;
using System.Text;
using epsilon.CodeAnalysis.Symbols;
using epsilon.CodeAnalysis.Text;

namespace epsilon.CodeAnalysis.Syntax;

internal sealed class Lexer {
    private readonly SyntaxTree _syntaxTree;
    private readonly SourceText _text;
    private readonly DiagnosticBag _diagnostics = new DiagnosticBag();
    private int _position;

    private int _start;
    private SyntaxKind _kind;
    private object? _value;
    private ImmutableArray<SyntaxTrivia>.Builder _triviaBuilder = ImmutableArray.CreateBuilder<SyntaxTrivia>();

    public Lexer(SyntaxTree syntaxTree) {
        _syntaxTree = syntaxTree;
        _text = syntaxTree.Text;
    }

    public DiagnosticBag Diagnostics => _diagnostics;

    private char Current => Peek(0);

    private char Lookahead => Peek(1);

    private char Peek(int offset) {
        var index = _position + offset;

        if (index >= _text.Length) {
            return '\0';
        }

        return _text[index];
    }

    public SyntaxToken Lex() {
        ReadTrivia(leading: true);
        var leadingTrivia = _triviaBuilder.ToImmutable();

        var tokenStart = _position;
        ReadToken();
        var tokenKind = _kind;
        var tokenValue = _value;
        var tokenLength = _position - _start;

        ReadTrivia(leading: false);
        var trailingTrivia = _triviaBuilder.ToImmutable();

        var tokenText = SyntaxFacts.GetText(tokenKind);
        if (tokenText == null) {
            tokenText = _text.ToString(tokenStart, tokenLength);
        }

        return new SyntaxToken(_syntaxTree, tokenKind, tokenStart, tokenText, tokenValue, leadingTrivia, trailingTrivia);
    }

    private void ReadToken() {
        _start = _position;
        _kind = SyntaxKind.BadToken;
        _value = null;
        switch (Current) {
            case '\0': {
                    _kind = SyntaxKind.EndOfFileToken;
                    break;
                }
            case '+': {
                    _position++;
                    if (Current != '=') {
                        _kind = SyntaxKind.PlusToken;
                    } else {
                        _kind = SyntaxKind.PlusEqualsToken;
                        _position++;
                    }
                    break;
                }
            case '-': {
                    _position++;
                    if (Current != '=') {
                        _kind = SyntaxKind.MinusToken;
                    } else {
                        _kind = SyntaxKind.MinusEqualsToken;
                        _position++;
                    }
                    break;
                }
            case '*': {
                    _position++;
                    if (Current != '=') {
                        _kind = SyntaxKind.StarToken;
                    } else {
                        _kind = SyntaxKind.StarEqualsToken;
                        _position++;
                    }
                    break;
                }
            case '/': {
                    _position++;
                    if (Current != '=') {
                        _kind = SyntaxKind.SlashToken;
                    } else {
                        _kind = SyntaxKind.SlashEqualsToken;
                        _position++;
                    }
                    break;
                }
            case '(': {
                    _kind = SyntaxKind.OpenParenthesisToken;
                    _position++;
                    break;
                }
            case ')': {
                    _kind = SyntaxKind.CloseParenthesisToken;
                    _position++;
                    break;
                }
            case '{': {
                    _kind = SyntaxKind.OpenBraceToken;
                    _position++;
                    break;
                }
            case '}': {
                    _kind = SyntaxKind.CloseBraceToken;
                    _position++;
                    break;
                }
            case ':': {
                    _kind = SyntaxKind.ColonToken;
                    _position++;
                    break;
                }
            case ',': {
                    _kind = SyntaxKind.CommaToken;
                    _position++;
                    break;
                }
            case '~': {
                    _kind = SyntaxKind.TildeToken;
                    _position++;
                    break;
                }
            case '^': {
                    _position++;
                    if (Current != '=') {
                        _kind = SyntaxKind.HatToken;
                    } else {
                        _kind = SyntaxKind.HatEqualsToken;
                        _position++;
                    }
                    break;
                }
            case '&': {
                    _position++;
                    if (Current == '&') {
                        _kind = SyntaxKind.AmpersandAmpersandToken;
                        _position++;
                    } else if (Current == '=') {
                        _kind = SyntaxKind.AmpersandEqualsToken;
                        _position++;
                    } else {
                        _kind = SyntaxKind.AmpersandToken;
                    }
                    break;
                }
            case '|': {
                    _position++;
                    if (Current == '|') {
                        _kind = SyntaxKind.PipePipeToken;
                        _position++;
                    } else if (Current == '=') {
                        _kind = SyntaxKind.PipeEqualsToken;
                        _position++;
                    } else {
                        _kind = SyntaxKind.PipeToken;
                    }
                    break;
                }
            case '=': {
                    _position++;
                    if (Current != '=') {
                        _kind = SyntaxKind.EqualsToken;
                    } else {
                        _position++;
                        _kind = SyntaxKind.EqualsEqualsToken;
                    }
                    break;
                }
            case '!': {
                    _position++;
                    if (Current != '=') {
                        _kind = SyntaxKind.BangToken;
                    } else {
                        _position++;
                        _kind = SyntaxKind.BangEqualsToken;
                    }
                    break;
                }
            case '<': {
                    _position++;
                    if (Current != '=') {
                        _kind = SyntaxKind.LessToken;
                    } else {
                        _position++;
                        _kind = SyntaxKind.LessOrEqualsToken;
                    }
                    break;
                }
            case '>': {
                    _position++;
                    if (Current != '=') {
                        _kind = SyntaxKind.GreaterToken;
                    } else {
                        _position++;
                        _kind = SyntaxKind.GreaterOrEqualsToken;
                    }
                    break;
                }
            case '"': {
                    ReadString();
                    break;
                }
            case '0':
            case '1':
            case '2':
            case '3':
            case '4':
            case '5':
            case '6':
            case '7':
            case '8':
            case '9': {
                    ReadNumberToken();
                    break;
                }
            case '_': {
                    ReadIdentifierOrKeyword();
                    break;
                }
            default: {
                    if (char.IsLetter(Current)) {
                        ReadIdentifierOrKeyword();
                    } else {
                        var span = new TextSpan(_position, 1);
                        var location = new TextLocation(_text, span);
                        _diagnostics.ReportBadCharacter(location, Current);
                        _position++;
                    }
                    break;
                }
        }
    }

    private void ReadTrivia(bool leading) {
        _triviaBuilder.Clear();

        var done = false;
        while (!done) {
            _start = _position;
            _kind = SyntaxKind.BadToken;
            _value = null;
            switch (Current) {
                case '\0': {
                        done = true;
                        break;
                    }
                case '/': {
                        if (Lookahead == '/') {
                            ReadSingleLineComment();
                        } else if (Lookahead == '*') {
                            ReadMultiLineComment();
                        } else {
                            done = true;
                        }
                        break;
                    }
                case '\n':
                case '\r': {
                        if (!leading) {
                            done = true;
                        }
                        ReadLineBreak();
                        break;
                    }
                case ' ':
                case '\t': {
                        ReadWhitespace();
                        break;
                    }
                default: {
                        if (char.IsWhiteSpace(Current)) {
                            ReadWhitespace();
                        } else {
                            done = true;
                        }
                        break;
                    }
            }

            var length = _position - _start;
            if (length > 0) {
                var text = _text.ToString(_start, length);
                var trivia = new SyntaxTrivia(_syntaxTree, _kind, _start, text);
                _triviaBuilder.Add(trivia);
            }
        }
    }

    private void ReadWhitespace() {
        var done = false;
        while (!done) {
            switch (Current) {
                case '\0':
                case '\r':
                case '\n': {
                        done = true;
                        break;
                    }
                default: {
                        if (!char.IsWhiteSpace(Current)) {
                            done = true;
                        } else {
                            _position++;
                        }
                        break;
                    }
            }
        }

        _kind = SyntaxKind.WhitespaceTrivia;
    }

    private void ReadLineBreak() {
        if (Current == '\r' && Lookahead == '\n') {
            _position += 2;
        } else {
            _position++;
        }

        _kind = SyntaxKind.LineBreakTrivia;
    }

    private void ReadSingleLineComment() {
        _position += 2;
        var done = false;

        while (!done) {
            switch (Current) {
                case '\0':
                case '\r':
                case '\n': {
                        done = true;
                        break;
                    }
                default: {
                        _position++;
                        break;
                    }
            }
        }

        _kind = SyntaxKind.SingleLineCommentTrivia;
    }

    private void ReadMultiLineComment() {
        _position += 2;
        var done = false;

        while (!done) {
            switch (Current) {
                case '\0': {
                        var span = new TextSpan(_start, 2);
                        var location = new TextLocation(_text, span);
                        _diagnostics.ReportUnterminatedMultiLineComment(location);
                        done = true;
                        break;
                    }
                case '*': {
                        if (Lookahead == '/') {
                            _position++;
                            done = true;
                        }
                        _position++;
                        break;
                    }
                default: {
                        _position++;
                        break;
                    }
            }
        }

        _kind = SyntaxKind.MultiLineCommentTrivia;
    }

    private void ReadString() {
        _position++;

        var sb = new StringBuilder();
        var done = false;

        while (!done) {
            switch (Current) {
                case '\0':
                case '\r':
                case '\n': {
                        var span = new TextSpan(_start, 1);
                        var location = new TextLocation(_text, span);
                        _diagnostics.ReportUnterminatedString(location);
                        done = true;
                        break;
                    }
                case '"': {
                        if (Lookahead == '"') {
                            sb.Append(Current);
                            _position += 2;
                        } else {
                            _position++;
                            done = true;
                        }
                        break;
                    }
                default: {
                        sb.Append(Current);
                        _position++;
                        break;
                    }
            }
        }

        _kind = SyntaxKind.StringToken;
        _value = sb.ToString();
    }
    private void ReadNumberToken() {
        var isFloat = false;
        var hasF = false;
        object value;

        while (char.IsDigit(Current)) {
            _position++;
        }

        if (Current == '.' && char.IsDigit(Lookahead)) {
            isFloat = true;
            _position++;
            while (char.IsDigit(Current)) {
                _position++;
            }
        }

        if (Current == 'f' || Current == 'F') {
            hasF = true;
            _position++;
        }

        var length = _position - _start;
        var text = _text.ToString(_start, length);
        if (isFloat || hasF) {
            if (hasF) {
                text = text[..^1];
            }
            if (!float.TryParse(text, out var floatValue)) {
                var span = new TextSpan(_start, length);
                var location = new TextLocation(_text, span);
                _diagnostics.ReportInvalidNumber(location, text, TypeSymbol.Float);
            }
            value = floatValue;
        } else {
            if (!int.TryParse(text, out var intValue)) {
                var span = new TextSpan(_start, length);
                var location = new TextLocation(_text, span);
                _diagnostics.ReportInvalidNumber(location, text, TypeSymbol.Int);
            }
            value = intValue;
        }
        _value = value;
        _kind = SyntaxKind.NumberToken;
    }

    private void ReadIdentifierOrKeyword() {
        while (char.IsLetterOrDigit(Current) || Current == '_') {
            _position++;
        }

        var length = _position - _start;
        var text = _text.ToString(_start, length);
        _kind = SyntaxFacts.GetKeywordKind(text);
    }
}

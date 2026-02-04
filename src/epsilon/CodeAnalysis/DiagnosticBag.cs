using System.Collections;
using System.Collections.Immutable;
using System.Text;
using epsilon.CodeAnalysis.Binding;
using epsilon.CodeAnalysis.Symbols;
using epsilon.CodeAnalysis.Syntax;
using epsilon.CodeAnalysis.Text;
using Mono.Cecil;

namespace epsilon.CodeAnalysis;

internal sealed class DiagnosticBag : IEnumerable<Diagnostic> {
    private readonly List<Diagnostic> _diagnostics = new List<Diagnostic>();

    public IEnumerator<Diagnostic> GetEnumerator() => _diagnostics.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void AddRange(IEnumerable<Diagnostic> diagnostics) {
        _diagnostics.AddRange(diagnostics);
    }

    private void ReportError(TextLocation location, string message) {
        var diagnostic = Diagnostic.Error(location, message);
        _diagnostics.Add(diagnostic);
    }

    private void ReportWarning(TextLocation location, string message) {
        var diagnostic = Diagnostic.Warning(location, message);
        _diagnostics.Add(diagnostic);
    }

    public void ReportInvalidNumber(TextLocation location, string text, TypeSymbol type) {
        var message = $"The number {text} isn't valid {type}.";
        ReportError(location, message);
    }

    public void ReportBadCharacter(TextLocation location, char character) {
        var message = $"Bad character input: '{character}'.";
        ReportError(location, message);
    }

    public void ReportBadInput(TextLocation location, string text) {
        var message = $"Bad input: \"{text}\".";
        ReportError(location, message);
    }

    public void ReportUnterminatedString(TextLocation location) {
        var message = $"Unterminated string literal.";
        ReportError(location, message);
    }

    public void ReportInvalidEscapeCharacter(TextLocation location) {
        var message = $"Invalid escape character.";
        ReportError(location, message);
    }

    public void ReportOutUnicodeRange(TextLocation location, int unicode) {
        var message = $"U{unicode} is out of Unicode's range. (0x0 ~ 0xFFFF)";
        ReportError(location, message);
    }

    public void ReportUnterminatedMultiLineComment(TextLocation location) {
        var message = $"Unterminated multi-line comment.";
        ReportError(location, message);
    }

    public void ReportUnexpectedToken(TextLocation location, SyntaxKind actualKind, SyntaxKind expectedKind) {
        var message = $"Unexpected token <{actualKind}>, expected <{expectedKind}>.";
        ReportError(location, message);
    }

    public void ReportUndefinedUnaryOperator(TextLocation location, string operatorText, TypeSymbol operandKind) {
        var message = $"Unary operator '{operatorText}' is not defined for type '{operandKind}'.";
        ReportError(location, message);
    }

    public void ReportUndefinedBinaryOperator(TextLocation location, string operatorText, TypeSymbol leftType, TypeSymbol rightType) {
        var message = $"Binary operator '{operatorText}' is not defined for types '{leftType}' and '{rightType}'.";
        ReportError(location, message);
    }

    public void ReportParameterAlreadyDeclared(TextLocation location, string parameterName) {
        var message = $"A parameter with the name '{parameterName}' already exists.";
        ReportError(location, message);
    }

    public void ReportUndefinedVariable(TextLocation location, string name) {
        var message = $"Variable '{name}' doesn't exist.";
        ReportError(location, message);
    }

    public void ReportUndefinedType(TextLocation location, string name) {
        var message = $"Type '{name}' doesn't exist.";
        ReportError(location, message);
    }

    public void ReportCannotConvert(TextLocation location, TypeSymbol fromType, TypeSymbol toType) {
        var message = $"Cannot convert type '{fromType}' to '{toType}'.";
        ReportError(location, message);
    }

    public void ReportCannotConvertImplicitly(TextLocation location, TypeSymbol fromType, TypeSymbol toType) {
        var message = $"Cannot convert type '{fromType}' to '{toType}'. An explicit conversion exits. (are you missing a cast?)";
        ReportError(location, message);

    }

    public void ReportVariableAlreadyDeclared(TextLocation location, VariableSymbol symbol) {
        var message = $"Variable '{symbol.Name}' is already declared.";
        ReportError(location, message);
    }

    public void ReportFunctionAlreadyDeclared(TextLocation location, FunctionSymbol symbol) {
        var message = $"Function '{symbol}' is already declared.";
        ReportError(location, message);
    }

    public void ReportCannotAssign(TextLocation location, string name) {
        var message = $"Variable '{name}' is read-only and cannot be assigned to.";
        ReportError(location, message);
    }

    public void ReportUndefinedFunction(TextLocation location, string name, ImmutableArray<BoundExpression> arguments) {
        var message = $"Function '{name}({string.Join(", ", arguments.Select(a => a.Type.Name))})' doesn't exist.";
        ReportError(location, message);
    }

    public void ReportNoMatchingOverloadFunction(TextLocation location, string name, IEnumerable<FunctionSymbol> functions) {
        var message = new StringBuilder();
        message.Append($"Cannot find a matching overload for {name} in the following function: ");
        foreach (var function in functions) {
            message.AppendLine();
            message.Append($"    {function.ToString()}");
        }
        message.AppendLine();
        ReportError(location, message.ToString());
    }

    public void ReportExpressionMustHaveValue(TextLocation location) {
        var message = "Expression must have a value.";
        ReportError(location, message);
    }

    public void ReportInvalidBreakOrContinue(TextLocation location, string text) {
        var message = $"The keyword '{text}' can only be used inside of loops.";
        ReportError(location, message);
    }

    public void ReportAllPathsMustReturn(TextLocation location) {
        var message = $"Not all code paths return a value.";
        ReportError(location, message);
    }

    public void ReportInvalidReturnExpression(TextLocation location, string functionName) {
        var message = $"Since the function '{functionName}' does not return a value the 'return' keyword cannot be followed by an expression.";
        ReportError(location, message);
    }

    public void ReportInvalidReturnWithValueInGlobalStatements(TextLocation location) {
        var message = "The 'return' keyword cannot be followed by an expression in global statements.";
        ReportError(location, message);
    }

    public void ReportMissingReturnExpression(TextLocation location, TypeSymbol returnType) {
        var message = $"An expression of type '{returnType}' expected.";
        ReportError(location, message);
    }

    public void ReportInvalidExpressionStatement(TextLocation location) {
        var message = $"Only assignment and call expressions can be used as a statement.";
        ReportError(location, message);
    }

    public void ReportCannotMixMainAndGlobalStatements(TextLocation location) {
        var message = $"Cannot declare main function when global statements are used.";
        ReportError(location, message);
    }

    public void ReportMainMustHaveCorrectSignature(TextLocation location) {
        var message = $"Main must not take arguments and not return anything.";
        ReportError(location, message);
    }

    public void ReportOnlyOneFileCanHaveGlobalStatements(TextLocation location) {
        var message = $"At most one file can have global statements.";
        ReportError(location, message);
    }

    public void ReportInvalidReference(string path) {
        var message = $"The reference is not a valid .NET assembly: '{path}'.";
        ReportError(default, message);
    }

    public void ReportRequiredTypeNotFound(string? epsilonName, string metadataName) {
        var message = epsilonName == null
            ? $"The built-in type '{metadataName}' cannot be resolve among the given references."
            : $"The built-in type '{epsilonName}' ('{metadataName}') cannot be resolve among the given references.";
        ReportError(default, message);
    }

    public void ReportRequiredTypeAmbiguous(string? epsilonName, string metadataName, TypeDefinition[] foundTypes) {
        var assemblyNames = foundTypes.Select(t => t.Module.Assembly.Name.Name);
        var assemblyNameList = string.Join(", ", assemblyNames);
        var message = epsilonName == null
            ? $"The required type '{metadataName}' was found in multiple references: {assemblyNameList}."
            : $"The required type '{epsilonName}' ('{metadataName}') was found in multiple references: {assemblyNameList}.";
        ReportError(default, message);
    }

    public void ReportRequiredMethodNotFound(string typeName, string methodName, string[] parameterTypeNames) {
        var parameterTypeNameList = string.Join(", ", parameterTypeNames);
        var message = $"The required method '{typeName}.{methodName}({parameterTypeNameList})' cannot be resolved among the given references.";
        ReportError(default, message);
    }

    public void ReportUnreachableCode(TextLocation location) {
        var message = $"Unreachable code detected.";
        ReportWarning(location, message);
    }

    public void ReportUnreachableCode(SyntaxNode node) {
        switch (node.Kind) {
            case SyntaxKind.BlockStatement: {
                    var firstStatement = ((BlockStatementSyntax)node).Statements.FirstOrDefault();
                    // Report just for non empty blocks.
                    if (firstStatement != null) {
                        ReportUnreachableCode(firstStatement);
                    }
                    return;
                }
            case SyntaxKind.VariableDeclarationStatement: {
                    ReportUnreachableCode(((VariableDeclarationSyntax)node).Keyword.Location);
                    return;
                }
            case SyntaxKind.IfStatement: {
                    ReportUnreachableCode(((IfStatementSyntax)node).IfKeyword.Location);
                    return;
                }
            case SyntaxKind.WhileStatement: {
                    ReportUnreachableCode(((WhileStatementSyntax)node).WhileKeyword.Location);
                    return;
                }
            case SyntaxKind.DoWhileStatement: {
                    ReportUnreachableCode(((DoWhileStatementSyntax)node).DoKeyword.Location);
                    return;
                }
            case SyntaxKind.ForStatement: {
                    ReportUnreachableCode(((ForStatementSyntax)node).Keyword.Location);
                    return;
                }
            case SyntaxKind.BreakStatement: {
                    ReportUnreachableCode(((BreakStatementSyntax)node).Keyword.Location);
                    return;
                }
            case SyntaxKind.ContinueStatement: {
                    ReportUnreachableCode(((ContinueStatementSyntax)node).Keyword.Location);
                    return;
                }
            case SyntaxKind.ReturnStatement: {
                    ReportUnreachableCode(((ReturnStatementSyntax)node).ReturnKeyword.Location);
                    return;
                }
            case SyntaxKind.ExpressionStatement: {
                    var expression = ((ExpressionStatementSyntax)node).Expression;
                    ReportUnreachableCode(expression);
                    return;
                }
            case SyntaxKind.CallExpression: {
                    ReportUnreachableCode(((CallExpressionSyntax)node).Identifier.Location);
                    return;
                }
            default: {
                    ReportUnreachableCode(node.Location);
                    break;
                }
        }
    }

    public void ReportCannotDeriveType(TextLocation location) {
        var message = $"Unable to deduce type.";
        ReportError(location, message);
    }

    public void ReportCannotDeriveValue(TextLocation location) {
        var message = $"Unable to deduce value.";
        ReportError(location, message);
    }
}
using System.Collections;
using epsilon.CodeAnalysis.Symbols;
using epsilon.CodeAnalysis.Syntax;
using epsilon.CodeAnalysis.Text;

namespace epsilon.CodeAnalysis;

internal sealed class DiagnosticBag : IEnumerable<Diagnostic> {
    private readonly List<Diagnostic> _diagnostics = new List<Diagnostic>();

    public IEnumerator<Diagnostic> GetEnumerator() => _diagnostics.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void AddRange(DiagnosticBag diagnostics){
        _diagnostics.AddRange(diagnostics._diagnostics);
    }

    private void Report(TextSpan span, string message){
        var diagnostic = new Diagnostic(span, message);
        _diagnostics.Add(diagnostic);
    }

    public void ReportInvalidNumber(TextSpan span, string text, TypeSymbol type){
        var message = $"The number {text} isn't valid {type}.";
        Report(span, message);
    }

    public void ReportBadCharacter(int position, char character){
        var span = new TextSpan(position, 1);
        var message = $"Bad character input: '{character}'.";
        Report(span, message);
    }

    public void ReportUnterminatedString(TextSpan span){
        var message = $"Unterminated string literal.";
        Report(span, message);
    }

    public void ReportUnexpectedToken(TextSpan span, SyntaxKind actualKind, SyntaxKind expectedKind){
        var message = $"Unexpected token <{actualKind}>, expected <{expectedKind}>.";
        Report(span, message);
    }

    public void ReportUndefinedUnaryOperator(TextSpan span, string operatorText, TypeSymbol operandKind){
        var message = $"Unary operator '{operatorText}' is not defined for type '{operandKind}'.";
        Report(span, message);
    }

    public void ReportBindefinedBinaryOperator(TextSpan span, string operatorText, TypeSymbol leftType, TypeSymbol rightType){
        var message = $"Binary operator '{operatorText}' is not defined for types '{leftType}' and '{rightType}'.";
        Report(span, message);
    }

    public void ReportParameterAlreadyDeclared(TextSpan span, string parameterName){
        var message = $"A parameter with the name '{parameterName}' already exists.";
        Report(span, message);
    }

    public void ReportUndefinedName(TextSpan span, string name){
        var message = $"Variable '{name}' doesn't exist.";
        Report(span, message);
    }

    public void ReportUndefinedType(TextSpan span, string name){
        var message = $"Type '{name}' doesn't exist.";
        Report(span, message);
    }

    public void ReportCannotConvert(TextSpan span, TypeSymbol fromType, TypeSymbol toType){
        var message = $"Cannot convert type '{fromType}' to '{toType}'.";
        Report(span, message);
    }

    public void ReportCannotConvertImplicitly(TextSpan span, TypeSymbol fromType, TypeSymbol toType){
        var message = $"Cannot convert type '{fromType}' to '{toType}'. An explicit conversion exits (are you missing a cast?)";
        Report(span, message); 
        
    }

    public void ReportSymbolAlreadyDeclared(TextSpan span, string name){
        var message = $"'{name}' is already declared.";
        Report(span, message);
    }

    public void ReportCannotAssign(TextSpan span, string name){
        var message = $"Variable '{name}' is read-only and cannot be assigned to.";
        Report(span, message);
    }

    public void ReportUndefinedFunction(TextSpan span, string name){
        var message = $"Function '{name}' doesn't exist.";
        Report(span, message);
    }

    public void ReportWrongArgumentCount(TextSpan span, string name, int expectedCount, int actualCount){
        var message = $"Function '{name}' requires {expectedCount} arguments but was given {actualCount}.";
        Report(span, message);
    }

    public void ReportWrongArgumentType(TextSpan span, string name, TypeSymbol expectedType, TypeSymbol actualType){
        var message = $"Parameter '{name}' requires a value of type '{expectedType}' but was given a value of type '{actualType}'.";
        Report(span, message);
    }

    public void ReportExpressionMustHaveValue(TextSpan span){
        var message = "Expression must have a value.";
        Report(span, message);
    }

    public void ReportInvalidBreakOrContinue(TextSpan span, string text){
        var message = $"The keyword '{text}' can only be used inside of loops.";
        Report(span, message);
    }

    public void XXX_ReportFunctionAreUnsupported(TextSpan span){
        var message = "Functions with return values are unsupported.";
        Report(span, message);
    }
}
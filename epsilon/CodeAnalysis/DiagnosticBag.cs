using System.Collections;

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

    public void ReportInvaildNumber(TextSpan span, string text, Type type){
        var message = $"The number {text} isn't vaild {type}.";
        Report(span, message);
    }

    public void ReportBadCharacter(int position, char character){
        var span = new TextSpan(position, 1);
        var message = $"Bad character input: '{character}'.";
        Report(span, message);
    }

    public void ReportUnexpectedToken(TextSpan span, SyntaxKind actualKind, SyntaxKind expectedKind){
        var message = $"Unexpected token <{actualKind}>, expected <{expectedKind}>.";
        Report(span, message);
    }

    public void ReportUndefinedUnaryOperator(TextSpan span, string operatorText, Type operandKind){
        var message = $"Unary operator '{operatorText}' is not defined for type {operandKind}.";
        Report(span, message);
    }

    public void ReportBindefinedUnaryOperator(TextSpan span, string operatorText, Type leftType, Type rightType){
        var message = $"Binary operator '{operatorText}' is not defined for type {leftType} and {rightType}.";
        Report(span, message);
    }

    public void ReportUndefinedName(TextSpan span, string name){
        var message = $"Variable '{name}' doesn't exist.";
        Report(span, message);
    }

    public void ReportCannotConvert(TextSpan span, Type fromType, Type toType){
        var message = $"Cannot convert type '{fromType}' to '{toType}'.";
        Report(span, message);
    }
}
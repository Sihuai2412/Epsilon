namespace epsilon.CodeAnalysis;

using System.Collections.Immutable;

public sealed class EvaluationResult {
    public EvaluationResult(ImmutableArray<Diagnostic> diagnostics, object value){
        Diagnostics = diagnostics;
        Value = value;
    }

    public ImmutableArray<Diagnostic> Diagnostics { get; }
    public object Value { get; }
}

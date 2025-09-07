using System.Collections.Immutable;

namespace epsilon.CodeAnalysis;

public static class DiagnosticExtensions {
    public static bool HasErrors(this ImmutableArray<Diagnostic> diagnostics) {
        return diagnostics.Any(d => d.IsError);
    }

    public static bool HasErrors(this IEnumerable<Diagnostic> diagnostics) {
        return diagnostics.Any(d => d.IsError);
    }
}
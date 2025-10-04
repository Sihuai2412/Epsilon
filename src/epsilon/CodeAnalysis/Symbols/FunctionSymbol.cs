using System.Collections.Immutable;
using System.Text;
using epsilon.CodeAnalysis.Syntax;

namespace epsilon.CodeAnalysis.Symbols;

public sealed class FunctionSymbol : Symbol {
    public FunctionSymbol(string name, ImmutableArray<ParameterSymbol> parameters, TypeSymbol type, FunctionDeclarationSyntax? declaration = null) : base(name) {
        Parameters = parameters;
        Type = type;
        Declaration = declaration;
    }

    public override SymbolKind Kind => SymbolKind.Function;
    public FunctionDeclarationSyntax? Declaration { get; }
    public ImmutableArray<ParameterSymbol> Parameters { get; }
    public TypeSymbol Type { get; }

    public override string ToString() {
        var function = new StringBuilder();
        function.Append(Name);
        function.Append('(');
        for (int i = 0; i < Parameters.Length; i++) {
            var parameter = Parameters[i];
            function.Append(parameter.Type.Name);
            if (i != Parameters.Length - 1) {
                function.Append(", ");
            }
        }
        function.Append(')');

        return function.ToString();
    }
}
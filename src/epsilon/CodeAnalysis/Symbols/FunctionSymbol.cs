using System.Collections.Immutable;
using epsilon.CodeAnalysis.Syntax;

namespace epsilon.CodeAnalysis.Symbols;

public sealed class FunctionSymbol : Symbol {
    public FunctionSymbol(string name, ImmutableArray<ParameterSymbol> parameters, TypeSymbol type, FunctionDeclarationSyntax declaration = null) : base(name) {
        Parameters = parameters;
        Type = type;
        Declaration = declaration;
    }

    public override SymbolKind Kind => SymbolKind.Function;
    public FunctionDeclarationSyntax Declaration { get; }
    public ImmutableArray<ParameterSymbol> Parameters { get; }
    public TypeSymbol Type { get; }
}
using epsilon.CodeAnalysis.Symbols;

namespace epsilon.CodeAnalysis.Binding;

internal sealed class SymbolKey {
    public SymbolKey(string name, SymbolKeyType type, IEnumerable<SymbolKey>? parameters = null) {
        Name = name;
        Type = type;
        Parameters = (parameters ?? Array.Empty<SymbolKey>()).ToArray();
    }

    public string Name { get; }
    public SymbolKeyType Type { get; }
    public SymbolKey[] Parameters { get; }

    public static SymbolKey Create(Symbol symbol) {
        return symbol switch {
            VariableSymbol variable => new SymbolKey(variable.Name, SymbolKeyType.Variable),
            FunctionSymbol function => new SymbolKey(
                function.Name, SymbolKeyType.Function,
                function.Parameters.Select(
                    p => new SymbolKey(p.Type?.Name ?? "?", SymbolKeyType.Type)
                )
            ),
            _ => throw new Exception($"Unexpected symbol type: {symbol.GetType()}")
        };
    }

    public override bool Equals(object? obj) {
        if (obj is not SymbolKey other) {
            return false;
        }

        if (Name != other.Name || Type != other.Type) {
            return false;
        }

        if (Parameters.Length != other.Parameters.Length) {
            return false;
        }

        return Parameters.Zip(other.Parameters, (a, b) => a.Equals(b)).All(x => x);
    }

    public override int GetHashCode() {
        var hash = new HashCode();
        hash.Add(Name);
        hash.Add(Type);
        hash.Add(Parameters.Length);

        foreach (var p in Parameters) {
            hash.Add(GetRecursiveHash(p));
        }

        return hash.ToHashCode();
    }

    private static int GetRecursiveHash(SymbolKey key) {
        var h = new HashCode();
        h.Add(key.Name);
        h.Add(key.Type);
        foreach (var p in key.Parameters) {
            h.Add(GetRecursiveHash(p));
        }
        return h.ToHashCode();
    }
}

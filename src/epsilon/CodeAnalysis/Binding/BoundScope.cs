using System.Collections.Immutable;
using epsilon.CodeAnalysis.Symbols;

namespace epsilon.CodeAnalysis.Binding;

internal sealed class BoundScope {
    private Dictionary<SymbolKey, Symbol>? _symbols;

    public BoundScope(BoundScope? parent) {
        Parent = parent;
    }

    public BoundScope? Parent { get; }

    public bool TryDeclareVariable(VariableSymbol variable) => TryDeclareSymbol(variable);

    public bool TryDeclareFunction(FunctionSymbol function) => TryDeclareSymbol(function);

    private bool TryDeclareSymbol<TSymbol>(TSymbol symbol) where TSymbol : Symbol {
        var key = SymbolKey.Create(symbol);
        if (_symbols == null) {
            _symbols = [];
        } else if (_symbols.ContainsKey(key)) {
            return false;
        }
        _symbols.Add(key, symbol);
        return true;
    }

    public VariableSymbol? TryLookupVariable(string name) {
        if (_symbols != null && _symbols.TryGetValue(new SymbolKey(name, SymbolKeyType.VARIABLE), out var symbol)) {
            return symbol as VariableSymbol;
        }

        return Parent?.TryLookupVariable(name);
    }

    public IEnumerable<FunctionSymbol>? TryLookupFunction(string name, ImmutableArray<BoundExpression> parameters) {
        if (_symbols != null) {
            var scores = new Dictionary<FunctionSymbol, int>();

            var functions = new List<FunctionSymbol>();

            BoundScope? current = this;
            while (current != null) {
                if (current._symbols != null) {
                    var parentFunctions = current._symbols.Values.OfType<FunctionSymbol>()
                                           .Where(f => f.Name == name);
                    functions.AddRange(parentFunctions);
                }
                current = current.Parent;
            }
            var sameArgumentCountFunctions = functions.Where(
                f => f.Parameters.Length == parameters.Length
            );
            foreach (var function in sameArgumentCountFunctions) {
                var score = 0;
                var mismatch = false;
                for (var i = 0; i < function.Parameters.Length; i++) {
                    var functionParameterType = function.Parameters[i].Type;
                    var parameterType = parameters[i].Type;
                    if (functionParameterType.Name == parameterType.Name) {
                        score += 2;
                    } else if (Conversion.Classify(parameterType, functionParameterType).IsImplicit) {
                        score += 1;
                    } else {
                        mismatch = true;
                        score = -1;
                    }
                }
                if (!mismatch) {
                    scores[function] = score;
                }
            }

            if (scores.Count == 0) {
                return null;
            }

            int maxScore = scores.Values.Max();
            var topFunctions = scores.Where(kv => kv.Value == maxScore)
                                     .Select(kv => kv.Key);

            return topFunctions;
        }

        return Parent?.TryLookupFunction(name, parameters);
    }

    public ImmutableArray<VariableSymbol> GetDeclaredVariables() => GetDeclaredSymbols<VariableSymbol>();

    public ImmutableArray<FunctionSymbol> GetDeclaredFunctions() => GetDeclaredSymbols<FunctionSymbol>();

    private ImmutableArray<TSymbol> GetDeclaredSymbols<TSymbol>() where TSymbol : Symbol {
        if (_symbols == null) {
            return [];
        }

        return _symbols.Values.OfType<TSymbol>().ToImmutableArray();
    }
}


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
            VariableSymbol variable => new SymbolKey(variable.Name, SymbolKeyType.VARIABLE),
            FunctionSymbol function => new SymbolKey(
                function.Name, SymbolKeyType.FUNCTION,
                function.Parameters.Select(
                    p => new SymbolKey(p.Type?.Name ?? "<unknown>", SymbolKeyType.TYPE)
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

public enum SymbolKeyType {
    VARIABLE,
    FUNCTION,
    TYPE
}
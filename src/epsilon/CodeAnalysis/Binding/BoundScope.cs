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

        if (HasKey(key)) {
            return false;
        }

        _symbols ??= [];
        _symbols.Add(key, symbol);
        return true;
    }

    private bool HasKey(SymbolKey key) {
        BoundScope? current = this;
        while (current != null) {
            if (current._symbols != null) {
                if (current._symbols.ContainsKey(key)) {
                    return true;
                }
            }
            current = current.Parent;
        }
        return false;
    }

    public VariableSymbol? TryLookupVariable(string name) {
        if (_symbols != null && _symbols.TryGetValue(new SymbolKey(name, SymbolKeyType.Variable), out var symbol)) {
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
            functions = functions
                .GroupBy(SymbolKey.Create)
                .Select(g => g.First())
                .ToList();

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

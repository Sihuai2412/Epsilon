using System.Collections.Immutable;

public sealed class Compilation {
    private BoundGlobalScope _globalScope;

    public Compilation(SyntaxTree syntaxTree) 
        : this(null, syntaxTree) {
        SyntaxTree = syntaxTree;
    }

    public Compilation(Compilation previous, SyntaxTree syntaxTree){
        Previous = previous;
        SyntaxTree = syntaxTree;
    }

    public Compilation Previous { get; }
    public SyntaxTree SyntaxTree { get; }

    internal BoundGlobalScope GlobalScope {
        get {
            if (_globalScope == null){
                var globalScope = Binder.BindGlobalScope(Previous?.GlobalScope, SyntaxTree.Root);
                Interlocked.CompareExchange(ref _globalScope, globalScope, null);
            }

            return _globalScope;
        }
    }

    public Compilation ContinueWith(SyntaxTree syntaxTree){
        return new Compilation(this, syntaxTree);
    }

    public EvaluationResult Evaluate(Dictionary<VariableSymbol, object> variables){
        var diagnostics = SyntaxTree.Diagnostics.Concat(GlobalScope.Diagnostics).ToImmutableArray();
        if (diagnostics.Any()){
            return new EvaluationResult(diagnostics, null);
        }

        var evaluator = new Evaluator(GlobalScope.Expression, variables);
        var value = evaluator.Evaluate();
        return new EvaluationResult(ImmutableArray<Diagnostic>.Empty, value);
    }
}
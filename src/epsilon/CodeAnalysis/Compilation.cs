using System.Collections.Immutable;
using epsilon.CodeAnalysis.Binding;
using epsilon.CodeAnalysis.Symbols;
using epsilon.CodeAnalysis.Syntax;

namespace epsilon.CodeAnalysis;

public sealed class Compilation {
    private BoundGlobalScope _globalScope;

    public Compilation(SyntaxTree syntaxTree) 
        : this(null, syntaxTree) {
        
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

        var program = Binder.BindProgram(GlobalScope);
        if (program.Diagnostics.Any()){
            return new EvaluationResult(program.Diagnostics.ToImmutableArray(), null);
        }

        var evaluator = new Evaluator(program, variables);
        var value = evaluator.Evaluate();
        return new EvaluationResult(ImmutableArray<Diagnostic>.Empty, value);
    }

    public void EmitTree(TextWriter writer){
        var program = Binder.BindProgram(GlobalScope);
        if (program.Statement.Statements.Any()){
            program.Statement.WriteTo(writer);
        } else {
            foreach (var functionBody in program.Functions){
                if (!GlobalScope.Functions.Contains(functionBody.Key)){
                    continue;
                }

                functionBody.Key.WriteTo(writer);
                functionBody.Value.WriteTo(writer);
            }
        }
    }
}
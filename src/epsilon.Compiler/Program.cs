using epsilon.CodeAnalysis;
using epsilon.CodeAnalysis.Symbols;
using epsilon.CodeAnalysis.Syntax;
using epsilon.IO;

namespace epsilon.Compiler;

internal static class Program {
    private static void Main(string[] args){
        if (args.Length == 0){
            Console.Error.WriteLine("usage: eps <source-paths>");
            return;
        }

        if (args.Length > 1){
            Console.Error.WriteLine("error: only one path supported right now");
            return;
        }

        var path = args.Single();

        var text = File.ReadAllText(path);
        var syntaxTree = SyntaxTree.Parse(text);

        var compilation = new Compilation(syntaxTree);
        var result = compilation.Evaluate(new Dictionary<VariableSymbol, object>());

        if (!result.Diagnostics.Any()){
            if (result.Value != null){
                Console.WriteLine(result.Value);
            }
        } else {
            Console.Error.WriteDiagnostics(result.Diagnostics, syntaxTree);
        }
    }
}
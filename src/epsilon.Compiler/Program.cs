using epsilon.CodeAnalysis;
using epsilon.CodeAnalysis.Symbols;
using epsilon.CodeAnalysis.Syntax;
using epsilon.IO;

namespace epsilon.Compiler;

internal static class Program {
    private static int Main(string[] args){
        if (args.Length == 0){
            Console.Error.WriteLine("usage: epsi <source-paths>");
            return 1;
        }

        var paths = GetFilePaths(args);
        var syntaxTrees = new List<SyntaxTree>();
        var hasErrors = false;

        foreach (var path in paths){
            if (!File.Exists(path)){
                Console.Error.WriteLine($"error: file '{path}' doesn't exist");
                hasErrors = true;
                continue;
            }
            var syntaxTree = SyntaxTree.Load(path);
            syntaxTrees.Add(syntaxTree);
        }

        if (hasErrors){
            return 1;
        }

        var compilation = Compilation.Create(syntaxTrees.ToArray());
        var result = compilation.Evaluate(new Dictionary<VariableSymbol, object>());

        if (!result.Diagnostics.Any()){
            if (result.Value != null){
                Console.WriteLine(result.Value);
            }
        } else {
            Console.Error.WriteDiagnostics(result.Diagnostics);
            return 1;
        }

        return 0;
    }

    private static IEnumerable<string> GetFilePaths(IEnumerable<string> paths){
        var result = new SortedSet<string>();

        foreach (var path in paths){
            if (Directory.Exists(path)){
                result.UnionWith(Directory.EnumerateFiles(path, "*.epsi", SearchOption.AllDirectories));
            } else {
                result.Add(path);
            }
        }

        return result;
    }
}
namespace epsilon.CodeAnalysis.Symbols;

public abstract class Symbol {
    internal Symbol(string name){
        Name = name;
    }
    
    public abstract SymbolKind Kind { get; }
    public string Name { get; }
    public override string ToString() => Name;
}

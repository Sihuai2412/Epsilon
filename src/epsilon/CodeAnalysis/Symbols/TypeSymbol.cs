namespace epsilon.CodeAnalysis.Symbols;

public sealed class TypeSymbol : Symbol {
    internal static List<TypeSymbol> types = [];

    public static readonly TypeSymbol Error = new TypeSymbol("?", new object(), false);
    public static readonly TypeSymbol Any = new TypeSymbol("any", new object());
    public static readonly TypeSymbol Bool = new TypeSymbol("bool", false);
    public static readonly TypeSymbol Int = new TypeSymbol("int", 0);
    public static readonly TypeSymbol Float = new TypeSymbol("float", 0.0f);
    public static readonly TypeSymbol String = new TypeSymbol("string", "");
    public static readonly TypeSymbol Void = new TypeSymbol("void", new object(), false);

    internal TypeSymbol(string name, object defaultValue, bool joinsInList = true) : base(name) {
        if (joinsInList) {
            types.Add(this);
        }
        DefaultValue = defaultValue;
    }

    public override SymbolKind Kind => SymbolKind.Type;
    public object DefaultValue { get; }
}

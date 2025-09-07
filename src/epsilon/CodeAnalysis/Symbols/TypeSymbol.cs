using System.Reflection;

namespace epsilon.CodeAnalysis.Symbols;

public sealed class TypeSymbol : Symbol {
    public static readonly TypeSymbol Error = new TypeSymbol("?");
    public static readonly TypeSymbol Any = new TypeSymbol("any");
    public static readonly TypeSymbol Bool = new TypeSymbol("bool");
    public static readonly TypeSymbol Int = new TypeSymbol("int");
    public static readonly TypeSymbol Float = new TypeSymbol("float");
    public static readonly TypeSymbol String = new TypeSymbol("string");
    public static readonly TypeSymbol Void = new TypeSymbol("void");

    public static IEnumerable<TypeSymbol> allTypes = GetAll();

    internal TypeSymbol(string name) : base(name) {

    }

    public override SymbolKind Kind => SymbolKind.Type;

    internal static IEnumerable<TypeSymbol> GetAll()
        => typeof(TypeSymbol).GetFields(BindingFlags.Public | BindingFlags.Static)
                             .Where(t => t.FieldType == typeof(TypeSymbol))
                             .Select(t => (TypeSymbol)t.GetValue(null)!);
}

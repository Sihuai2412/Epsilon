using epsilon.CodeAnalysis.Binding;

namespace epsilon.CodeAnalysis.Lowering;

internal sealed class Lowerer : BoundTreeRewriter {
    public Lowerer(){

    }

    public static BoundStatement Lower(BoundStatement statement){
        var lowerer = new Lowerer();
        return lowerer.RewriteStatement(statement);
    }
}
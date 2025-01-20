using System.Collections.Immutable;
using epsilon.CodeAnalysis.Binding;
using epsilon.CodeAnalysis.Syntax;

namespace epsilon.CodeAnalysis.Lowering;

internal sealed class Lowerer : BoundTreeRewriter {
    public Lowerer(){

    }

    public static BoundStatement Lower(BoundStatement statement){
        var lowerer = new Lowerer();
        return lowerer.RewriteStatement(statement);
    }

    protected override BoundStatement RewriteForStatement(BoundForStatement node){
        var variableDeclaration = new BoundVariableDeclaration(node.Variable, node.LowerBound);
        var variableExpression = new BoundVariableExpression(node.Variable);
        var condition = new BoundBinaryExpression(
            variableExpression,
            BoundBinaryOperator.Bind(SyntaxKind.LessOrEqualsToken, typeof(int), typeof(int)),
            node.UpperBound
        );
        var increment = new BoundExpressionStatement(
            new BoundAssignmentExpression(
                node.Variable,
                new BoundBinaryExpression(
                    variableExpression,
                    BoundBinaryOperator.Bind(SyntaxKind.PlusToken, typeof(int), typeof(int)),
                    new BoundLiteralExpression(1)
                )
            )
        );
        var whileBody = new BoundBlockStatement(ImmutableArray.Create<BoundStatement>(node.Body, increment));
        var whileStatement = new BoundWhileStatement(condition, whileBody);
        var result = new BoundBlockStatement(ImmutableArray.Create<BoundStatement>(variableDeclaration, whileStatement));
        
        return RewriteStatement(result);
    }
}
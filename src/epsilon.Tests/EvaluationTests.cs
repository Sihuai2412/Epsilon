using epsilon.CodeAnalysis;
using epsilon.CodeAnalysis.Symbols;
using epsilon.CodeAnalysis.Syntax;

namespace epsilon.Tests.CodeAnalysis;

public class EvaluationTests {
    [Theory]
    [InlineData("1;", 1)]
    [InlineData("+1;", 1)]
    [InlineData("-1;", -1)]
    [InlineData("~1;", -2)]
    [InlineData("14 + 12;", 26)]
    [InlineData("12 - 3;", 9)]
    [InlineData("4 * 2;", 8)]
    [InlineData("9 / 3;", 3)]
    [InlineData("1.0;", 1.0f)]
    [InlineData("+1.0;", 1.0f)]
    [InlineData("-1.0;", -1.0f)]
    [InlineData("(10);", 10)]
    [InlineData("12 == 3;", false)]
    [InlineData("3 == 3;", true)]
    [InlineData("12 != 3;", true)]
    [InlineData("3 != 3;", false)]
    [InlineData("3 < 4;", true)]
    [InlineData("5 < 4;", false)]
    [InlineData("4 <= 4;", true)]
    [InlineData("4 <= 5;", true)]
    [InlineData("5 <= 4;", false)]
    [InlineData("4 > 3;", true)]
    [InlineData("4 > 5;", false)]
    [InlineData("4 >= 4;", true)]
    [InlineData("5 >= 4;", true)]
    [InlineData("4 >= 5;", false)]
    [InlineData("1 | 2;", 3)]
    [InlineData("1 | 0;", 1)]
    [InlineData("1 & 3;", 1)]
    [InlineData("1 & 0;", 0)]
    [InlineData("1 ^ 0;", 1)]
    [InlineData("0 ^ 1;", 1)]
    [InlineData("1 ^ 3;", 2)]
    [InlineData("false == false;", true)]
    [InlineData("true == false;", false)]
    [InlineData("false != false;", false)]
    [InlineData("true != false;", true)]
    [InlineData("true && true;", true)]
    [InlineData("false || false;", false)]
    [InlineData("false | false;", false)]
    [InlineData("false | true;", true)]
    [InlineData("true | false;", true)]
    [InlineData("true | true;", true)]
    [InlineData("false & false;", false)]
    [InlineData("false & true;", false)]
    [InlineData("true & false;", false)]
    [InlineData("true & true;", true)]
    [InlineData("false ^ false;", false)]
    [InlineData("true ^ false;", true)]
    [InlineData("false ^ true;", true)]
    [InlineData("true ^ true;", false)]
    [InlineData("true;", true)]
    [InlineData("false;", false)]
    [InlineData("!true;", false)]
    [InlineData("!false;", true)]
    [InlineData("var a = 10; return a;", 10)]
    [InlineData("\"test\";", "test")]
    [InlineData("\"te\"\"st\";", "te\"st")]
    [InlineData("\"test\" == \"test\";", true)]
    [InlineData("\"test\" != \"test\";", false)]
    [InlineData("\"test\" == \"abc\";", false)]
    [InlineData("\"test\" != \"abc\";", true)]
    [InlineData("\"test\" + \"abc\";", "testabc")]
    [InlineData("{ var a as any = 0; var b as any = \"b\"; return a == b; }", false)]
    [InlineData("{ var a as any = 0; var b as any = \"b\"; return a != b; }", true)]
    [InlineData("{ var a as any = 0; var b as any = 0; return a == b; }", true)]
    [InlineData("{ var a as any = 0; var b as any = 0; return a != b; }", false)]
    [InlineData("{ var a = 7; return a is bool; }", false)]
    [InlineData("{ var a = 7; return a is int; }", true)]
    [InlineData("{ var a = 7; return a is any; }", true)]
    [InlineData("{ var a = false; return a is bool; }", true)]
    [InlineData("{ var a = 8.2; return a is float; }", true)]
    [InlineData("{ var a = \"hello\"; return a is string; }", true)]
    [InlineData("{ var a = 10; return a * a; }", 100)]
    [InlineData("{ var a = 0; return (a = 10) * a; }", 100)]
    [InlineData("{ var a = 0; if a == 0 a = 10; return a; }", 10)]
    [InlineData("{ var a = 0; if a == 4 a = 10; return a; }", 0)]
    [InlineData("{ var a = 0; if a == 0 a = 10; else a = 5; return a; }", 10)]
    [InlineData("{ var a = 0; if a == 4 a = 10; else a = 5; return a; }", 5)]
    [InlineData("{ var i = 10; var result = 0; while i > 0 { result = result + i; i = i - 1;} return result; }", 55)]
    [InlineData("{ var result = 0; for i = 1 to 10 { result = result + i; } return result; }", 55)]
    [InlineData("{ var a = 10; for i = 1 to (a = a - 1) { } return a; }", 9)]
    [InlineData("{ var a = 0; do a = a + 1; while a < 10; return a;}", 10)]
    [InlineData("{ var i = 0; while i < 5 { i = i + 1; if i == 5 continue; } return i; }", 5)]
    [InlineData("{ var i = 0; do { i = i + 1; if i == 5 continue; } while i < 5; return i; }", 5)]
    [InlineData("{ var a = 1; a += (2 + 3); return a; }", 6)]
    [InlineData("{ var a = 1; a -= (2 + 3); return a; }", -4)]
    [InlineData("{ var a = 1; a *= (2 + 3); return a; }", 5)]
    [InlineData("{ var a = 1; a /= (2 + 3); return a; }", 0)]
    [InlineData("{ var a = true; a &= (false); return a; }", false)]
    [InlineData("{ var a = true; a |= (false); return a; }", true)]
    [InlineData("{ var a = true; a ^= (true); return a; }", false)]
    [InlineData("{ var a = 1; a |= 0; return a; }", 1)]
    [InlineData("{ var a = 1; a &= 3; return a; }", 1)]
    [InlineData("{ var a = 1; a &= 0; return a; }", 0)]
    [InlineData("{ var a = 1; a ^= 0; return a; }", 1)]
    [InlineData("{ var a = 1; var b = 2; var c = 3; a += b += c; return a; }", 6)]
    [InlineData("{ var a = 1; var b = 2; var c = 3; a += b += c; return b; }", 5)]
    public void Evaluator_Computes_CorrectValues(string text, object expectedValue) {
        AssertValue(text, expectedValue);
    }

    [Fact]
    public void Evaluator_VariableDeclaration_Reports_Redeclaration() {
        var text = @"
            {
                var x = 10;
                var y = 100;
                {
                    var x = 10;
                }
                var [x] = 5;
            }
        ";

        var diagnostics = @"
            'x' is already declared.
        ";

        AssertDiagnostics(text, diagnostics);
    }

    [Fact]
    public void Evaluator_BlockStatement_NoInfiniteLoop() {
        var text = @"
            {
            )[[]]
        ";

        var diagnostics = @"
            Unexpected token <EndOfFileToken>, expected <SemicolonToken>.
            Unexpected token <EndOfFileToken>, expected <CloseBraceToken>.
        ";

        AssertDiagnostics(text, diagnostics);
    }

    [Fact]
    public void Evaluator_InvokeFunctionArguments_Missing() {
        var text = @"
            print([)];
        ";

        var diagnostics = @"
            Function 'print' requires 1 arguments but was given 0.
        ";

        AssertDiagnostics(text, diagnostics);
    }

    [Fact]
    public void Evaluator_InvokeFunctionArguments_Exceeding() {
        var text = @"
            print(""Hello""[, "" "", "" world!""]);
        ";

        var diagnostics = @"
            Function 'print' requires 1 arguments but was given 3.
        ";

        AssertDiagnostics(text, diagnostics);
    }

    [Fact]
    public void Evaluator_InvokeFunctionArguments_NoInfiniteLoop() {
        var text = @"
            print(""Hi""[[=]][)];
        ";

        var diagnostics = @"
            Unexpected token <EqualsToken>, expected <CloseParenthesisToken>.
            Unexpected token <EqualsToken>, expected <SemicolonToken>.
            Unexpected token <CloseParenthesisToken>, expected <SemicolonToken>.
        ";

        AssertDiagnostics(text, diagnostics);
    }

    [Fact]
    public void Evaluator_FunctionParameters_NoInfiniteLoop() {
        var text = @"
            fun hi(name as string[[=]][)][{]
                print(""Hi "" + name + ""!"" );
            }[]
        ";

        var diagnostics = @"
            Unexpected token <EqualsToken>, expected <CloseParenthesisToken>.
            Unexpected token <EqualsToken>, expected <OpenBraceToken>.
            Unexpected token <CloseParenthesisToken>, expected <SemicolonToken>.
            Unexpected token <OpenBraceToken>, expected <SemicolonToken>.
            Unexpected token <EndOfFileToken>, expected <CloseBraceToken>.
        ";

        AssertDiagnostics(text, diagnostics);
    }

    [Fact]
    public void Evaluator_FunctionReturn_Missing() {
        var text = @"
            fun [add](a as int, b as int) as int{
            }
        ";

        var diagnostics = @"
            Not all code paths return a value.
        ";

        AssertDiagnostics(text, diagnostics);
    }

    [Fact]
    public void Evaluator_IfStatement_Reports_CannotConvert() {
        var text = @"
            {
                var x = 0;
                if [10]
                    x = 10;
            }
        ";

        var diagnostics = @"
            Cannot convert type 'int' to 'bool'.
        ";

        AssertDiagnostics(text, diagnostics);
    }

    [Fact]
    public void Evaluator_WhileStatement_Reports_CannotConvert() {
        var text = @"
            {
                var x = 0;
                while [10]
                    x = 10;
            }
        ";

        var diagnostics = @"
            Cannot convert type 'int' to 'bool'.
        ";

        AssertDiagnostics(text, diagnostics);
    }

    [Fact]
    public void Evaluator_DoWhileStatement_Reports_CannotConvert() {
        var text = @"
            {
                var x = 0;
                do
                    x = 10;
                while [10];
            }
        ";

        var diagnostics = @"
            Cannot convert type 'int' to 'bool'.
        ";

        AssertDiagnostics(text, diagnostics);
    }

    [Fact]
    public void Evaluator_ForStatement_Reports_CannotConvert_LowerBound() {
        var text = @"
            {
                var result = 0;
                for i = [false] to 10
                    result = result + i;
            }
        ";

        var diagnostics = @"
            Cannot convert type 'bool' to 'int'.
        ";

        AssertDiagnostics(text, diagnostics);
    }

    [Fact]
    public void Evaluator_ForStatement_Reports_CannotConvert_UpperBound() {
        var text = @"
            {
                var result = 0;
                for i = 1 to [true]
                    result = result + i;
            }
        ";

        var diagnostics = @"
            Cannot convert type 'bool' to 'int'.
        ";

        AssertDiagnostics(text, diagnostics);
    }

    [Fact]
    public void Evaluator_NameExpression_Reports_Undefined() {
        var text = @"[x] * 10;";

        var diagnostics = @"
            Variable 'x' doesn't exist.
        ";

        AssertDiagnostics(text, diagnostics);
    }

    [Fact]
    public void Evaluator_NameExpression_Reports_NoErrorForInsertedToken() {
        var text = @"1 + [[]]";

        var diagnostics = @"
            Unexpected token <EndOfFileToken>, expected <IdentifierToken>.
            Unexpected token <EndOfFileToken>, expected <SemicolonToken>.
        ";

        AssertDiagnostics(text, diagnostics);
    }

    [Fact]
    public void Evaluator_UnaryExpression_Reports_Undefined() {
        var text = @"[+]true;";

        var diagnostics = @"
            Unary operator '+' is not defined for type 'bool'.
        ";

        AssertDiagnostics(text, diagnostics);
    }

    [Fact]
    public void Evaluator_BinaryExpression_Reports_Undefined() {
        var text = @"10 [*] false;";

        var diagnostics = @"
            Binary operator '*' is not defined for types 'int' and 'bool'.
        ";

        AssertDiagnostics(text, diagnostics);
    }

    [Fact]
    public void Evaluator_CompoundExpression_Reports_Undefined() {
        var text = @"var x = 10;
                     x [+=] false;";

        var diagnostics = @"
            Binary operator '+=' is not defined for types 'int' and 'bool'.
        ";

        AssertDiagnostics(text, diagnostics);
    }

    [Fact]
    public void Evaluator_AssignmentExpression_Reports_Undefined() {
        var text = @"[x] = 10;";

        var diagnostics = @"
            Variable 'x' doesn't exist.
        ";

        AssertDiagnostics(text, diagnostics);
    }

    [Fact]
    public void Evaluator_CompoundExpression_Assignment_NonDefinedVariable_Reports_Undefined() {
        var text = @"[x] += 10;";

        var diagnostics = @"
            Variable 'x' doesn't exist.
        ";

        AssertDiagnostics(text, diagnostics);
    }

    [Fact]
    public void Evaluator_AssignmentExpression_Reports_NotAVariable() {
        var text = @"[print] = 42;";

        var diagnostics = @"
            'print' is not a variable.
        ";

        AssertDiagnostics(text, diagnostics);
    }

    [Fact]
    public void Evaluator_AssignmentExpression_Reports_CannotAssign() {
        var text = @"
            {
                val x = 10;
                x [=] 0;
            }
        ";

        var diagnostics = @"
            Variable 'x' is read-only and cannot be assigned to.
        ";

        AssertDiagnostics(text, diagnostics);
    }

    [Fact]
    public void Evaluator_CompoundDeclarationExpression_Reports_CannotAssign() {
        var text = @"
            {
                val x = 10;
                x [+=] 1;
            }
        ";

        var diagnostics = @"
            Variable 'x' is read-only and cannot be assigned to.
        ";

        AssertDiagnostics(text, diagnostics);
    }

    [Fact]
    public void Evaluator_AssignmentExpression_Reports_CannotConvert() {
        var text = @"
            {
                var x = 10;
                x = [true];
            }
        ";

        var diagnostics = @"
            Cannot convert type 'bool' to 'int'.
        ";

        AssertDiagnostics(text, diagnostics);
    }

    [Fact]
    public void Evaluator_CallExpression_Reports_Undefined() {
        var text = @"[foo](42);";

        var diagnostics = @"
            Function 'foo' doesn't exist.
        ";

        AssertDiagnostics(text, diagnostics);
    }

    [Fact]
    public void Evaluator_CallExpression_Reports_NotAFunction() {
        var text = @"
            {
                val foo = 42;
                [foo](42);
            }
        ";

        var diagnostics = @"
            'foo' is not a function.
        ";

        AssertDiagnostics(text, diagnostics);
    }

    [Fact]
    public void Evaluator_Variables_Can_Shadow_Functions() {
        var text = @"
            {
                val print = 42;
                [print](""test"");
            }
        ";

        var diagnostics = @"
            'print' is not a function.
        ";

        AssertDiagnostics(text, diagnostics);
    }

    [Fact]
    public void Evaluator_Void_Function_Should_Not_Return_Value() {
        var text = @"
            fun test(){
                return [1];
            }
        ";

        var diagnostics = @"
            Since the function 'test' does not return a value the 'return' keyword cannot be followed by an expression.
        ";

        AssertDiagnostics(text, diagnostics);
    }

    [Fact]
    public void Evaluator_Function_With_ReturnValue_Should_Not_Return_Void() {
        var text = @"
            fun test() as int{
                [return];
            }
        ";

        var diagnostics = @"
            An expression of type 'int' expected.
        ";

        AssertDiagnostics(text, diagnostics);
    }

    [Fact]
    public void Evaluator_Not_All_Code_Paths_Return_Value() {
        var text = @"
            fun [test](n as int) as bool{
                if (n > 10)
                   return true;
            }
        ";

        var diagnostics = @"
            Not all code paths return a value.
        ";

        AssertDiagnostics(text, diagnostics);
    }

    [Fact]
    public void Evaluator_Expression_Must_Have_Value() {
        var text = @"
            fun test(n as int){
                return;
            }
            val value = [test(100)];
        ";

        var diagnostics = @"
            Expression must have a value.
        ";

        AssertDiagnostics(text, diagnostics);
    }

    [Fact]
    public void Evaluator_IfStatement_Reports_NotReachableCode_Warning() {
        var text = @"
                fun test(){
                    val x = 4 * 3;
                    if x > 12 {
                        [print](""x"");
                    } else {
                        print(""x"");
                    }
                }
            ";

        var diagnostics = @"
                Unreachable code detected.
            ";
        AssertDiagnostics(text, diagnostics);
    }

    [Fact]
    public void Evaluator_ElseStatement_Reports_NotReachableCode_Warning() {
        var text = @"
                fun test() as int{
                    if true {
                        return 1;
                    } else {
                        [return] 0;
                    }
                }
            ";

        var diagnostics = @"
                Unreachable code detected.
            ";

        AssertDiagnostics(text, diagnostics);
    }

    [Fact]
    public void Evaluator_WhileStatement_Reports_NotReachableCode_Warning() {
        var text = @"
                fun test() {
                    while false {
                        [continue];
                    }
                }
            ";

        var diagnostics = @"
                Unreachable code detected.
            ";

        AssertDiagnostics(text, diagnostics);
    }

    [Theory]
    [InlineData("[break];", "break")]
    [InlineData("[continue];", "continue")]
    public void Evaluator_Invalid_Break_Or_Continue(string text, string keyword) {
        var diagnostics = $@"
            The keyword '{keyword}' can only be used inside of loops.
        ";

        AssertDiagnostics(text, diagnostics);
    }

    [Fact]
    public void Evaluator_Script_Return() {
        var text = @"
            return;
        ";

        AssertValue(text, "");
    }

    [Fact]
    public void Evaluator_Parameter_Already_Declared() {
        var text = @"
            fun sum(a as int, b as int, [a as int]) as int{
                return a + b + c;
            }
        ";

        var diagnostics = @"
            A parameter with the name 'a' already exists.
        ";

        AssertDiagnostics(text, diagnostics);
    }

    [Fact]
    public void Evaluator_Function_Must_Have_Name() {
        var text = @"
            fun [(]a as int, b as int) as int{
                return a + b;
            }
        ";

        var diagnostics = @"
            Unexpected token <OpenParenthesisToken>, expected <IdentifierToken>.
        ";

        AssertDiagnostics(text, diagnostics);
    }

    [Fact]
    public void Evaluator_Wrong_Argument_Type() {
        var text = @"
            fun test(n as int) as bool{
                return n > 10;
            }
            val testValue = ""string"";
            test([testValue]);
        ";

        var diagnostics = @"
            Cannot convert type 'string' to 'int'. An explicit conversion exits. (are you missing a cast?)
        ";

        AssertDiagnostics(text, diagnostics);
    }

    [Fact]
    public void Evaluator_Bad_Type() {
        var text = @"
            fun test(n as [invalidtype]){
            }
        ";

        var diagnostics = @"
            Type 'invalidtype' doesn't exist.
        ";

        AssertDiagnostics(text, diagnostics);
    }

    private static void AssertValue(string text, object expectedValue) {
        var syntaxTree = SyntaxTree.Parse(text);
        var compilation = Compilation.CreateScript(null, syntaxTree);
        var variables = new Dictionary<VariableSymbol, object>();
        var result = compilation.Evaluate(variables);

        Assert.False(result.Diagnostics.HasErrors());
        Assert.Equal(expectedValue, result.Value);
    }

    private void AssertDiagnostics(string text, string diagnosticText) {
        var annotatedText = AnnotatedText.Parse(text);
        var syntaxTree = SyntaxTree.Parse(annotatedText.Text);
        var compilation = Compilation.CreateScript(null, syntaxTree);
        var result = compilation.Evaluate(new Dictionary<VariableSymbol, object>());

        var expectedDiagnostics = AnnotatedText.UnindentLines(diagnosticText);

        if (annotatedText.Spans.Length != expectedDiagnostics.Length) {
            throw new Exception("ERROR: Must mark as many spans as there are expected diagnostics");
        }

        var diagnostics = result.Diagnostics;
        Assert.Equal(expectedDiagnostics.Length, diagnostics.Length);

        for (var i = 0; i < expectedDiagnostics.Length; i++) {
            var expectedMessage = expectedDiagnostics[i];
            var actualMessage = diagnostics[i].Message;
            Assert.Equal(expectedMessage, actualMessage);

            var expectedSpan = annotatedText.Spans[i];
            var actualSpan = diagnostics[i].Location.Span;
            Assert.Equal(expectedSpan, actualSpan);
        }
    }
}
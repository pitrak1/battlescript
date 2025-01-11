using Battlescript;

namespace BattlescriptTests;

public static class InterpreterTests
{
    // [TestFixture]
    // public class InterpreterAssignments
    // {
    //     [Test]
    //     public void HandlesBasicAssignmentsFromLiteralToVariable()
    //     {
    //         var lexer = new Lexer("x = 5");
    //         var lexerResult = lexer.Run();
    //         var parser = new Parser(lexerResult);
    //         var parserResult = parser.Run();
    //         var interpreter = new Interpreter(parserResult);
    //         var interpreterResult = interpreter.Run();
    //
    //         var expected = new List<Dictionary<string, Variable>>
    //         {
    //             new ()
    //             {
    //                 { "x", new Variable(Consts.VariableTypes.Number, 5) }
    //             }
    //         };
    //         
    //         Assertions.AssertScopeListEqual(interpreterResult, expected);
    //     }
    // }
}
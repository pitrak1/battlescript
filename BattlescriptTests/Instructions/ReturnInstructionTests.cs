using Battlescript;

namespace BattlescriptTests.Instructions;

[TestFixture]
public static partial class InstructionTests
{
    [TestFixture]
    public class ReturnInstructionParse
    {
        [Test]
        public void HandlesSimpleValue()
        {
            var lexer = new Lexer("return 4");
            var lexerResult = lexer.Run();
            
            var expected = new ReturnInstruction(new IntegerInstruction(4));
            
            Assert.That(InstructionFactory.Create(lexerResult), Is.EqualTo(expected));
        }
    }

    [TestFixture]
    public class ReturnInstructionInterpret
    {
        [Test]
        public void HandlesExpressions()
        {
            var memory = Runner.Run("def func(asdf):\n\treturn asdf + 5\nx = func(4)");

            var funcVariable = new FunctionVariable(
                [new VariableInstruction("asdf")],
                [
                    new ReturnInstruction(
                        new OperationInstruction(
                            "+",
                            new VariableInstruction("asdf"),
                            new IntegerInstruction(5)))
                ]);
            
            Assert.That(memory.Scopes[0]["func"], Is.EqualTo(funcVariable));
            Assert.That(memory.Scopes[0]["x"], Is.EqualTo(new IntegerVariable(9)));
        }
    }
}
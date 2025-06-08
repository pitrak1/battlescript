using Battlescript;

namespace BattlescriptTests;

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
            
            Assert.That(Instruction.Parse(lexerResult), Is.EqualTo(expected));
        }
    }

    [TestFixture]
    public class ReturnInstructionInterpret
    {
        [Test]
        public void HandlesExpressions()
        {
            var result = Runner.Run("def func(asdf):\n\treturn asdf + 5\nx = func(4)");

            var expected = new Dictionary<string, Variable>()
            {
                ["func"] = new FunctionVariable(
                    [new VariableInstruction("asdf")],
                    [
                        new ReturnInstruction(
                            new OperationInstruction(
                                "+",
                                new VariableInstruction("asdf"),
                                new IntegerInstruction(5)))
                    ]),
                ["x"] = new IntegerVariable(9)
            };
            
            Assert.That(result[0], Is.EquivalentTo(expected));
        }
    }
}
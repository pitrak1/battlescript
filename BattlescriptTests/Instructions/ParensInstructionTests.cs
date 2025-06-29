using Battlescript;

namespace BattlescriptTests;

[TestFixture]
public static partial class InstructionTests
{
    [TestFixture]
    public class ParensInstructionParse
    {
        [Test]
        public void HandlesSimpleValueLists()
        {
            var lexer = new Lexer("(4, 'asdf')");
            var lexerResult = lexer.Run();
            
            var expected = new ParensInstruction(
                [
                    new IntegerInstruction(4),
                    new StringInstruction("asdf")
                ]
            );
            
            Assert.That(InstructionFactory.Create(lexerResult), Is.EqualTo(expected));
        }
    }
    
    [TestFixture]
    public class ParensInstructionInterpret
    {
        [Test]
        public void HandlesFunctionCalls()
        {
            var memory = Runner.Run("def func(asdf):\n\treturn asdf + 5\nx = func(4)");

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
            
            Assert.That(memory.Scopes[0], Is.EquivalentTo(expected));
        }
        
        [Test]
        public void HandlesObjectInstantiation()
        {
            var memory = Runner.Run("class asdf():\n\tx = 6\ny = asdf()");

            var classBody = new Dictionary<string, Variable>()
            {
                ["x"] = new IntegerVariable(6)
            };
            var expected = new ObjectVariable(classBody, new ClassVariable(classBody));
            
            Assert.That(memory.Scopes[0]["y"], Is.EqualTo(expected));
        }
    }
}
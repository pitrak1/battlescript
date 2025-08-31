using Battlescript;

namespace BattlescriptTests.Instructions;

[TestFixture]
public static class BuiltInInstructionTests
{
    [TestFixture]
    public class Parse
    {
        [Test]
        public void HandlesNoArguments()
        {
            var expected = new BuiltInInstruction(
                name: "super",
                arguments: []
            );
            Assertions.AssertInputProducesParserOutput("super()", expected);
        }
        
        [Test]
        public void HandlesArguments()
        {
            var expected = new BuiltInInstruction(
                name: "super",
                arguments: [new VariableInstruction("x"), new VariableInstruction("y")]
            );
            Assertions.AssertInputProducesParserOutput("super(x, y)", expected);
        }
        
        [Test]
        public void HandlesTokensAfterArguments()
        {
            var expected = new BuiltInInstruction(
                name: "super",
                arguments: [new VariableInstruction("x"), new VariableInstruction("y")],
                next: new MemberInstruction("asdf")
            );
            Assertions.AssertInputProducesParserOutput("super(x, y).asdf", expected);
        }
    }
}
using Battlescript;

namespace BattlescriptTests.Instructions;

[TestFixture]
public static class BuiltInInstructionTests
{
    [TestFixture]
    public class Parse
    {
        [Test]
        public void NoArguments()
        {
            var input = "super()";
            var expected = new BuiltInInstruction(
                name: "super",
                arguments: []
            );
            var result = Runner.Parse(input);
            Assert.That(result[0], Is.EqualTo(expected));
        }
        
        [Test]
        public void Arguments()
        {
            var input = "super(x, y)";
            var expected = new BuiltInInstruction(
                name: "super",
                arguments: [new VariableInstruction("x"), new VariableInstruction("y")]
            );
            var result = Runner.Parse(input);
            Assert.That(result[0], Is.EqualTo(expected));
        }
        
        [Test]
        public void HandlesTokensAfterArguments()
        {
            var input = "super(x, y).asdf";
            var expected = new BuiltInInstruction(
                name: "super",
                arguments: [new VariableInstruction("x"), new VariableInstruction("y")],
                next: new MemberInstruction("asdf")
            );
            var result = Runner.Parse(input);
            Assert.That(result[0], Is.EqualTo(expected));
        }
    }
}
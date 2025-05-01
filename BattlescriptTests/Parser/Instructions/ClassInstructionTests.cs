using Battlescript;

namespace BattlescriptTests;

[TestFixture]
public static class ClassInstructionTests
{
    [TestFixture]
    public class Parse
    {
        [Test]
        public void HandlesBasicClassDefinition()
        {
            var expected = new ClassInstruction("MyClass");
            ParserAssertions.AssertInputProducesInstruction("class MyClass:", expected);
        }
        
        [Test]
        public void HandlesClassDefinitionWithInheritance()
        {
            var expected = new ClassInstruction(
                "MyClass",
                [new VariableInstruction("asdf")]
            );
            ParserAssertions.AssertInputProducesInstruction("class MyClass(asdf):", expected);
        }
    }
}
using Battlescript;

namespace BattlescriptTests.Instructions;

[TestFixture]
public static class ClassInstructionTests
{
    [TestFixture]
    public class Parse
    {
        [Test]
        public void BasicDefinition()
        {
            var input = "class MyClass:";
            var expected = new ClassInstruction("MyClass");
            var result = Runner.Parse(input, false);
            Assert.That(result[0], Is.EqualTo(expected));
        }

        [Test]
        public void DefinitionWithInheritance()
        {
            var input = "class MyClass(asdf):";
            var expected = new ClassInstruction(
                "MyClass",
                [new VariableInstruction("asdf")]
            );
            var result = Runner.Parse(input, false);
            Assert.That(result[0], Is.EqualTo(expected));
        }
        
        [Test]
        public void DefinitionWithMultipleInheritance()
        {
            var input = "class MyClass(asdf, qwer):";
            var expected = new ClassInstruction(
                "MyClass",
                [new VariableInstruction("asdf"), new VariableInstruction("qwer")]
            );
            var result = Runner.Parse(input, false);
            Assert.That(result[0], Is.EqualTo(expected));
        }
    }

    [TestFixture]
    public class Interpret
    {
        [Test]
        public void BasicDefinition()
        {
            var (callStack, closure) = Runner.Run("class MyClass:\n\tx = 1");
            var expected = new ClassVariable("MyClass", new Dictionary<string, Variable>()
            {
                { "x", BtlTypes.Create(BtlTypes.Types.Int, 1) }
            }, closure);
            Assert.That(closure.GetVariable(callStack, "MyClass"), Is.EqualTo(expected));
        }
        
        [Test]
        public void DefinitionWithInheritance()
        {
            var (callStack, closure) = Runner.Run("""
                                    class asdf():
                                        x = 1

                                    class qwer(asdf):
                                        y = 2

                                    """);
            var asdf = new ClassVariable("asdf", new Dictionary<string, Variable>()
            {
                { "x", BtlTypes.Create(BtlTypes.Types.Int, 1) }
            }, closure);

            var qwer = new ClassVariable("qwer", new Dictionary<string, Variable>()
            {
                { "y", BtlTypes.Create(BtlTypes.Types.Int, 2) }
            }, closure, [asdf]);

            Assert.That(closure.GetVariable(callStack, "asdf"), Is.EqualTo(asdf));
            Assert.That(closure.GetVariable(callStack, "qwer"), Is.EqualTo(qwer));
        }
        
        [Test]
        public void DefinitionWithMultipleInheritance()
        {
            var (callStack, closure) = Runner.Run("""
                                    class asdf:
                                        x = 1

                                    class qwer:
                                        y = 2

                                    class zxcv(asdf, qwer):
                                        z = 3

                                    """);
            
            var asdf = new ClassVariable("asdf", new Dictionary<string, Variable>()
            {
                { "x", BtlTypes.Create(BtlTypes.Types.Int, 1) }
            }, closure);

            var qwer = new ClassVariable("qwer", new Dictionary<string, Variable>()
            {
                { "y", BtlTypes.Create(BtlTypes.Types.Int, 2) }
            }, closure);

            var zxcv = new ClassVariable("zxcv", new Dictionary<string, Variable>()
            {
                { "z", BtlTypes.Create(BtlTypes.Types.Int, 3) }
            }, closure, [asdf, qwer]);
            
            Assert.That(closure.GetVariable(callStack, "asdf"), Is.EqualTo(asdf));
            Assert.That(closure.GetVariable(callStack, "qwer"), Is.EqualTo(qwer));
            Assert.That(closure.GetVariable(callStack, "zxcv"), Is.EqualTo(zxcv));
        }
    }
}
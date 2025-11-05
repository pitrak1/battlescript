using Battlescript;

namespace BattlescriptTests.Instructions;

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
            Assertions.AssertInputProducesParserOutput("class MyClass:", expected);
        }

        [Test]
        public void HandlesClassDefinitionWithInheritance()
        {
            var expected = new ClassInstruction(
                "MyClass",
                [new VariableInstruction("asdf")]
            );
            Assertions.AssertInputProducesParserOutput("class MyClass(asdf):", expected);
        }
        
        [Test]
        public void HandlesClassDefinitionWithMultipleInheritance()
        {
            var expected = new ClassInstruction(
                "MyClass",
                [new VariableInstruction("asdf"), new VariableInstruction("qwer")]
            );
            Assertions.AssertInputProducesParserOutput("class MyClass(asdf, qwer):", expected);
        }
    }

    [TestFixture]
    public class Interpret
    {
        [Test]
        public void HandlesBasicClassDefinition()
        {
            var (callStack, closure) = Runner.Run("class MyClass:\n\tx = 1");
            var expected = new ClassVariable("MyClass", new Dictionary<string, Variable>()
            {
                { "x", BsTypes.Create(BsTypes.Types.Int, 1) }
            }, closure);
            Assertions.AssertVariable(callStack, closure, "MyClass", expected);
        }
        
        [Test]
        public void HandlesClassDefinitionWithInheritance()
        {
            var (callStack, closure) = Runner.Run("""
                                    class asdf():
                                        x = 1

                                    class qwer(asdf):
                                        y = 2

                                    """);
            var asdf = new ClassVariable("asdf", new Dictionary<string, Variable>()
            {
                { "x", BsTypes.Create(BsTypes.Types.Int, 1) }
            }, closure);

            var qwer = new ClassVariable("qwer", new Dictionary<string, Variable>()
            {
                { "y", BsTypes.Create(BsTypes.Types.Int, 2) }
            }, closure, [asdf]);

            Assertions.AssertVariable(callStack, closure, "asdf", asdf);
            Assertions.AssertVariable(callStack, closure, "qwer", qwer);
        }
        
        [Test]
        public void HandlesClassDefinitionWithMultipleInheritance()
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
                { "x", BsTypes.Create(BsTypes.Types.Int, 1) }
            }, closure);

            var qwer = new ClassVariable("qwer", new Dictionary<string, Variable>()
            {
                { "y", BsTypes.Create(BsTypes.Types.Int, 2) }
            }, closure);

            var zxcv = new ClassVariable("zxcv", new Dictionary<string, Variable>()
            {
                { "z", BsTypes.Create(BsTypes.Types.Int, 3) }
            }, closure, [asdf, qwer]);
            
            Assertions.AssertVariable(callStack, closure, "asdf", asdf);
            Assertions.AssertVariable(callStack, closure, "qwer", qwer);
            Assertions.AssertVariable(callStack, closure, "zxcv", zxcv);
        }
    }
}
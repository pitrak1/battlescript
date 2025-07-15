using Battlescript;

namespace BattlescriptTests.Instructions;

[TestFixture]
public static partial class InstructionTests
{
    [TestFixture]
    public class ClassInstructionParse
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
    }

    [TestFixture]
    public class ClassInstructionInterpret
    {
        [Test]
        public void HandlesBasicClassDefinition()
        {
            var memory = Runner.Run("class MyClass:\n\tx = 1");
            var expected = new ClassVariable("MyClass", new Dictionary<string, Variable>()
            {
                { "x", BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 1) }
            });
            Assert.That(memory.Scopes.First()["MyClass"], Is.EqualTo(expected));
        }
        
        [Test]
        public void HandlesClassDefinitionWithInheritance()
        {
            var memory = Runner.Run("""
                                    class asdf():
                                        x = 1

                                    class qwer(asdf):
                                        y = 2

                                    """);
            var asdf = new ClassVariable("asdf", new Dictionary<string, Variable>()
            {
                { "x", BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 1) }
            });

            var qwer = new ClassVariable("qwer", new Dictionary<string, Variable>()
            {
                { "y", BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 2) }
            }, [asdf]);

            Assert.That(memory.Scopes.First()["asdf"], Is.EqualTo(asdf));
            Assert.That(memory.Scopes.First()["qwer"], Is.EqualTo(qwer));
        }
        
        [Test]
        public void HandlesClassDefinitionWithMultipleInheritance()
        {
            var memory = Runner.Run("""
                                    class asdf:
                                        x = 1

                                    class qwer:
                                        y = 2

                                    class zxcv(asdf, qwer):
                                        z = 3

                                    """);
            
            var asdf = new ClassVariable("asdf", new Dictionary<string, Variable>()
            {
                { "x", BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 1) }
            });

            var qwer = new ClassVariable("qwer", new Dictionary<string, Variable>()
            {
                { "y", BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 2) }
            });

            var zxcv = new ClassVariable("zxcv", new Dictionary<string, Variable>()
            {
                { "z", BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 3) }
            }, [asdf, qwer]);
            
            var expected = new Dictionary<string, Variable>()
            {
                {"asdf", asdf},
                {"qwer", qwer},
                {"zxcv", zxcv}
            };
            Assert.That(memory.Scopes.First()["asdf"], Is.EqualTo(asdf));
            Assert.That(memory.Scopes.First()["qwer"], Is.EqualTo(qwer));
            Assert.That(memory.Scopes.First()["zxcv"], Is.EqualTo(zxcv));
        }
    }
}
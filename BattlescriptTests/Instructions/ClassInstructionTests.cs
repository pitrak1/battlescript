using Battlescript;

namespace BattlescriptTests;

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

    [TestFixture]
    public class ClassInstructionInterpret
    {
        [Test]
        public void HandlesBasicClassDefinition()
        {
            var expected = new Dictionary<string, Variable>()
            {
                {
                    "MyClass", new ClassVariable(new Dictionary<string, Variable>()
                    {
                        { "x", new NumberVariable(1.0) }
                    })
                }
            };
            InterpreterAssertions.AssertInputProducesOutput("class MyClass:\n\tx = 1", [expected]);
        }
        
        [Test]
        public void HandlesClassDefinitionWithInheritance()
        {
            var asdf = new ClassVariable(new Dictionary<string, Variable>()
            {
                { "x", new NumberVariable(1.0) }
            });

            var qwer = new ClassVariable(new Dictionary<string, Variable>()
            {
                { "y", new NumberVariable(2.0) }
            }, [asdf]);
            
            var expected = new Dictionary<string, Variable>()
            {
                {"asdf", asdf},
                {"qwer", qwer}
            };
            InterpreterAssertions.AssertInputProducesOutput(@"
class asdf:
    x = 1

class qwer(asdf):
    y = 2
", [expected]);
        }
        
        [Test]
        public void HandlesClassDefinitionWithMultipleInheritance()
        {
            var asdf = new ClassVariable(new Dictionary<string, Variable>()
            {
                { "x", new NumberVariable(1.0) }
            });

            var qwer = new ClassVariable(new Dictionary<string, Variable>()
            {
                { "y", new NumberVariable(2.0) }
            });

            var zxcv = new ClassVariable(new Dictionary<string, Variable>()
            {
                { "z", new NumberVariable(3.0) }
            }, [asdf, qwer]);
            
            var expected = new Dictionary<string, Variable>()
            {
                {"asdf", asdf},
                {"qwer", qwer},
                {"zxcv", zxcv}
            };
            InterpreterAssertions.AssertInputProducesOutput(@"
class asdf:
    x = 1

class qwer:
    y = 2

class zxcv(asdf, qwer):
    z = 3
", [expected]);
        }
    }
}
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
            var lexer = new Lexer("class MyClass:");
            var lexerResult = lexer.Run();
            
            var expected = new ClassInstruction("MyClass");
            
            Assert.That(Instruction.Parse(lexerResult), Is.EqualTo(expected));
        }
        
        [Test]
        public void HandlesClassDefinitionWithInheritance()
        {
            var lexer = new Lexer("class MyClass(asdf):");
            var lexerResult = lexer.Run();
            
            var expected = new ClassInstruction(
                "MyClass",
                [new VariableInstruction("asdf")]
            );
            
            Assert.That(Instruction.Parse(lexerResult), Is.EqualTo(expected));
        }
    }

    [TestFixture]
    public class ClassInstructionInterpret
    {
        [Test]
        public void HandlesBasicClassDefinition()
        {
            var result = Runner.Run("class MyClass:\n\tx = 1");
            var expected = new Dictionary<string, Variable>()
            {
                {
                    "MyClass", new ClassVariable(new Dictionary<string, Variable>()
                    {
                        { "x", new IntegerVariable(1) }
                    })
                }
            };
            Assert.That(result.First(), Is.EquivalentTo(expected));
        }
        
        [Test]
        public void HandlesClassDefinitionWithInheritance()
        {
            var result = Runner.Run(@"
class asdf():
    x = 1

class qwer(asdf):
    y = 2
");
            var asdf = new ClassVariable(new Dictionary<string, Variable>()
            {
                { "x", new IntegerVariable(1) }
            });

            var qwer = new ClassVariable(new Dictionary<string, Variable>()
            {
                { "y", new IntegerVariable(2) }
            }, [asdf]);
            
            var expected = new Dictionary<string, Variable>()
            {
                {"asdf", asdf},
                {"qwer", qwer}
            };
            Assert.That(result.First(), Is.EquivalentTo(expected));
        }
        
        [Test]
        public void HandlesClassDefinitionWithMultipleInheritance()
        {
            var result = Runner.Run(@"
class asdf:
    x = 1

class qwer:
    y = 2

class zxcv(asdf, qwer):
    z = 3
");
            
            var asdf = new ClassVariable(new Dictionary<string, Variable>()
            {
                { "x", new IntegerVariable(1) }
            });

            var qwer = new ClassVariable(new Dictionary<string, Variable>()
            {
                { "y", new IntegerVariable(2) }
            });

            var zxcv = new ClassVariable(new Dictionary<string, Variable>()
            {
                { "z", new IntegerVariable(3) }
            }, [asdf, qwer]);
            
            var expected = new Dictionary<string, Variable>()
            {
                {"asdf", asdf},
                {"qwer", qwer},
                {"zxcv", zxcv}
            };
            Assert.That(result.First(), Is.EquivalentTo(expected));
        }
    }
}
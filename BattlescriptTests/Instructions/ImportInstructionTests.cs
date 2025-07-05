using Battlescript;

namespace BattlescriptTests.Instructions;

[TestFixture]
public static class ImportInstructionTests
{
    [TestFixture]
    public class ImportInstructionParse
    {
        [Test]
        public void HandlesImportingListsOfVariables()
        {
            var lexer = new Lexer("from '/test/x.bs' import x, y, z");
            var lexerResult = lexer.Run();
            
            var expected = new ImportInstruction("/test/x.bs", ["x", "y", "z"]);
            
            Assert.That(InstructionFactory.Create(lexerResult), Is.EqualTo(expected));
        }

        [Test]
        public void HandlesImportingEntireModule()
        {
            var lexer = new Lexer("from '/test/x.bs' import *");
            var lexerResult = lexer.Run();
            
            var expected = new ImportInstruction("/test/x.bs", ["*"]);
            
            Assert.That(InstructionFactory.Create(lexerResult), Is.EqualTo(expected));
        }
    }

    [TestFixture]
    public class ImportInstructionInterpret
    {
        [Test]
        public void HandlesImportingSimpleVariables()
        {
            var filePath = @"/Users/nickpitrak/Desktop/Battlescript/BattlescriptTests/TestFiles/import.bs";
            var memory = Runner.Run($"from '{filePath}' import x");
            var expected = new Dictionary<string, Variable>()
            {
                ["x"] = new IntegerVariable(5)
            };
            
            Assert.That(memory.Scopes[0], Is.EquivalentTo(expected));
        }
        
        [Test]
        public void HandlesImportingListOfVariables()
        {
            var filePath = @"/Users/nickpitrak/Desktop/Battlescript/BattlescriptTests/TestFiles/import.bs";
            var memory = Runner.Run($"from '{filePath}' import x, y, z");
            var expected = new Dictionary<string, Variable>()
            {
                ["x"] = new IntegerVariable(5),
                ["y"] = new ListVariable([new IntegerVariable(1), new IntegerVariable(2), new IntegerVariable(3)]),
                ["z"] = new StringVariable("asdf")
            };
            
            Assert.That(memory.Scopes[0], Is.EquivalentTo(expected));
        }
        
        [Test]
        public void HandlesImportingEntireModule()
        {
            var filePath = @"/Users/nickpitrak/Desktop/Battlescript/BattlescriptTests/TestFiles/import.bs";
            var memory = Runner.Run($"from '{filePath}' import *");
            var expected = new Dictionary<string, Variable>()
            {
                ["import"] = new DictionaryVariable(new Dictionary<Variable, Variable>()
                {
                    {new StringVariable("x"), new IntegerVariable(5)},
                    {new StringVariable("y"), new ListVariable([new IntegerVariable(1), new IntegerVariable(2), new IntegerVariable(3)])},
                    {new StringVariable("z"), new StringVariable("asdf")}
                })
            };
            
            Assert.That(memory.Scopes[0], Is.EquivalentTo(expected));
        }
    }
    
}
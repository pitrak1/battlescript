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
            Assert.That(memory.Scopes[0]["x"], Is.EqualTo(BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 5)));
        }
        
        [Test]
        public void HandlesImportingListOfVariables()
        {
            var filePath = @"/Users/nickpitrak/Desktop/Battlescript/BattlescriptTests/TestFiles/import.bs";
            var memory = Runner.Run($"from '{filePath}' import x, y, z");

            Assert.That(memory.Scopes[0]["x"], Is.EqualTo(BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 5)));
            Assert.That(memory.Scopes[0]["y"], Is.EqualTo(
                new ListVariable([
                    BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 1), 
                    BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 2), 
                    BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 3)])));
            Assert.That(memory.Scopes[0]["z"], Is.EqualTo(new StringVariable("asdf")));
        }
        
        [Test]
        public void HandlesImportingEntireModule()
        {
            var filePath = @"/Users/nickpitrak/Desktop/Battlescript/BattlescriptTests/TestFiles/import.bs";
            var memory = Runner.Run($"from '{filePath}' import *");
            var expected = new DictionaryVariable(new Dictionary<Variable, Variable>()
            {
                { new StringVariable("x"), BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 5) },
                {
                    new StringVariable("y"),
                    new ListVariable([
                        BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 1), 
                        BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 2), 
                        BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 3)])
                },
                { new StringVariable("z"), new StringVariable("asdf") }
            });
            
            Assert.That(memory.Scopes[0]["import"], Is.EqualTo(expected));
        }
    }
    
}
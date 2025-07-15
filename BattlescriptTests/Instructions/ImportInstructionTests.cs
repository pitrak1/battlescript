using Battlescript;

namespace BattlescriptTests.Instructions;

[TestFixture]
public static class ImportInstructionTests
{
    [TestFixture]
    public class Parse
    {
        [Test]
        public void HandlesImportingListsOfVariables()
        {
            var expected = new ImportInstruction("/test/x.bs", ["x", "y", "z"]);
            Assertions.AssertInputProducesParserOutput("from '/test/x.bs' import x, y, z", expected);
        }

        [Test]
        public void HandlesImportingEntireModule()
        {
            var expected = new ImportInstruction("/test/x.bs", ["*"]);
            Assertions.AssertInputProducesParserOutput("from '/test/x.bs' import *", expected);
        }
    }

    [TestFixture]
    public class Interpret
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
            var expected = new DictionaryVariable(null, new Dictionary<string, Variable>()
            {
                { "x", BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 5) },
                {
                    "y",
                    new ListVariable([
                        BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 1), 
                        BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 2), 
                        BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 3)])
                },
                { "z", new StringVariable("asdf") }
            });
            
            Assert.That(memory.Scopes[0]["import"], Is.EqualTo(expected));
        }
    }
    
}
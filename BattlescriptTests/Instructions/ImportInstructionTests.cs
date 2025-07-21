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
            Assertions.AssertVariablesEqual(memory.Scopes[0]["x"], BsTypes.Create(memory, BsTypes.Types.Int, 5));
        }
        
        [Test]
        public void HandlesImportingListOfVariables()
        {
            var filePath = @"/Users/nickpitrak/Desktop/Battlescript/BattlescriptTests/TestFiles/import.bs";
            var memory = Runner.Run($"from '{filePath}' import x, y, z");

            Assertions.AssertVariablesEqual(memory.Scopes[0]["x"], BsTypes.Create(memory, BsTypes.Types.Int, 5));
            Assertions.AssertVariablesEqual(memory.Scopes[0]["y"], 
                BsTypes.Create(memory, BsTypes.Types.List, new List<Variable>() {
                        BsTypes.Create(memory, BsTypes.Types.Int, 1), 
                        BsTypes.Create(memory, BsTypes.Types.Int, 2), 
                        BsTypes.Create(memory, BsTypes.Types.Int, 3)}));
            Assertions.AssertVariablesEqual(memory.Scopes[0]["z"], new StringVariable("asdf"));
        }
        
        [Test]
        public void HandlesImportingEntireModule()
        {
            var filePath = @"/Users/nickpitrak/Desktop/Battlescript/BattlescriptTests/TestFiles/import.bs";
            var memory = Runner.Run($"from '{filePath}' import *");
            var expected = new DictionaryVariable(null, new Dictionary<string, Variable>()
            {
                { "x", BsTypes.Create(memory, BsTypes.Types.Int, 5) },
                {
                    "y", BsTypes.Create(memory, BsTypes.Types.List, new List<Variable>() {
                        BsTypes.Create(memory, BsTypes.Types.Int, 1), 
                        BsTypes.Create(memory, BsTypes.Types.Int, 2), 
                        BsTypes.Create(memory, BsTypes.Types.Int, 3)})
                },
                { "z", new StringVariable("asdf") }
            });
            Assertions.AssertVariablesEqual(memory.Scopes[0]["import"], expected);
        }
    }
    
}
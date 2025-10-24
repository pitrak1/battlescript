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
            Assertions.AssertVariable(memory, "x", BsTypes.Create(BsTypes.Types.Int, 5));
        }
        
        [Test]
        public void HandlesImportingListOfVariables()
        {
            var filePath = @"/Users/nickpitrak/Desktop/Battlescript/BattlescriptTests/TestFiles/import.bs";
            var memory = Runner.Run($"from '{filePath}' import x, y, z");

            Assertions.AssertVariable(memory, "x", BsTypes.Create(BsTypes.Types.Int, 5));
            Assertions.AssertVariable(memory, "y", 
                BsTypes.Create(BsTypes.Types.List, new List<Variable>() {
                        BsTypes.Create(BsTypes.Types.Int, 1), 
                        BsTypes.Create(BsTypes.Types.Int, 2), 
                        BsTypes.Create(BsTypes.Types.Int, 3)}));
            Assertions.AssertVariable(memory, "z", BsTypes.Create(BsTypes.Types.String, "asdf"));
        }
        
        [Test]
        public void HandlesImportingEntireModule()
        {
            var filePath = @"/Users/nickpitrak/Desktop/Battlescript/BattlescriptTests/TestFiles/import.bs";
            var memory = Runner.Run($"from '{filePath}' import *");
            var expected = BsTypes.Create(BsTypes.Types.Dictionary, new MappingVariable(null, new Dictionary<string, Variable>()
            {
                { "x", BsTypes.Create(BsTypes.Types.Int, 5) },
                {
                    "y", BsTypes.Create(BsTypes.Types.List, new List<Variable>() {
                        BsTypes.Create(BsTypes.Types.Int, 1), 
                        BsTypes.Create(BsTypes.Types.Int, 2), 
                        BsTypes.Create(BsTypes.Types.Int, 3)})
                },
                { "z", BsTypes.Create(BsTypes.Types.String, "asdf") }
            }));
            Assertions.AssertVariable(memory, "import", expected);
        }
    }
    
}
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
            Assertions.AssertVariable(memory, "x", memory.CreateBsType(Memory.BsTypes.Int, 5));
        }
        
        [Test]
        public void HandlesImportingListOfVariables()
        {
            var filePath = @"/Users/nickpitrak/Desktop/Battlescript/BattlescriptTests/TestFiles/import.bs";
            var memory = Runner.Run($"from '{filePath}' import x, y, z");

            Assertions.AssertVariable(memory, "x", memory.CreateBsType(Memory.BsTypes.Int, 5));
            Assertions.AssertVariable(memory, "y", 
                memory.CreateBsType(Memory.BsTypes.List, new List<Variable>() {
                        memory.CreateBsType(Memory.BsTypes.Int, 1), 
                        memory.CreateBsType(Memory.BsTypes.Int, 2), 
                        memory.CreateBsType(Memory.BsTypes.Int, 3)}));
            Assertions.AssertVariable(memory, "z", memory.CreateBsType(Memory.BsTypes.String, "asdf"));
        }
        
        [Test]
        public void HandlesImportingEntireModule()
        {
            var filePath = @"/Users/nickpitrak/Desktop/Battlescript/BattlescriptTests/TestFiles/import.bs";
            var memory = Runner.Run($"from '{filePath}' import *");
            var expected = memory.CreateBsType(Memory.BsTypes.Dictionary, new MappingVariable(null, new Dictionary<string, Variable>()
            {
                { "x", memory.CreateBsType(Memory.BsTypes.Int, 5) },
                {
                    "y", memory.CreateBsType(Memory.BsTypes.List, new List<Variable>() {
                        memory.CreateBsType(Memory.BsTypes.Int, 1), 
                        memory.CreateBsType(Memory.BsTypes.Int, 2), 
                        memory.CreateBsType(Memory.BsTypes.Int, 3)})
                },
                { "z", memory.CreateBsType(Memory.BsTypes.String, "asdf") }
            }));
            Assertions.AssertVariable(memory, "import", expected);
        }
    }
    
}
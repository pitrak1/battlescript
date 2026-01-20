using Battlescript;

namespace BattlescriptTests.Instructions;

[TestFixture]
public static class ImportInstructionTests
{
    [TestFixture]
    public class Parse
    {
        [Test]
        public void SingleVariable()
        {
            var expected = new ImportInstruction("/test/x.bs", ["x"]);
            Assertions.AssertInputProducesParserOutput("from '/test/x.bs' import x", expected);
        }
        
        [Test]
        public void ListsOfVariables()
        {
            var expected = new ImportInstruction("/test/x.bs", ["x", "y", "z"]);
            Assertions.AssertInputProducesParserOutput("from '/test/x.bs' import x, y, z", expected);
        }

        [Test]
        public void Module()
        {
            var expected = new ImportInstruction("/test/x.bs", ["*"]);
            Assertions.AssertInputProducesParserOutput("from '/test/x.bs' import *", expected);
        }

        [Test]
        public void ModuleInList()
        {
            var expected = new ImportInstruction("/test/x.bs", ["x", "y", "*"]);
            Assertions.AssertInputProducesParserOutput("from '/test/x.bs' import x, y, *", expected);
        }
    }

    [TestFixture]
    public class Interpret
    {
        [Test]
        public void ImportsSingleVariable()
        {
            var filePath = @"/Users/nickpitrak/Desktop/Battlescript/BattlescriptTests/TestFiles/import.bs";
            var (callStack, closure) = Runner.Run($"from '{filePath}' import x");
            Assertions.AssertVariable(callStack, closure, "x", BtlTypes.Create(BtlTypes.Types.Int, 5));
        }
        
        [Test]
        public void ImportsMultipleVariables()
        {
            var filePath = @"/Users/nickpitrak/Desktop/Battlescript/BattlescriptTests/TestFiles/import.bs";
            var (callStack, closure) = Runner.Run($"from '{filePath}' import x, y, z");

            Assertions.AssertVariable(callStack, closure, "x", BtlTypes.Create(BtlTypes.Types.Int, 5));
            Assertions.AssertVariable(callStack, closure, "y", 
                BtlTypes.Create(BtlTypes.Types.List, new List<Variable>() {
                        BtlTypes.Create(BtlTypes.Types.Int, 1), 
                        BtlTypes.Create(BtlTypes.Types.Int, 2), 
                        BtlTypes.Create(BtlTypes.Types.Int, 3)}));
            Assertions.AssertVariable(callStack, closure, "z", BtlTypes.Create(BtlTypes.Types.String, "asdf"));
        }
        
        [Test]
        public void ImportsModule()
        {
            var filePath = @"/Users/nickpitrak/Desktop/Battlescript/BattlescriptTests/TestFiles/import.bs";
            var (callStack, closure) = Runner.Run($"from '{filePath}' import *");
            var expected = BtlTypes.Create(BtlTypes.Types.Dictionary, new MappingVariable(null, new Dictionary<string, Variable>()
            {
                { "x", BtlTypes.Create(BtlTypes.Types.Int, 5) },
                {
                    "y", BtlTypes.Create(BtlTypes.Types.List, new List<Variable>() {
                        BtlTypes.Create(BtlTypes.Types.Int, 1), 
                        BtlTypes.Create(BtlTypes.Types.Int, 2), 
                        BtlTypes.Create(BtlTypes.Types.Int, 3)})
                },
                { "z", BtlTypes.Create(BtlTypes.Types.String, "asdf") }
            }));
            Assertions.AssertVariable(callStack, closure, "import", expected);
        }
        
        [Test]
        public void ImportsMultipleVariablesAndModule()
        {
            var filePath = @"/Users/nickpitrak/Desktop/Battlescript/BattlescriptTests/TestFiles/import.bs";
            var (callStack, closure) = Runner.Run($"from '{filePath}' import x, y, z, *");
            var expected = BtlTypes.Create(BtlTypes.Types.Dictionary, new MappingVariable(null, new Dictionary<string, Variable>()
            {
                { "x", BtlTypes.Create(BtlTypes.Types.Int, 5) },
                {
                    "y", BtlTypes.Create(BtlTypes.Types.List, new List<Variable>() {
                        BtlTypes.Create(BtlTypes.Types.Int, 1), 
                        BtlTypes.Create(BtlTypes.Types.Int, 2), 
                        BtlTypes.Create(BtlTypes.Types.Int, 3)})
                },
                { "z", BtlTypes.Create(BtlTypes.Types.String, "asdf") }
            }));
            
            Assertions.AssertVariable(callStack, closure, "import", expected);
            Assertions.AssertVariable(callStack, closure, "x", BtlTypes.Create(BtlTypes.Types.Int, 5));
            Assertions.AssertVariable(callStack, closure, "y", 
                BtlTypes.Create(BtlTypes.Types.List, new List<Variable>() {
                    BtlTypes.Create(BtlTypes.Types.Int, 1), 
                    BtlTypes.Create(BtlTypes.Types.Int, 2), 
                    BtlTypes.Create(BtlTypes.Types.Int, 3)}));
            Assertions.AssertVariable(callStack, closure, "z", BtlTypes.Create(BtlTypes.Types.String, "asdf"));
        }
    }
    
}
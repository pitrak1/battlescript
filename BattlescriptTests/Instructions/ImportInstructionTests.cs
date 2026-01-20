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
            var result = Runner.Parse("from '/test/x.bs' import x");
            Assert.That(result[0], Is.EqualTo(expected));
        }

        [Test]
        public void ListsOfVariables()
        {
            var expected = new ImportInstruction("/test/x.bs", ["x", "y", "z"]);
            var result = Runner.Parse("from '/test/x.bs' import x, y, z");
            Assert.That(result[0], Is.EqualTo(expected));
        }

        [Test]
        public void Module()
        {
            var expected = new ImportInstruction("/test/x.bs", ["*"]);
            var result = Runner.Parse("from '/test/x.bs' import *");
            Assert.That(result[0], Is.EqualTo(expected));
        }

        [Test]
        public void ModuleInList()
        {
            var expected = new ImportInstruction("/test/x.bs", ["x", "y", "*"]);
            var result = Runner.Parse("from '/test/x.bs' import x, y, *");
            Assert.That(result[0], Is.EqualTo(expected));
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
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, 5)));
        }

        [Test]
        public void ImportsMultipleVariables()
        {
            var filePath = @"/Users/nickpitrak/Desktop/Battlescript/BattlescriptTests/TestFiles/import.bs";
            var (callStack, closure) = Runner.Run($"from '{filePath}' import x, y, z");

            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, 5)));
            Assert.That(closure.GetVariable(callStack, "y"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.List, new List<Variable>() {
                        BtlTypes.Create(BtlTypes.Types.Int, 1),
                        BtlTypes.Create(BtlTypes.Types.Int, 2),
                        BtlTypes.Create(BtlTypes.Types.Int, 3)})));
            Assert.That(closure.GetVariable(callStack, "z"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.String, "asdf")));
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
            Assert.That(closure.GetVariable(callStack, "import"), Is.EqualTo(expected));
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

            Assert.That(closure.GetVariable(callStack, "import"), Is.EqualTo(expected));
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, 5)));
            Assert.That(closure.GetVariable(callStack, "y"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.List, new List<Variable>() {
                    BtlTypes.Create(BtlTypes.Types.Int, 1),
                    BtlTypes.Create(BtlTypes.Types.Int, 2),
                    BtlTypes.Create(BtlTypes.Types.Int, 3)})));
            Assert.That(closure.GetVariable(callStack, "z"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.String, "asdf")));
        }
    }

}

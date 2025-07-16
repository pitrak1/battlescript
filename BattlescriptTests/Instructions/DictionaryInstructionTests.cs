using Battlescript;

namespace BattlescriptTests.Instructions;

[TestFixture]
public static class DictionaryInstructionTests
{
    [TestFixture]
    public class Parse
    {
        [Test]
        public void AllowsNumbersToBeUsedAsKeys()
        {
            var expected = new ArrayInstruction([
                new ArrayInstruction([new NumericInstruction(4), new NumericInstruction(5)], delimiter: ":"),
                new ArrayInstruction([new NumericInstruction(6), new StringInstruction("asdf")], delimiter: ":"),
            ], separator: "{", delimiter: ",");
            Assertions.AssertInputProducesParserOutput("{4: 5, 6: 'asdf'}", expected);
        }
        
        [Test]
        public void AllowsStringsToBeUsedAsKeys()
        {
            var expected = new ArrayInstruction([
                new ArrayInstruction([new StringInstruction("asdf"), new NumericInstruction(5)], delimiter: ":"),
                new ArrayInstruction([new StringInstruction("qwer"), new StringInstruction("asdf")], delimiter: ":"),
            ], separator: "{", delimiter: ",");
            Assertions.AssertInputProducesParserOutput("{'asdf': 5, 'qwer': 'asdf'}", expected);
        }
    }

    [TestFixture]
    public class Interpret
    {
        [Test]
        public void HandlesSimpleValues()
        {
            var memory = Runner.Run("x = {'asdf': 5, 'qwer': 'asdf'}");
            var expected = new DictionaryVariable(null, new Dictionary<string, Variable>()
            {
                { "asdf", BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 5) },
                { "qwer", new StringVariable("asdf") }
            });
            Assertions.AssertVariablesEqual(memory.Scopes[0]["x"], expected);
        }
        
        [Test]
        public void AllowsStringsAndNumbersToBeUsedAsKeys()
        {
            var memory = Runner.Run("x = {'asdf': 5, 4: 'asdf'}");
            var expected = new DictionaryVariable(
                new Dictionary<int, Variable>() { { 4, new StringVariable("asdf") } },
                new Dictionary<string, Variable>()
                    { { "asdf", BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 5) } });
            Assertions.AssertVariablesEqual(memory.Scopes[0]["x"], expected);
        }
        
        [Test]
        public void HandlesExpressionValues()
        {
            var memory = Runner.Run("x = {'asdf': 5 + 6, 'qwer': 3 * 4}");
            var expected = new DictionaryVariable(null, new Dictionary<string, Variable>()
            {
                { "asdf", BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 11) },
                { "qwer", BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 12) }
            });
            Assertions.AssertVariablesEqual(memory.Scopes[0]["x"], expected);
        }
    }
}
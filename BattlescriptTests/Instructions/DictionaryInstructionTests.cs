using Battlescript;

namespace BattlescriptTests.Instructions;

[TestFixture]
public static partial class InstructionTests
{
    [TestFixture]
    public class DictionaryInstructionParse
    {
        [Test]
        public void AllowsNumbersToBeUsedAsKeys()
        {
            var lexer = new Lexer("{4: 5, 6: 'asdf'}");
            var lexerResult = lexer.Run();

            var expected = new ArrayInstruction([
                new ArrayInstruction([new NumericInstruction(4), new NumericInstruction(5)], delimiter: ":"),
                new ArrayInstruction([new NumericInstruction(6), new StringInstruction("asdf")], delimiter: ":"),
            ], separator: "{", delimiter: ",");
            
            Assert.That(InstructionFactory.Create(lexerResult), Is.EqualTo(expected));
        }
        
        [Test]
        public void AllowsStringsToBeUsedAsKeys()
        {
            var lexer = new Lexer("{'asdf': 5, 'qwer': 'asdf'}");
            var lexerResult = lexer.Run();
            
            var expected = new ArrayInstruction([
                new ArrayInstruction([new StringInstruction("asdf"), new NumericInstruction(5)], delimiter: ":"),
                new ArrayInstruction([new StringInstruction("qwer"), new StringInstruction("asdf")], delimiter: ":"),
            ], separator: "{", delimiter: ",");
            
            Assert.That(InstructionFactory.Create(lexerResult), Is.EqualTo(expected));
        }
    }

    [TestFixture]
    public class DictionaryInstructionInterpret
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
            Assert.That(memory.Scopes[0]["x"], Is.EqualTo(expected));
        }
        
        [Test]
        public void AllowsStringsAndNumbersToBeUsedAsKeys()
        {
            var memory = Runner.Run("x = {'asdf': 5, 4: 'asdf'}");
            var expected = new DictionaryVariable(
                new Dictionary<int, Variable>() { { 4, new StringVariable("asdf") } },
                new Dictionary<string, Variable>()
                    { { "asdf", BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 5) } });
            Assert.That(memory.Scopes[0]["x"], Is.EqualTo(expected));
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
            Assert.That(memory.Scopes[0]["x"], Is.EqualTo(expected));
        }
    }
}
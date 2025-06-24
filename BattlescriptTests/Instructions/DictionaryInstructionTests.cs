using Battlescript;

namespace BattlescriptTests;

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
            
            var expected = new DictionaryInstruction(
                [
                    new KeyValuePairInstruction(
                        left: new IntegerInstruction(4), 
                        right: new IntegerInstruction(5)
                    ),
                    new KeyValuePairInstruction(
                        left: new IntegerInstruction(6), 
                        right: new StringInstruction("asdf")
                    )
                ]
            );
            var result = Instruction.Parse(lexerResult);
            
            Assert.That(Instruction.Parse(lexerResult), Is.EqualTo(expected));
        }
        
        [Test]
        public void AllowsStringsToBeUsedAsKeys()
        {
            var lexer = new Lexer("{'asdf': 5, 'qwer': 'asdf'}");
            var lexerResult = lexer.Run();
            
            var expected = new DictionaryInstruction(
                [
                    new KeyValuePairInstruction(
                        left: new StringInstruction("asdf"), 
                        right: new IntegerInstruction(5)
                    ),
                    new KeyValuePairInstruction(
                        left: new StringInstruction("qwer"), 
                        right: new StringInstruction("asdf")
                    )
                ]
            );
            
            Assert.That(Instruction.Parse(lexerResult), Is.EqualTo(expected));
        }
    }

    [TestFixture]
    public class DictionaryInstructionInterpret
    {
        [Test]
        public void HandlesSimpleValues()
        {
            var memory = Runner.Run("x = {'asdf': 5, 'qwer': 'asdf'}");
            var expected = new Dictionary<string, Variable>()
            {
                {
                    "x", new DictionaryVariable([
                        new KeyValuePairVariable(new StringVariable("asdf"), new IntegerVariable(5)),
                        new KeyValuePairVariable(new StringVariable("qwer"), new StringVariable("asdf"))
                    ])
                }
            };
            Assert.That(memory.Scopes[0], Is.EquivalentTo(expected));
        }
        
        [Test]
        public void AllowsStringsAndNumbersToBeUsedAsKeys()
        {
            var memory = Runner.Run("x = {'asdf': 5, 4: 'asdf'}");
            var expected = new Dictionary<string, Variable>()
            {
                {
                    "x", new DictionaryVariable([
                        new KeyValuePairVariable(new StringVariable("asdf"), new IntegerVariable(5)),
                        new KeyValuePairVariable(new IntegerVariable(4), new StringVariable("asdf"))
                    ])
                }
            };
            Assert.That(memory.Scopes[0], Is.EquivalentTo(expected));
        }
        
        [Test]
        public void HandlesExpressionValues()
        {
            var memory = Runner.Run("x = {'asdf': 5 + 6, 'qwer': 3 * 4}");
            var expected = new Dictionary<string, Variable>()
            {
                {
                    "x", new DictionaryVariable([
                        new KeyValuePairVariable(new StringVariable("asdf"), new IntegerVariable(11)),
                        new KeyValuePairVariable(new StringVariable("qwer"), new IntegerVariable(12))
                    ])
                }
            };
            Assert.That(memory.Scopes[0], Is.EquivalentTo(expected));
        }
    }
}
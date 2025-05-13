using Battlescript;

namespace BattlescriptTests;

[TestFixture]
public static class DictionaryInstructionTests
{
    [TestFixture]
    public class Parse
    {
        [Test]
        public void AllowsNumbersToBeUsedAsKeys()
        {
            var expected = new DictionaryInstruction(
                [
                    new KeyValuePairInstruction(
                        left: new NumberInstruction(4), 
                        right: new NumberInstruction(5)
                    ),
                    new KeyValuePairInstruction(
                        left: new NumberInstruction(6), 
                        right: new StringInstruction("asdf")
                    )
                ]
            );
            ParserAssertions.AssertInputProducesInstruction("{4: 5, 6: 'asdf'}", expected);
        }
        
        [Test]
        public void AllowsStringsToBeUsedAsKeys()
        {
            var expected = new DictionaryInstruction(
                [
                    new KeyValuePairInstruction(
                        left: new StringInstruction("asdf"), 
                        right: new NumberInstruction(5)
                    ),
                    new KeyValuePairInstruction(
                        left: new StringInstruction("qwer"), 
                        right: new StringInstruction("asdf")
                    )
                ]
            );
            ParserAssertions.AssertInputProducesInstruction("{'asdf': 5, 'qwer': 'asdf'}", expected);
        }
    }

    [TestFixture]
    public class Interpret
    {
        [Test]
        public void HandlesSimpleValues()
        {
            var expected = new Dictionary<string, Variable>()
            {
                {
                    "x", new DictionaryVariable([
                        new KeyValuePairVariable(new StringVariable("asdf"), new NumberVariable(5)),
                        new KeyValuePairVariable(new StringVariable("qwer"), new StringVariable("asdf"))
                    ])
                }
            };
            InterpreterAssertions.AssertInputProducesOutput(
                "x = {'asdf': 5, 'qwer': 'asdf'}", 
                [expected]);
        }
        
        [Test]
        public void HandlesExpressionValues()
        {
            var expected = new Dictionary<string, Variable>()
            {
                {
                    "x", new DictionaryVariable([
                        new KeyValuePairVariable(new StringVariable("asdf"), new NumberVariable(11)),
                        new KeyValuePairVariable(new StringVariable("qwer"), new NumberVariable(12))
                    ])
                }
            };
            InterpreterAssertions.AssertInputProducesOutput(
                "x = {'asdf': 5 + 6, 'qwer': 3 * 4}", 
                [expected]);
        }
    }
}
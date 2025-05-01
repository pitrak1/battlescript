using Battlescript;

namespace BattlescriptTests;

[TestFixture]
public static class DictionaryInstructionTests
{
    [TestFixture]
    public class Parse
    {
        [Test]
        public void HandlesSimpleKeyValuePairLists()
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
    }
}
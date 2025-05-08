using Battlescript;

namespace BattlescriptTests;

[TestFixture]
public static partial class E2ETests {
    [TestFixture]
    public class Dictionaries
    {
        [Test]
        public void SupportsDictionaryDefinition()
        {
            var input = "x = {'asdf': 5, 'qwer': '5'}";
            var expected = new DictionaryVariable(
                new List<KeyValuePairVariable>()
                {
                    new (new StringVariable("asdf"), new NumberVariable(5)),
                    new (new StringVariable("qwer"), new StringVariable("5"))
                }
            );
            E2EAssertions.AssertVariableValueFromInput(input, "x", expected);
        }

        [Test]
        public void SupportsDictionaryIndexing()
        {
            var input = "x = {'asdf': 5, 'qwer': '5'}\ny = x['qwer']";
            var expected = new StringVariable("5");
            E2EAssertions.AssertVariableValueFromInput(input, "y", expected);
        }
    }
}
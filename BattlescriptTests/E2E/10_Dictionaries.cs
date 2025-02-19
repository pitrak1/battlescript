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
            var expected = new Variable(
                Consts.VariableTypes.Dictionary, 
                new Dictionary<string, Variable>
                {
                    {"asdf", new (Consts.VariableTypes.Number, 5)},
                    {"qwer", new (Consts.VariableTypes.String, "5")}
                }
            );
            E2EAssertions.AssertVariableValueFromInput(input, "x", expected);
        }

        [Test]
        public void SupportsDictionaryIndexing()
        {
            var input = "x = {'asdf': 5, 'qwer': '5'}\ny = x['qwer']";
            var expected = new Variable(Consts.VariableTypes.String, "5");
            E2EAssertions.AssertVariableValueFromInput(input, "y", expected);
        }
    }
}
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
                    new (new StringVariable("asdf"), new IntegerVariable(5)),
                    new (new StringVariable("qwer"), new StringVariable("5"))
                }
            );
            var memory = Runner.Run(input);
            Assert.That(memory.Scopes[0], Contains.Key("x"));
            Assert.That(memory.Scopes[0]["x"], Is.EqualTo(expected));
        }

        [Test]
        public void SupportsDictionaryIndexing()
        {
            var input = "x = {'asdf': 5, 'qwer': '5'}\ny = x['qwer']";
            var expected = new StringVariable("5");
            var memory = Runner.Run(input);
            
            Assert.That(memory.Scopes[0], Contains.Key("y"));
            Assert.That(memory.Scopes[0]["y"], Is.EqualTo(expected));
        }
    }
}
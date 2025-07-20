using Battlescript;

namespace BattlescriptTests.E2ETests;

[TestFixture]
public class Dictionaries
{
    [Test]
    public void SupportsDictionaryDefinition()
    {
        var input = "x = {'asdf': 5, 'qwer': '5'}";
        var memory = Runner.Run(input);
        var expected = new DictionaryVariable(null, 
            new Dictionary<string, Variable>()
            {
                { "asdf", BsTypes.Create(memory, "int", 5)},
                {"qwer", new StringVariable("5")}
            }
        );
        Assertions.AssertVariablesEqual(memory.Scopes[0]["x"], expected);
    }

    [Test]
    public void SupportsDictionaryIndexing()
    {
        var input = "x = {'asdf': 5, 'qwer': '5'}\ny = x['qwer']";
        var expected = new StringVariable("5");
        var memory = Runner.Run(input);
        
        Assertions.AssertVariablesEqual(memory.Scopes[0]["y"], expected);
    }
}

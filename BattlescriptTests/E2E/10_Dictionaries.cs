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
                { "asdf", BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 5)},
                {"qwer", new StringVariable("5")}
            }
        );
        
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

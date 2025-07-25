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
        var expected = memory.CreateBsType(Memory.BsTypes.Dictionary, new MappingVariable(null, 
            new Dictionary<string, Variable>()
            {
                { "asdf", memory.CreateBsType(Memory.BsTypes.Int, 5)},
                {"qwer", new StringVariable("5")}
            }
        ));
        Assertions.AssertVariable(memory, "x", expected);
    }

    [Test]
    public void SupportsDictionaryIndexing()
    {
        var input = "x = {'asdf': 5, 'qwer': '5'}\ny = x['qwer']";
        var expected = new StringVariable("5");
        var memory = Runner.Run(input);
        
        Assertions.AssertVariable(memory, "y", expected);
    }
}

using Battlescript;

namespace BattlescriptTests.E2ETests;

[TestFixture]
public class DictionariesOld
{
    [Test]
    public void SupportsDictionaryDefinition()
    {
        var input = "x = {'asdf': 5, 'qwer': '5'}";
        var (callStack, closure) = Runner.Run(input);
        var expected = BsTypes.Create(BsTypes.Types.Dictionary, new MappingVariable(null, 
            new Dictionary<string, Variable>()
            {
                { "asdf", BsTypes.Create(BsTypes.Types.Int, 5)},
                {"qwer", BsTypes.Create(BsTypes.Types.String, "5")}
            }
        ));
        Assertions.AssertVariable(callStack, closure, "x", expected);
    }

    [Test]
    public void SupportsDictionaryIndexing()
    {
        var input = "x = {'asdf': 5, 'qwer': '5'}\ny = x['qwer']";
        var (callStack, closure) = Runner.Run(input);
        var expected = BsTypes.Create(BsTypes.Types.String, "5");
        
        Assertions.AssertVariable(callStack, closure, "y", expected);
    }
}

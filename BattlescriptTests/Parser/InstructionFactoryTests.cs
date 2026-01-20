using Battlescript;

namespace BattlescriptTests.ParserTests;

[TestFixture]
public class InstructionFactoryTests
{
    [Test]
    public void NoTokens()
    {
        Assert.That(InstructionFactory.Create([]), Is.Null);
    }
    
    [Test]
    public void CreatesListComprehensionIfGivenListComprehension()
    {
        var tokens = Runner.Tokenize("[x for x in [1, 2, 3]]");
        Assert.That(InstructionFactory.Create(tokens), Is.InstanceOf<ListComprehensionInstruction>());
    }
}
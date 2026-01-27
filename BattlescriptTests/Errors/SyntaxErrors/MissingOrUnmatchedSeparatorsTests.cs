using Battlescript;

namespace BattlescriptTests.Errors.SyntaxErrors;

public class MissingOrUnmatchedSeparatorsTests
{
    [Test]
    public void UnmatchedParentheses()
    {
        var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run("result = 2 * (3 + 4"));
        Assert.That(ex.Message, Is.EqualTo("unexpected EOF while parsing"));
        Assert.That(ex.Type, Is.EqualTo("SyntaxError"));
    }

    [Test]
    public void UnmatchedBrackets()
    {
        var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run("my_list = [1, 2, 3"));
        Assert.That(ex.Message, Is.EqualTo("unexpected EOF while parsing"));
        Assert.That(ex.Type, Is.EqualTo("SyntaxError"));
    }

    [Test]
    public void UnmatchedBraces()
    {
        var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run("my_dict = { 'key1': 'value1',"));
        Assert.That(ex.Message, Is.EqualTo("unexpected EOF while parsing"));
        Assert.That(ex.Type, Is.EqualTo("SyntaxError"));
    }

    [Test]
    public void UnexpectedClosingSeparator()
    {
        var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run("my_dict = { 'key1': 'value1' )}"));
        Assert.That(ex.Message, Is.EqualTo("closing parenthesis ')' does not match opening parenthesis '{'"));
        Assert.That(ex.Type, Is.EqualTo("SyntaxError"));
    }
}
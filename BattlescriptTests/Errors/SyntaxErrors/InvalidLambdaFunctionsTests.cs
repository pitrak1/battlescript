using Battlescript;

namespace BattlescriptTests.Errors.SyntaxErrors;

public class InvalidLambdaFunctionsTests
{
    [Test]
    public void MissingColon()
    {
        var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run("lambda x x + 5"));
        Assert.That(ex.Message, Is.EqualTo("invalid syntax"));
        Assert.That(ex.Type, Is.EqualTo(Memory.BsTypes.SyntaxError));
    }

    [Test]
    public void MissingExpression()
    {
        var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run("lambda x:"));
        Assert.That(ex.Message, Is.EqualTo("invalid syntax"));
        Assert.That(ex.Type, Is.EqualTo(Memory.BsTypes.SyntaxError));
    }
    
    [Test]
    public void MissingArguments()
    {
        var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run("lambda: x + 4"));
        Assert.That(ex.Message, Is.EqualTo("invalid syntax"));
        Assert.That(ex.Type, Is.EqualTo(Memory.BsTypes.SyntaxError));
    }

    [Test]
    public void MissingCommaBetweenArguments()
    {
        var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run("lambda x y: x + y"));
        Assert.That(ex.Message, Is.EqualTo("invalid syntax"));
        Assert.That(ex.Type, Is.EqualTo(Memory.BsTypes.SyntaxError));
    }
}
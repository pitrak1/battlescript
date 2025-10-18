using Battlescript;

namespace BattlescriptTests.Errors.SyntaxErrors;

public class ImproperUseOfKeywordsTests
{
    [Test]
    public void KeywordAsVariableName()
    {
        var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run("if = 3"));
        Assert.That(ex.Message, Is.EqualTo("invalid syntax"));
        Assert.That(ex.Type, Is.EqualTo("SyntaxError"));
    }
    
    [Test]
    public void KeywordUseInExpressions()
    {
        var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run("3 + if"));
        Assert.That(ex.Message, Is.EqualTo("invalid syntax"));
        Assert.That(ex.Type, Is.EqualTo("SyntaxError"));
    }
    
    [Test]
    public void KeywordUseInFunctionDefinition()
    {
        var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run("def class():\n\tpass"));
        Assert.That(ex.Message, Is.EqualTo("invalid syntax"));
        Assert.That(ex.Type, Is.EqualTo("SyntaxError"));
    }
    
    [Test]
    public void UsingReturnOutsideOfFunction()
    {
        var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run("return 'Hello, world'"));
        Assert.That(ex.Message, Is.EqualTo("'return' outside function"));
        Assert.That(ex.Type, Is.EqualTo("SyntaxError"));
    }
    
    [Test]
    public void UsingBreakOutsideOfLoop()
    {
        var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run("break"));
        Assert.That(ex.Message, Is.EqualTo("'break' outside loop"));
        Assert.That(ex.Type, Is.EqualTo("SyntaxError"));
    }
}
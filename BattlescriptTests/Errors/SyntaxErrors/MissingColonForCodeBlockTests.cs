using Battlescript;

namespace BattlescriptTests.Errors.SyntaxErrors;

public class MissingColonForCodeBlockTests
{
    [Test]
    public void ForIfStatement()
    {
        var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run("if True"));
        Assert.That(ex.Message, Is.EqualTo("invalid syntax"));
        Assert.That(ex.Type, Is.EqualTo(Memory.BsTypes.SyntaxError));
    }
    
    [Test]
    public void ForForStatement()
    {
        var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run("for i in range(10)"));
        Assert.That(ex.Message, Is.EqualTo("invalid syntax"));
        Assert.That(ex.Type, Is.EqualTo(Memory.BsTypes.SyntaxError));
    }
    
    [Test]
    public void ForWhileStatement()
    {
        var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run("while True"));
        Assert.That(ex.Message, Is.EqualTo("invalid syntax"));
        Assert.That(ex.Type, Is.EqualTo(Memory.BsTypes.SyntaxError));
    }
    
    [Test]
    public void ForElseStatement()
    {
        var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run("""
                                                                        if True:
                                                                            print('Hello, world')
                                                                        else
                                                                            print('Goodbye, world')
                                                                        """));
        Assert.That(ex.Message, Is.EqualTo("invalid syntax"));
        Assert.That(ex.Type, Is.EqualTo(Memory.BsTypes.SyntaxError));
    }
    
    [Test]
    public void FunctionDefinition()
    {
        var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run("""
                                                                        def func()
                                                                            pass
                                                                        """));
        Assert.That(ex.Message, Is.EqualTo("invalid syntax"));
        Assert.That(ex.Type, Is.EqualTo(Memory.BsTypes.SyntaxError));
    }
    
    [Test]
    public void ClassDefinition()
    {
        var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run("""
                                                                        class MyClass
                                                                            pass
                                                                        """));
        Assert.That(ex.Message, Is.EqualTo("invalid syntax"));
        Assert.That(ex.Type, Is.EqualTo(Memory.BsTypes.SyntaxError));
    }
}
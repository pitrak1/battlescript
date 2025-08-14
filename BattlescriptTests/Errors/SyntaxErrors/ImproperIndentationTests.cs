using Battlescript;

namespace BattlescriptTests.Errors.SyntaxErrors;

public class ImproperIndentationTests
{
    [Test]
    public void UnexpectedIndent()
    {
        var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run("""
                                                                        print('No indent should follow this line')
                                                                            print('Indentation error')
                                                                        """));
        Assert.That(ex.Message, Is.EqualTo("unexpected indent"));
        Assert.That(ex.Type, Is.EqualTo(Memory.BsTypes.SyntaxError));
    }
    
    [Test]
    public void UnexpectedIndentOnFirstLine()
    {
        var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run("\tprint('No indent should be on the first line of input')"));
        Assert.That(ex.Message, Is.EqualTo("unexpected indent"));
        Assert.That(ex.Type, Is.EqualTo(Memory.BsTypes.SyntaxError));
    }

    [Test]
    public void TooMuchIndentWhenExpectingIndent()
    {
        var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run("""
                                                                        if True:
                                                                                print('Indentation error')
                                                                        """));
        Assert.That(ex.Message, Is.EqualTo("unindent does not match any outer indentation level"));
        Assert.That(ex.Type, Is.EqualTo(Memory.BsTypes.SyntaxError));
    }

    [Test]
    public void NoIndentWhenExpectingIndent()
    {
        var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run("""
                                                                        if True:
                                                                        print('Indentation error')
                                                                        """));
        Assert.That(ex.Message, Is.EqualTo("expected an indented block"));
        Assert.That(ex.Type, Is.EqualTo(Memory.BsTypes.SyntaxError));
    }
}
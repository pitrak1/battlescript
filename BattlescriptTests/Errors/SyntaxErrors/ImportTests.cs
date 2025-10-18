using Battlescript;

namespace BattlescriptTests.Errors.SyntaxErrors;

public class ImportTests
{
    [Test]
    public void FileNameIsNotString()
    {
        var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run("from asdf import *"));
        Assert.That(ex.Message, Is.EqualTo("expected file path to be a string"));
        Assert.That(ex.Type, Is.EqualTo("SyntaxError"));
    }
    
    [Test]
    public void MissingImportKeyword()
    {
        var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run("from 'asdf' *"));
        Assert.That(ex.Message, Is.EqualTo("expected 'import' keyword"));
        Assert.That(ex.Type, Is.EqualTo("SyntaxError"));
    }
    
    [Test]
    public void MissingCommaBetweenImports()
    {
        var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run("from 'asdf' import foo bar"));
        Assert.That(ex.Message, Is.EqualTo("invalid syntax"));
        Assert.That(ex.Type, Is.EqualTo("SyntaxError"));
    }
}
using Battlescript;

namespace BattlescriptTests.Errors;

[TestFixture]
public class SyntaxErrorTests
{
    [TestFixture]
    public class ImproperCombinationOfStatements
    {
        [Test]
        public void MultipleStatementsWithoutDelimiters()
        {
            var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run("print('Hello, world') print('Goodbye, world')"));
            Assert.That(ex.Message, Is.EqualTo("invalid syntax"));
            Assert.That(ex.Type, Is.EqualTo("SyntaxError"));
        }
    }
    
    [TestFixture]
    public class StringLiterals
    {
        [Test]
        public void MissingClosingQuote()
        {
            var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run("'Hello, world"));
            Assert.That(ex.Message, Is.EqualTo("EOL while scanning string literal"));
            Assert.That(ex.Type, Is.EqualTo("SyntaxError"));
        }
    }

    [TestFixture]
    public class FunctionDefinitions
    {
        [Test]
        public void MissingParentheses()
        {
            var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run("def my_function:\n\tpass"));
            Assert.That(ex.Message, Is.EqualTo("invalid syntax"));
            Assert.That(ex.Type, Is.EqualTo("SyntaxError"));
        }
    }
    
    [TestFixture]
    public class VariableNames
    {
        [Test]
        public void NameStartsWithNumber()
        {
            var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run("123name = 3"));
            Assert.That(ex.Message, Is.EqualTo("invalid syntax"));
            Assert.That(ex.Type, Is.EqualTo("SyntaxError"));
        }
        
        [Test]
        public void NameStartsWithSpecialCharacter()
        {
            var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run("$variable = 3"));
            Assert.That(ex.Message, Is.EqualTo("invalid syntax"));
            Assert.That(ex.Type, Is.EqualTo("SyntaxError"));
        }
    }
    
    // [TestFixture]
    // public class EscapeSequences
    // {
    //     [Test]
    //     public void InvalidEscapeSequence()
    //     {
    //         try
    //         {
    //             Runner.Run("print('Hello, \\x')");
    //         }
    //         catch (InternalRaiseException e)
    //         {
    //             // SyntaxError: (unicode error) 'unicodeescape' codec can't decode bytes in position 7-8: truncated \xXX escape
    //         }
    //     }
    // }
    //
    // [TestFixture]
    // public class Decorators
    // {
    //     [Test]
    //     public void ImproperDecoratorPlacement()
    //     {
    //         try
    //         {
    //             Runner.Run("@123decorator\ndef my_function():\n\tpass");
    //         }
    //         catch (InternalRaiseException e)
    //         {
    //             // SyntaxError: invalid syntax
    //         }
    //     }
    //
    //     [Test]
    //     public void DecoratorWithoutFunction()
    //     {
    //         try
    //         {
    //             Runner.Run("@decorator_function");
    //         }
    //         catch (InternalRaiseException e)
    //         {
    //             // SyntaxError: unexpected EOF while parsing
    //         }
    //     }
    // }
}
using Battlescript;

namespace BattlescriptTests.Errors;

[TestFixture]
public class SyntaxErrorTests
{
    [TestFixture]
    public class MissingOrUnmatchedSeparators
    {

        [Test]
        public void MissingParenthesesInFunctionCall()
        {
            var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run("print 'Hello, world'"));
            Assert.That(ex.Message, Is.EqualTo("Missing parentheses in call to 'print'"));
            Assert.That(ex.Type, Is.EqualTo(Memory.BsTypes.SyntaxError));
        }

        [Test]
        public void UnmatchedParentheses()
        {
            var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run("result = (2 * (3 + 4"));
            Assert.That(ex.Message, Is.EqualTo("unexpected EOF while parsing"));
            Assert.That(ex.Type, Is.EqualTo(Memory.BsTypes.SyntaxError));
        }

        [Test]
        public void UnmatchedBrackets()
        {
            var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run("my_list = [1, 2, 3"));
            Assert.That(ex.Message, Is.EqualTo("unexpected EOF while parsing"));
            Assert.That(ex.Type, Is.EqualTo(Memory.BsTypes.SyntaxError));
        }

        [Test]
        public void UnmatchedBraces()
        {
            var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run("my_dict = { 'key1': 'value1',"));
            Assert.That(ex.Message, Is.EqualTo("unexpected EOF while parsing"));
            Assert.That(ex.Type, Is.EqualTo(Memory.BsTypes.SyntaxError));
        }
    }

    [TestFixture]
    public class ImproperIndentation
    {
        [Test]
        public void UnexpectedIndent()
        {
            try
            {
                Runner.Run("\tprint('IndentationError')");
            }
            catch (InternalRaiseException e)
            {
                // SyntaxError: unexpected indent
            }
        }

        [Test]
        public void InconsistentIndent()
        {
            try
            {
                Runner.Run("""
                           if True:
                                    print("Good")
                                print("Bad")
                           """);
            }
            catch (InternalRaiseException e)
            {
                // SyntaxError: unindent does not match any outer indentation level
            }
        }
    }

    [TestFixture]
    public class InvalidAssignment
    {
        [Test]
        public void AssigningToALiteral()
        {
            try
            {
                Runner.Run("3 = x");
            }
            catch (InternalRaiseException e)
            {
                // SyntaxError: cannot assign to literal
            }
        }

        [Test]
        public void AssigningToAFunctionCall()
        {
            try
            {
                Runner.Run("print(x) = 3");
            }
            catch (InternalRaiseException e)
            {
                // SyntaxError: cannot assign to function call
            }
        }
    }

    [TestFixture]
    public class InvalidKeywordUse
    {
        [Test]
        public void KeywordAsVariableName()
        {
            try
            {
                Runner.Run("if = 3");
            }
            catch (InternalRaiseException e)
            {
                // SyntaxError: invalid syntax
            }
        }

        [Test]
        public void KeywordUseInExpressions()
        {
            try
            {
                Runner.Run("3 + if");
            }
            catch (InternalRaiseException e)
            {
                // SyntaxError: invalid syntax
            }
        }
    }

    [TestFixture]
    public class ImproperCombinationOfStatements
    {
        [Test]
        public void MultipleStatementsWithoutDelimiters()
        {
            try
            {
                Runner.Run("print('Hello, world') print('Goodbye, world')");
            }
            catch (InternalRaiseException e)
            {
                // SyntaxError: invalid syntax
            }
        }

        [Test]
        public void MisplacedReturnStatement()
        {
            try
            {
                Runner.Run("return 'Hello, world'");
            }
            catch (InternalRaiseException e)
            {
                // SyntaxError: 'return' outside function
            }
        }
    }

    [TestFixture]
    public class StringLiterals
    {
        [Test]
        public void MissingClosingQuote()
        {
            try
            {
                Runner.Run("'Hello, world");
            }
            catch (InternalRaiseException e)
            {
                // SyntaxError: EOL while scanning string literal
            }
        }

        [Test]
        public void IncorrectlyNestedStrings()
        {
            // THIS PASSES IN PYTHON *shrug*
            try
            {
                Runner.Run("This string is 'invalid");
            }
            catch (InternalRaiseException e)
            {
                // SyntaxError: EOL while scanning string literal
            }
        }
    }

    [TestFixture]
    public class FunctionDefinitions
    {
        [Test]
        public void MissingColon()
        {
            try
            {
                Runner.Run("def my_function(x, y)");
            }
            catch (InternalRaiseException e)
            {
                // SyntaxError: invalid syntax
            }
        }
        
        [Test]
        public void NoIndentedBlock()
        {
            try
            {
                Runner.Run("""
                           def my_function(x, y):
                           print('Hello, world')
                           """);
            }
            catch (InternalRaiseException e)
            {
                // IndentationError: expected an indented block
            }
        }

        [Test]
        public void MissingParentheses()
        {
            try
            {
                Runner.Run("""
                           def my_function:
                            pass
                           """);
            }
            catch (InternalRaiseException e)
            {
                // SyntaxError: invalid syntax
            }
        }
        
        [Test]
        public void ReservedWordsInFunctionName()
        {
            try
            {
                Runner.Run("""
                           def class():
                            pass
                           """);
            }
            catch (InternalRaiseException e)
            {
                // SyntaxError: invalid syntax
            }
        }
    }

    [TestFixture]
    public class Imports
    {
        [Test]
        public void InvalidSyntaxImport()
        {
            try
            {
                Runner.Run("import 123module");
            }
            catch (InternalRaiseException e)
            {
                // SyntaxError: invalid syntax
            }
        }

        [Test]
        public void InvalidSyntaxImportFrom()
        {
            try
            {
                Runner.Run("from 123module import * foo");
            }
            catch (InternalRaiseException e)
            {
                // SyntaxError: invalid syntax
            }
        }
    }

    [TestFixture]
    public class ImproperLoopsOrConditionals
    {
        [Test]
        public void MissingColonInLoop()
        {
            try
            {
                Runner.Run("while True\n\tprint('Hello, world')");
            }
            catch (InternalRaiseException e)
            {
                // SyntaxError: invalid syntax
            }
        }
        
        [Test]
        public void MissingColonInIf()
        {
            try
            {
                Runner.Run("if True\n\tprint('Hello, world')");
            }
            catch (InternalRaiseException e)
            {
                // SyntaxError: invalid syntax
            }
        }
    }

    [TestFixture]
    public class InvalidLambdaFunctions
    {
        [Test]
        public void LambdaWithoutExpression()
        {
            try
            {
                Runner.Run("lambda x: ");
            }
            catch (InternalRaiseException e)
            {
                // SyntaxError: invalid syntax
            }
        }
    }

    [TestFixture]
    public class EscapeSequences
    {
        [Test]
        public void InvalidEscapeSequence()
        {
            try
            {
                Runner.Run("print('Hello, \\x')");
            }
            catch (InternalRaiseException e)
            {
                // SyntaxError: (unicode error) 'unicodeescape' codec can't decode bytes in position 7-8: truncated \xXX escape
            }
        }
    }

    [TestFixture]
    public class Decorators
    {
        [Test]
        public void ImproperDecoratorPlacement()
        {
            try
            {
                Runner.Run("@123decorator\ndef my_function():\n\tpass");
            }
            catch (InternalRaiseException e)
            {
                // SyntaxError: invalid syntax
            }
        }

        [Test]
        public void DecoratorWithoutFunction()
        {
            try
            {
                Runner.Run("@decorator_function");
            }
            catch (InternalRaiseException e)
            {
                // SyntaxError: unexpected EOF while parsing
            }
        }
    }

    [TestFixture]
    public class VariableNames
    {
        [Test]
        public void NameStartsWithNumber()
        {
            try
            {
                Runner.Run("123name = 3");
            }
            catch (InternalRaiseException e)
            {
                // SyntaxError: invalid syntax
            }
        }
        
        [Test]
        public void NameStartsWithSpecialCharacter()
        {
            try
            {
                Runner.Run("$variable = 3");
            }
            catch (InternalRaiseException e)
            {
                // SyntaxError: invalid syntax
            }
        }
    }
}
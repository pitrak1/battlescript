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
            var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run(
                "\tprint('IndentationError')"));
            Assert.That(ex.Message, Is.EqualTo("unexpected indent"));
            Assert.That(ex.Type, Is.EqualTo(Memory.BsTypes.SyntaxError));
        }

        [Test]
        public void InconsistentIndent()
        {
            var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run(
                "if True:\n\t\tprint('Good')\n\tprint('Bad')"));
            Assert.That(ex.Message, Is.EqualTo("unindent does not match any outer indentation level"));
            Assert.That(ex.Type, Is.EqualTo(Memory.BsTypes.SyntaxError));
        }
    }

    [TestFixture]
    public class InvalidAssignment
    {
        [Test]
        public void AssigningToALiteral()
        {
            var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run("3 = x"));
            Assert.That(ex.Message, Is.EqualTo("cannot assign to literal"));
            Assert.That(ex.Type, Is.EqualTo(Memory.BsTypes.SyntaxError));
        }
    
        [Test]
        public void AssigningToAFunctionCall()
        {
            var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run("print('asdf') = 3"));
            Assert.That(ex.Message, Is.EqualTo("cannot assign to function call"));
            Assert.That(ex.Type, Is.EqualTo(Memory.BsTypes.SyntaxError));
        }
        
        [Test]
        public void AssigningToAUserDefinedFunctionCall()
        {
            var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run("""
                                                                            def func():
                                                                                pass
                                                                            
                                                                            func() = 3
                                                                            """));
            Assert.That(ex.Message, Is.EqualTo("cannot assign to function call"));
            Assert.That(ex.Type, Is.EqualTo(Memory.BsTypes.SyntaxError));
        }
        
        [Test]
        public void AssigningToAFunctionCallWithIndex()
        {
            var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run("""
                                                                            def func():
                                                                                pass
                                                                                
                                                                            x = [func]
                                                                            x[0]() = 3
                                                                            """));
            Assert.That(ex.Message, Is.EqualTo("cannot assign to function call"));
            Assert.That(ex.Type, Is.EqualTo(Memory.BsTypes.SyntaxError));
        }
        
        [Test]
        public void AssigningToAFunctionCallWithMember()
        {
            var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run("""
                                                                            class MyClass:
                                                                                def func(self):
                                                                                    pass
                                                                                
                                                                            x = MyClass()
                                                                            x.func() = 3
                                                                            """));
            Assert.That(ex.Message, Is.EqualTo("cannot assign to function call"));
            Assert.That(ex.Type, Is.EqualTo(Memory.BsTypes.SyntaxError));
        }
    }
    
    [TestFixture]
    public class InvalidKeywordUse
    {
        [Test]
        public void KeywordAsVariableName()
        {
            var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run("if = 3"));
            Assert.That(ex.Message, Is.EqualTo("invalid syntax"));
            Assert.That(ex.Type, Is.EqualTo(Memory.BsTypes.SyntaxError));
        }
    
        [Test]
        public void KeywordUseInExpressions()
        {
            var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run("3 + if"));
            Assert.That(ex.Message, Is.EqualTo("invalid syntax"));
            Assert.That(ex.Type, Is.EqualTo(Memory.BsTypes.SyntaxError));
        }
    }
    
    [TestFixture]
    public class ImproperCombinationOfStatements
    {
        [Test]
        public void MultipleStatementsWithoutDelimiters()
        {
            var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run("print('Hello, world') print('Goodbye, world')"));
            Assert.That(ex.Message, Is.EqualTo("invalid syntax"));
            Assert.That(ex.Type, Is.EqualTo(Memory.BsTypes.SyntaxError));
        }
    
        [Test]
        public void MisplacedReturnStatement()
        {
            var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run("return 'Hello, world'"));
            Assert.That(ex.Message, Is.EqualTo("'return' outside function"));
            Assert.That(ex.Type, Is.EqualTo(Memory.BsTypes.SyntaxError));
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
            Assert.That(ex.Type, Is.EqualTo(Memory.BsTypes.SyntaxError));
        }
    }

    [TestFixture]
    public class FunctionDefinitions
    {
        [Test]
        public void MissingColon()
        {
            var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run("def my_function(x, y)"));
            Assert.That(ex.Message, Is.EqualTo("invalid syntax"));
            Assert.That(ex.Type, Is.EqualTo(Memory.BsTypes.SyntaxError));
        }
        
        [Test]
        public void NoIndentedBlock()
        {
            var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run("def my_function(x, y):\nprint('Hello, world')"));
            Assert.That(ex.Message, Is.EqualTo("expected an indented block"));
            Assert.That(ex.Type, Is.EqualTo(Memory.BsTypes.SyntaxError));
        }
    
        [Test]
        public void MissingParentheses()
        {
            var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run("def my_function:\n\tpass"));
            Assert.That(ex.Message, Is.EqualTo("invalid syntax"));
            Assert.That(ex.Type, Is.EqualTo(Memory.BsTypes.SyntaxError));
        }
        
        [Test]
        public void ReservedWordsInFunctionName()
        {
            var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run("def class():\n\tpass"));
            Assert.That(ex.Message, Is.EqualTo("invalid syntax"));
            Assert.That(ex.Type, Is.EqualTo(Memory.BsTypes.SyntaxError));
        }
    }
    
    // [TestFixture]
    // public class Imports
    // {
    //     [Test]
    //     public void InvalidSyntaxImport()
    //     {
    //         try
    //         {
    //             Runner.Run("import 123module");
    //         }
    //         catch (InternalRaiseException e)
    //         {
    //             // SyntaxError: invalid syntax
    //         }
    //     }
    //
    //     [Test]
    //     public void InvalidSyntaxImportFrom()
    //     {
    //         try
    //         {
    //             Runner.Run("from 123module import * foo");
    //         }
    //         catch (InternalRaiseException e)
    //         {
    //             // SyntaxError: invalid syntax
    //         }
    //     }
    // }
    //
    // [TestFixture]
    // public class ImproperLoopsOrConditionals
    // {
    //     [Test]
    //     public void MissingColonInLoop()
    //     {
    //         try
    //         {
    //             Runner.Run("while True\n\tprint('Hello, world')");
    //         }
    //         catch (InternalRaiseException e)
    //         {
    //             // SyntaxError: invalid syntax
    //         }
    //     }
    //     
    //     [Test]
    //     public void MissingColonInIf()
    //     {
    //         try
    //         {
    //             Runner.Run("if True\n\tprint('Hello, world')");
    //         }
    //         catch (InternalRaiseException e)
    //         {
    //             // SyntaxError: invalid syntax
    //         }
    //     }
    // }
    //
    // [TestFixture]
    // public class InvalidLambdaFunctions
    // {
    //     [Test]
    //     public void LambdaWithoutExpression()
    //     {
    //         try
    //         {
    //             Runner.Run("lambda x: ");
    //         }
    //         catch (InternalRaiseException e)
    //         {
    //             // SyntaxError: invalid syntax
    //         }
    //     }
    // }
    //
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
    //
    // [TestFixture]
    // public class VariableNames
    // {
    //     [Test]
    //     public void NameStartsWithNumber()
    //     {
    //         try
    //         {
    //             Runner.Run("123name = 3");
    //         }
    //         catch (InternalRaiseException e)
    //         {
    //             // SyntaxError: invalid syntax
    //         }
    //     }
    //     
    //     [Test]
    //     public void NameStartsWithSpecialCharacter()
    //     {
    //         try
    //         {
    //             Runner.Run("$variable = 3");
    //         }
    //         catch (InternalRaiseException e)
    //         {
    //             // SyntaxError: invalid syntax
    //         }
    //     }
    // }
}
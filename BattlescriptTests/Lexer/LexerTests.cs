using Battlescript;

namespace BattlescriptTests.LexerTests;

[TestFixture]
public static class LexerTests
{
    [TestFixture]
    public class Numerics
    {
        [Test]
        public void HandlesIntegers()
        {
            var expected = new List<Token>() { new Token(Consts.TokenTypes.Numeric, "213") };
            Assertions.AssertInputProducesLexerOutput("213", expected);
        }

        [Test]
        public void HandlesFloats()
        {
            var expected = new List<Token>() { new Token(Consts.TokenTypes.Numeric, "1.23") };
            Assertions.AssertInputProducesLexerOutput("1.23", expected);
        }
        
        [Test]
        public void HandlesFloatsWithStartingDecimal()
        {
            var expected = new List<Token>() { new Token(Consts.TokenTypes.Numeric, ".23") };
            Assertions.AssertInputProducesLexerOutput(".23", expected);
        }
    }

    [TestFixture]
    public class Strings
    {
        [Test]
        public void HandlesDoubleQuotes()
        {
            var expected = new List<Token>() { new Token(Consts.TokenTypes.String, "asdf") };
            Assertions.AssertInputProducesLexerOutput("\"asdf\"", expected);
        }

        [Test]
        public void HandlesSingleQuotes()
        {
            var expected = new List<Token>() { new Token(Consts.TokenTypes.String, "asdf") };
            Assertions.AssertInputProducesLexerOutput("'asdf'", expected);
        }

        [Test]
        public void HandlesFormattedStringsWithSingleQuotes()
        {
            var expected = new List<Token>() { new Token(Consts.TokenTypes.FormattedString, "asdf") };
            Assertions.AssertInputProducesLexerOutput("f'asdf'", expected);
        }
        
        [Test]
        public void HandlesFormattedStringsWithDoubleQuotes()
        {
            var expected = new List<Token>() { new Token(Consts.TokenTypes.FormattedString, "asdf") };
            Assertions.AssertInputProducesLexerOutput("f\"asdf\"", expected);
        }
    }

    [TestFixture]
    public class Separators
    {
        [Test]
        public void HandlesSeparators()
        {
            var expected = new List<Token>() { new Token(Consts.TokenTypes.Separator, ",") };
            Assertions.AssertInputProducesLexerOutput(",", expected);
        }
    }

    [TestFixture]
    public class Words
    {
        [Test]
        public void HandlesKeywords()
        {
            var expected = new List<Token>() { new Token(Consts.TokenTypes.Keyword, "async") };
            Assertions.AssertInputProducesLexerOutput("async", expected);
        }

        [Test]
        public void HandlesConstants()
        {
            var expected = new List<Token>() { new Token(Consts.TokenTypes.Constant, "True") };
            Assertions.AssertInputProducesLexerOutput("True", expected);
        }

        [Test]
        public void HandlesWordOperators()
        {
            var expected = new List<Token>() { new Token(Consts.TokenTypes.Operator, "and") };
            Assertions.AssertInputProducesLexerOutput("and", expected);
        }

        [Test]
        public void HandlesIdentifiers()
        {
            var expected = new List<Token>() { new Token(Consts.TokenTypes.Identifier, "asdf") };
            Assertions.AssertInputProducesLexerOutput("asdf", expected);
        }

        [Test]
        public void HandlesBuiltIns()
        {
            var expected = new List<Token>()
            {
                new Token(Consts.TokenTypes.BuiltIn, "super"), 
                new Token(Consts.TokenTypes.Separator, "("), 
                new Token(Consts.TokenTypes.Separator, ")")
            };
            Assertions.AssertInputProducesLexerOutput("super()", expected);
        }
    }

    [TestFixture]
    public class Operators
    {
        [Test]
        public void HandlesSingleCharacterOperators()
        {
            var expected = new List<Token>() { new Token(Consts.TokenTypes.Operator, "+") };
            Assertions.AssertInputProducesLexerOutput("+", expected);
        }
        
        [Test]
        public void HandlesDoubleCharacterOperators()
        {
            var expected = new List<Token>() { new Token(Consts.TokenTypes.Operator, "**") };
            Assertions.AssertInputProducesLexerOutput("**", expected);
        }
        
        [Test]
        public void HandlesTripleCharacterOperators()
        {
            var expected = new List<Token>() { new Token(Consts.TokenTypes.Operator, "not") };
            Assertions.AssertInputProducesLexerOutput("not", expected);
        }
        
        [Test]
        public void HandlesIsNotOperator()
        {
            var expected = new List<Token>()
            {
                new(Consts.TokenTypes.Identifier, "x"),
                new(Consts.TokenTypes.Operator, "is not"),
                new(Consts.TokenTypes.Identifier, "y"),
            };
            Assertions.AssertInputProducesLexerOutput("x is not y", expected);
        }
        
        [Test]
        public void HandlesNotInOperator()
        {
            var expected = new List<Token>()
            {
                new(Consts.TokenTypes.Identifier, "x"),
                new(Consts.TokenTypes.Operator, "not in"),
                new(Consts.TokenTypes.Identifier, "y"),
            };
            Assertions.AssertInputProducesLexerOutput("x not in y", expected);
        }
    }
    
    [TestFixture]
    public class AssignmentOperators
    {
        [Test]
        public void HandlesSingleCharacterOperators()
        {
            var expected = new List<Token>() { new Token(Consts.TokenTypes.Assignment, "=") };
            Assertions.AssertInputProducesLexerOutput("=", expected);
        }
        
        [Test]
        public void HandlesDoubleCharacterOperators()
        {
            var expected = new List<Token>() { new Token(Consts.TokenTypes.Assignment, "*=") };
            Assertions.AssertInputProducesLexerOutput("*=", expected);
        }
        
        [Test]
        public void HandlesTripleCharacterOperators()
        {
            var expected = new List<Token>() { new Token(Consts.TokenTypes.Assignment, "**=") };
            Assertions.AssertInputProducesLexerOutput("**=", expected);
        }
    }

    [TestFixture]
    public class Comments
    {
        [Test]
        public void IgnoresComments()
        {
            var expected = new List<Token>() { new Token(Consts.TokenTypes.Assignment, "=") };
            Assertions.AssertInputProducesLexerOutput("= #asdfadsf", expected);
        }
    }

    [TestFixture]
    public class EscapedCharacters
    {
        [Test]
        public void IgnoresEscapedReturns()
        {
            var expected = new List<Token>() { new Token(Consts.TokenTypes.Numeric, "1234"), new Token(Consts.TokenTypes.Numeric, "2345") };
            Assertions.AssertInputProducesLexerOutput("""
                                                      1234\
                                                      2345
                                                      """, expected);
        }
        
        [Test]
        public void IncludesEscapedCharactersInStrings()
        {
            var expected = new List<Token>() { new Token(Consts.TokenTypes.String, """asdf\'asdf""") };
            Assertions.AssertInputProducesLexerOutput("""'asdf\'asdf'""", expected);
        }
    }
    
    [TestFixture]
    public class NewLine
    {
        [Test]
        public void DetectsNewLineAndIndents()
        {
            // This has four spaces in addition to two tabs, meaning it should detect three tabs
            var expected = new List<Token>()
            {
                new(Consts.TokenTypes.Identifier, "asdf"),
                new(Consts.TokenTypes.Newline, "3"),
                new(Consts.TokenTypes.Identifier, "asdf")
            };
            Assertions.AssertInputProducesLexerOutput("asdf\n\t\t    asdf", expected);
        }

        [Test]
        public void IgnoresTrailingSpaces()
        {
            // This has three spaces
            var expected = new List<Token>()
            {
                new(Consts.TokenTypes.Identifier, "asdf"),
                new(Consts.TokenTypes.Newline, "2"),
                new(Consts.TokenTypes.Identifier, "asdf")
            };
            Assertions.AssertInputProducesLexerOutput("asdf\n\t\t   asdf", expected);
        }
        
        [Test]
        public void HandlesNoIndent()
        {
            var expected = new List<Token>()
            {
                new(Consts.TokenTypes.Identifier, "asdf"),
                new(Consts.TokenTypes.Newline, "0"),
                new(Consts.TokenTypes.Identifier, "asdf")
            };
            Assertions.AssertInputProducesLexerOutput("asdf\nasdf", expected);
        }
    }
}
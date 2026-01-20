using Battlescript;

namespace BattlescriptTests.LexerTests;

[TestFixture]
public class LexerTests
{
    [TestFixture]
    public class Numerics
    {
        [Test]
        public void Integers()
        {
            var expected = new List<Token>() { new Token(Consts.TokenTypes.Numeric, "213") };
            Assertions.AssertInputProducesLexerOutput("213", expected);
        }

        [Test]
        public void Floats()
        {
            var expected = new List<Token>() { new Token(Consts.TokenTypes.Numeric, "1.23") };
            Assertions.AssertInputProducesLexerOutput("1.23", expected);
        }

        [Test]
        public void FloatsWithLeadingDecimal()
        {
            var expected = new List<Token>() { new Token(Consts.TokenTypes.Numeric, ".23") };
            Assertions.AssertInputProducesLexerOutput(".23", expected);
        }
    }

    [TestFixture]
    public class Strings
    {
        [Test]
        public void DoubleQuotes()
        {
            var expected = new List<Token>() { new Token(Consts.TokenTypes.String, "asdf") };
            Assertions.AssertInputProducesLexerOutput("\"asdf\"", expected);
        }

        [Test]
        public void SingleQuotes()
        {
            var expected = new List<Token>() { new Token(Consts.TokenTypes.String, "asdf") };
            Assertions.AssertInputProducesLexerOutput("'asdf'", expected);
        }

        [Test]
        public void FormattedStringsWithSingleQuotes()
        {
            var expected = new List<Token>() { new Token(Consts.TokenTypes.FormattedString, "asdf") };
            Assertions.AssertInputProducesLexerOutput("f'asdf'", expected);
        }

        [Test]
        public void FormattedStringsWithDoubleQuotes()
        {
            var expected = new List<Token>() { new Token(Consts.TokenTypes.FormattedString, "asdf") };
            Assertions.AssertInputProducesLexerOutput("f\"asdf\"", expected);
        }
    }

    [Test]
    public void Brackets()
    {
        var expected = new List<Token>() { new Token(Consts.TokenTypes.Bracket, "["), new Token(Consts.TokenTypes.Bracket, "]") };
        Assertions.AssertInputProducesLexerOutput("[]", expected);
    }

    [Test]
    public void Delimiters()
    {
        var expected = new List<Token>() { new Token(Consts.TokenTypes.Delimiter, ":") };
        Assertions.AssertInputProducesLexerOutput(":", expected);
    }

    [Test]
    public void Periods()
    {
        var expected = new List<Token>() { new Token(Consts.TokenTypes.Period, ".") };
        Assertions.AssertInputProducesLexerOutput(".", expected);
    }

    [TestFixture]
    public class Words
    {
        [Test]
        public void Keywords()
        {
            var expected = new List<Token>() { new Token(Consts.TokenTypes.Keyword, "async") };
            Assertions.AssertInputProducesLexerOutput("async", expected);
        }

        [Test]
        public void Constants()
        {
            var expected = new List<Token>() { new Token(Consts.TokenTypes.Constant, "True") };
            Assertions.AssertInputProducesLexerOutput("True", expected);
        }

        [Test]
        public void WordOperators()
        {
            var expected = new List<Token>() { new Token(Consts.TokenTypes.Operator, "and") };
            Assertions.AssertInputProducesLexerOutput("and", expected);
        }

        [Test]
        public void Identifiers()
        {
            var expected = new List<Token>() { new Token(Consts.TokenTypes.Identifier, "asdf") };
            Assertions.AssertInputProducesLexerOutput("asdf", expected);
        }

        [Test]
        public void BuiltIns()
        {
            var expected = new List<Token>()
            {
                new Token(Consts.TokenTypes.BuiltIn, "super"),
                new Token(Consts.TokenTypes.Bracket, "("),
                new Token(Consts.TokenTypes.Bracket, ")")
            };
            Assertions.AssertInputProducesLexerOutput("super()", expected);
        }
    }

    [TestFixture]
    public class Operators
    {
        [Test]
        public void SingleCharacterOperators()
        {
            var expected = new List<Token>() { new Token(Consts.TokenTypes.Operator, "+") };
            Assertions.AssertInputProducesLexerOutput("+", expected);
        }

        [Test]
        public void DoubleCharacterOperators()
        {
            var expected = new List<Token>() { new Token(Consts.TokenTypes.Operator, "**") };
            Assertions.AssertInputProducesLexerOutput("**", expected);
        }

        [Test]
        public void TripleCharacterOperators()
        {
            var expected = new List<Token>() { new Token(Consts.TokenTypes.Operator, "not") };
            Assertions.AssertInputProducesLexerOutput("not", expected);
        }

        [Test]
        public void IsNotOperator()
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
        public void NotInOperator()
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
        public void SingleCharacterOperators()
        {
            var expected = new List<Token>() { new Token(Consts.TokenTypes.Assignment, "=") };
            Assertions.AssertInputProducesLexerOutput("=", expected);
        }

        [Test]
        public void DoubleCharacterOperators()
        {
            var expected = new List<Token>() { new Token(Consts.TokenTypes.Assignment, "*=") };
            Assertions.AssertInputProducesLexerOutput("*=", expected);
        }

        [Test]
        public void TripleCharacterOperators()
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
            var expected = new List<Token>() { new Token(Consts.TokenTypes.String, @"asdf'asdf") };
            Assertions.AssertInputProducesLexerOutput(@"'asdf\'asdf'", expected);
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
        public void NoIndent()
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
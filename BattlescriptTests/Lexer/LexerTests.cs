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
            var expected = new List<Token> { new(Consts.TokenTypes.Numeric, "213") };
            var result = Runner.Tokenize("213");
            Assert.That(result, Is.EquivalentTo(expected));
        }

        [Test]
        public void Floats()
        {
            var expected = new List<Token> { new(Consts.TokenTypes.Numeric, "1.23") };
            var result = Runner.Tokenize("1.23");
            Assert.That(result, Is.EquivalentTo(expected));
        }

        [Test]
        public void FloatsWithLeadingDecimal()
        {
            var expected = new List<Token> { new(Consts.TokenTypes.Numeric, ".23") };
            var result = Runner.Tokenize(".23");
            Assert.That(result, Is.EquivalentTo(expected));
        }
    }

    [TestFixture]
    public class Strings
    {
        [Test]
        public void DoubleQuotes()
        {
            var expected = new List<Token> { new(Consts.TokenTypes.String, "asdf") };
            var result = Runner.Tokenize("\"asdf\"");
            Assert.That(result, Is.EquivalentTo(expected));
        }

        [Test]
        public void SingleQuotes()
        {
            var expected = new List<Token> { new(Consts.TokenTypes.String, "asdf") };
            var result = Runner.Tokenize("'asdf'");
            Assert.That(result, Is.EquivalentTo(expected));
        }

        [Test]
        public void FormattedStringsWithSingleQuotes()
        {
            var expected = new List<Token> { new(Consts.TokenTypes.FormattedString, "asdf") };
            var result = Runner.Tokenize("f'asdf'");
            Assert.That(result, Is.EquivalentTo(expected));
        }

        [Test]
        public void FormattedStringsWithDoubleQuotes()
        {
            var expected = new List<Token> { new(Consts.TokenTypes.FormattedString, "asdf") };
            var result = Runner.Tokenize("f\"asdf\"");
            Assert.That(result, Is.EquivalentTo(expected));
        }
    }

    [Test]
    public void Brackets()
    {
        var expected = new List<Token> { new(Consts.TokenTypes.Bracket, "["), new(Consts.TokenTypes.Bracket, "]") };
        var result = Runner.Tokenize("[]");
        Assert.That(result, Is.EquivalentTo(expected));
    }

    [Test]
    public void Delimiters()
    {
        var expected = new List<Token> { new(Consts.TokenTypes.Delimiter, ":") };
        var result = Runner.Tokenize(":");
        Assert.That(result, Is.EquivalentTo(expected));
    }

    [Test]
    public void Periods()
    {
        var expected = new List<Token> { new(Consts.TokenTypes.Period, ".") };
        var result = Runner.Tokenize(".");
        Assert.That(result, Is.EquivalentTo(expected));
    }

    [TestFixture]
    public class Words
    {
        [Test]
        public void Keywords()
        {
            var expected = new List<Token> { new(Consts.TokenTypes.Keyword, "async") };
            var result = Runner.Tokenize("async");
            Assert.That(result, Is.EquivalentTo(expected));
        }

        [Test]
        public void Constants()
        {
            var expected = new List<Token> { new(Consts.TokenTypes.Constant, "True") };
            var result = Runner.Tokenize("True");
            Assert.That(result, Is.EquivalentTo(expected));
        }

        [Test]
        public void WordOperators()
        {
            var expected = new List<Token> { new(Consts.TokenTypes.Operator, "and") };
            var result = Runner.Tokenize("and");
            Assert.That(result, Is.EquivalentTo(expected));
        }

        [Test]
        public void Identifiers()
        {
            var expected = new List<Token> { new(Consts.TokenTypes.Identifier, "asdf") };
            var result = Runner.Tokenize("asdf");
            Assert.That(result, Is.EquivalentTo(expected));
        }
    }

    [TestFixture]
    public class SpecialVariables
    {
        [Test]
        public void SingleAsteriskParameter()
        {
            var expected = new List<Token> { new(Consts.TokenTypes.SpecialVariable, "*args") };
            var result = Runner.Tokenize("*args");
            Assert.That(result, Is.EquivalentTo(expected));
        }

        [Test]
        public void DoubleAsteriskParameter()
        {
            var expected = new List<Token> { new(Consts.TokenTypes.SpecialVariable, "**kwargs") };
            var result = Runner.Tokenize("**kwargs");
            Assert.That(result, Is.EquivalentTo(expected));
        }
    }

    [TestFixture]
    public class Operators
    {
        [Test]
        public void SingleCharacterOperators()
        {
            var expected = new List<Token> { new(Consts.TokenTypes.Operator, "+") };
            var result = Runner.Tokenize("+");
            Assert.That(result, Is.EquivalentTo(expected));
        }

        [Test]
        public void DoubleCharacterOperators()
        {
            var expected = new List<Token> { new(Consts.TokenTypes.Operator, "**") };
            var result = Runner.Tokenize("**");
            Assert.That(result, Is.EquivalentTo(expected));
        }

        [Test]
        public void TripleCharacterOperators()
        {
            var expected = new List<Token> { new(Consts.TokenTypes.Operator, "not") };
            var result = Runner.Tokenize("not");
            Assert.That(result, Is.EquivalentTo(expected));
        }

        [Test]
        public void IsNotOperator()
        {
            var expected = new List<Token>
            {
                new(Consts.TokenTypes.Identifier, "x"),
                new(Consts.TokenTypes.Operator, "is not"),
                new(Consts.TokenTypes.Identifier, "y")
            };
            var result = Runner.Tokenize("x is not y");
            Assert.That(result, Is.EquivalentTo(expected));
        }

        [Test]
        public void NotInOperator()
        {
            var expected = new List<Token>
            {
                new(Consts.TokenTypes.Identifier, "x"),
                new(Consts.TokenTypes.Operator, "not in"),
                new(Consts.TokenTypes.Identifier, "y")
            };
            var result = Runner.Tokenize("x not in y");
            Assert.That(result, Is.EquivalentTo(expected));
        }
    }

    [TestFixture]
    public class AssignmentOperators
    {
        [Test]
        public void SingleCharacterOperators()
        {
            var expected = new List<Token> { new(Consts.TokenTypes.Assignment, "=") };
            var result = Runner.Tokenize("=");
            Assert.That(result, Is.EquivalentTo(expected));
        }

        [Test]
        public void DoubleCharacterOperators()
        {
            var expected = new List<Token> { new(Consts.TokenTypes.Assignment, "*=") };
            var result = Runner.Tokenize("*=");
            Assert.That(result, Is.EquivalentTo(expected));
        }

        [Test]
        public void TripleCharacterOperators()
        {
            var expected = new List<Token> { new(Consts.TokenTypes.Assignment, "**=") };
            var result = Runner.Tokenize("**=");
            Assert.That(result, Is.EquivalentTo(expected));
        }
    }

    [TestFixture]
    public class Comments
    {
        [Test]
        public void IgnoresComments()
        {
            var expected = new List<Token> { new(Consts.TokenTypes.Assignment, "=") };
            var result = Runner.Tokenize("= #asdfadsf");
            Assert.That(result, Is.EquivalentTo(expected));
        }
    }

    [TestFixture]
    public class EscapedCharacters
    {
        [Test]
        public void IgnoresEscapedReturns()
        {
            var expected = new List<Token> { new(Consts.TokenTypes.Numeric, "1234"), new(Consts.TokenTypes.Numeric, "2345") };
            var result = Runner.Tokenize("""
                                         1234\
                                         2345
                                         """);
            Assert.That(result, Is.EquivalentTo(expected));
        }

        [Test]
        public void IncludesEscapedCharactersInStrings()
        {
            var expected = new List<Token> { new(Consts.TokenTypes.String, @"asdf'asdf") };
            var result = Runner.Tokenize(@"'asdf\'asdf'");
            Assert.That(result, Is.EquivalentTo(expected));
        }
    }

    [TestFixture]
    public class NewLine
    {
        [Test]
        public void DetectsNewLineAndIndents()
        {
            // This has four spaces in addition to two tabs, meaning it should detect three tabs
            var expected = new List<Token>
            {
                new(Consts.TokenTypes.Identifier, "asdf"),
                new(Consts.TokenTypes.Newline, "3"),
                new(Consts.TokenTypes.Identifier, "asdf")
            };
            var result = Runner.Tokenize("asdf\n\t\t    asdf");
            Assert.That(result, Is.EquivalentTo(expected));
        }

        [Test]
        public void IgnoresTrailingSpaces()
        {
            // This has three spaces
            var expected = new List<Token>
            {
                new(Consts.TokenTypes.Identifier, "asdf"),
                new(Consts.TokenTypes.Newline, "2"),
                new(Consts.TokenTypes.Identifier, "asdf")
            };
            var result = Runner.Tokenize("asdf\n\t\t   asdf");
            Assert.That(result, Is.EquivalentTo(expected));
        }

        [Test]
        public void NoIndent()
        {
            var expected = new List<Token>
            {
                new(Consts.TokenTypes.Identifier, "asdf"),
                new(Consts.TokenTypes.Newline, "0"),
                new(Consts.TokenTypes.Identifier, "asdf")
            };
            var result = Runner.Tokenize("asdf\nasdf");
            Assert.That(result, Is.EquivalentTo(expected));
        }
    }
}
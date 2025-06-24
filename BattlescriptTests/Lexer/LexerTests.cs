using Battlescript;

namespace BattlescriptTests;

[TestFixture]
public static class LexerTests
{
    [TestFixture]
    public class Numbers
    {
        [Test]
        public void HandlesIntegers()
        {
            var lexer = new Lexer("213");
            var result = lexer.Run();
            var expected = new List<Token>() { new Token(Consts.TokenTypes.Integer, "213") };
            Assert.That(result, Is.EquivalentTo(expected));
        }

        [Test]
        public void HandlesFloats()
        {
            var lexer = new Lexer("1.23");
            var result = lexer.Run();
            var expected = new List<Token>() { new Token(Consts.TokenTypes.Float, "1.23") };
            Assert.That(result, Is.EquivalentTo(expected));
        }
        
        [Test]
        public void HandlesFloatsWithStartingDecimal()
        {
            var lexer = new Lexer(".23");
            var result = lexer.Run();
            var expected = new List<Token>() { new Token(Consts.TokenTypes.Float, ".23") };
            Assert.That(result, Is.EquivalentTo(expected));
        }
        
        [Test]
        public void HandlesNegatives()
        {
            var lexer = new Lexer("-213");
            var result = lexer.Run();
            var expected = new List<Token>() { new Token(Consts.TokenTypes.Integer, "-213") };
            Assert.That(result, Is.EquivalentTo(expected));
        }
    }

    [TestFixture]
    public class Strings
    {
        [Test]
        public void HandlesDoubleQuotes()
        {
            var lexer = new Lexer("\"asdf\"");
            var result = lexer.Run();
            var expected = new List<Token>() { new Token(Consts.TokenTypes.String, "asdf") };
            Assert.That(result, Is.EquivalentTo(expected));
        }

        [Test]
        public void HandlesSingleQuotes()
        {
            var lexer = new Lexer("'asdf'");
            var result = lexer.Run();
            var expected = new List<Token>() { new Token(Consts.TokenTypes.String, "asdf") };
            Assert.That(result, Is.EquivalentTo(expected));
        }
    }

    [TestFixture]
    public class Separators
    {
        [Test]
        public void HandlesSeparators()
        {
            var lexer = new Lexer(",");
            var result = lexer.Run();
            var expected = new List<Token>() { new Token(Consts.TokenTypes.Separator, ",") };
            Assert.That(result, Is.EquivalentTo(expected));
        }
    }

    [TestFixture]
    public class Words
    {
        [Test]
        public void HandlesKeywords()
        {
            var lexer = new Lexer("async");
            var result = lexer.Run();
            var expected = new List<Token>() { new Token(Consts.TokenTypes.Keyword, "async") };
            Assert.That(result, Is.EquivalentTo(expected));
        }

        [Test]
        public void HandlesBooleans()
        {
            var lexer = new Lexer("True");
            var result = lexer.Run();
            var expected = new List<Token>() { new Token(Consts.TokenTypes.Boolean, "True") };
            Assert.That(result, Is.EquivalentTo(expected));
        }

        [Test]
        public void HandlesWordOperators()
        {
            var lexer = new Lexer("and");
            var result = lexer.Run();
            var expected = new List<Token>() { new Token(Consts.TokenTypes.Operator, "and") };
            Assert.That(result, Is.EquivalentTo(expected));
        }

        [Test]
        public void HandlesIdentifiers()
        {
            var lexer = new Lexer("asdf");
            var result = lexer.Run();
            var expected = new List<Token>() { new Token(Consts.TokenTypes.Identifier, "asdf") };
            Assert.That(result, Is.EquivalentTo(expected));
        }

        [Test]
        public void HandlesBuiltIns()
        {
            var lexer = new Lexer("super");
            var result = lexer.Run();
            var expected = new List<Token>() { new Token(Consts.TokenTypes.BuiltIn, "super") };
            Assert.That(result, Is.EquivalentTo(expected));
        }
    }

    [TestFixture]
    public class Operators
    {
        [Test]
        public void HandlesSingleCharacterOperators()
        {
            var lexer = new Lexer("+");
            var result = lexer.Run();
            var expected = new List<Token>() { new Token(Consts.TokenTypes.Operator, "+") };
            Assert.That(result, Is.EquivalentTo(expected));
        }
        
        [Test]
        public void HandlesDoubleCharacterOperators()
        {
            var lexer = new Lexer("**");
            var result = lexer.Run();
            var expected = new List<Token>() { new Token(Consts.TokenTypes.Operator, "**") };
            Assert.That(result, Is.EquivalentTo(expected));
        }
        
        [Test]
        public void HandlesTripleCharacterOperators()
        {
            var lexer = new Lexer("not");
            var result = lexer.Run();
            var expected = new List<Token>() { new Token(Consts.TokenTypes.Operator, "not") };
            Assert.That(result, Is.EquivalentTo(expected));
        }
        
        [Test]
        public void HandlesIsNotOperator()
        {
            var lexer = new Lexer("x is not y");
            var result = lexer.Run();
            Postlexer.Run(result);
            var expected = new List<Token>()
            {
                new(Consts.TokenTypes.Identifier, "x"),
                new(Consts.TokenTypes.Operator, "is not"),
                new(Consts.TokenTypes.Identifier, "y"),
            };
            Assert.That(result, Is.EquivalentTo(expected));
        }
        
        [Test]
        public void HandlesNotInOperator()
        {
            var lexer = new Lexer("x not in y");
            var result = lexer.Run();
            Postlexer.Run(result);
            var expected = new List<Token>()
            {
                new(Consts.TokenTypes.Identifier, "x"),
                new(Consts.TokenTypes.Operator, "not in"),
                new(Consts.TokenTypes.Identifier, "y"),
            };
            Assert.That(result, Is.EquivalentTo(expected));
        }
    }
    
    [TestFixture]
    public class AssignmentOperators
    {
        [Test]
        public void HandlesSingleCharacterOperators()
        {
            var lexer = new Lexer("=");
            var result = lexer.Run();
            var expected = new List<Token>() { new Token(Consts.TokenTypes.Assignment, "=") };
            Assert.That(result, Is.EquivalentTo(expected));
        }
        
        [Test]
        public void HandlesDoubleCharacterOperators()
        {
            var lexer = new Lexer("*=");
            var result = lexer.Run();
            var expected = new List<Token>() { new Token(Consts.TokenTypes.Assignment, "*=") };
            Assert.That(result, Is.EquivalentTo(expected));
        }
        
        [Test]
        public void HandlesTripleCharacterOperators()
        {
            var lexer = new Lexer("**=");
            var result = lexer.Run();
            var expected = new List<Token>() { new Token(Consts.TokenTypes.Assignment, "**=") };
            Assert.That(result, Is.EquivalentTo(expected));
        }
    }

    [TestFixture]
    public class Comments
    {
        [Test]
        public void IgnoresComments()
        {
            var lexer = new Lexer("= #asdfadsf");
            var result = lexer.Run();
            var expected = new List<Token>() { new Token(Consts.TokenTypes.Assignment, "=") };
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
            var lexer = new Lexer("asdf\n\t\t    asdf");
            var result = lexer.Run();
            var expected = new List<Token>()
            {
                new(Consts.TokenTypes.Identifier, "asdf"),
                new(Consts.TokenTypes.Newline, "3"),
                new(Consts.TokenTypes.Identifier, "asdf")
            };
            Assert.That(result, Is.EquivalentTo(expected));
        }

        [Test]
        public void IgnoresTrailingSpaces()
        {
            // This has three spaces
            var lexer = new Lexer("asdf\n\t\t   asdf");
            var result = lexer.Run();
            var expected = new List<Token>()
            {
                new(Consts.TokenTypes.Identifier, "asdf"),
                new(Consts.TokenTypes.Newline, "2"),
                new(Consts.TokenTypes.Identifier, "asdf")
            };
            Assert.That(result, Is.EquivalentTo(expected));
        }
        
        [Test]
        public void HandlesNoIndent()
        {
            var lexer = new Lexer("asdf\nasdf");
            var result = lexer.Run();
            var expected = new List<Token>()
            {
                new(Consts.TokenTypes.Identifier, "asdf"),
                new(Consts.TokenTypes.Newline, "0"),
                new(Consts.TokenTypes.Identifier, "asdf")
            };
            Assert.That(result, Is.EquivalentTo(expected));
        }
    }
}
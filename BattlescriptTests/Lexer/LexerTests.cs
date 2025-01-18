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
            Assertions.AssertTokenListEqual(result, new List<Token>()
            {
                new (Consts.TokenTypes.Number, "213")
            });
        }

        [Test]
        public void HandlesFloats()
        {
            var lexer = new Lexer("1.23");
            var result = lexer.Run();
            Assertions.AssertTokenListEqual(result, new List<Token>()
            {
                new (Consts.TokenTypes.Number, "1.23")
            });
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
            Assertions.AssertTokenListEqual(result, new List<Token>()
            {
                new (Consts.TokenTypes.String, "asdf")
            });
        }

        [Test]
        public void HandlesSingleQuotes()
        {
            var lexer = new Lexer("'asdf'");
            var result = lexer.Run();
            Assertions.AssertTokenListEqual(result, new List<Token>()
            {
                new (Consts.TokenTypes.String, "asdf")
            });
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
            Assertions.AssertTokenListEqual(result, new List<Token>()
            {
                new (Consts.TokenTypes.Separator, ",")
            });
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
            Assertions.AssertTokenListEqual(result, new List<Token>()
            {
                new (Consts.TokenTypes.Keyword, "async")
            });
        }

        [Test]
        public void HandlesBooleans()
        {
            var lexer = new Lexer("True");
            var result = lexer.Run();
            Assertions.AssertTokenListEqual(result, new List<Token>()
            {
                new (Consts.TokenTypes.Boolean, "True")
            });
        }

        [Test]
        public void HandlesWordOperators()
        {
            var lexer = new Lexer("and");
            var result = lexer.Run();
            Assertions.AssertTokenListEqual(result, new List<Token>()
            {
                new(Consts.TokenTypes.Operator, "and")
            });
        }

        [Test]
        public void HandlesIdentifiers()
        {
            var lexer = new Lexer("asdf");
            var result = lexer.Run();
            Assertions.AssertTokenListEqual(result, new List<Token>()
            {
                new(Consts.TokenTypes.Identifier, "asdf")
            });
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
            Assertions.AssertTokenListEqual(result, new List<Token>()
            {
                new(Consts.TokenTypes.Operator, "+")
            });
        }
        
        [Test]
        public void HandlesDoubleCharacterOperators()
        {
            var lexer = new Lexer("**");
            var result = lexer.Run();
            Assertions.AssertTokenListEqual(result, new List<Token>()
            {
                new(Consts.TokenTypes.Operator, "**")
            });
        }
        
        [Test]
        public void HandlesTripleCharacterOperators()
        {
            var lexer = new Lexer("not");
            var result = lexer.Run();
            Assertions.AssertTokenListEqual(result, new List<Token>()
            {
                new(Consts.TokenTypes.Operator, "not")
            });
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
            Assertions.AssertTokenListEqual(result, new List<Token>()
            {
                new(Consts.TokenTypes.Assignment, "=")
            });
        }
        
        [Test]
        public void HandlesDoubleCharacterOperators()
        {
            var lexer = new Lexer("*=");
            var result = lexer.Run();
            Assertions.AssertTokenListEqual(result, new List<Token>()
            {
                new(Consts.TokenTypes.Assignment, "*=")
            });
        }
        
        [Test]
        public void HandlesTripleCharacterOperators()
        {
            var lexer = new Lexer("**=");
            var result = lexer.Run();
            Assertions.AssertTokenListEqual(result, new List<Token>()
            {
                new(Consts.TokenTypes.Assignment, "**=")
            });
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
            Assertions.AssertTokenListEqual(result, new List<Token>()
            {
                new(Consts.TokenTypes.Assignment, "=")
            });
        }
    }
    
    [TestFixture]
    public class NewLine
    {
        [Test]
        public void DetectsNewLineAndIndents()
        {
            // This has 4 spaces
            var lexer = new Lexer("asdf\n\t\t    asdf");
            var result = lexer.Run();
            Assertions.AssertTokenListEqual(result, new List<Token>()
            {
                new(Consts.TokenTypes.Identifier, "asdf"),
                new(Consts.TokenTypes.Newline, "3"),
                new(Consts.TokenTypes.Identifier, "asdf")
            });
        }

        [Test]
        public void IgnoresTrailingSpaces()
        {
            // This has 3 spaces
            var lexer = new Lexer("asdf\n\t\t   asdf");
            var result = lexer.Run();
            Assertions.AssertTokenListEqual(result, new List<Token>()
            {
                new(Consts.TokenTypes.Identifier, "asdf"),
                new(Consts.TokenTypes.Newline, "2"),
                new(Consts.TokenTypes.Identifier, "asdf")
            });
        }
        
        [Test]
        public void HandlesNoIndent()
        {
            // This has 3 spaces
            var lexer = new Lexer("asdf\nasdf");
            var result = lexer.Run();
            Assertions.AssertTokenListEqual(result, new List<Token>()
            {
                new(Consts.TokenTypes.Identifier, "asdf"),
                new(Consts.TokenTypes.Newline, "0"),
                new(Consts.TokenTypes.Identifier, "asdf")
            });
        }
    }
}
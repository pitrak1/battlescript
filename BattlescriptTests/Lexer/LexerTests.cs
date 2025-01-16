using Battlescript;

namespace BattlescriptTests;

[TestFixture]
public static class LexerTests
{
    [TestFixture]
    public class LexerNumbers
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
    public class LexerStrings
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
    public class LexerSeparators
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
    public class LexerWords
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
    public class LexerOperators
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
    public class LexerAssignmentOperators
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
    public class LexerComments
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
}
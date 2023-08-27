using BattleScript.LexerNS;
using BattleScript.Tokens;

namespace BattleScript.Tests;

public class LexerTests
{
    [Test]
    public void Integers()
    {
        Lexer lexer = new Lexer();
        var tokens = lexer.Run("123456");
        var expectedTokens = new List<Token>() {
            new NumberToken("123456")
        };
        Assertions.AssertTokens(tokens, expectedTokens);
    }

    [Test]
    public void FloatingPointNumbers()
    {
        Lexer lexer = new Lexer();
        var tokens = lexer.Run("123.456");
        var expectedTokens = new List<Token>() {
            new NumberToken("123.456")
        };
        Assertions.AssertTokens(tokens, expectedTokens);
    }

    [Test]
    public void SingleQuoteStrings()
    {
        Lexer lexer = new Lexer();
        var tokens = lexer.Run("'testString'");
        var expectedTokens = new List<Token>() {
            new StringToken("'testString'")
        };
        Assertions.AssertTokens(tokens, expectedTokens);
    }

    [Test]
    public void DoubleQuoteStrings()
    {
        Lexer lexer = new Lexer();
        var tokens = lexer.Run("\"testString\"");
        var expectedTokens = new List<Token>() {
            new StringToken("\"testString\"")
        };
        Assertions.AssertTokens(tokens, expectedTokens);
    }

    [Test]
    public void MixedQuoteStrings()
    {
        Lexer lexer = new Lexer();
        var tokens = lexer.Run("\"'testString'\"");
        var expectedTokens = new List<Token>() {
            new StringToken("\"'testString'\"")
        };
        Assertions.AssertTokens(tokens, expectedTokens);
    }

    [Test]
    public void Separators()
    {
        Lexer lexer = new Lexer();
        var tokens = lexer.Run("(");
        var expectedTokens = new List<Token>() {
            new SeparatorToken("(")
        };
        Assertions.AssertTokens(tokens, expectedTokens);
    }

    [Test]
    public void Keywords()
    {
        Lexer lexer = new Lexer();
        var tokens = lexer.Run("constructor");
        var expectedTokens = new List<Token>() {
            new KeywordToken("constructor")
        };
        Assertions.AssertTokens(tokens, expectedTokens);
    }

    [Test]
    public void Booleans()
    {
        Lexer lexer = new Lexer();
        var tokens = lexer.Run("true");
        var expectedTokens = new List<Token>() {
            new BooleanToken("true")
        };
        Assertions.AssertTokens(tokens, expectedTokens);
    }

    [Test]
    public void Identifiers()
    {
        Lexer lexer = new Lexer();
        var tokens = lexer.Run("my_identifier1");
        var expectedTokens = new List<Token>() {
            new IdentifierToken("my_identifier1")
        };
        Assertions.AssertTokens(tokens, expectedTokens);
    }

    [Test]
    public void SingleCharacterOperators()
    {
        Lexer lexer = new Lexer();
        var tokens = lexer.Run("+");
        var expectedTokens = new List<Token>() {
            new OperatorToken("+")
        };
        Assertions.AssertTokens(tokens, expectedTokens);
    }

    [Test]
    public void MultipleCharacterOperators()
    {
        Lexer lexer = new Lexer();
        var tokens = lexer.Run("==");
        var expectedTokens = new List<Token>() {
            new OperatorToken("==")
        };
        Assertions.AssertTokens(tokens, expectedTokens);
    }

    [Test]
    public void Assignments()
    {
        Lexer lexer = new Lexer();
        var tokens = lexer.Run("=");
        var expectedTokens = new List<Token>() {
            new AssignmentToken()
        };
        Assertions.AssertTokens(tokens, expectedTokens);
    }

    [Test]
    public void Comments()
    {
        Lexer lexer = new Lexer();
        var tokens = lexer.Run("//");
        var expectedTokens = new List<Token>() { };
        Assertions.AssertTokens(tokens, expectedTokens);
    }
}
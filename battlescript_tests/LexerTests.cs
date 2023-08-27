using BattleScript.LexerNS;
using BattleScript.Tokens;

namespace BattleScript.Tests;

public class LexerTests
{
    [Test]
    public void Integers()
    {
        var tokens = Lexer.Run("123456");
        var expectedTokens = new List<Token>() {
            new NumberToken("123456")
        };
        Assertions.AssertTokens(tokens, expectedTokens);
    }

    [Test]
    public void FloatingPointNumbers()
    {
        var tokens = Lexer.Run("123.456");
        var expectedTokens = new List<Token>() {
            new NumberToken("123.456")
        };
        Assertions.AssertTokens(tokens, expectedTokens);
    }

    [Test]
    public void SingleQuoteStrings()
    {
        var tokens = Lexer.Run("'testString'");
        var expectedTokens = new List<Token>() {
            new StringToken("'testString'")
        };
        Assertions.AssertTokens(tokens, expectedTokens);
    }

    [Test]
    public void DoubleQuoteStrings()
    {
        var tokens = Lexer.Run("\"testString\"");
        var expectedTokens = new List<Token>() {
            new StringToken("\"testString\"")
        };
        Assertions.AssertTokens(tokens, expectedTokens);
    }

    [Test]
    public void MixedQuoteStrings()
    {
        var tokens = Lexer.Run("\"'testString'\"");
        var expectedTokens = new List<Token>() {
            new StringToken("\"'testString'\"")
        };
        Assertions.AssertTokens(tokens, expectedTokens);
    }

    [Test]
    public void Separators()
    {
        var tokens = Lexer.Run("(");
        var expectedTokens = new List<Token>() {
            new SeparatorToken("(")
        };
        Assertions.AssertTokens(tokens, expectedTokens);
    }

    [Test]
    public void Keywords()
    {
        var tokens = Lexer.Run("constructor");
        var expectedTokens = new List<Token>() {
            new KeywordToken("constructor")
        };
        Assertions.AssertTokens(tokens, expectedTokens);
    }

    [Test]
    public void Booleans()
    {
        var tokens = Lexer.Run("true");
        var expectedTokens = new List<Token>() {
            new BooleanToken("true")
        };
        Assertions.AssertTokens(tokens, expectedTokens);
    }

    [Test]
    public void Identifiers()
    {
        var tokens = Lexer.Run("my_identifier1");
        var expectedTokens = new List<Token>() {
            new IdentifierToken("my_identifier1")
        };
        Assertions.AssertTokens(tokens, expectedTokens);
    }

    [Test]
    public void SingleCharacterOperators()
    {
        var tokens = Lexer.Run("+");
        var expectedTokens = new List<Token>() {
            new OperatorToken("+")
        };
        Assertions.AssertTokens(tokens, expectedTokens);
    }

    [Test]
    public void MultipleCharacterOperators()
    {
        var tokens = Lexer.Run("==");
        var expectedTokens = new List<Token>() {
            new OperatorToken("==")
        };
        Assertions.AssertTokens(tokens, expectedTokens);
    }

    [Test]
    public void Assignments()
    {
        var tokens = Lexer.Run("=");
        var expectedTokens = new List<Token>() {
            new AssignmentToken()
        };
        Assertions.AssertTokens(tokens, expectedTokens);
    }

    [Test]
    public void Comments()
    {
        var tokens = Lexer.Run("//");
        var expectedTokens = new List<Token>() { };
        Assertions.AssertTokens(tokens, expectedTokens);
    }
}
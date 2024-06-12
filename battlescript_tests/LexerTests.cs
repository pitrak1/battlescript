using BattleScript.Core;
using BattleScript.Tokens;
using BattleScript.Tests;

namespace BattleScript.LexerTests;

public class LexerTests
{
    [Test]
    public void Integers()
    {
        Lexer lexer = new Lexer("123456");
        var tokens = lexer.Run();
        var expectedTokens = new List<Token>() {
            new Token(Consts.TokenTypes.Number, "123456")
        };
        Assertions.AssertTokens(tokens, expectedTokens);
    }

    [Test]
    public void FloatingPointNumbers()
    {
        Lexer lexer = new Lexer("123.456");
        var tokens = lexer.Run();
        var expectedTokens = new List<Token>() {
            new Token(Consts.TokenTypes.Number, "123.456")
        };
        Assertions.AssertTokens(tokens, expectedTokens);
    }

    [Test]
    public void SingleQuoteStrings()
    {
        Lexer lexer = new Lexer("'testString'");
        var tokens = lexer.Run();
        var expectedTokens = new List<Token>() {
            new Token(Consts.TokenTypes.String, "'testString'")
        };
        Assertions.AssertTokens(tokens, expectedTokens);
    }

    [Test]
    public void DoubleQuoteStrings()
    {
        Lexer lexer = new Lexer("\"testString\"");
        var tokens = lexer.Run();
        var expectedTokens = new List<Token>() {
            new Token(Consts.TokenTypes.String, "\"testString\"")
        };
        Assertions.AssertTokens(tokens, expectedTokens);
    }

    [Test]
    public void MixedQuoteStrings()
    {
        Lexer lexer = new Lexer("\"'testString'\"");
        var tokens = lexer.Run();
        var expectedTokens = new List<Token>() {
            new Token(Consts.TokenTypes.String, "\"'testString'\"")
        };
        Assertions.AssertTokens(tokens, expectedTokens);
    }

    [Test]
    public void Separators()
    {
        Lexer lexer = new Lexer("(");
        var tokens = lexer.Run();
        var expectedTokens = new List<Token>() {
            new Token(Consts.TokenTypes.Separator, "(")
        };
        Assertions.AssertTokens(tokens, expectedTokens);
    }

    [Test]
    public void Keywords()
    {
        Lexer lexer = new Lexer("function");
        var tokens = lexer.Run();
        var expectedTokens = new List<Token>() {
            new Token(Consts.TokenTypes.Keyword, "function")
        };
        Assertions.AssertTokens(tokens, expectedTokens);
    }

    [Test]
    public void Booleans()
    {
        Lexer lexer = new Lexer("true");
        var tokens = lexer.Run();
        var expectedTokens = new List<Token>() {
            new Token(Consts.TokenTypes.Boolean, "true")
        };
        Assertions.AssertTokens(tokens, expectedTokens);
    }

    [Test]
    public void Identifiers()
    {
        Lexer lexer = new Lexer("my_identifier1");
        var tokens = lexer.Run();
        var expectedTokens = new List<Token>() {
            new Token(Consts.TokenTypes.Identifier, "my_identifier1")
        };
        Assertions.AssertTokens(tokens, expectedTokens);
    }

    [Test]
    public void SingleCharacterOperators()
    {
        Lexer lexer = new Lexer("+");
        var tokens = lexer.Run();
        var expectedTokens = new List<Token>() {
            new Token(Consts.TokenTypes.Operator, "+")
        };
        Assertions.AssertTokens(tokens, expectedTokens);
    }

    [Test]
    public void MultipleCharacterOperators()
    {
        Lexer lexer = new Lexer("==");
        var tokens = lexer.Run();
        var expectedTokens = new List<Token>() {
            new Token(Consts.TokenTypes.Operator, "==")
        };
        Assertions.AssertTokens(tokens, expectedTokens);
    }

    [Test]
    public void Assignments()
    {
        Lexer lexer = new Lexer("=");
        var tokens = lexer.Run();
        var expectedTokens = new List<Token>() {
            new Token(Consts.TokenTypes.Assignment, "=")
        };
        Assertions.AssertTokens(tokens, expectedTokens);
    }

    [Test]
    public void Comments()
    {
        Lexer lexer = new Lexer("//");
        var tokens = lexer.Run();
        var expectedTokens = new List<Token>() { };
        Assertions.AssertTokens(tokens, expectedTokens);
    }

    [Test]
    public void Breakpoint()
    {
        Lexer lexer = new Lexer("breakpoint");
        var tokens = lexer.Run();
        var expectedTokens = new List<Token>() {
            new Token(Consts.TokenTypes.Keyword, "breakpoint")
        };
        Assertions.AssertTokens(tokens, expectedTokens);
    }
}
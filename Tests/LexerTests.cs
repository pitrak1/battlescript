namespace BattleScript.Tests;

public class LexerTests {
    [Test]
    public void Integers() {
        var tokens = Lexer.Run("123456");
        var expectedTokens = new List<Token>() {
            new Token(Consts.TokenTypes.Number, "123456")
        };
        Assertions.AssertTokens(tokens, expectedTokens);
    }

    [Test] public void FloatingPointNumbers() {
        var tokens = Lexer.Run("123.456");
        var expectedTokens = new List<Token>() {
            new Token(Consts.TokenTypes.Number, "123.456")
        };
        Assertions.AssertTokens(tokens, expectedTokens);
    }
    
    [Test]
    public void SingleQuoteStrings() {
        var tokens = Lexer.Run("'testString'");
        var expectedTokens = new List<Token>() {
            new Token(Consts.TokenTypes.String, "'testString'")
        };
        Assertions.AssertTokens(tokens, expectedTokens);
    }
    
    [Test]
    public void DoubleQuoteStrings() {
        var tokens = Lexer.Run("\"testString\"");
        var expectedTokens = new List<Token>() {
            new Token(Consts.TokenTypes.String, "\"testString\"")
        };
        Assertions.AssertTokens(tokens, expectedTokens);
    }
    
    [Test]
    public void MixedQuoteStrings() {
        var tokens = Lexer.Run("\"'testString'\"");
        var expectedTokens = new List<Token>() {
            new Token(Consts.TokenTypes.String, "\"'testString'\"")
        };
        Assertions.AssertTokens(tokens, expectedTokens);
    }
    
    [Test]
    public void Separators() {
        var tokens = Lexer.Run("(");
        var expectedTokens = new List<Token>() {
            new Token(Consts.TokenTypes.Separator, "(")
        };
        Assertions.AssertTokens(tokens, expectedTokens);
    }
}
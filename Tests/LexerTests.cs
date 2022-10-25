namespace BattleScript.Tests;

public class LexerTests {
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
    public void Integers() {
        var tokens = Lexer.Run("123456");
        var expectedTokens = new List<Token>() {
            new Token(Consts.TokenTypes.Number, "123456")
        };
        Assertions.AssertTokens(tokens, expectedTokens);
    }
}
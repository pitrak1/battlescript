using Battlescript;

namespace BattlescriptTests;

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
                new Token(Consts.TokenTypes.Number, "213")
            });
        }
    }
}
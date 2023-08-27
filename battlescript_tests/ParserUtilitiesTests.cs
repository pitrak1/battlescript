namespace BattleScript.Tests;
using BattleScript.Core;
using BattleScript.Tokens;
using BattleScript.LexerNS;
using BattleScript.ParserNS;

public class ParserUtilitiesTests
{
    [TestFixture]
    public class GetAssignmentOperatorIndexTests
    {
        [Test]
        public void Exists()
        {
            Lexer lexer = new Lexer();
            var tokens = lexer.Run("var x = 1 + 2;");
            int index = ParserUtilities.GetAssignmentOperatorIndex(tokens);
            Assert.That(index, Is.EqualTo(2));
        }

        [Test]
        public void DoesNotExist()
        {
            Lexer lexer = new Lexer();
            var tokens = lexer.Run("var x 1 + 2;");
            int index = ParserUtilities.GetAssignmentOperatorIndex(tokens);
            Assert.That(index, Is.EqualTo(-1));
        }
    }

    [TestFixture]
    public class GetMathematicalOperatorIndexTests
    {
        [Test]
        public void Exists()
        {
            Lexer lexer = new Lexer();
            var tokens = lexer.Run("var x = 1 + 2;");
            int index = ParserUtilities.GetMathematicalOperatorIndex(tokens);
            Assert.That(index, Is.EqualTo(4));
        }

        [Test]
        public void EvaluatesRightToLeft()
        {
            Lexer lexer = new Lexer();
            var tokens = lexer.Run("var x = 1 + 2 + 3;");
            int index = ParserUtilities.GetMathematicalOperatorIndex(tokens);
            Assert.That(index, Is.EqualTo(6));
        }

        [Test]
        public void EvaluatesLowestToHighestPriority()
        {
            Lexer lexer = new Lexer();
            var tokens = lexer.Run("var x = 1 + 2 * 3;");
            int index = ParserUtilities.GetMathematicalOperatorIndex(tokens);
            Assert.That(index, Is.EqualTo(4));
        }

        [Test]
        public void SkipsTokensInSeparatorBlocks()
        {
            Lexer lexer = new Lexer();
            var tokens = lexer.Run("var x = 1 + x[1 + 2];");
            int index = ParserUtilities.GetMathematicalOperatorIndex(tokens);
            Assert.That(index, Is.EqualTo(4));
        }

        [Test]
        public void DoesNotExist()
        {
            Lexer lexer = new Lexer();
            var tokens = lexer.Run("var x = 12;");
            int index = ParserUtilities.GetMathematicalOperatorIndex(tokens);
            Assert.That(index, Is.EqualTo(-1));
        }
    }

    [TestFixture]
    public class ParseUntilMatchingSeparator
    {
        [Test]
        public void NoSeparators()
        {
            Lexer lexer = new Lexer();
            var tokens = lexer.Run("[1]");
            List<List<Token>> entries = ParserUtilities.ParseUntilMatchingSeparator(tokens, new List<string>());

            Assert.That(entries.Count, Is.EqualTo(1));
            Assert.That(entries[0].Count, Is.EqualTo(1));
            Assert.That(entries[0][0].Type, Is.EqualTo(Consts.TokenTypes.Number));
            Assert.That(entries[0][0].Value, Is.EqualTo("1"));
        }

        [Test]
        public void WithSeparatingCharacters()
        {
            Lexer lexer = new Lexer();
            var tokens = lexer.Run("[1, 2]");
            List<string> separatingCharacters = new List<string>() { "," };
            List<List<Token>> entries = ParserUtilities.ParseUntilMatchingSeparator(tokens, separatingCharacters);

            Assert.That(entries.Count, Is.EqualTo(2));
            Assert.That(entries[0].Count, Is.EqualTo(1));
            Assert.That(entries[0][0].Type, Is.EqualTo(Consts.TokenTypes.Number));
            Assert.That(entries[0][0].Value, Is.EqualTo("1"));
            Assert.That(entries[1].Count, Is.EqualTo(1));
            Assert.That(entries[1][0].Type, Is.EqualTo(Consts.TokenTypes.Number));
            Assert.That(entries[1][0].Value, Is.EqualTo("2"));
        }

        [Test]
        public void RespectsInnerSeparators()
        {
            Lexer lexer = new Lexer();
            var tokens = lexer.Run("[[1], [2]]");
            List<string> separatingCharacters = new List<string>() { "," };
            List<List<Token>> entries = ParserUtilities.ParseUntilMatchingSeparator(tokens, separatingCharacters);

            Assert.That(entries.Count, Is.EqualTo(2));

            Assert.That(entries[0].Count, Is.EqualTo(3));
            Assert.That(entries[0][0].Type, Is.EqualTo(Consts.TokenTypes.Separator));
            Assert.That(entries[0][0].Value, Is.EqualTo("["));
            Assert.That(entries[0][1].Type, Is.EqualTo(Consts.TokenTypes.Number));
            Assert.That(entries[0][1].Value, Is.EqualTo("1"));
            Assert.That(entries[0][2].Type, Is.EqualTo(Consts.TokenTypes.Separator));
            Assert.That(entries[0][2].Value, Is.EqualTo("]"));

            Assert.That(entries[1].Count, Is.EqualTo(3));
            Assert.That(entries[1][0].Type, Is.EqualTo(Consts.TokenTypes.Separator));
            Assert.That(entries[1][0].Value, Is.EqualTo("["));
            Assert.That(entries[1][1].Type, Is.EqualTo(Consts.TokenTypes.Number));
            Assert.That(entries[1][1].Value, Is.EqualTo("2"));
            Assert.That(entries[1][2].Type, Is.EqualTo(Consts.TokenTypes.Separator));
            Assert.That(entries[1][2].Value, Is.EqualTo("]"));
        }
    }
}
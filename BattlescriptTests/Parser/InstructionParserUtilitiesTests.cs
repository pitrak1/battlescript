using Battlescript;

namespace BattlescriptTests;

public static class InstructionParserUtilitiesTests
{
    [TestFixture]
    public class GetAssignmentIndex
    {
        [Test]
        public void Exists()
        {
            var lexer = new Lexer("x = 1 + 2");
            var tokens = lexer.Run();
            var index = InstructionParserUtilities.GetAssignmentIndex(tokens);
            Assert.That(index, Is.EqualTo(1));
        }

        [Test]
        public void DoesNotExist()
        {
            Lexer lexer = new Lexer("x 1 + 2");
            var tokens = lexer.Run();
            int index = InstructionParserUtilities.GetAssignmentIndex(tokens);
            Assert.That(index, Is.EqualTo(-1));
        }
    }

    [TestFixture]
    public class GetMathematicalOperatorIndex
    {
        [Test]
        public void Exists()
        {
            Lexer lexer = new Lexer("x = 1 + 2");
            var tokens = lexer.Run();
            int index = InstructionParserUtilities.GetOperatorIndex(tokens);
            Assert.That(index, Is.EqualTo(3));
        }

        [Test]
        public void EvaluatesRightToLeft()
        {
            Lexer lexer = new Lexer("x = 1 + 2 + 3");
            var tokens = lexer.Run();
            int index = InstructionParserUtilities.GetOperatorIndex(tokens);
            Assert.That(index, Is.EqualTo(5));
        }

        [Test]
        public void EvaluatesLowestToHighestPriority()
        {
            Lexer lexer = new Lexer("x = 1 + 2 * 3");
            var tokens = lexer.Run();
            int index = InstructionParserUtilities.GetOperatorIndex(tokens);
            Assert.That(index, Is.EqualTo(3));
        }

        // [Test]
        // public void SkipsTokensInSeparatorBlocks()
        // {
        //     Lexer lexer = new Lexer("var x = 1 + x[1 + 2];");
        //     var tokens = lexer.Run();
        //     int index = InstructionParserUtilities.GetOperatorIndex(tokens);
        //     Assert.That(index, Is.EqualTo(4));
        // }

        [Test]
        public void DoesNotExist()
        {
            Lexer lexer = new Lexer("x = 12");
            var tokens = lexer.Run();
            int index = InstructionParserUtilities.GetOperatorIndex(tokens);
            Assert.That(index, Is.EqualTo(-1));
        }
    }

    [TestFixture]
    public class ParseUntilMatchingSeparator
    {
        [Test]
        public void NoTokens()
        {
            var parsed = 
                InstructionParserUtilities.ParseTokensUntilMatchingSeparator([], []);

            Assert.That(parsed.Count, Is.EqualTo(0));
            Assert.That(parsed.Entries.Count, Is.EqualTo(0));
        }

        [Test]
        public void NoValues()
        {
            var lexer = new Lexer("[]");
            var tokens = lexer.Run();
            var parsed =
                InstructionParserUtilities.ParseTokensUntilMatchingSeparator(tokens, []);
            
            Assert.That(parsed.Count, Is.EqualTo(2));
            Assert.That(parsed.Entries.Count, Is.EqualTo(0));
        }

        [Test]
        public void NoSeparatingCharacters()
        {
            var lexer = new Lexer("[1]");
            var tokens = lexer.Run();
            var parsed =
                InstructionParserUtilities.ParseTokensUntilMatchingSeparator(tokens, []);

            Assert.That(parsed.Count, Is.EqualTo(3));

            Assert.That(parsed.Entries.Count, Is.EqualTo(1));
            
            Assertions.AssertTokenListEqual(parsed.Entries[0], [
                new Token(Consts.TokenTypes.Number, "1")
            ]);
        }

        [Test]
        public void WithSeparatingCharacters()
        {
            var lexer = new Lexer("[1, 2]");
            var tokens = lexer.Run();
            var parsed =
                InstructionParserUtilities.ParseTokensUntilMatchingSeparator(tokens, [","]);
            Assert.That(parsed.Count, Is.EqualTo(5));

            Assert.That(parsed.Entries.Count, Is.EqualTo(2));
            
            Assertions.AssertTokenListEqual(parsed.Entries[0], [
                new Token(Consts.TokenTypes.Number, "1")
            ]);
            
            Assertions.AssertTokenListEqual(parsed.Entries[1], [
                new Token(Consts.TokenTypes.Number, "2")
            ]);
        }

        [Test]
        public void RespectsInnerSeparators()
        {
            var lexer = new Lexer("[[1], [2]]");
            var tokens = lexer.Run();
            var parsed =
                InstructionParserUtilities.ParseTokensUntilMatchingSeparator(tokens, [","]);

            Assert.That(parsed.Count, Is.EqualTo(9));

            Assert.That(parsed.Entries.Count, Is.EqualTo(2));

            Assertions.AssertTokenListEqual(parsed.Entries[0], [
                new Token(Consts.TokenTypes.Separator, "["),
                new Token(Consts.TokenTypes.Number, "1"),
                new Token(Consts.TokenTypes.Separator, "]")
            ]);

            Assertions.AssertTokenListEqual(parsed.Entries[1], [
                new Token(Consts.TokenTypes.Separator, "["),
                new Token(Consts.TokenTypes.Number, "2"),
                new Token(Consts.TokenTypes.Separator, "]")
            ]);
        }
    }
}
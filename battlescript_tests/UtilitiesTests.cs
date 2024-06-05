namespace BattleScript.Tests;
using BattleScript.Core;

public class UtilitiesTests
{
    [TestFixture]
    public class GetCharactersUsingCollection
    {
        [Test]
        public void SimpleInclusiveSearch()
        {
            var result = Utilities.GetCharactersUsingCollection(
                "asdf.",
                0,
                Consts.Letters,
                CollectionType.Inclusive
            );

            Assert.That(result, Is.EqualTo("asdf"));
        }

        [Test]
        public void SimpleExclusiveSearch()
        {
            var result = Utilities.GetCharactersUsingCollection(
                "asdf.",
                0,
                Consts.Separators,
                CollectionType.Exclusive
            );

            Assert.That(result, Is.EqualTo("asdf"));
        }

        [Test]
        public void RespectsIndex()
        {
            var result = Utilities.GetCharactersUsingCollection(
                "asdf.",
                2,
                Consts.Letters,
                CollectionType.Inclusive
            );

            Assert.That(result, Is.EqualTo("df"));
        }
    }
    // [TestFixture]
    // public class GetAssignmentOperatorIndexTests
    // {
    //     [Test]
    //     public void Exists()
    //     {
    //         Lexer lexer = new Lexer("var x = 1 + 2;");
    //         var tokens = lexer.Run();
    //         int index = Utilities.GetAssignmentOperatorIndex(tokens);
    //         Assert.That(index, Is.EqualTo(2));
    //     }

    //     [Test]
    //     public void DoesNotExist()
    //     {
    //         Lexer lexer = new Lexer("var x 1 + 2;");
    //         var tokens = lexer.Run();
    //         int index = Utilities.GetAssignmentOperatorIndex(tokens);
    //         Assert.That(index, Is.EqualTo(-1));
    //     }
    // }

    // [TestFixture]
    // public class GetMathematicalOperatorIndexTests
    // {
    //     [Test]
    //     public void Exists()
    //     {
    //         Lexer lexer = new Lexer("var x = 1 + 2;");
    //         var tokens = lexer.Run();
    //         int index = Utilities.GetMathematicalOperatorIndex(tokens);
    //         Assert.That(index, Is.EqualTo(4));
    //     }

    //     [Test]
    //     public void EvaluatesRightToLeft()
    //     {
    //         Lexer lexer = new Lexer("var x = 1 + 2 + 3;");
    //         var tokens = lexer.Run();
    //         int index = Utilities.GetMathematicalOperatorIndex(tokens);
    //         Assert.That(index, Is.EqualTo(6));
    //     }

    //     [Test]
    //     public void EvaluatesLowestToHighestPriority()
    //     {
    //         Lexer lexer = new Lexer("var x = 1 + 2 * 3;");
    //         var tokens = lexer.Run();
    //         int index = Utilities.GetMathematicalOperatorIndex(tokens);
    //         Assert.That(index, Is.EqualTo(4));
    //     }

    //     [Test]
    //     public void SkipsTokensInSeparatorBlocks()
    //     {
    //         Lexer lexer = new Lexer("var x = 1 + x[1 + 2];");
    //         var tokens = lexer.Run();
    //         int index = Utilities.GetMathematicalOperatorIndex(tokens);
    //         Assert.That(index, Is.EqualTo(4));
    //     }

    //     [Test]
    //     public void DoesNotExist()
    //     {
    //         Lexer lexer = new Lexer("var x = 12;");
    //         var tokens = lexer.Run();
    //         int index = Utilities.GetMathematicalOperatorIndex(tokens);
    //         Assert.That(index, Is.EqualTo(-1));
    //     }
    // }

    // [TestFixture]
    // public class ParseUntilMatchingSeparator
    // {
    //     [Test]
    //     public void NoSeparators()
    //     {
    //         Lexer lexer = new Lexer("[1]");
    //         var tokens = lexer.Run();
    //         List<List<Token>> entries = Utilities.ParseUntilMatchingSeparator(tokens, new List<string>());

    //         Assert.That(entries.Count, Is.EqualTo(1));
    //         Assert.That(entries[0].Count, Is.EqualTo(1));
    //         Assert.That(entries[0][0].Type, Is.EqualTo(Consts.TokenTypes.Number));
    //         Assert.That(entries[0][0].Value, Is.EqualTo("1"));
    //     }

    //     [Test]
    //     public void WithSeparatingCharacters()
    //     {
    //         Lexer lexer = new Lexer("[1, 2]");
    //         var tokens = lexer.Run();
    //         List<string> separatingCharacters = new List<string>() { "," };
    //         List<List<Token>> entries = Utilities.ParseUntilMatchingSeparator(tokens, separatingCharacters);

    //         Assert.That(entries.Count, Is.EqualTo(2));
    //         Assert.That(entries[0].Count, Is.EqualTo(1));
    //         Assert.That(entries[0][0].Type, Is.EqualTo(Consts.TokenTypes.Number));
    //         Assert.That(entries[0][0].Value, Is.EqualTo("1"));
    //         Assert.That(entries[1].Count, Is.EqualTo(1));
    //         Assert.That(entries[1][0].Type, Is.EqualTo(Consts.TokenTypes.Number));
    //         Assert.That(entries[1][0].Value, Is.EqualTo("2"));
    //     }

    //     [Test]
    //     public void RespectsInnerSeparators()
    //     {
    //         Lexer lexer = new Lexer("[[1], [2]]");
    //         var tokens = lexer.Run();
    //         List<string> separatingCharacters = new List<string>() { "," };
    //         List<List<Token>> entries = Utilities.ParseUntilMatchingSeparator(tokens, separatingCharacters);

    //         Assert.That(entries.Count, Is.EqualTo(2));

    //         Assert.That(entries[0].Count, Is.EqualTo(3));
    //         Assert.That(entries[0][0].Type, Is.EqualTo(Consts.TokenTypes.Separator));
    //         Assert.That(entries[0][0].Value, Is.EqualTo("["));
    //         Assert.That(entries[0][1].Type, Is.EqualTo(Consts.TokenTypes.Number));
    //         Assert.That(entries[0][1].Value, Is.EqualTo("1"));
    //         Assert.That(entries[0][2].Type, Is.EqualTo(Consts.TokenTypes.Separator));
    //         Assert.That(entries[0][2].Value, Is.EqualTo("]"));

    //         Assert.That(entries[1].Count, Is.EqualTo(3));
    //         Assert.That(entries[1][0].Type, Is.EqualTo(Consts.TokenTypes.Separator));
    //         Assert.That(entries[1][0].Value, Is.EqualTo("["));
    //         Assert.That(entries[1][1].Type, Is.EqualTo(Consts.TokenTypes.Number));
    //         Assert.That(entries[1][1].Value, Is.EqualTo("2"));
    //         Assert.That(entries[1][2].Type, Is.EqualTo(Consts.TokenTypes.Separator));
    //         Assert.That(entries[1][2].Value, Is.EqualTo("]"));
    //     }
    // }
}
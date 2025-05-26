using Battlescript;

namespace BattlescriptTests;

[TestFixture]
public static class ParserUtilitiesTests
{
    [TestFixture]
    public class GetTokenIndex
    {
        [Test]
        public void FindsByType()
        {
            var lexer = new Lexer("x = 1 + 2");
            var tokens = lexer.Run();
            var index = ParserUtilities.GetTokenIndex(tokens, null, [Consts.TokenTypes.Assignment]);
            Assert.That(index, Is.EqualTo(1));
        }

        [Test]
        public void TypeDoesNotExist()
        {
            var lexer = new Lexer("x 1 + 2");
            var tokens = lexer.Run();
            var index = ParserUtilities.GetTokenIndex(tokens, null, [Consts.TokenTypes.Assignment]);
            Assert.That(index, Is.EqualTo(-1));
        }
        
        [Test]
        public void FindsByValue()
        {
            var lexer = new Lexer("x = 1 + 2");
            var tokens = lexer.Run();
            var index = ParserUtilities.GetTokenIndex(tokens, ["+"]);
            Assert.That(index, Is.EqualTo(3));
        }

        [Test]
        public void ValueDoesNotExist()
        {
            var lexer = new Lexer("x = 1 2");
            var tokens = lexer.Run();
            var index = ParserUtilities.GetTokenIndex(tokens, ["+"]);
            Assert.That(index, Is.EqualTo(-1));
        }

        [Test]
        public void IgnoresTokensWithinSeparators()
        {
            var lexer = new Lexer("x = [4 + 5]");
            var tokens = lexer.Run();
            var index = ParserUtilities.GetTokenIndex(tokens, ["+"]);
            Assert.That(index, Is.EqualTo(-1));
        }
        
        [Test]
        public void AllowsFindingUnnestedOpeningSeparators()
        {
            var lexer = new Lexer("x = [4 + 5]");
            var tokens = lexer.Run();
            var index = ParserUtilities.GetTokenIndex(tokens, ["["]);
            Assert.That(index, Is.EqualTo(2));
        }

        [Test]
        public void AllowsFindingUnnestedClosingSeparators()
        {
            var lexer = new Lexer("x = [4 + 5]");
            var tokens = lexer.Run();
            var index = ParserUtilities.GetTokenIndex(tokens, ["]"]);
            Assert.That(index, Is.EqualTo(6));
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
            int index = ParserUtilities.GetOperatorIndex(tokens);
            Assert.That(index, Is.EqualTo(3));
        }

        [Test]
        public void EvaluatesRightToLeft()
        {
            Lexer lexer = new Lexer("x = 1 + 2 + 3");
            var tokens = lexer.Run();
            int index = ParserUtilities.GetOperatorIndex(tokens);
            Assert.That(index, Is.EqualTo(5));
        }

        [Test]
        public void EvaluatesLowestToHighestPriority()
        {
            Lexer lexer = new Lexer("x = 1 + 2 * 3");
            var tokens = lexer.Run();
            int index = ParserUtilities.GetOperatorIndex(tokens);
            Assert.That(index, Is.EqualTo(3));
        }

        [Test]
        public void SkipsTokensInSeparatorBlocks()
        {
            Lexer lexer = new Lexer("var x = 1 + x[1 + 2]");
            var tokens = lexer.Run();
            int index = ParserUtilities.GetOperatorIndex(tokens);
            Assert.That(index, Is.EqualTo(4));
        }

        [Test]
        public void DoesNotExist()
        {
            Lexer lexer = new Lexer("x = 12");
            var tokens = lexer.Run();
            int index = ParserUtilities.GetOperatorIndex(tokens);
            Assert.That(index, Is.EqualTo(-1));
        }
    }

    [TestFixture]
    public class GroupTokensWithinSeparators
    {
        [Test]
        public void NoTokens()
        {
            var parsed = 
                ParserUtilities.GroupTokensWithinSeparators([], []);

            Assert.That(parsed.Count, Is.EqualTo(0));
            Assert.That(parsed.Entries.Count, Is.EqualTo(0));
        }

        [Test]
        public void NoValues()
        {
            var lexer = new Lexer("[]");
            var tokens = lexer.Run();
            var parsed =
                ParserUtilities.GroupTokensWithinSeparators(tokens, []);
            
            Assert.That(parsed.Count, Is.EqualTo(2));
            Assert.That(parsed.Entries.Count, Is.EqualTo(0));
        }

        [Test]
        public void NoSeparatingCharacters()
        {
            var lexer = new Lexer("[1]");
            var tokens = lexer.Run();
            var parsed =
                ParserUtilities.GroupTokensWithinSeparators(tokens, []);

            Assert.That(parsed.Count, Is.EqualTo(3));

            Assert.That(parsed.Entries.Count, Is.EqualTo(1));

            var expected = new List<Token>() { new Token(Consts.TokenTypes.Number, "1") };
            Assert.That(parsed.Entries[0], Is.EquivalentTo(expected));
        }

        [Test]
        public void WithSeparatingCharacters()
        {
            var lexer = new Lexer("[1, 2]");
            var tokens = lexer.Run();
            var parsed =
                ParserUtilities.GroupTokensWithinSeparators(tokens, [","]);
            Assert.That(parsed.Count, Is.EqualTo(5));

            var expected = new List<List<Token>>()
            {
                new List<Token>() { new Token(Consts.TokenTypes.Number, "1") },
                new List<Token>() { new Token(Consts.TokenTypes.Number, "2") },
            };
            Assert.That(parsed.Entries, Is.EquivalentTo(expected));
        }

        [Test]
        public void RespectsInnerSeparators()
        {
            var lexer = new Lexer("[[1], [2]]");
            var tokens = lexer.Run();
            var parsed =
                ParserUtilities.GroupTokensWithinSeparators(tokens, [","]);

            Assert.That(parsed.Count, Is.EqualTo(9));

            var expected = new List<List<Token>>()
            {
                new List<Token>()
                {
                    new Token(Consts.TokenTypes.Separator, "["),
                    new Token(Consts.TokenTypes.Number, "1"),
                    new Token(Consts.TokenTypes.Separator, "]")
                },
                new List<Token>()
                {
                    new Token(Consts.TokenTypes.Separator, "["),
                    new Token(Consts.TokenTypes.Number, "2"),
                    new Token(Consts.TokenTypes.Separator, "]")
                }
            };

            Assert.That(parsed.Entries, Is.EquivalentTo(expected));
        }
    }
    
    [TestFixture]
    public class ParseLeftAndRightAroundIndex
    {
        [Test]
        public void HandlesNoTokens()
        {
            var result = ParserUtilities.ParseLeftAndRightAroundIndex([], 0);
            Assert.That(result.Left, Is.Null);
            Assert.That(result.Right, Is.Null);
        }
        
        [Test]
        public void HandlesDivisionAtFirstEntry()
        {
            var lexer = new Lexer("x = 5 + 6");
            var lexerResult = lexer.Run();
            var result = ParserUtilities.ParseLeftAndRightAroundIndex(lexerResult, 0);
            Assert.That(result.Left, Is.Null);
        }
        
        [Test]
        public void HandlesDivisionAtLastEntry()
        {
            var lexer = new Lexer("x = 5 + 6");
            var lexerResult = lexer.Run();
            var result = ParserUtilities.ParseLeftAndRightAroundIndex(lexerResult, lexerResult.Count - 1);
            Assert.That(result.Right, Is.Null);
        }
        
        [Test]
        public void HandlesStandardDivision()
        {
            var lexer = new Lexer("x = 5");
            var lexerResult = lexer.Run();
            var result = ParserUtilities.ParseLeftAndRightAroundIndex(lexerResult, 1);
            Assert.That(result.Left, Is.TypeOf<VariableInstruction>());
            Assert.That(result.Right, Is.TypeOf<NumberInstruction>());
        }
    }
    
    [TestFixture]
    public class ParseEntriesWithinSeparator
    {
        [Test]
        public void NoTokens()
        {
            var parsed = 
                ParserUtilities.ParseEntriesWithinSeparator([], []);

            Assert.That(parsed.Count, Is.EqualTo(0));
            Assert.That(parsed.Values.Count, Is.EqualTo(0));
        }

        [Test]
        public void NoValues()
        {
            var lexer = new Lexer("[]");
            var tokens = lexer.Run();
            var parsed =
                ParserUtilities.ParseEntriesWithinSeparator(tokens, []);
            
            Assert.That(parsed.Count, Is.EqualTo(2));
            Assert.That(parsed.Values.Count, Is.EqualTo(0));
        }

        [Test]
        public void NoSeparatingCharacters()
        {
            var lexer = new Lexer("[1]");
            var tokens = lexer.Run();
            var parsed =
                ParserUtilities.ParseEntriesWithinSeparator(tokens, []);

            Assert.That(parsed.Count, Is.EqualTo(3));

            Assert.That(parsed.Values.Count, Is.EqualTo(1));

            var expected = new NumberInstruction(1.0);
            Assert.That(parsed.Values[0], Is.EqualTo(expected));
        }

        [Test]
        public void WithSeparatingCharacters()
        {
            var lexer = new Lexer("[1, 2]");
            var tokens = lexer.Run();
            var parsed =
                ParserUtilities.ParseEntriesWithinSeparator(tokens, [","]);
            Assert.That(parsed.Count, Is.EqualTo(5));

            var expected = new List<Instruction>()
            {
                new NumberInstruction(1.0),
                new NumberInstruction(2.0)
            };
            Assert.That(parsed.Values, Is.EquivalentTo(expected));
        }

        [Test]
        public void RespectsInnerSeparators()
        {
            var lexer = new Lexer("[[1], [2]]");
            var tokens = lexer.Run();
            var parsed =
                ParserUtilities.ParseEntriesWithinSeparator(tokens, [","]);

            Assert.That(parsed.Count, Is.EqualTo(9));

            var expected = new List<Instruction>()
            {
                new SquareBracketsInstruction([new NumberInstruction(1.0)]),
                new SquareBracketsInstruction([new NumberInstruction(2.0)])
            };

            Assert.That(parsed.Values, Is.EquivalentTo(expected));
        }
    }
}
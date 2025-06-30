using Battlescript;

namespace BattlescriptTests.ParserTests;

[TestFixture]
public static class InstructionUtilitiesTests
{
    [TestFixture]
    public class GetTokenIndex
    {
        [Test]
        public void FindsByType()
        {
            var lexer = new Lexer("x = 1 + 2");
            var tokens = lexer.Run();
            var index = InstructionUtilities.GetTokenIndex(tokens, null, [Consts.TokenTypes.Assignment]);
            Assert.That(index, Is.EqualTo(1));
        }

        [Test]
        public void TypeDoesNotExist()
        {
            var lexer = new Lexer("x 1 + 2");
            var tokens = lexer.Run();
            var index = InstructionUtilities.GetTokenIndex(tokens, null, [Consts.TokenTypes.Assignment]);
            Assert.That(index, Is.EqualTo(-1));
        }
        
        [Test]
        public void FindsByValue()
        {
            var lexer = new Lexer("x = 1 + 2");
            var tokens = lexer.Run();
            var index = InstructionUtilities.GetTokenIndex(tokens, ["+"]);
            Assert.That(index, Is.EqualTo(3));
        }

        [Test]
        public void ValueDoesNotExist()
        {
            var lexer = new Lexer("x = 1 2");
            var tokens = lexer.Run();
            var index = InstructionUtilities.GetTokenIndex(tokens, ["+"]);
            Assert.That(index, Is.EqualTo(-1));
        }

        [Test]
        public void IgnoresTokensWithinSeparators()
        {
            var lexer = new Lexer("x = [4 + 5]");
            var tokens = lexer.Run();
            var index = InstructionUtilities.GetTokenIndex(tokens, ["+"]);
            Assert.That(index, Is.EqualTo(-1));
        }
        
        [Test]
        public void AllowsFindingUnnestedOpeningSeparators()
        {
            var lexer = new Lexer("x = [4 + 5]");
            var tokens = lexer.Run();
            var index = InstructionUtilities.GetTokenIndex(tokens, ["["]);
            Assert.That(index, Is.EqualTo(2));
        }

        [Test]
        public void AllowsFindingUnnestedClosingSeparators()
        {
            var lexer = new Lexer("x = [4 + 5]");
            var tokens = lexer.Run();
            var index = InstructionUtilities.GetTokenIndex(tokens, ["]"]);
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
            int index = InstructionUtilities.GetOperatorIndex(tokens);
            Assert.That(index, Is.EqualTo(3));
        }

        [Test]
        public void EvaluatesRightToLeft()
        {
            Lexer lexer = new Lexer("x = 1 + 2 + 3");
            var tokens = lexer.Run();
            int index = InstructionUtilities.GetOperatorIndex(tokens);
            Assert.That(index, Is.EqualTo(5));
        }

        [Test]
        public void EvaluatesLowestToHighestPriority()
        {
            Lexer lexer = new Lexer("x = 1 + 2 * 3");
            var tokens = lexer.Run();
            int index = InstructionUtilities.GetOperatorIndex(tokens);
            Assert.That(index, Is.EqualTo(3));
        }

        [Test]
        public void SkipsTokensInSeparatorBlocks()
        {
            Lexer lexer = new Lexer("var x = 1 + x[1 + 2]");
            var tokens = lexer.Run();
            int index = InstructionUtilities.GetOperatorIndex(tokens);
            Assert.That(index, Is.EqualTo(4));
        }

        [Test]
        public void DoesNotExist()
        {
            Lexer lexer = new Lexer("x = 12");
            var tokens = lexer.Run();
            int index = InstructionUtilities.GetOperatorIndex(tokens);
            Assert.That(index, Is.EqualTo(-1));
        }
    }
    
    [TestFixture]
    public class ParseLeftAndRightAroundIndex
    {
        [Test]
        public void HandlesNoTokens()
        {
            var result = InstructionUtilities.ParseLeftAndRightAroundIndex([], 0);
            Assert.That(result.Left, Is.Null);
            Assert.That(result.Right, Is.Null);
        }
        
        [Test]
        public void HandlesDivisionAtFirstEntry()
        {
            var lexer = new Lexer("x = 5 + 6");
            var lexerResult = lexer.Run();
            var result = InstructionUtilities.ParseLeftAndRightAroundIndex(lexerResult, 0);
            Assert.That(result.Left, Is.Null);
        }
        
        [Test]
        public void HandlesDivisionAtLastEntry()
        {
            var lexer = new Lexer("x = 5 + 6");
            var lexerResult = lexer.Run();
            var result = InstructionUtilities.ParseLeftAndRightAroundIndex(lexerResult, lexerResult.Count - 1);
            Assert.That(result.Right, Is.Null);
        }
        
        [Test]
        public void HandlesStandardDivision()
        {
            var lexer = new Lexer("x = 5");
            var lexerResult = lexer.Run();
            var result = InstructionUtilities.ParseLeftAndRightAroundIndex(lexerResult, 1);
            Assert.That(result.Left, Is.TypeOf<VariableInstruction>());
            Assert.That(result.Right, Is.TypeOf<IntegerInstruction>());
        }
    }
    
    [TestFixture]
    public class ParseEntriesWithinSeparator
    {
        [Test]
        public void NoTokens()
        {
            var parsed = 
                InstructionUtilities.ParseEntriesBetweenSeparatingCharacters([], []);

            Assert.That(parsed.Count, Is.EqualTo(0));
        }
        
        [Test]
        public void NoSeparatingCharacters()
        {
            var lexer = new Lexer("1");
            var tokens = lexer.Run();
            var parsed =
                InstructionUtilities.ParseEntriesBetweenSeparatingCharacters(tokens, []);
            var expected = new IntegerInstruction(1);
            Assert.That(parsed[0], Is.EqualTo(expected));
        }

        [Test]
        public void WithSeparatingCharacters()
        {
            var lexer = new Lexer("1, 2");
            var tokens = lexer.Run();
            var parsed =
                InstructionUtilities.ParseEntriesBetweenSeparatingCharacters(tokens, [","]);

            var expected = new List<Instruction>()
            {
                new IntegerInstruction(1),
                new IntegerInstruction(2)
            };
            Assert.That(parsed, Is.EquivalentTo(expected));
        }

        [Test]
        public void RespectsInnerSeparators()
        {
            var lexer = new Lexer("[1, 2], [2]");
            var tokens = lexer.Run();
            var parsed =
                InstructionUtilities.ParseEntriesBetweenSeparatingCharacters(tokens, [","]);
            
            var expected = new List<Instruction>()
            {
                new ArrayInstruction([new IntegerInstruction(1), new IntegerInstruction(2)], delimiter: ",", separator: "["),
                new ArrayInstruction([new IntegerInstruction(2)], separator: "[")
            };

            Assert.That(parsed, Is.EquivalentTo(expected));
        }
    }
}
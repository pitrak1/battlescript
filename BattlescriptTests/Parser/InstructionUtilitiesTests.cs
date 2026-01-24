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
        public void IgnoresTokensWithinStrings()
        {
            var lexer = new Lexer("x = ['4 + 5']");
            var tokens = lexer.Run();
            var index = InstructionUtilities.GetTokenIndex(tokens, ["+"]);
            Assert.That(index, Is.EqualTo(-1));
        }

        [Test]
        public void AllowsFindingOpeningSeparators()
        {
            var lexer = new Lexer("x = [4 + 5]");
            var tokens = lexer.Run();
            var index = InstructionUtilities.GetTokenIndex(tokens, ["["]);
            Assert.That(index, Is.EqualTo(2));
        }

        [Test]
        public void AllowsFindingClosingSeparators()
        {
            var lexer = new Lexer("x = [4 + 5]");
            var tokens = lexer.Run();
            var index = InstructionUtilities.GetTokenIndex(tokens, ["]"]);
            Assert.That(index, Is.EqualTo(6));
        }

        [Test]
        public void EmptyTokenListReturnsNegativeOne()
        {
            var index = InstructionUtilities.GetTokenIndex([], ["+"]);
            Assert.That(index, Is.EqualTo(-1));
        }

        [Test]
        public void BothValuesAndTypesFindsFirstMatch()
        {
            var lexer = new Lexer("x = 1 + 2");
            var tokens = lexer.Run();
            var index = InstructionUtilities.GetTokenIndex(tokens, ["+"], [Consts.TokenTypes.Assignment]);
            Assert.That(index, Is.EqualTo(1));
        }

        [Test]
        public void NestedBracketsFindsOuterClosing()
        {
            var lexer = new Lexer("[[1]]");
            var tokens = lexer.Run();
            var index = InstructionUtilities.GetTokenIndex(tokens, ["]"]);
            Assert.That(index, Is.EqualTo(4));
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
            int index = InstructionUtilities.GetLowestPriorityOperatorIndex(tokens);
            Assert.That(index, Is.EqualTo(3));
        }

        [Test]
        public void EvaluatesRightToLeft()
        {
            Lexer lexer = new Lexer("x = 1 + 2 + 3");
            var tokens = lexer.Run();
            int index = InstructionUtilities.GetLowestPriorityOperatorIndex(tokens);
            Assert.That(index, Is.EqualTo(5));
        }

        [Test]
        public void EvaluatesLowestToHighestPriority()
        {
            Lexer lexer = new Lexer("x = 1 + 2 * 3");
            var tokens = lexer.Run();
            int index = InstructionUtilities.GetLowestPriorityOperatorIndex(tokens);
            Assert.That(index, Is.EqualTo(3));
        }

        [Test]
        public void SkipsTokensInSeparatorBlocks()
        {
            Lexer lexer = new Lexer("var x = 1 + x[1 + 2]");
            var tokens = lexer.Run();
            int index = InstructionUtilities.GetLowestPriorityOperatorIndex(tokens);
            Assert.That(index, Is.EqualTo(4));
        }

        [Test]
        public void DoesNotExist()
        {
            Lexer lexer = new Lexer("x = 12");
            var tokens = lexer.Run();
            int index = InstructionUtilities.GetLowestPriorityOperatorIndex(tokens);
            Assert.That(index, Is.EqualTo(-1));
        }

        [Test]
        public void EmptyTokenListReturnsNegativeOne()
        {
            var index = InstructionUtilities.GetLowestPriorityOperatorIndex([]);
            Assert.That(index, Is.EqualTo(-1));
        }

        [Test]
        public void UnaryMinusAtStartFindsBinaryOperator()
        {
            var lexer = new Lexer("-5 + 3");
            var tokens = lexer.Run();
            var index = InstructionUtilities.GetLowestPriorityOperatorIndex(tokens);
            Assert.That(index, Is.EqualTo(2));
        }

        [Test]
        public void UnaryMinusAfterOperatorFindsBinaryOperator()
        {
            var lexer = new Lexer("3 * -5");
            var tokens = lexer.Run();
            var index = InstructionUtilities.GetLowestPriorityOperatorIndex(tokens);
            Assert.That(index, Is.EqualTo(1));
        }

        [Test]
        public void UnaryPlusAfterOpenParenTreatedAsUnary()
        {
            var lexer = new Lexer("(+5)");
            var tokens = lexer.Run();
            var index = InstructionUtilities.GetLowestPriorityOperatorIndex(tokens);
            Assert.That(index, Is.EqualTo(-1));
        }

        [Test]
        public void UnaryAfterCommaTreatedAsUnary()
        {
            var lexer = new Lexer("foo(-1, -2)");
            var tokens = lexer.Run();
            var index = InstructionUtilities.GetLowestPriorityOperatorIndex(tokens);
            Assert.That(index, Is.EqualTo(-1));
        }
    }

    [TestFixture]
    public class ParseLeftAndRightAroundIndex
    {
        [Test]
        public void NoTokens()
        {
            var result = InstructionUtilities.ParseLeftAndRightAroundIndex([], 0);
            Assert.That(result.Left, Is.Null);
            Assert.That(result.Right, Is.Null);
        }

        [Test]
        public void DivisionAtFirstEntry()
        {
            var lexer = new Lexer("x = 5 + 6");
            var lexerResult = lexer.Run();
            var result = InstructionUtilities.ParseLeftAndRightAroundIndex(lexerResult, 0);
            Assert.That(result.Left, Is.Null);
        }

        [Test]
        public void DivisionAtLastEntry()
        {
            var lexer = new Lexer("x = 5 + 6");
            var lexerResult = lexer.Run();
            var result = InstructionUtilities.ParseLeftAndRightAroundIndex(lexerResult, lexerResult.Count - 1);
            Assert.That(result.Right, Is.Null);
        }

        [Test]
        public void StandardDivision()
        {
            var lexer = new Lexer("x = 5");
            var lexerResult = lexer.Run();
            var result = InstructionUtilities.ParseLeftAndRightAroundIndex(lexerResult, 1);
            Assert.That(result.Left, Is.TypeOf<VariableInstruction>());
            Assert.That(result.Right, Is.TypeOf<NumericInstruction>());
        }

        [Test]
        public void SingleTokenBothNull()
        {
            var lexer = new Lexer("x");
            var lexerResult = lexer.Run();
            var result = InstructionUtilities.ParseLeftAndRightAroundIndex(lexerResult, 0);
            Assert.That(result.Left, Is.Null);
            Assert.That(result.Right, Is.Null);
        }
    }

    [TestFixture]
    public class ParseEntriesBetweenDelimiters
    {
        [Test]
        public void NoTokens()
        {
            var parsed =
                InstructionUtilities.ParseEntriesBetweenDelimiters([], []);

            Assert.That(parsed.Count, Is.EqualTo(0));
        }

        [Test]
        public void NoSeparatingCharacters()
        {
            var lexer = new Lexer("1");
            var tokens = lexer.Run();
            var parsed =
                InstructionUtilities.ParseEntriesBetweenDelimiters(tokens, []);
            var expected = new NumericInstruction(1);
            Assert.That(parsed[0], Is.EqualTo(expected));
        }

        [Test]
        public void WithSeparatingCharacters()
        {
            var lexer = new Lexer("1, 2");
            var tokens = lexer.Run();
            var parsed =
                InstructionUtilities.ParseEntriesBetweenDelimiters(tokens, [","]);

            var expected = new List<Instruction>()
            {
                new NumericInstruction(1),
                new NumericInstruction(2)
            };
            Assert.That(parsed, Is.EquivalentTo(expected));
        }

        [Test]
        public void RespectsInnerSeparators()
        {
            var lexer = new Lexer("[1, 2], [2]");
            var tokens = lexer.Run();
            var parsed =
                InstructionUtilities.ParseEntriesBetweenDelimiters(tokens, [","]);

            var expected = new List<Instruction>()
            {
                new ArrayInstruction([new NumericInstruction(1), new NumericInstruction(2)], ArrayInstruction.BracketTypes.SquareBrackets, ArrayInstruction.DelimiterTypes.Comma),
                new ArrayInstruction([new NumericInstruction(2)], ArrayInstruction.BracketTypes.SquareBrackets)
            };

            Assert.That(parsed, Is.EquivalentTo(expected));
        }

        [Test]
        public void TrailingDelimiterCreatesNullEntry()
        {
            var lexer = new Lexer("1,");
            var tokens = lexer.Run();
            var parsed = InstructionUtilities.ParseEntriesBetweenDelimiters(tokens, [","]);
            Assert.That(parsed.Count, Is.EqualTo(2));
            Assert.That(parsed[0], Is.EqualTo(new NumericInstruction(1)));
            Assert.That(parsed[1], Is.Null);
        }

        [Test]
        public void LeadingDelimiterCreatesNullEntry()
        {
            var lexer = new Lexer(",1");
            var tokens = lexer.Run();
            var parsed = InstructionUtilities.ParseEntriesBetweenDelimiters(tokens, [","]);
            Assert.That(parsed.Count, Is.EqualTo(2));
            Assert.That(parsed[0], Is.Null);
            Assert.That(parsed[1], Is.EqualTo(new NumericInstruction(1)));
        }

        [Test]
        public void ConsecutiveDelimitersCreatesNullEntry()
        {
            var lexer = new Lexer("1,,2");
            var tokens = lexer.Run();
            var parsed = InstructionUtilities.ParseEntriesBetweenDelimiters(tokens, [","]);
            Assert.That(parsed.Count, Is.EqualTo(3));
            Assert.That(parsed[0], Is.EqualTo(new NumericInstruction(1)));
            Assert.That(parsed[1], Is.Null);
            Assert.That(parsed[2], Is.EqualTo(new NumericInstruction(2)));
        }
    }

    [TestFixture]
    public class GetGroupedTokensAtStart
    {
        [Test]
        public void NoOpeningBracketReturnsEmpty()
        {
            var lexer = new Lexer("x + 1");
            var tokens = lexer.Run();
            var result = InstructionUtilities.GetGroupedTokensAtStart(tokens);
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void EmptyBracketsReturnsEmpty()
        {
            var lexer = new Lexer("()");
            var tokens = lexer.Run();
            var result = InstructionUtilities.GetGroupedTokensAtStart(tokens);
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void NestedBracketsReturnsInnerContent()
        {
            var lexer = new Lexer("([1, 2])");
            var tokens = lexer.Run();
            var result = InstructionUtilities.GetGroupedTokensAtStart(tokens);
            Assert.That(result.Count, Is.EqualTo(5));
            Assert.That(result[0].Value, Is.EqualTo("["));
            Assert.That(result[4].Value, Is.EqualTo("]"));
        }
    }
}

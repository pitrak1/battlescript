using Battlescript;

namespace BattlescriptTests;

[TestFixture]
public static class LexerTests
{
    [TestFixture]
    public class Numbers
    {
        [Test]
        public void HandlesIntegers()
        {
            LexerAssertions.AssertInputProducesOutput("213", Consts.TokenTypes.Number, "213");
        }

        [Test]
        public void HandlesFloats()
        {
            LexerAssertions.AssertInputProducesOutput("1.23", Consts.TokenTypes.Number, "1.23");
        }
        
        [Test]
        public void HandlesFloatsWithStartingDecimal()
        {
            LexerAssertions.AssertInputProducesOutput(".23", Consts.TokenTypes.Number, ".23");
        }
        
        [Test]
        public void HandlesNegatives()
        {
            LexerAssertions.AssertInputProducesOutput("-213", Consts.TokenTypes.Number, "-213");
        }
    }

    [TestFixture]
    public class Strings
    {
        [Test]
        public void HandlesDoubleQuotes()
        {
            LexerAssertions.AssertInputProducesOutput("\"asdf\"", Consts.TokenTypes.String, "asdf");
        }

        [Test]
        public void HandlesSingleQuotes()
        {
            LexerAssertions.AssertInputProducesOutput("'asdf'", Consts.TokenTypes.String, "asdf");
        }
    }

    [TestFixture]
    public class Separators
    {
        [Test]
        public void HandlesSeparators()
        {
            LexerAssertions.AssertInputProducesOutput(",", Consts.TokenTypes.Separator, ",");
        }
    }

    [TestFixture]
    public class Words
    {
        [Test]
        public void HandlesKeywords()
        {
            LexerAssertions.AssertInputProducesOutput("async", Consts.TokenTypes.Keyword, "async");
        }

        [Test]
        public void HandlesBooleans()
        {
            LexerAssertions.AssertInputProducesOutput("True", Consts.TokenTypes.Boolean, "True");
        }

        [Test]
        public void HandlesWordOperators()
        {
            LexerAssertions.AssertInputProducesOutput("and", Consts.TokenTypes.Operator, "and");
        }

        [Test]
        public void HandlesIdentifiers()
        {
            LexerAssertions.AssertInputProducesOutput("asdf", Consts.TokenTypes.Identifier, "asdf");
        }
    }

    [TestFixture]
    public class Operators
    {
        [Test]
        public void HandlesSingleCharacterOperators()
        {
            LexerAssertions.AssertInputProducesOutput("+", Consts.TokenTypes.Operator, "+");
        }
        
        [Test]
        public void HandlesDoubleCharacterOperators()
        {
            LexerAssertions.AssertInputProducesOutput("**", Consts.TokenTypes.Operator, "**");
        }
        
        [Test]
        public void HandlesTripleCharacterOperators()
        {
            LexerAssertions.AssertInputProducesOutput("not", Consts.TokenTypes.Operator, "not");
        }
        
        [Test]
        public void HandlesIsNotOperator()
        {
            var expected = new List<Token>()
            {
                new(Consts.TokenTypes.Identifier, "x"),
                new(Consts.TokenTypes.Operator, "is not"),
                new(Consts.TokenTypes.Identifier, "y"),
            };
            LexerAssertions.AssertInputProducesOutputList("x is not y", expected);
        }
        
        [Test]
        public void HandlesNotInOperator()
        {
            var expected = new List<Token>()
            {
                new(Consts.TokenTypes.Identifier, "x"),
                new(Consts.TokenTypes.Operator, "not in"),
                new(Consts.TokenTypes.Identifier, "y"),
            };
            LexerAssertions.AssertInputProducesOutputList("x not in y", expected);
        }
    }
    
    [TestFixture]
    public class AssignmentOperators
    {
        [Test]
        public void HandlesSingleCharacterOperators()
        {
            LexerAssertions.AssertInputProducesOutput("=", Consts.TokenTypes.Assignment, "=");
        }
        
        [Test]
        public void HandlesDoubleCharacterOperators()
        {
            LexerAssertions.AssertInputProducesOutput("*=", Consts.TokenTypes.Assignment, "*=");
        }
        
        [Test]
        public void HandlesTripleCharacterOperators()
        {
            LexerAssertions.AssertInputProducesOutput("**=", Consts.TokenTypes.Assignment, "**=");
        }
    }

    [TestFixture]
    public class Comments
    {
        [Test]
        public void IgnoresComments()
        {
            LexerAssertions.AssertInputProducesOutput("= #asdfadsf", Consts.TokenTypes.Assignment, "=");
        }
    }
    
    [TestFixture]
    public class NewLine
    {
        [Test]
        public void DetectsNewLineAndIndents()
        {
            // This has four spaces in addition to two tabs, meaning it should detect three tabs
            var expected = new List<Token>()
            {
                new(Consts.TokenTypes.Identifier, "asdf"),
                new(Consts.TokenTypes.Newline, "3"),
                new(Consts.TokenTypes.Identifier, "asdf")
            };
            LexerAssertions.AssertInputProducesOutputList("asdf\n\t\t    asdf", expected);
        }

        [Test]
        public void IgnoresTrailingSpaces()
        {
            // This has three spaces
            var expected = new List<Token>()
            {
                new(Consts.TokenTypes.Identifier, "asdf"),
                new(Consts.TokenTypes.Newline, "2"),
                new(Consts.TokenTypes.Identifier, "asdf")
            };
            LexerAssertions.AssertInputProducesOutputList("asdf\n\t\t   asdf", expected);
        }
        
        [Test]
        public void HandlesNoIndent()
        {
            var expected = new List<Token>()
            {
                new(Consts.TokenTypes.Identifier, "asdf"),
                new(Consts.TokenTypes.Newline, "0"),
                new(Consts.TokenTypes.Identifier, "asdf")
            };
            LexerAssertions.AssertInputProducesOutputList("asdf\nasdf", expected);
        }
    }
}
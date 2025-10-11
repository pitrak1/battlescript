using Battlescript;

namespace BattlescriptTests.LexerTests;

[TestFixture]
public static class LexerUtilitiesTests
{
    [TestFixture]
    public class GetNextCharactersInCollectionIncludingEscapes
    {
        [Test]
        public void SimpleInclusiveSearch()
        {
            var (length, result) = LexerUtilities.GetNextCharactersInCollectionIncludingEscapes(
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
            var (length, result) = LexerUtilities.GetNextCharactersInCollectionIncludingEscapes(
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
            var (length, result) = LexerUtilities.GetNextCharactersInCollectionIncludingEscapes(
                "asdf.",
                2,
                Consts.Letters,
                CollectionType.Inclusive
            );

            Assert.That(result, Is.EqualTo("df"));
        }

        [Test]
        public void HandlesEscapedCharacters()
        {
            var (length, result) = LexerUtilities.GetNextCharactersInCollectionIncludingEscapes(
                @"as\.df.",
                0,
                Consts.Letters,
                CollectionType.Inclusive
            );

            Assert.That(result, Is.EqualTo("as.df"));
        }
        
        [Test]
        public void LengthIsNumberOfCharactersIncludingSlashes()
        {
            var (length, result) = LexerUtilities.GetNextCharactersInCollectionIncludingEscapes(
                @"as\.df\.\'",
                0,
                Consts.Letters,
                CollectionType.Inclusive
            );

            Assert.That(result, Is.EqualTo("as.df.'"));
            Assert.That(length, Is.EqualTo(10));
        }
    }
    
    [TestFixture]
    public class GetNextCharactersInCollection
    {
        [Test]
        public void SimpleInclusiveSearch()
        {
            var result = LexerUtilities.GetNextCharactersInCollection(
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
            var result = LexerUtilities.GetNextCharactersInCollection(
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
            var result = LexerUtilities.GetNextCharactersInCollection(
                "asdf.",
                2,
                Consts.Letters,
                CollectionType.Inclusive
            );

            Assert.That(result, Is.EqualTo("df"));
        }

        [Test]
        public void DoesNotHandleEscapedCharacters()
        {
            var result = LexerUtilities.GetNextCharactersInCollection(
                @"as\.df.",
                0,
                Consts.Letters,
                CollectionType.Inclusive
            );

            Assert.That(result, Is.EqualTo("as"));
        }
    }
    
    [TestFixture]
    public class GetIndentValueFromIndentationString
    {
        [Test]
        public void DetectsTabs()
        {
            var indent = LexerUtilities.GetIndentValueFromIndentationString("\t\t\t");
            Assert.That(indent, Is.EqualTo(3));
        }

        [Test]
        public void DetectsSpacesInSetsOfFour()
        {
            // this is 8 spaces
            var indent = LexerUtilities.GetIndentValueFromIndentationString("        ");
            Assert.That(indent, Is.EqualTo(2));
        }
        
        [Test]
        public void DetectsCombinationsOfTabsAndSpaces()
        {
            // this is 8 spaces total
            var indent = LexerUtilities.GetIndentValueFromIndentationString("  \t \t  \t   ");
            Assert.That(indent, Is.EqualTo(5));
        }
        
        [Test]
        public void ExtraSpacesAreRoundedDown()
        {
            // this is 6 spaces total
            var indent = LexerUtilities.GetIndentValueFromIndentationString("      ");
            Assert.That(indent, Is.EqualTo(1));
        }
        
        [Test]
        public void HandlesEmptyString()
        {
            var indent = LexerUtilities.GetIndentValueFromIndentationString("");
            Assert.That(indent, Is.EqualTo(0));
        }
    }

    [TestFixture]
    public class GetNextThreeCharacters
    {
        [Test]
        public void IncludesAllResultsWhenStringIsNotLimited()
        {
            var result = LexerUtilities.GetNextThreeCharacters("asd", 0);
            Assert.That(result.Item1, Is.EqualTo("d"));
            Assert.That(result.Item2, Is.EqualTo("s"));
            Assert.That(result.Item3, Is.EqualTo("a"));
        }

        [Test]
        public void IncludesAllResultsWhenStringHasOnlyTwoCharacters()
        {
            var result = LexerUtilities.GetNextThreeCharacters("as", 0);
            Assert.That(result.Item1, Is.EqualTo(""));
            Assert.That(result.Item2, Is.EqualTo("s"));
            Assert.That(result.Item3, Is.EqualTo("a"));
        }
        
        [Test]
        public void IncludesAllResultsWhenStringHasOnlyOneCharacter()
        {
            var result = LexerUtilities.GetNextThreeCharacters("a", 0);
            Assert.That(result.Item1, Is.EqualTo(""));
            Assert.That(result.Item2, Is.EqualTo(""));
            Assert.That(result.Item3, Is.EqualTo("a"));
        }

        [Test]
        public void ReturnsReasonableValuesIfGivenEmptyInput()
        {
            var result = LexerUtilities.GetNextThreeCharacters("", 0);
            Assert.That(result.Item1, Is.EqualTo(""));
            Assert.That(result.Item2, Is.EqualTo(""));
            Assert.That(result.Item3, Is.EqualTo(""));
        }

        [Test]
        public void RespectsIndex()
        {
            var result = LexerUtilities.GetNextThreeCharacters("asdfasdf", 3);
            Assert.That(result.Item1, Is.EqualTo("s"));
            Assert.That(result.Item2, Is.EqualTo("a"));
            Assert.That(result.Item3, Is.EqualTo("f"));
        }

        [Test]
        public void RespectsIndexWithEndOfInput()
        {
            var result = LexerUtilities.GetNextThreeCharacters("asdfasdf", 7);
            Assert.That(result.Item1, Is.EqualTo(""));
            Assert.That(result.Item2, Is.EqualTo(""));
            Assert.That(result.Item3, Is.EqualTo("f"));
        }
    }
}
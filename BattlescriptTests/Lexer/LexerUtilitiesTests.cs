using Battlescript;

namespace BattlescriptTests.LexerTests;

[TestFixture]
public static class LexerUtilitiesTests
{
    [TestFixture]
    public class GetNextCharactersInCollection
    {
        [Test]
        public void SimpleInclusiveSearch()
        {
            var (length, result) = LexerUtilities.GetNextCharactersInCollection(
                "asdf.",
                0,
                Consts.Letters
            );
            Assert.That(result, Is.EqualTo("asdf"));
        }
        
        [Test]
        public void SimpleExclusiveSearch()
        {
            var (length, result) = LexerUtilities.GetNextCharactersInCollection(
                "asdf.",
                0,
                ['.'],
                CollectionType.Exclusive
            );

            Assert.That(result, Is.EqualTo("asdf"));
        }
        
        [Test]
        public void RespectsIndex()
        {
            var (length, result) = LexerUtilities.GetNextCharactersInCollection(
                "asdf.",
                2,
                Consts.Letters
            );

            Assert.That(result, Is.EqualTo("df"));
        }
        
        [Test]
        public void DoesNotHandleEscapedCharacters()
        {
            var (length, result) = LexerUtilities.GetNextCharactersInCollection(
                @"as\.df.",
                0,
                Consts.Letters
            );

            Assert.That(result, Is.EqualTo("as"));
        }

        [Test]
        public void HandlesEscapedCharacters()
        {
            var (length, result) = LexerUtilities.GetNextCharactersInCollection(
                @"as\.df.",
                0,
                Consts.Letters,
                CollectionType.Inclusive,
                true
            );

            Assert.That(result, Is.EqualTo("as.df"));
        }
        
        [Test]
        public void LengthIsNumberOfCharactersIncludingSlashes()
        {
            var (length, result) = LexerUtilities.GetNextCharactersInCollection(
                @"as\.df\.\'",
                0,
                Consts.Letters,
                CollectionType.Inclusive,
                true
            );

            Assert.That(result, Is.EqualTo("as.df.'"));
            Assert.That(length, Is.EqualTo(10));
        }
    }
    
    [TestFixture]
    public class GetIndentValue
    {
        [Test]
        public void DetectsTabs()
        {
            var (length, result) = LexerUtilities.GetIndentValue("\t\t\t", 0);
            Assert.That(result, Is.EqualTo("3"));
        }

        [Test]
        public void DetectsSpacesInSetsOfFour()
        {
            // this is 8 spaces
            var (length, result) = LexerUtilities.GetIndentValue("        ", 0);
            Assert.That(result, Is.EqualTo("2"));
        }
        
        [Test]
        public void DetectsCombinationsOfTabsAndSpaces()
        {
            // this is 8 spaces total
            var (length, result) = LexerUtilities.GetIndentValue("  \t \t  \t   ", 0);
            Assert.That(result, Is.EqualTo("5"));
        }
        
        [Test]
        public void ExtraSpacesAreRoundedDown()
        {
            // this is 6 spaces total
            var (length, result) = LexerUtilities.GetIndentValue("      ", 0);
            Assert.That(result, Is.EqualTo("1"));
        }
        
        [Test]
        public void HandlesEmptyString()
        {
            var (length, result) = LexerUtilities.GetIndentValue("", 0);
            Assert.That(result, Is.EqualTo("0"));
        }
        
        [Test]
        public void RespectsIndex()
        {
            var (length, result) = LexerUtilities.GetIndentValue("\t\t\t\t", 2);
            Assert.That(result, Is.EqualTo("2"));
        }
    }
}
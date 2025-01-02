using Battlescript;

namespace BattlescriptTests;

public static class LexerUtilitiesTests
{
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
    }

    [TestFixture]
    public class GetNextThreeCharacters
    {
        [Test]
        public void IncludesAllResultsWhenStringIsNotLimited()
        {
            var result = LexerUtilities.GetNextThreeCharacters("asd", 0);
            Assert.That(result.Item1, Is.EqualTo("asd"));
            Assert.That(result.Item2, Is.EqualTo("as"));
            Assert.That(result.Item3, Is.EqualTo('a'));
        }

        [Test]
        public void IncludesAllResultsWhenStringHasOnlyTwoCharacters()
        {
            var result = LexerUtilities.GetNextThreeCharacters("as", 0);
            Assert.That(result.Item1, Is.EqualTo("as"));
            Assert.That(result.Item2, Is.EqualTo("as"));
            Assert.That(result.Item3, Is.EqualTo('a'));
        }
        
        [Test]
        public void IncludesAllResultsWhenStringHasOnlyOneCharacter()
        {
            var result = LexerUtilities.GetNextThreeCharacters("a", 0);
            Assert.That(result.Item1, Is.EqualTo("a"));
            Assert.That(result.Item2, Is.EqualTo("a"));
            Assert.That(result.Item3, Is.EqualTo('a'));
        }

        [Test]
        public void ReturnsReasonableValuesIfGivenEmptyInput()
        {
            var result = LexerUtilities.GetNextThreeCharacters("", 0);
            Assert.That(result.Item1, Is.EqualTo(""));
            Assert.That(result.Item2, Is.EqualTo(""));
            Assert.That(result.Item3, Is.EqualTo(' '));
        }

        [Test]
        public void RespectsIndex()
        {
            var result = LexerUtilities.GetNextThreeCharacters("asdfasdf", 3);
            Assert.That(result.Item1, Is.EqualTo("fas"));
            Assert.That(result.Item2, Is.EqualTo("fa"));
            Assert.That(result.Item3, Is.EqualTo('f'));
        }

        [Test]
        public void RespectsIndexWithEndOfInput()
        {
            var result = LexerUtilities.GetNextThreeCharacters("asdfasdf", 7);
            Assert.That(result.Item1, Is.EqualTo("f"));
            Assert.That(result.Item2, Is.EqualTo("f"));
            Assert.That(result.Item3, Is.EqualTo('f'));
        }
    }
}
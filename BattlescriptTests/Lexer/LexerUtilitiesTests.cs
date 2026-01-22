using Battlescript;

namespace BattlescriptTests.LexerTests;

[TestFixture]
public static class LexerUtilitiesTests
{
    [TestFixture]
    public class GetNextCharactersWhile
    {
        [Test]
        public void SimpleSearch()
        {
            var (length, result) = LexerUtilities.GetNextCharactersWhile(
                "asdf.",
                0,
                char.IsAsciiLetter
            );
            Assert.That(result, Is.EqualTo("asdf"));
            Assert.That(length, Is.EqualTo(4));
        }

        [Test]
        public void RespectsIndex()
        {
            var (length, result) = LexerUtilities.GetNextCharactersWhile(
                "asdf.",
                2,
                char.IsAsciiLetter
            );
            Assert.That(result, Is.EqualTo("df"));
            Assert.That(length, Is.EqualTo(2));
        }

        [Test]
        public void StopsAtNonMatchingCharacter()
        {
            var (length, result) = LexerUtilities.GetNextCharactersWhile(
                "abc123def",
                0,
                char.IsAsciiLetter
            );
            Assert.That(result, Is.EqualTo("abc"));
            Assert.That(length, Is.EqualTo(3));
        }

        [Test]
        public void EmptyResultWhenFirstCharDoesNotMatch()
        {
            var (length, result) = LexerUtilities.GetNextCharactersWhile(
                "123abc",
                0,
                char.IsAsciiLetter
            );
            Assert.That(result, Is.EqualTo(""));
            Assert.That(length, Is.EqualTo(0));
        }

        [Test]
        public void WithEscapesHandlesEscapedCharacters()
        {
            var (length, result) = LexerUtilities.GetNextCharactersWhile(
                @"hello\'world'end",
                0,
                c => c != '\'',
                allowEscapes: true
            );
            Assert.That(result, Is.EqualTo("hello'world"));
            Assert.That(length, Is.EqualTo(12));
        }

        [Test]
        public void WithEscapesHandlesMultipleEscapes()
        {
            var (length, result) = LexerUtilities.GetNextCharactersWhile(
                @"a\'b\'c'",
                0,
                c => c != '\'',
                allowEscapes: true
            );
            Assert.That(result, Is.EqualTo("a'b'c"));
            Assert.That(length, Is.EqualTo(7));
        }

        [Test]
        public void WithEscapesHandlesEscapedBackslash()
        {
            var (length, result) = LexerUtilities.GetNextCharactersWhile(
                @"a\\b'",
                0,
                c => c != '\'',
                allowEscapes: true
            );
            Assert.That(result, Is.EqualTo(@"a\b"));
            Assert.That(length, Is.EqualTo(4));
        }

        [Test]
        public void WithoutEscapesDoesNotHandleEscapedCharacters()
        {
            var (length, result) = LexerUtilities.GetNextCharactersWhile(
                @"hello\'world",
                0,
                char.IsAsciiLetter
            );
            Assert.That(result, Is.EqualTo("hello"));
            Assert.That(length, Is.EqualTo(5));
        }

        [Test]
        public void WithEscapesHandlesEscapedBackslashAtEndOfString()
        {
            var (length, result) = LexerUtilities.GetNextCharactersWhile(
                @"abc\\",
                0,
                c => c != '\'',
                allowEscapes: true
            );
            Assert.That(result, Is.EqualTo(@"abc\"));
            Assert.That(length, Is.EqualTo(5));
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
        public void EmptyString()
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

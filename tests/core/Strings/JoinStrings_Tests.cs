using Xunit;

using static System.Environment;

namespace Core.Tests.Strings
{
    using static Luminous.Code.Strings.Concatenation;

    public class JoinStrings_Tests
    {
        [Fact(DisplayName = nameof(ValidFirstValidSecondValidSeparatorReturnsFirstSeparatorSecond))]
        public void ValidFirstValidSecondValidSeparatorReturnsFirstSeparatorSecond()
        {
            const string first = "1";
            const string second = "2";
            const string separator = "-";
            var expected = $"{first}{separator}{second}";

            var actual = JoinStrings(first, second, separator);

            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = nameof(ValidFirstValidSecondNullSeparatorReturnsFirstNewLineNewLineSecond))]
        public void ValidFirstValidSecondNullSeparatorReturnsFirstNewLineNewLineSecond()
        {
            const string first = "1";
            const string second = "2";
            const string separator = null;
            var expected = $"{first}{NewLine}{NewLine}{second}";

            var actual = JoinStrings(first, second, separator);

            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = nameof(ValidFirstValidSecondEmptySeparatorReturnsFirstSecond))]
        public void ValidFirstValidSecondEmptySeparatorReturnsFirstSecond()
        {
            const string first = "1";
            const string second = "2";
            const string separator = "";
            var expected = $"{first}{second}";

            var actual = JoinStrings(first, second, separator);

            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = nameof(NullFirstValidSecondValidSeparatorReturnsSecond))]
        public void NullFirstValidSecondValidSeparatorReturnsSecond()
        {
            const string first = null;
            const string second = "2";
            const string separator = "-";
            const string expected = second;

            var actual = JoinStrings(first, second, separator);

            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = nameof(NullFirstValidSecondNullSeparatorReturnsSecond))]
        public void NullFirstValidSecondNullSeparatorReturnsSecond()
        {
            const string first = null;
            const string second = "2";
            const string separator = null;
            const string expected = second;

            var actual = JoinStrings(first, second, separator);

            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = nameof(NullFirstValidSecondEmptySeparatorReturnsSecond))]
        public void NullFirstValidSecondEmptySeparatorReturnsSecond()
        {
            const string first = null;
            const string second = "2";
            const string separator = "";
            const string expected = second;

            var actual = JoinStrings(first, second, separator);

            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = nameof(ValidFirstNullSecondValidSeparatorReturnsFirst))]
        public void ValidFirstNullSecondValidSeparatorReturnsFirst()
        {
            const string first = "1";
            const string second = null;
            const string separator = "-";
            const string expected = first;

            var actual = JoinStrings(first, second, separator);

            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = nameof(ValidFirstNullSecondNullSeparatorReturnsFirst))]
        public void ValidFirstNullSecondNullSeparatorReturnsFirst()
        {
            const string first = "1";
            const string second = null;
            const string separator = null;
            const string expected = first;

            var actual = JoinStrings(first, second, separator);

            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = nameof(ValidFirstNullSecondEmptySeparatorReturnsFirst))]
        public void ValidFirstNullSecondEmptySeparatorReturnsFirst()
        {
            const string first = "1";
            const string second = null;
            const string separator = null;
            const string expected = first;

            var actual = JoinStrings(first, second, separator);

            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = nameof(NullFirstNullSecondValidSeparatorReturnsEmptyString))]
        public void NullFirstNullSecondValidSeparatorReturnsEmptyString()
        {
            const string first = null;
            const string second = null;
            const string separator = "-";
            const string expected = "";

            var actual = JoinStrings(first, second, separator);

            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = nameof(NullFirstNullSecondNullSeparatorReturnsEmptyString))]
        public void NullFirstNullSecondNullSeparatorReturnsEmptyString()
        {
            const string first = null;
            const string second = null;
            const string separator = null;
            const string expected = "";

            var actual = JoinStrings(first, second, separator);

            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = nameof(NullFirstNullSecondEmptySeparatorReturnsEmptyString))]
        public void NullFirstNullSecondEmptySeparatorReturnsEmptyString()
        {
            const string first = null;
            const string second = null;
            const string separator = "";
            const string expected = "";

            var actual = JoinStrings(first, second, separator);

            Assert.Equal(expected, actual);
        }
    }
}
﻿using FluentAssertions;
using Xunit;

namespace Core.Tests
{
    using Luminous.Code.Strings.Extensions;

    public class StringExtensions_Tests
    {
        #region int

        [Fact(DisplayName = nameof(NumericString_ReturnsInt))]
        public void NumericString_ReturnsInt()
        {
            const string value = "5";
            var expected = 5;

            var actual = value.To<int>();

            actual.Should().Be(expected);
        }

        [Fact(DisplayName = nameof(TextString_ReturnsZero))]
        public void TextString_ReturnsZero()
        {
            const string value = "five";
            var expected = 0;

            var actual = value.To<int>();

            actual.Should().Be(expected);
        }

        [Fact(DisplayName = nameof(EmptyString_ReturnsZero))]
        public void EmptyString_ReturnsZero()
        {
            const string value = "";
            var expected = 0;

            var actual = value.To<int>();

            actual.Should().Be(expected);
        }

        [Fact(DisplayName = nameof(EmptyString_ReturnsZero))]
        public void NullString_ReturnsZero()
        {
            const string value = null;
            var expected = 0;

            var actual = value.To<int>();

            actual.Should().Be(expected);
        }

        #endregion

        #region bool

        [Fact(DisplayName = nameof(ConvertLowerCaseTrueToTrue))]
        public void ConvertLowerCaseTrueToTrue()
        {
            const string value = "true";
            var expected = true;

            var actual = value.To<bool>();

            actual.Should().Be(expected);
        }

        [Fact(DisplayName = nameof(ConvertLowerCaseFalseToFalse))]
        public void ConvertLowerCaseFalseToFalse()
        {
            const string value = "false";
            var expected = false;

            var actual = value.To<bool>();

            actual.Should().Be(expected);
        }

        [Fact(DisplayName = nameof(ConvertUpperCaseTrueStringToTrue))]
        public void ConvertUpperCaseTrueStringToTrue()
        {
            const string value = "True";
            var expected = true;

            var actual = value.To<bool>();

            actual.Should().Be(expected);
        }

        [Fact(DisplayName = nameof(ConvertUpperCaseFalseStringToFalse))]
        public void ConvertUpperCaseFalseStringToFalse()
        {
            const string value = "False";
            var expected = false;

            var actual = value.To<bool>();

            actual.Should().Be(expected);
        }

        [Fact(DisplayName = nameof(ConvertEmptyStringToFalse))]
        public void ConvertEmptyStringToFalse()
        {
            const string value = "";
            var expected = false;

            var actual = value.To<bool>();

            actual.Should().Be(expected);
        }

        [Fact(DisplayName = nameof(ConvertNullStringToFalse))]
        public void ConvertNullStringToFalse()
        {
            const string value = null;
            var expected = false;

            var actual = value.To<bool>();

            actual.Should().Be(expected);
        }

        #endregion
    }
}
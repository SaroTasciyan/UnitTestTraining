using System;
using System.Collections.Generic;

using FluentAssertions;
using Xunit;

using Innova.Utilities.Enumerable;
using Innova.Utilities.Test.Stubs;

namespace Innova.Utilities.Test.Fixtures
{
    public class EnumerableFixture
    {
        #region Properties

        public IEnumerable<string> EmptyReferenceEnumerable
        {
            get
            {
                yield break;
            }
        }

        public IEnumerable<int> EmptyValueEnumerable
        {
            get
            {
                yield break;
            }
        }

        #endregion

        #region LastOrDefault Tests

        [Fact]
        public void LastOrDefault_OfEmptyReferenceTypeEnumerable_ShouldReturnNull()
        {
            // # Create Test Case
            IEnumerable<string> enumerable = EmptyReferenceEnumerable;

            // # Execute System Under Test
            string actual = enumerable.LastOrDefault();

            // # Assert Expectation
            actual.Should().BeNull();
        }

        [Fact]
        public void LastOrDefault_OfEmptyValueTypeEnumerable_ShouldReturnDefaultValue()
        {
            // # Create Test Case
            IEnumerable<int> enumerable = EmptyValueEnumerable;

            // # Execute System Under Test
            int actual = enumerable.LastOrDefault();

            // # Assert Expectation
            actual.Should().Be(0);
        }

        [Fact]
        public void LastOrDefault_OfNonEmptyEnumerable_ShouldReturnLastElement()
        {
            // # Create Test Case
            IEnumerable<string> enumerable = GetEnumerable("Saro", "Emre", "Onur", "Ahmet", "Barış");

            // # Execute System Under Test
            string actual = enumerable.LastOrDefault();

            // # Assert Expectation
            actual.Should().Be("Barış");
        }

        [Fact]
        public void LastOrDefault_OfNullEnumerable_ShouldThrowArgumentNullException()
        {
            // # Create Test Case
            IEnumerable<string> enumerable = null;

            // # Execute System Under Test
            Action action = () => { enumerable.LastOrDefault(); };

            // # Assert Expectation
            action.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void LastOrDefault_OfEmptyIList_ShouldReturnDefaultValue()
        {
            // # Create Test Case
            string[] enumerable = new string[0];
            
            // # Execute System Under Test
            string actual = enumerable.LastOrDefault();

            // # Assert Expectation
            actual.Should().BeNull();
        }

        [Fact]
        public void LastOrDefault_OfNonEmptyIList_ShouldReturnLastElement()
        {
            // # Create Test Case
            string[] enumerable = new string[] { "Saro", "Emre", "Onur", "Ahmet", "Barış" };

            // # Execute System Under Test
            string actual = enumerable.LastOrDefault();

            // # Assert Expectation
            actual.Should().Be("Barış");
        }

        [Fact]
        public void LastOrDefault_OfNonEmptyIList_ShouldNotEnumerate()
        {
            // # Create Test Case
            ListStub<string> enumerable = new ListStub<string>(new List<string> { "Saro", "Emre", "Onur", "Ahmet", "Barış" });

            // # Execute System Under Test
            enumerable.LastOrDefault();

            // # Assert Expectation
            enumerable.IsEnumerated.Should().BeFalse();
        }

        #endregion

        #region Helpers

        public IEnumerable<T> GetEnumerable<T>(params T[] @params)
        {
            foreach (T param in @params)
            {
                yield return param;
            }
        }

        #endregion
    }
}
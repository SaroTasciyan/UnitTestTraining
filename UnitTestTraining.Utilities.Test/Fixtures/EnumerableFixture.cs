using System;
using System.Collections.Generic;

using FluentAssertions;
using Xunit;

using UnitTestTraining.Utilities.Enumerable;
using UnitTestTraining.Utilities.Test.Stubs;

namespace UnitTestTraining.Utilities.Test.Fixtures
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
            // # Arrange
            IEnumerable<string> enumerable = EmptyReferenceEnumerable;

            // # Act
            string actual = enumerable.LastOrDefault();

            // # Assert
            actual.Should().BeNull();
        }

        [Fact]
        public void LastOrDefault_OfEmptyValueTypeEnumerable_ShouldReturnDefaultValue()
        {
            // # Arrange
            IEnumerable<int> enumerable = EmptyValueEnumerable;

            // # Act
            int actual = enumerable.LastOrDefault();

            // # Assert
            actual.Should().Be(0);
        }

        [Fact]
        public void LastOrDefault_OfNonEmptyEnumerable_ShouldReturnLastElement()
        {
            // # Arrange
            IEnumerable<string> enumerable = GetEnumerable("Saro", "Emre", "Onur", "Ahmet", "Barış");

            // # Act
            string actual = enumerable.LastOrDefault();

            // # Assert
            actual.Should().Be("Barış");
        }

        [Fact]
        public void LastOrDefault_OfNullEnumerable_ShouldThrowArgumentNullException()
        {
            // # Arrange
            IEnumerable<string> enumerable = null;

            // # Act
            Action action = () => { enumerable.LastOrDefault(); };

            // # Assert
            action.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void LastOrDefault_OfEmptyIList_ShouldReturnDefaultValue()
        {
            // # Arrange
            string[] enumerable = new string[0];
            
            // # Act
            string actual = enumerable.LastOrDefault();

            // # Assert
            actual.Should().BeNull();
        }

        [Fact]
        public void LastOrDefault_OfNonEmptyIList_ShouldReturnLastElement()
        {
            // # Arrange
            string[] enumerable = new string[] { "Saro", "Emre", "Onur", "Ahmet", "Barış" };

            // # Act
            string actual = enumerable.LastOrDefault();

            // # Assert
            actual.Should().Be("Barış");
        }

        [Fact]
        public void LastOrDefault_OfNonEmptyIList_ShouldNotEnumerate()
        {
            // # Arrange
            ListStub<string> enumerable = new ListStub<string>(new List<string> { "Saro", "Emre", "Onur", "Ahmet", "Barış" });

            // # Act
            enumerable.LastOrDefault();

            // # Assert
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
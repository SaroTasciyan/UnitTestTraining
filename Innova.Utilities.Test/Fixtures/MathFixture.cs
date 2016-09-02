using Xunit;
using FluentAssertions;

using Innova.Utilities.Math;

namespace Innova.Utilities.Test.Fixtures
{
    public class MathFixture
    {
        #region Power Tests

        [Fact]
        public void Power_PositivePowerOfPositiveNumber_ShouldBePositive()
        {
            2.Power(3).Should().Be(8);
        }

        [Fact]
        public void Power_ZeroPowerOfPositiveNumber_ShouldBeOne()
        {
            2.Power(0).Should().Be(1);
        }

        [Fact]
        public void Power_PositivePowerOfZeroNumber_ShouldBeZero()
        {
            0.Power(2).Should().Be(0);
        }

        [Fact]
        public void Power_PositiveOddPowerOfNegativeNumber_ShouldBeNegative()
        {
            (-2).Power(3).Should().Be(-8);
        }

        [Fact]
        public void Power_NegativePowerOfPositiveNumber_ShouldBeFractional()
        {
            2.Power(-2).Should().Be(0.25D);
        }

        [Fact]
        public void Power_NegativePowerOfZeroNumber_ShouldBePositiveInfinity()
        {
            0.Power(-2).Should().Be(double.PositiveInfinity);
        }

        #endregion
    }
}
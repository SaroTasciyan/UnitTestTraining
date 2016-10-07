using System;
using System.Collections.Generic;

using Xunit.Extensions;
using FluentAssertions;
using Moq;
using Ploeh.AutoFixture.Xunit;

using UnitTestTraining.DiscountEngine.Dependencies;
using UnitTestTraining.DiscountEngine.Models;
using UnitTestTraining.DiscountEngine.Test.FakeData;

using Customization = UnitTestTraining.DiscountEngine.Test.DiscountEngineCustomizaiton;

namespace UnitTestTraining.DiscountEngine.Test.Fixtures
{
    public class DiscountEngineFixture
    {
        #region Without Mock

        // # Using AutoDataMoq attribute to ensure default dependency mocks are injected to DiscountEngine instance. Otherwise; DiscountEngine won't be initialized since it doesn't have a default constructor.

        [Theory, AutoDataMoq]
        public void Execute_BasketIsNotProvided_ShouldThrowArgumentNullException(DiscountEngine discountEngine)
        {
            // # Arrange - Act
            Action action = () => discountEngine.Execute(null);

            // # Assert
            action.ShouldThrow<ArgumentNullException>();
        }

        [Theory, AutoDataMoq]
        public void Execute_BasketProductsAreNotProvided_ShouldThrowArgumentException(DiscountEngine discountEngine)
        {
            // # Arrange
            Basket basket = new Basket { Products = null };

            // # Act
            Action action = () => discountEngine.Execute(basket);

            // # Assert
            action.ShouldThrow<ArgumentException>();
        }

        [Theory, AutoDataMoq]
        public void Execute_BasketHasZeroProducts_ShouldNotApplyDiscount(DiscountEngine discountEngine)
        {
            // # Arrange
            Basket basket = new Basket
            {
                Products = new List<Product>()
            };

            Discount expectation = new Discount();

            // # Act
            Discount actual = discountEngine.Execute(basket);

            // # Assert
            actual.Amount.Should().Be(expectation.Amount);
        }

        #endregion

        #region With Mock (Without Customization)

        // # With AutoDataMoq decorated; test method gets the required objects as paramters. Thus, it becomes easier and more clean to arrange the test case and act on system under test
        // # System Under Test object 'DiscountEngine' instance can be obtained as a parameter. However, in order to get the same dependency mock that was injected to 'DiscountEngine', it should be the last parameter
        // # And Mock should be obtained as a parameter using [Frozen] attribute

        [Theory, AutoDataMoq]
        public void Execute_MinimumNumberOfProductsAreNotSatisfied_ShouldNotApplyDiscount([Frozen] Mock<IConfigurationStore> configurationStoreMock, DiscountEngine discountEngine)
        {
            // # Arrange
            Basket basket = new Basket
            {
                Products = new List<Product> { new Product() }
            };

            configurationStoreMock.Setup(x => x.Get<int>(It.IsAny<string>())).Returns(2);

            Discount expectation = new Discount();

            // # Act
            Discount actual = discountEngine.Execute(basket);

            // # Assert
            actual.Amount.Should().Be(expectation.Amount);
            //configurationStoreMock.Verify(x => x.Get<int>(It.IsAny<string>())); // <-- Not necessary to Verify since method under test would not return the expected results if mock wasn't used
        }

        #endregion

        #region With Customization (Without FakeData)

        // # Customization is used in order to avoid arranging IConfigurationStore and IRepository<Campaign> mocks for each test.
        // # It's fine to have single Customization for a fixture. Customization can be a great place to arrange general/default setup.

        [Theory, AutoDataMoq(typeof(Customization))]
        public void Execute_WhenNoCampaignExist_ShouldNotApplyDiscount(DiscountEngine discountEngine)
        {
            // # Arrange
            Basket basket = new Basket
            {
                Products = new List<Product> { new Product() }
            };

            Discount expectation = new Discount();

            // # Act
            Discount actual = discountEngine.Execute(basket);

            // # Assert
            actual.Amount.Should().Be(expectation.Amount); // # No mocks arranged at all. However, the expectation is satisfied. Dependency mocks arranged in Customization were injected to DiscountEngine.
        }

        [Theory, AutoDataMoq(typeof(Customization))]
        public void Execute_WhenThereIsOnlyOneCampaignEligable_ShouldApplyDiscount(Mock<IRepository<Campaign>> campaignRepositoryMock, DiscountEngine discountEngine)
        {
            // # Arrange
            Basket basket = new Basket
            {
                Products = new List<Product>
                {
                    new Product
                    {
                        Quantity = 3,
                        Amount = 10
                    }
                }
            };

            Campaign campaign = new Campaign
            {
                StartingAmount = 20,
                EndingAmount = 100,
                Discount = new Discount { Amount = 5 }
            };

            campaignRepositoryMock.Setup(x => x.Get()).Returns(new Campaign[] { campaign });

            Discount expectation = new Discount { Amount = 5 };

            // # Act
            Discount actual = discountEngine.Execute(basket);

            // # Assert
            actual.Amount.Should().Be(expectation.Amount);
        }

        #endregion

        #region With FakeData

        // # If we run code coverage statistics with the tests up to this point, we will see that it's fully covered. Yet, we are still missing test cases. 
        // # For instance; there are no tests for checking if higher discount value is applied when there are multiple Campaigns
        // # Replacing .Max(..) with .Min(..) withing GetBestDiscount(..) the tests will still pass with full coverage. Simple because we are not done testing yet!
        // # When test cases start to get complicated, the line of codes within the test scope increases. Most of the lines are withing 'Arrange' part. It basically gets harder to read and understand what is being testing.
        // # Hiding the information which is not critical for defining the test case can be a good practice. Such as 'how the data is created'. 
        // # Along with third party libraries, custom FakeData implementations can be used for hiding how the data is created.

        [Theory, AutoDataMoq(typeof(Customization))]
        public void Execute_WhenThereAreMultipleCampaignsEligable_ShouldApplyBestDiscount(Mock<IRepository<Campaign>> campaignRepositoryMock, DiscountEngine discountEngine)
        {
            // # Arrange
            FakeBasket basket = FakeBasket.Create;
            basket.AddProduct(new Product { Quantity = 3, Amount = 10 });
            
            FakeCampaign campaign1 = FakeCampaign.Create;
            campaign1.AmountRange(0, 50);
            campaign1.DiscountRate(5);

            FakeCampaign campaign2 = FakeCampaign.Create;
            campaign2.AmountRange(20, 100);
            campaign2.DiscountRate(10);

            FakeCampaigns campaigns = FakeCampaigns.Create;
            campaigns.AddCampaigns(campaign1, campaign2);

            campaignRepositoryMock.Setup(x => x.Get()).Returns(campaigns);

            Discount expectation = new Discount { Amount = 10 };

            // # Act
            Discount actual = discountEngine.Execute(basket);

            // # Assert
            actual.Amount.Should().Be(expectation.Amount);
        }

        #endregion

        #region With Fluent FakeData

        // # Fluent version of custom FakeData implementation

        [Theory, AutoDataMoq(typeof(Customization))]
        public void Execute_WhenThereIsOnlyOneCampaignEligable_ShouldApplyEligableCampaignDiscount(Mock<IRepository<Campaign>> campaignRepositoryMock, DiscountEngine discountEngine)
        {
            // # Arrange
            FluentBasket basket = FluentBasket.Create
                .AddingProduct(x => x.WithQuantity(3).WithAmount(10))
                .AddingProduct(x => x.WithQuantity(1).WithQuantity(20));

            FluentCampaigns campaigns = FluentCampaigns.Create
                .AddingCampaign(x => x.WithAmountRange(0, 100).WithDiscountRate(5))
                .AddingCampaign(x => x.WithAmountRange(101, 250).WithDiscountRate(10));

            campaignRepositoryMock.Setup(x => x.Get()).Returns(campaigns);

            FluentDiscount expectation = FluentDiscount.Create.WithAmount(5);

            // # Act
            Discount actual = discountEngine.Execute(basket);

            // # Assert
            actual.Amount.Should().Be(expectation.Amount);
        }

        #endregion
    }
}
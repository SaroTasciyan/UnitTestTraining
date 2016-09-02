using System;
using System.Collections.Generic;

using Xunit.Extensions;
using FluentAssertions;
using Moq;
using Ploeh.AutoFixture.Xunit;

using Innova.DiscountEngine.Dependencies;
using Innova.DiscountEngine.Models;
using Innova.DiscountEngine.Test.FakeData;

using Customization = Innova.DiscountEngine.Test.DiscountEngineCustomizaiton;

namespace Innova.DiscountEngine.Test.Fixtures
{
    public class DiscountEngineFixture
    {
        #region Without Mock
        // # AutoDataMoq attribute'ü default'da olsa DiscountEngine'e mock injection yapılması için kullanıyoruz. Kullanmasaydık; DiscountEngine'in default constructor'ı olmadığı için instantiate edemezdi!

        [Theory, AutoDataMoq]
        public void Execute_BasketIsNotProvided_ShouldThrowArgumentNullException(DiscountEngine discountEngine)
        {
            // # Create Test Case

            // # Execute System Under Test
            Action action = () => discountEngine.Execute(null);

            // # Assert Expectation
            action.ShouldThrow<ArgumentNullException>();
        }

        [Theory, AutoDataMoq]
        public void Execute_BasketProductsAreNotProvided_ShouldThrowArgumentException(DiscountEngine discountEngine)
        {
            // # Create Test Case
            Basket basket = new Basket { Products = null };

            // # Execute System Under Test
            Action action = () => discountEngine.Execute(basket);

            // # Assert Expectation
            action.ShouldThrow<ArgumentException>();
        }

        [Theory, AutoDataMoq]
        public void Execute_BasketHasZeroProducts_ShouldNotApplyDiscount(DiscountEngine discountEngine)
        {
            // # Create Test Case
            Basket basket = new Basket
            {
                Products = new List<Product>()
            };

            Discount expectation = new Discount();

            // # Execute System Under Test
            Discount actual = discountEngine.Execute(basket);

            // # Assert Expectation
            actual.Amount.Should().Be(expectation.Amount);
        }

        #endregion

        #region With Mock (Without Customization)
        // # Customization kullanılMAyan örnek;
        // # AutoDataMoq (AutoFixture) sayesinde gereksinim duyduğumuz objeleri parametre olarak alabiliyor, böylece her test scope'da tanımlamak zorunda kalmıyoruz
        // # System Under Test olan 'DiscountEngine' instance'ını parametre olarak alabiliyoruz fakat kullandığımız mock'un constructor'a inject edilmiş halini almamız için en son oluşturmamız (son parametre) olması gerekli
        // # Yine aynı şekilde kullanıdığımız (test scope içerisinde davranış biçimini ayarladığımız) mock'un System Under Test'e inject olan mock olması için [Frozen] attribute'ü kullanıyoruz

        [Theory, AutoDataMoq]
        public void Execute_MinimumNumberOfProductsAreNotSatisfied_ShouldNotApplyDiscount([Frozen] Mock<IConfigurationStore> configurationStoreMock, DiscountEngine discountEngine)
        {
            // # Create Test Case
            Basket basket = new Basket
            {
                Products = new List<Product> { new Product() }
            };

            configurationStoreMock.Setup(x => x.Get<int>(It.IsAny<string>())).Returns(2);

            Discount expectation = new Discount();

            // # Execute System Under Test
            Discount actual = discountEngine.Execute(basket);

            // # Assert Expectation
            actual.Amount.Should().Be(expectation.Amount);
            //configurationStoreMock.Verify(x => x.Get<int>(It.IsAny<string>())); // <-- Verify etmiyorum çünkü zaten bu mock çağırılmasa bu test koşmazdı..
        }

        #endregion

        #region With Customization (Without FakeData)
        // # Her testte IConfigurationStore ve IRepository<Campaign> için mock oluşturmak zorunda olacağım ve bunu tek tek yapmak istemediğim için Customization oluşturuyorum
        // # Bir fixture'un sadece bir customization'ı olmasında fayda var, customization fixture için 'genel geçer' ayarların yapıldığı yer olarak kabul edilmeli
        // # Bu kuralı sağlamak için PBL'de her fixture'da customization'ın ismini Customization olarak alias verdim. Burada da aynısını yapacağım.
        // # Bazı testler için; Customization'da mock'lar fixture'a register ben setup'ı ( :D ) değiştireceksem ilgili mock'u teste inject etmeliyim.

        [Theory, AutoDataMoq(typeof(Customization))]
        public void Execute_WhenNoCampaignExist_ShouldNotApplyDiscount(DiscountEngine discountEngine)
        {
            // # Create Test Case
            Basket basket = new Basket
            {
                Products = new List<Product> { new Product() }
            };

            Discount expectation = new Discount();

            // # Execute System Under Test
            Discount actual = discountEngine.Execute(basket); // # Mock yok fixture'da; discountEngine hangi mock'u kullanacağını, Mock'da nasıl davranacağını Customization'dan dolayı biliyor

            // # Assert Expectation
            actual.Amount.Should().Be(expectation.Amount);
        }

        [Theory, AutoDataMoq(typeof(Customization))]
        public void Execute_WhenThereIsOnlyOneCampaignEligable_ShouldApplyDiscount(Mock<IRepository<Campaign>> campaignRepositoryMock, DiscountEngine discountEngine)
        {
            // # Create Test Case
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

            // # Execute System Under Test
            Discount actual = discountEngine.Execute(basket);

            // # Assert Expectation
            actual.Amount.Should().Be(expectation.Amount);
        }

        #endregion

        #region With FakeData
        // # Bu noktaya kadar olan testleri koştuğunuzda Coverage'ın full olduğunu görebilirsiniz. Fakat birçok case'i test etmemiş durumdayız; örneğin 2 kampanya olduğunda yüksek olandan kazandırdığı case'i
        // # GetBestDiscount(..) methodundaki .Max(..) yerine .Min(..) veya .First(..) yazdığınızda testlerin hala koşacağını ve converage'ın full olacağını görebilirsiniz. Çünkü en yükseğin indirimin uygulandığına dair bir test yazmadık!
        // # Farklı kombinasyonları test ettiğimiz (n-ürün, n-kampanya vb) testlerde data yaratma kodları uzun olacaktır. En son testte (bir önceki) data yaratma kodlarının test kodunun ~%90'ı olduğunu görebilirsiniz. Diğer kısımlar zor okunuyor.
        // # Dolayısı ile basit FakeData kullanarak ilgili kodları kısaltmaya çalışacağız. Sistemde veri anlamında 'genel' kabuller olmadığı için arka planda bazı kabuller yapıyor olmayacak işimize müthiş derecede yaramayacak.
        // # ! FakeData yazarken unutulmaması gereken şey; iş kurallarının baştan yazılmadığı ve sadece test ihtiyacına yönetik olduğu. Dolayısı ile oturup baştan FakeData'ya başlayıp bitirmeyin. Test yazarken ihtiyaç oldukça alanları ekleyin. Örneğin; Id alanı yok, çünkü ihtiyacımız olmadı.
        // # Ancak; Currency parametresi olsaydı, Campaign tanımları ve Product fiyatları için FakeData'lar sistemin default Currency bilgisini oluşturmak yönünde kabul yaparak hem yeni eklenen testlerde, hem de eski testlerin mevcut hali korunmasında işimize yarayabilirdi!

        [Theory, AutoDataMoq(typeof(Customization))]
        public void Execute_WhenThereAreMultipleCampaignsEligable_ShouldApplyBestDiscount(Mock<IRepository<Campaign>> campaignRepositoryMock, DiscountEngine discountEngine)
        {
            // # Create Test Case
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

            // # Execute System Under Test
            Discount actual = discountEngine.Execute(basket);

            // # Assert Expectation
            actual.Amount.Should().Be(expectation.Amount);
        }

        #endregion

        #region With Fluent FakeData
        // # FakeData kullanım kolaylığını yeterli bulmayıp, daha da gaza gelip fluent yazabilirsiniz. İhtiyaç olan FakeData ve işlevleri mevcut değilse test yazma eforu artacaktır. Mevcut ise efor çok azalacaktır.
        // # Fluent kısmı Fake olacak, önceki sınıf isimleri ile çakışmaması için bu şekilde isimlendirdim

        [Theory, AutoDataMoq(typeof(Customization))]
        public void Execute_WhenThereIsOnlyOneCampaignEligable_ShouldApplyEligableCampaignDiscount(Mock<IRepository<Campaign>> campaignRepositoryMock, DiscountEngine discountEngine)
        {
            // # Create Test Case
            FluentBasket basket = FluentBasket.Create
                .AddingProduct(x => x.WithQuantity(3).WithAmount(10))
                .AddingProduct(x => x.WithQuantity(1).WithQuantity(20));

            FluentCampaigns campaigns = FluentCampaigns.Create
                .AddingCampaign(x => x.WithAmountRange(0, 100).WithDiscountRate(5))
                .AddingCampaign(x => x.WithAmountRange(101, 250).WithDiscountRate(10));

            campaignRepositoryMock.Setup(x => x.Get()).Returns(campaigns);

            FluentDiscount expectation = FluentDiscount.Create.WithAmount(5);

            // # Execute System Under Test
            Discount actual = discountEngine.Execute(basket);

            // # Assert Expectation
            actual.Amount.Should().Be(expectation.Amount);
        }

        #endregion
    }
}
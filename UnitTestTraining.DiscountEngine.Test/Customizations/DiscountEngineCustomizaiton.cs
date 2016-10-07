using UnitTestTraining.DiscountEngine.Dependencies;
using UnitTestTraining.DiscountEngine.Models;

using Moq;

using Ploeh.AutoFixture;

namespace UnitTestTraining.DiscountEngine.Test
{
    public class DiscountEngineCustomizaiton : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            Mock<IConfigurationStore> configurationStoreMock = fixture.Create<Mock<IConfigurationStore>>();
            Mock<IRepository<Campaign>> campaignRepositoryMock = fixture.Create<Mock<IRepository<Campaign>>>();

            configurationStoreMock.Setup(x => x.Get<int>(It.IsAny<string>())).Returns(0); // # Lets assume that by default I want to have value 0 for any int config values
            campaignRepositoryMock.Setup(x => x.Get()).Returns(new Campaign[0]); // # Lets assume that by default I do not want to have any Campaigns defined in the system

            fixture.Register(() => configurationStoreMock);
            fixture.Register(() => campaignRepositoryMock);
        }
    }
}
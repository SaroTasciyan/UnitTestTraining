using Innova.DiscountEngine.Dependencies;
using Innova.DiscountEngine.Models;

using Moq;

using Ploeh.AutoFixture;

namespace Innova.DiscountEngine.Test
{
    public class DiscountEngineCustomizaiton : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            Mock<IConfigurationStore> configurationStoreMock = fixture.Create<Mock<IConfigurationStore>>();
            Mock<IRepository<Campaign>> campaignRepositoryMock = fixture.Create<Mock<IRepository<Campaign>>>();

            configurationStoreMock.Setup(x => x.Get<int>(It.IsAny<string>())).Returns(0); // # Genel olarak config'deki değerin 0 olduğunu kabul etmek istiyorum (config'de tek değerimiz olduğu kabulu var şimdilik)
            campaignRepositoryMock.Setup(x => x.Get()).Returns(new Campaign[0]); // # Genel olarak veritabanında kampanya olmadığı kabulunu yapmak istiyorum.. aksi takdirde test case datasını refactor etmiş olacağım ve testler buradaki değere bağımlı olacak!

            fixture.Register(() => configurationStoreMock);
            fixture.Register(() => campaignRepositoryMock);
        }
    }
}
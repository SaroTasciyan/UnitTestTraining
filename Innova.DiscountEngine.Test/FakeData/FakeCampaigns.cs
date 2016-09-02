using System.Collections.ObjectModel;

using Innova.DiscountEngine.Models;

namespace Innova.DiscountEngine.Test.FakeData
{
    public class FakeCampaigns : Collection<Campaign>
    {
        protected FakeCampaigns() { }

        public static FakeCampaigns Create => new FakeCampaigns();

        public void AddCampaigns(params Campaign[] campaigns)
        {
            foreach (Campaign campaign in campaigns)
            {
                Add(campaign);
            }
        }
    }
}
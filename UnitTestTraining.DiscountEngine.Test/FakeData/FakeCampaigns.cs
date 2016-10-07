using System.Collections.ObjectModel;

using UnitTestTraining.DiscountEngine.Models;

namespace UnitTestTraining.DiscountEngine.Test.FakeData
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
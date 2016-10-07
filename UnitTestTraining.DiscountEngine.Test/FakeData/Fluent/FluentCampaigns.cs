using System;
using System.Collections.ObjectModel;

using UnitTestTraining.DiscountEngine.Models;

namespace UnitTestTraining.DiscountEngine.Test.FakeData
{
    public class FluentCampaigns : Collection<Campaign>
    {
        protected FluentCampaigns() { }

        public static FluentCampaigns Create => new FluentCampaigns();

        public FluentCampaigns AddingCampaign(Func<FluentCampaign, FluentCampaign> builder)
        {
            FluentCampaign campaign = builder(FluentCampaign.Create);

            Add(campaign);

            return this;
        }
    }
}
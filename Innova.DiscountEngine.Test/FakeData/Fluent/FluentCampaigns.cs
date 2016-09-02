using System;
using System.Collections.ObjectModel;

using Innova.DiscountEngine.Models;

namespace Innova.DiscountEngine.Test.FakeData
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
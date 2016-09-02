using Innova.DiscountEngine.Models;

namespace Innova.DiscountEngine.Test.FakeData
{
    public class FluentCampaign : Campaign
    {
        protected FluentCampaign() { }

        public static FluentCampaign Create => new FluentCampaign();

        public FluentCampaign WithAmountRange(decimal startingAmount, decimal endingAmount)
        {
            StartingAmount = startingAmount;
            EndingAmount = endingAmount;

            return this;
        }

        public FluentCampaign WithDiscountRate(decimal discountAmount)
        {
            Discount = FluentDiscount.Create.WithAmount(discountAmount);

            return this;
        }
    }
}
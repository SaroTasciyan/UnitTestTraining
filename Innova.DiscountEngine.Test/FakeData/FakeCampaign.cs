using Innova.DiscountEngine.Models;

namespace Innova.DiscountEngine.Test.FakeData
{
    public class FakeCampaign : Campaign
    {
        protected FakeCampaign() { } 

        public static FakeCampaign Create => new FakeCampaign();

        public void AmountRange(decimal startingAmount, decimal endingAmount)
        {
            StartingAmount = startingAmount;
            EndingAmount = endingAmount;
        }

        public void DiscountRate(decimal discountAmount)
        {
            Discount = new Discount { Amount = discountAmount };
        }
    }
}
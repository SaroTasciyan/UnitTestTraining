using System;
using System.Collections.Generic;
using System.Linq;

using UnitTestTraining.DiscountEngine.Dependencies;
using UnitTestTraining.DiscountEngine.Models;

namespace UnitTestTraining.DiscountEngine
{
    public class DiscountEngine
    {
        private const string MinimumNumberOfProductsRequiredKey = "MinimumNumberOfProductsRequired";

        private readonly IRepository<Campaign> campaignRepository;
        private readonly IConfigurationStore configurationStore;

        public DiscountEngine(IRepository<Campaign> campaignRepository, IConfigurationStore configurationStore)
        {
            this.campaignRepository = campaignRepository;
            this.configurationStore = configurationStore;
        }

        public Discount Execute(Basket basket)
        {
            if (basket == null) { throw new ArgumentNullException(nameof(basket), "Argument is null!"); }
            if (basket.Products == null) { throw new ArgumentException("Basket has null products!", nameof(basket.Products)); }

            if (basket.Products.Count == 0)
            {
                return new Discount();
            }

            int minimumNumberOfProductsRequired = configurationStore.Get<int>(MinimumNumberOfProductsRequiredKey); 
            if (basket.Products.Count < minimumNumberOfProductsRequired)
            {
                return new Discount();
            }

            Campaign[] campaigns = campaignRepository.Get().ToArray();
            if (campaigns.Length == 0)
            {
                return new Discount();
            }

            decimal basketTotalAmount = GetTotalAmount(basket);
            Campaign[] eligableCampaigns = GetEligableCampaigns(campaigns, basketTotalAmount);

            decimal bestDiscount = GetBestDiscount(eligableCampaigns);

            return new Discount { Amount = bestDiscount };
        }

        private decimal GetTotalAmount(Basket basket)
        {
            return basket.Products.Sum(x => x.Amount * x.Quantity);
        }

        private Campaign[] GetEligableCampaigns(Campaign[] campaigns, decimal basketTotalAmount)
        {
            return campaigns.Where(x => x.StartingAmount <= basketTotalAmount && x.EndingAmount >= basketTotalAmount).ToArray();
        }

        private decimal GetBestDiscount(IEnumerable<Campaign> campaigns)
        {
            return campaigns.Max(x => x.Discount.Amount);
        }
    }
}
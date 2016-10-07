using UnitTestTraining.DiscountEngine.Models;

namespace UnitTestTraining.DiscountEngine.Test.FakeData
{
    public class FluentDiscount : Discount
    {
        protected FluentDiscount() { }

        public static FluentDiscount Create => new FluentDiscount();

        public FluentDiscount WithAmount(decimal amount)
        {
            Amount = amount;

            return this;
        }
    }
}
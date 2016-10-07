using UnitTestTraining.DiscountEngine.Models;

namespace UnitTestTraining.DiscountEngine.Test.FakeData
{
    public class FluentProduct : Product
    {
        protected FluentProduct() { }

        public static FluentProduct Create => new FluentProduct();

        public FluentProduct WithQuantity(int quantity)
        {
            Quantity = quantity;

            return this;
        }

        public FluentProduct WithAmount(decimal amount)
        {
            Amount = amount;

            return this;
        }
    }
}
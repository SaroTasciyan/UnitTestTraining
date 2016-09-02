using System.Collections.Generic;

using Innova.DiscountEngine.Models;

namespace Innova.DiscountEngine.Test.FakeData
{
    public class FakeBasket : Basket
    {
        protected FakeBasket() { }

        public static FakeBasket Create => new FakeBasket
        {
            Products = new List<Product>()
        };

        public void AddProduct(Product product)
        {
            Products.Add(product);
        }
    }
}
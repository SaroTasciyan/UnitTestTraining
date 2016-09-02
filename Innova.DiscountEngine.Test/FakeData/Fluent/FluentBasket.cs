using System;
using System.Collections.Generic;

using Innova.DiscountEngine.Models;

namespace Innova.DiscountEngine.Test.FakeData
{
    public class FluentBasket : Basket
    {
        protected FluentBasket() { }

        public static FluentBasket Create => new FluentBasket
        {
            Products = new List<Product>()
        };

        public FluentBasket AddingProduct(Func<FluentProduct, FluentProduct> builder)
        {
            FluentProduct product = builder(FluentProduct.Create);

            Products.Add(product);

            return this;
        }
    }
}
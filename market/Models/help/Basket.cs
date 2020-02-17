
using System.Collections.Generic;

namespace market.Models.help
{
    public partial class Basket
    {
        public List<BasketElement> BasketIElements { get; set; }

        public int Total()
        {
            int x = 0;
            foreach (var basketIElement in BasketIElements)
            {
                var itemFullPrice = basketIElement.Quantity * basketIElement.Item.Price;
                x = x + itemFullPrice;
            }

            return x;
        }
    }
}
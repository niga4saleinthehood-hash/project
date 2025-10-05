using Code_Brew_Coffe_Shop.domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code_Brew_Coffe_Shop.Pricing
{
    public class DefaultPriceRule: IPriceRule
    {
        public decimal ComputeLineAmount(OrderItem it)
        { 
            return it.ItemTotal();
        }
    }
}

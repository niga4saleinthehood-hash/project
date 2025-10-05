using Code_Brew_Coffe_Shop.domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace Code_Brew_Coffe_Shop.Pricing
{
    public interface IPriceRule
    {
        decimal ComputeLineAmount(OrderItem it);
    }
}

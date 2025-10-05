using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code_Brew_Coffe_Shop.Pricing
{
    public interface ITaxCaculator
    {
        decimal ComputeTax(decimal amount);
    }
}

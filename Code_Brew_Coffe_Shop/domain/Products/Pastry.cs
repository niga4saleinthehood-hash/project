using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using static Code_Brew_Coffe_Shop.domain.Enum;

namespace Code_Brew_Coffe_Shop.domain
{
    public class Pastry: Product 
    {
        public PastryType pastryType {  get; set; }
        public bool IsFresh { get; set; } = true;
        public ProductCategory category = ProductCategory.Pastry;

        public override decimal CalculateFinalPrice()
        {
            if (!IsFresh)
            {
                return basePrice * 0.8m;
            }
            return basePrice;
        }

        public override string getProductType() => "Pastry";

        public override string description()
        {
            return "";
        }

    }
}

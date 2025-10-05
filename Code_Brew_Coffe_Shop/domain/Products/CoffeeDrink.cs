using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Code_Brew_Coffe_Shop.domain.Enum;

namespace Code_Brew_Coffe_Shop.domain
{
    public class CoffefDrink: Product
    {
        public RoastLevel roastlevel {  get; set; } = RoastLevel.Medium;
        public bool hasSugar { get; set; } = true;
        public bool hasMilk { get; set; } = true;
        public Size size { get; set; }
        public ProductCategory category { get; set; } = ProductCategory.Coffee;

        public override decimal CalculateFinalPrice()
        {
            switch (size) {
                case Size.S: return basePrice;
                case Size.M: return basePrice + 10.000m;
                case Size.L: return basePrice + 20.000m;
                default: return basePrice;
            }

        }

        public override string getProductType() => "Coffee";
        
        public override string description()
        {
            string desc = "";
            if (!hasMilk)
            {
                desc += " No Milk ";
            }
            if (!hasSugar)
            {
                desc += " No Sugar ";
            }
            return desc;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Code_Brew_Coffe_Shop.domain.Enum;

namespace Code_Brew_Coffe_Shop.domain
{
    public class TeaDrink:Product
    {
        public SurgarLevel sugarLevel { get; set; } = SurgarLevel.Noraml;
        public List<Topping> toppings = new List<Topping>();
        public ProductCategory category { get; set; } = ProductCategory.Tea;
        public Size size { get; set; }

        public void AddTopping(Topping topping)
        {
            if (!toppings.Contains(topping))
            {
                toppings.Add(topping);
            }
        }

        public void RemoveTopping(Topping topping)
        {
            if (toppings.Contains(topping))
            {
                toppings.Remove(topping);
            }
        }

        public override string getProductType() => "Tea";


        public override decimal CalculateFinalPrice()   
        {
            decimal price = 5.0m * toppings.Count;
            switch (size)
            {
                case Size.S: return price + basePrice;
                case Size.M: return price + basePrice + 10.00m;
                case Size.L: return price + basePrice + 20.00m;
                default:return basePrice + price;
                    
            }
        }

        public override string description()
        {
            string desc = "";
            if (sugarLevel != SurgarLevel.Noraml)
            {
                desc += " Sugar " + sugarLevel.ToString();
            }
            if(toppings.Count > 0)
            {
                desc += " Toppings: " + string.Join(", ", toppings);
            }
            return desc;
        }
    }
}

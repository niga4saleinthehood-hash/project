using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Code_Brew_Coffe_Shop.domain.Enum;

namespace Code_Brew_Coffe_Shop.domain
{
    public abstract class Product
    {
        // attribute
        public string productId { get; set; } = System.Guid.NewGuid().ToString();
        public string name { get; set; }
        public decimal basePrice { get; set; }
        public int stockQty { get; set; }
        // abstract methods

        public abstract string getProductType();
        public abstract decimal CalculateFinalPrice();

        public abstract string description();
  

    }
}

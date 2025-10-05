using Code_Brew_Coffe_Shop.Promotion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Code_Brew_Coffe_Shop.domain
{
    public class OrderItem
    {
        public Product product { get;  set; }
        public int Quantity { get; set; }
        public decimal unitPrice { get; set; }
        public decimal LineDiscountAmount { get; set; }
        public bool isPromotionItem { get; set; } = false;
        public OrderItem(Product product , int Quantity,decimal unitPrice , decimal LineDiscountAmount)
        {
            if (product == null) throw new ArgumentNullException(nameof(product));
            if (Quantity < 0) throw new ArgumentException("Quantity must greater than 0");
            if(unitPrice < 0) throw new ArgumentException("Unit price must greater than 0");
            if(LineDiscountAmount < 0m || LineDiscountAmount > 0.5m ) throw new ArgumentException("Line discount must be between 0 and 0.5");
            this.product = product;
            this.unitPrice = unitPrice;
            this.LineDiscountAmount = LineDiscountAmount;
            this.Quantity = Quantity;
        }

        public decimal ItemTotal ( ) {
            //if (isPromotionItem == true) return 0m;
            return unitPrice * Quantity * (1 - LineDiscountAmount); 
        }


    }
}

using Code_Brew_Coffe_Shop.domain;
using Code_Brew_Coffe_Shop.domain.Customers;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code_Brew_Coffe_Shop.Promotion
{
    public class ProductContext : PromotionContext
    {
        public Product product { get; set; }
        public int quantity { get; set; }
        public string description { get; set; } = "";

        public ProductContext (Product product, int quantity, string description)
        {
            this.product = product;
            this.quantity = quantity;
            this.description = description;
        }
    }
}

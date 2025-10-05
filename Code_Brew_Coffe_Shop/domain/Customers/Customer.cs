using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Code_Brew_Coffe_Shop.domain.Customers
{
    public abstract class Customer
    {
        public string customerId { get; private set; } = System.Guid.NewGuid().ToString();
        public string name { get; set; } = "";
        public string email { get; set; } = "";
        public DateTime joinDate = DateTime.Now;

        public abstract decimal GetDiscountRate(Order order);
        public abstract int CalculatePoints(decimal amount);
    }
}

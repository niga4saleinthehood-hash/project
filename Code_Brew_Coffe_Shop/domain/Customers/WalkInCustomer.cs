using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code_Brew_Coffe_Shop.domain.Customers.customers
{
    public class WalkInCustomer:Customer
    {
        public override decimal GetDiscountRate(Order order)
        {
            return 0.0m;
        }
        public override int CalculatePoints(decimal amount)
        {
            return 0;
        }
    }
}

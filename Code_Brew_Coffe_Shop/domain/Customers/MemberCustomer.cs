using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using static Code_Brew_Coffe_Shop.domain.Enum;

namespace Code_Brew_Coffe_Shop.domain.Customers
{
    public class MemberCustomer :Customer
    {
        public string customerCode { private get;  set; } = "";
        public int points { get;  set; }
        public MemberShip memberShip { get;  set; } = MemberShip.Basic;
        public override decimal GetDiscountRate(Order order)
        {
            switch (memberShip)
            {
                case MemberShip.Basic: return 0.05m;
                case MemberShip.Premium: return 0.1m;
                default: return 0.05m;
            }
        }



        public override int CalculatePoints(decimal amount)
        {
            return (int) amount/50;
        }



    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code_Brew_Coffe_Shop.Promotion
{
    public class PromotionResult
    {
        public decimal discountAmount { get; set; }
        public string description { get; set; } = "";

        public string promotionCode { get; set; }
        public bool isSuccess { get; set; }
    }
}

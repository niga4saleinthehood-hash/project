using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code_Brew_Coffe_Shop.Promotion
{
    public interface IPriceRulePromotion<TContext>
    {
        string promotionName { get; }
        string description { get; }
        bool IsEligable (TContext context);
        PromotionResult Apply(TContext context);

    }
}

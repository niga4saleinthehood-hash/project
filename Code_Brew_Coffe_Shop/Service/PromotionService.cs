using Code_Brew_Coffe_Shop.domain;
using Code_Brew_Coffe_Shop.Promotion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Code_Brew_Coffe_Shop.domain.Event;

namespace Code_Brew_Coffe_Shop.Service
{
    public class PromotionService
    {
        public readonly BuyXGetYPromotion buyXGetYPromotion;
        public event EventHandler<AnnouceGift> OnAnouceGift;
        
        public PromotionService(BuyXGetYPromotion buyXGetYPromotion)
        {
            this.buyXGetYPromotion = buyXGetYPromotion;
        }

        public void ApplyAllPromotion(Order order)
        {
            foreach (var group in order.items.Where(i => !i.isPromotionItem).GroupBy(i => i.product.productId)){
                var product = group.First().product;
                int totalQty = group.Sum(i => i.Quantity);
                var context = new ProductContext(product, totalQty, "Buy 3 same size get 1 pastry");
                if (buyXGetYPromotion.IsEligable(context))
                {
                    buyXGetYPromotion.Apply(context);

                    // Xóa free items cũ (nếu đã có)
                    order.items.RemoveAll(x => x.isPromotionItem && x.product.productId == product.productId);

                    // Thêm free item mới
                    var freeItem = buyXGetYPromotion.getFreeOrderItem();
                    if (freeItem != null)
                    {
                        freeItem.unitPrice = 0m;
                        order.AddItem(freeItem);
                    }
                    if(OnAnouceGift != null)
                    {
                        OnAnouceGift(this, new AnnouceGift(product.productId));
                    }
                }
                
            } 


        }


            
    }
}

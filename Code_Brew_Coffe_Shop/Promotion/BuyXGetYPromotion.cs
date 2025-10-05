using Code_Brew_Coffe_Shop.domain;
using Code_Brew_Coffe_Shop.repository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Code_Brew_Coffe_Shop.Promotion
{
    public class BuyXGetYPromotion : IPriceRulePromotion<ProductContext>
    {
        public OrderItem freeOrderItemOffficial { get; private set; }
        public string promotionName { get;  set; }
        public string description { get; private set; }
        public string productXID { get;  set; }
        public int buyX { private get;  set; }
        public string productYID { get; set; }
        public int getY {private get;  set; }
        public decimal discountAmount { get; set; }
        public int totalFreeItems { get;private set; }
        public InMemoryRepository<Product, string> _productRepo { get; set; }

        public bool IsEligable(ProductContext context)
        {
            if (context.product.productId == productXID && context.quantity >= buyX)
            {
                return true;
            }
            return false;
        }



        public PromotionResult Apply(ProductContext context)
        {
            if (!IsEligable(context))
            {
                return new PromotionResult
                {
                    isSuccess = false,
                    discountAmount = 0,
                    description = "Not Eligable for the promotion",
                    promotionCode = promotionName
                };
            }
            
            int numberOfPromotions = context.quantity / buyX;
            totalFreeItems = numberOfPromotions * getY;

            var freeProduct = _productRepo.GetById(productYID);
            var buyingProduct = _productRepo.GetById(productXID);

            var freeOrderItem = new OrderItem(freeProduct, totalFreeItems, freeProduct.CalculateFinalPrice(), 0m)
            {
                isPromotionItem = true
            };
            freeOrderItemOffficial = freeOrderItem;

            if (freeProduct == null) throw new InvalidOperationException("Free product not found in repository");
            if(buyingProduct == null) throw new InvalidOperationException("Buying product not found in repository");

            if (freeProduct.stockQty < totalFreeItems) throw new InvalidOperationException("Not enough stock for free product");


            return new PromotionResult
            {
                isSuccess = true,
                discountAmount = discountAmount,
                description = $"Applied {promotionName}, Buy {buyX} Get {getY} free on product {productYID}",
                promotionCode = "BuyXGetY"
                
            };
        }

        public decimal CalcFreeProduct(OrderItem freeorderitem)
        {
            return freeorderitem.product.CalculateFinalPrice() * totalFreeItems;
        }
       
        public OrderItem getFreeOrderItem()
        {
            return freeOrderItemOffficial;
        }

    }

}

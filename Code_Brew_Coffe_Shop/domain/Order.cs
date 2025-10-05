using Code_Brew_Coffe_Shop.domain.Customers;
using Code_Brew_Coffe_Shop.Pricing;
using Code_Brew_Coffe_Shop.Promotion;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Code_Brew_Coffe_Shop.domain.Enum;
using static Code_Brew_Coffe_Shop.domain.Event;

namespace Code_Brew_Coffe_Shop.domain
{
    public class Order
    {
        public string orderno { get; private set; } = DateTime.Now.Ticks.ToString();
        public Customer customer { get; private set; }
        public DateTime dateTime { get; private set; } = DateTime.Now;
        public OrderStatus status { get; private set; } = OrderStatus.Pending;
        public List<OrderItem> items { get;  private set; } = new List<OrderItem>();

        public decimal SubTotal { get; private set; }  
        public decimal DiscountOnOrder { get; private set; }
        public decimal CustomerDiscount { get; private set; }
        public decimal VAT {  get; private set; }
        public decimal VatPercent { get; set; } = 0.08m;
        public decimal Total {  get; private set; }

        
        public readonly IPriceRule _priceRules;
        public readonly ITaxCaculator _taxCaculator;
        public readonly IOrderDiscountPolicy _orderDiscountPolicy;
        public readonly IPriceRulePromotion<ProductContext> priceRulePromotion;

        public  BuyXGetYPromotion buyXGetYPromotion;

        public delegate void OrderStatusChangeHandler(object sender, OrderStatusChangedEvent args);
        public event OrderStatusChangeHandler OnOrderStatusChanged;

        public Order(Customer customer , IPriceRule priceRule, ITaxCaculator taxCaculator, IOrderDiscountPolicy orderDiscountPolicy, BuyXGetYPromotion buyXGetYPromotion)
        {
            if(customer == null) throw new ArgumentNullException(nameof(customer));
            this.customer = customer;
            this._priceRules = priceRule;
            this._taxCaculator = taxCaculator;
            this._orderDiscountPolicy = orderDiscountPolicy;
            this.buyXGetYPromotion = buyXGetYPromotion;
        }

        public void AddItem (OrderItem item)
        {
            if(status != OrderStatus.Pending)
            {
                throw new InvalidOperationException("Cannot add items to an order that is not in Pending status.");
            }
            items.Add(item);
        }

        public void RecalcTotal()
        {
            SubTotal = items.Sum(item => _priceRules.ComputeLineAmount(item));  
            CustomerDiscount = SubTotal * customer.GetDiscountRate(this);
            DiscountOnOrder = _orderDiscountPolicy.ComputeOrderDiscountLevel(this);
            var taxable = Math.Max(0, SubTotal - CustomerDiscount - DiscountOnOrder);
            VAT = _taxCaculator.ComputeTax(taxable);
            Total = taxable + VAT;
        }



        public void ChangeStatus(OrderStatus newStatus)
        {
            if(status == newStatus) return; // No change
            
            var oldStatus = status;
            status = newStatus;
            if(OnOrderStatusChanged != null)
            {
                OnOrderStatusChanged(this, new OrderStatusChangedEvent(orderno, oldStatus, newStatus));
            }
        }


    }
}

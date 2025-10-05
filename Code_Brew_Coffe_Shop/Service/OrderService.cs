using Code_Brew_Coffe_Shop.domain;
using Code_Brew_Coffe_Shop.domain.Customers;
using Code_Brew_Coffe_Shop.Pricing;
using Code_Brew_Coffe_Shop.Promotion;
using Code_Brew_Coffe_Shop.repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using static Code_Brew_Coffe_Shop.domain.Event;

namespace Code_Brew_Coffe_Shop.Service
{
    public class OrderService 
    {
        public readonly InMemoryRepository<Order , string> _orderRepository;
        public readonly InMemoryRepository<Customer , string> _customerRepository;
        public readonly InMemoryRepository<Product , string> _productRepository;

        public readonly ITaxCaculator taxCaculator;
        public readonly IPriceRule priceRule;
        public readonly IOrderDiscountPolicy orderDiscountPolicy;
        public readonly IRecieptFormat recieptFormatter;
        public readonly InventoryService inventoryService;
        public readonly IPriceRulePromotion<ProductContext> priceRulePromotion;

        public BuyXGetYPromotion buyXGetYPromotion;

        
        public event EventHandler<OrderStatusChangedEvent> OnOrderStatusChanged;
        public event EventHandler<PointAccumulatedEvent> OnPointAccumulateChanged;


        public OrderService(
            InMemoryRepository<Order, string> orderRepository,
            InMemoryRepository<Customer, string> customerRepository,
            InMemoryRepository<Product, string> productRepository,
            ITaxCaculator taxCaculator,
            IPriceRule priceRule,
            IOrderDiscountPolicy orderDiscountPolicy,
            IRecieptFormat recieptFormatter,
            InventoryService inventoryService,
            BuyXGetYPromotion buyXGetYPromotion
            )
        {
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
            _productRepository = productRepository;
            this.taxCaculator = taxCaculator;
            this.priceRule = priceRule;
            this.orderDiscountPolicy = orderDiscountPolicy;
            this.recieptFormatter = recieptFormatter;
            this.inventoryService = inventoryService;
            this.buyXGetYPromotion = buyXGetYPromotion;
        }


        public Order CreateOrder(string customerId)
        {
            var customer = _customerRepository.GetById(customerId);
            if (customer == null) throw new ArgumentException("Invalid customer ID");
            var order = new Order(customer, priceRule, taxCaculator, orderDiscountPolicy, buyXGetYPromotion);
            
            order.OnOrderStatusChanged += (sender , event_) =>
            {
                if(OnOrderStatusChanged != null)
                {
                    OnOrderStatusChanged(this, event_);
                }
            };
            _orderRepository.Add(order);
            return order;
        }


        public void AddItem(string orderno, string productid, int qty, decimal lineDiscountPercent)
        {
            var order = _orderRepository.GetById(orderno);
            if (order == null) throw new InvalidOperationException("Order not found");
            var product = _productRepository.GetById(productid);
            if (product == null) throw new InvalidOperationException("Product not found");
            var item = new OrderItem(product, qty , product.CalculateFinalPrice(), lineDiscountPercent);
            order.AddItem(item);
            _orderRepository.Update(order);
        }


        public void Recal(string orderno)
        {
            var order = _orderRepository.GetById(orderno);
            if(order == null) throw new InvalidOperationException("Order not found");
            order.RecalcTotal();
            _orderRepository.Update(order);
        }

        public void Confirm(string orderno)
        {
            var order = _orderRepository.GetById(orderno);
            if (order == null) throw new ArgumentException("Order not found");
            if(order.status != domain.Enum.OrderStatus.Pending)
            {
                throw new InvalidOperationException("Only pending orders can be confirmed.");
            }
            foreach(var item in order.items)
            {
                inventoryService.Decrease_Stock(item.product.productId, item.Quantity);
            }

            order.ChangeStatus(domain.Enum.OrderStatus.Confirm);
            _orderRepository.Update(order);

        }

        public void Paid(string orderno)
        {
            var order = _orderRepository.GetById(orderno);
            if (order == null) throw new ArgumentException("Order not found");
            if (order.status != domain.Enum.OrderStatus.Confirm) throw new InvalidOperationException("Only confirmed orders can be paid.");
            order.RecalcTotal();
            _orderRepository.Update(order);

            var member = order.customer as domain.Customers.MemberCustomer;
            var pointadded = member.CalculatePoints(order.Total);
            member.points += pointadded;
            _customerRepository.Update(member);

            if (OnPointAccumulateChanged != null)
            {
                var event_ = new PointAccumulatedEvent(member.customerId, member.name, pointadded, member.points);
                OnPointAccumulateChanged(this, event_);
            }
            order.ChangeStatus(domain.Enum.OrderStatus.Paid);
            _orderRepository.Update(order);
        }
        public string PrintReceipt(string orderno)
        {
            var order = _orderRepository.GetById(orderno);
            if (order == null) throw new InvalidOperationException("Order not found");
            return recieptFormatter.format(order);
        }


    }
}
using Code_Brew_Coffe_Shop.domain;
using Code_Brew_Coffe_Shop.domain.Customers;
using Code_Brew_Coffe_Shop.Pricing;
using Code_Brew_Coffe_Shop.repository;
using Code_Brew_Coffe_Shop.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static Code_Brew_Coffe_Shop.domain.Enum;

namespace Code_Brew_Coffe_Shop
{
    public class Program
    {
        static void Main(string[] args)
        {
            var productRepo = new InMemoryRepository<Product, string>(p => p.productId);
            var oderRepo = new InMemoryRepository<Order, string>(p => p.orderno);
            var customerRepo = new InMemoryRepository<Customer, string>(p => p.customerId);

            var buyXGetY = new Code_Brew_Coffe_Shop.Promotion.BuyXGetYPromotion
            {
                _productRepo = productRepo,
                productXID = "Esp 122",  // sản phẩm áp dụng khuyến mãi
                buyX = 3,                // mua 3
                productYID = "Cake 001",  // tặng cùng loại
                getY = 1,                // tặng 1
                promotionName = "Buy 3 Product Same Size Get 1 Free Pastry",
                discountAmount = 0m
            };

            var promoService = new PromotionService(buyXGetY);


            var p1 = new CoffefDrink
            {
                productId = "Esp 122",
                name = "Espresso",
                basePrice = 30.000m,
                stockQty = 50,
                roastlevel = RoastLevel.Dark,
                hasSugar = false,
                hasMilk = false,
                size = Size.L
            };


            var cake = new Pastry
            {
                productId = "Cake 001",
                name = "Cheese Cake",
                basePrice = 40.000m,
                stockQty = 20,
                pastryType = PastryType.Cake,
                IsFresh = true
            };

            var p2 = new TeaDrink
            {
                productId = "ST2",
                name = "Matcha Latte",
                basePrice = 25.000m,
                stockQty = 30,
                size = Size.M,
                sugarLevel = SurgarLevel.Less,
            };
            p2.AddTopping(Topping.Bubble);
            p2.AddTopping(Topping.Cream);

            productRepo.Add(p1);
            productRepo.Add(cake);
            productRepo.Add(p2);

            var customer = new MemberCustomer
            {
               
                name = "John Doe",
                email = "ntkhoaaa996@gmail.com",
                memberShip = MemberShip.Premium,
                points = 0,
                customerCode = "MEM123"
            };

            customerRepo.Add(customer);

            IPriceRule priceRule = new DefaultPriceRule();
            IOrderDiscountPolicy orderDiscount = new NoOrderDiscount();
            ITaxCaculator taxCalculator = new DefaultVatPercent();
            IRecieptFormat receiptFormatter = new SimpleReciepFormater();

            var inventoryService = new InventoryService(productRepo, reorderThreshold: 20);
            var orderService = new OrderService(oderRepo, customerRepo, productRepo,
                                    taxCalculator,
                                    priceRule, orderDiscount,
                                    receiptFormatter, inventoryService,
                                    buyXGetY);



            orderService.OnOrderStatusChanged += (s, e) =>
            {
                Console.WriteLine($"[EVENT] Order status: {e.orderno} {e.OldStatus} -> {e.NewStatus}");
            };
            inventoryService.OnInventoryLow += (s, e) =>
            {
                Console.WriteLine($"[EVENT] LOW STOCK: {e.product.productId}, Qty={e.product.stockQty}");
            };
            orderService.OnPointAccumulateChanged += (s, e) =>
            {
                Console.WriteLine($"[EVENT] POINTS: {e.customerName} +{e.pointadded} (Total={e.newPoint})");
            };
            promoService.OnAnouceGift += (s, e) =>
            {
                Console.WriteLine("--------Happy Mid Autumn Festival. Get A Discount Right Now---------");
            };




            // === Lập 1 đơn hàng demo (có 2 dòng, 1 dòng giảm 10%) ===
            // === Lập 1 đơn hàng demo Buy 2 Get 1 Free ===
            var order = orderService.CreateOrder(customer.customerId);

            // Mua 3 ly espresso → sẽ được tặng 1 ly miễn phí
            orderService.AddItem(order.orderno, "Esp 122", qty: 5, lineDiscountPercent: 0m);
            

            // Thêm vài món khác để demo
            //orderService.AddItem(order.orderno, "Cake 001", qty: 1, lineDiscountPercent: 0m);
            orderService.AddItem(order.orderno, "ST2", qty: 2, lineDiscountPercent: 0m);
            promoService.ApplyAllPromotion(order);

            // Tính & in hóa đơn trước khi xác nhận
            orderService.Recal(order.orderno);  
            Console.WriteLine("=== BEFORE CONFIRM ===");
            Console.WriteLine(receiptFormatter.format(order));

            // Xác nhận và thanh toán
            orderService.Confirm(order.orderno);
            orderService.Paid(order.orderno);
            
            // In hóa đơn sau khi thanh toán
            Console.WriteLine("=== AFTER PAID ===");
            
            Console.WriteLine(receiptFormatter.format(order));
            

            Console.WriteLine("\nHoàn tất demo. Nhấn phím bất kỳ để thoát...");
            Console.ReadKey();
        }

    }
}
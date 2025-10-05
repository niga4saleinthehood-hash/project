using Code_Brew_Coffe_Shop.domain;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code_Brew_Coffe_Shop.Pricing
{
    public class SimpleReciepFormater: IRecieptFormat
    {
        private static readonly CultureInfo viVN = new CultureInfo("vi-VN");

        public string format(Order order)
        {
            var sb = new StringBuilder();
            sb.AppendLine("================================ BILL ================================");
            sb.AppendLine($"Order No : {order.orderno}");
            sb.AppendLine($"Date     : {order.dateTime:dd/MM/yyyy HH:mm}");
            sb.AppendLine($"Customer : {order.customer.name}");
            sb.AppendLine($"Status   : {order.status}");
            sb.AppendLine("Description: Thank you for your purchase!");
            sb.AppendLine("-----------------------------------------------------------------------");
            sb.AppendLine(string.Format(
                "{0,-22} {1,-30} {2,5} {3,15} {4,12} {5,15}",
                "Product", "Description", "Qty", "Unit Price", "Discount", "Total"));

            foreach (var l in order.items)
            {
                var lineAmount = l.ItemTotal();
                string productName = l.product.name.Length > 20 ? l.product.name.Substring(0, 20) : l.product.name.PadRight(20);
                string description = l.product.description().Length > 28 ? l.product.description().Substring(0, 28) : l.product.description().PadRight(28);

                // Nếu là hàng khuyến mãi thì thêm (Free)
                if (l.isPromotionItem)
                {
                    description = $"(Free) {description}";
                }

                sb.AppendLine(string.Format(
                    "{0,-22} {1,-30} {2,5} {3,15} {4,12:P0} {5,15}",
                    productName,
                    description,
                    l.Quantity,
                    l.unitPrice.ToString("N0", viVN),     // Dùng N0 để canh phải dễ hơn
                    l.LineDiscountAmount,
                    lineAmount.ToString("N0", viVN)));
            }

            sb.AppendLine("-----------------------------------------------------------------------");
            sb.AppendLine(string.Format("{0,-50} {1,15:N0}", "Pre-Total:", order.SubTotal));
            sb.AppendLine(string.Format("{0,-50} {1,15:N0}", "Discount Customer:", order.CustomerDiscount));
            sb.AppendLine(string.Format("{0,-50} {1,15:N0}", "Discount Order:", order.DiscountOnOrder));
            sb.AppendLine(string.Format("{0,-50} {1,15:N0}", "VAT 8%:", order.VAT));
            sb.AppendLine(string.Format("{0,-50} {1,15:N0}", "Total:", order.Total));
            sb.AppendLine("=======================================================================");
            return sb.ToString();
        }

    }
}

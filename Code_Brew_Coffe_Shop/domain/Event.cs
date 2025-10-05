using Code_Brew_Coffe_Shop.domain.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Code_Brew_Coffe_Shop.domain.Enum;

namespace Code_Brew_Coffe_Shop.domain
{
    public class Event
    {
        public class OrderStatusChangedEvent : EventArgs
        {
            public string orderno { get; set; }
            public OrderStatus NewStatus { get; set; }
            public OrderStatus OldStatus { get; set; }

            public OrderStatusChangedEvent(string orderno, OrderStatus oldStatus, OrderStatus newStatus)
            {
                this.orderno = orderno;
                this.OldStatus = oldStatus;
                this.NewStatus = newStatus;
            }

        }

        public class InventoryLowHandlerEvent : EventArgs
        {
            public Product product { get; }
            public InventoryLowHandlerEvent(Product product)
            {
                this.product = product;
            }
        }

        public class PointAccumulatedEvent : EventArgs
        {
            public string customerId { get; }
            public string customerName { get; } 
            public int pointadded { get; }  
            public int newPoint { get; }

            public PointAccumulatedEvent(string customerId, string customerName, int pointadded, int newPoint)
            {
                this.customerId = customerId;
                this.customerName = customerName;
                this.pointadded = pointadded;
                this.newPoint = newPoint;
            }

        }

        public class AnnouceGift : EventArgs
        {
            public string productId { get; set; }
            public AnnouceGift (string productId) {
                this.productId = productId;
            }
        }
    }
}

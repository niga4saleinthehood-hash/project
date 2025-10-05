using Code_Brew_Coffe_Shop.domain;
using Code_Brew_Coffe_Shop.repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Code_Brew_Coffe_Shop.domain.Event;

namespace Code_Brew_Coffe_Shop.Service
{
    public class InventoryService
    {
        public readonly IRepository<Product, string> _productRepo;
        public readonly int reorderThreshold;

        public InventoryService(IRepository<Product, string> productRepo, int reorderThreshold)
        {
            _productRepo = productRepo;
            this.reorderThreshold = reorderThreshold;
        }

        public delegate void InventoryHandler(object sender, InventoryLowHandlerEvent args);
        public event InventoryHandler OnInventoryLow;

        public void Decrease_Stock(string productId, int qty)
        {
            var p = _productRepo.GetById(productId);
            if(p == null) throw new ArgumentNullException("No product found!");
            if(p.stockQty < qty) throw new InvalidOperationException("Not enough stock!");
            p.stockQty -= qty;
            _productRepo.Update(p);
            if(p.stockQty < reorderThreshold)
            {
                if(OnInventoryLow != null)
                    this.OnInventoryLow(this, new InventoryLowHandlerEvent(p));
            }

        }
    }
}

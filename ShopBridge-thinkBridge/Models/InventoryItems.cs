using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopBridge_thinkBridge.Models
{
    public class InventoryItems
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }
    }
}
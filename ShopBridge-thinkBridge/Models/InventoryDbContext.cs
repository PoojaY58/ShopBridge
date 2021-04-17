using System;
using System.Data.Entity;

namespace ShopBridge_thinkBridge.Models
{
    public class InventoryDbContext  : DbContext
    {
        public InventoryDbContext() : base("ShopBridgeConnection")
        {

        }
        public DbSet<InventoryItems> InventoryItems { get; set; }
    }
}
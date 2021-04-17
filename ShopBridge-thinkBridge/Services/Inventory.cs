using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShopBridge_thinkBridge.Models;



namespace ShopBridge_thinkBridge.Services
{
    public class Inventory : IInventory
    {
        private readonly InventoryDbContext _context;

        public Inventory(InventoryDbContext context)
        {
            context = _context;
        }

        public IList<InventoryItems> GetInventoryItems()
        {
            var inventoryItems = _context.InventoryItems.ToList();
            return inventoryItems;
        }

        public InventoryItems GetInventoryItemById(int ID)
        {
            return _context.InventoryItems.Find(ID);
        }

        public void SaveInventoryItem(InventoryItems inventoryItem)
        {
            _context.InventoryItems.Add(inventoryItem);
            _context.SaveChanges();
        }

        public InventoryItems DeleteInventoryItem(int ID)
        {
            var inventoryItem = _context.InventoryItems.Find(ID);
            _context.InventoryItems.Remove(inventoryItem);
            _context.SaveChanges();

            return inventoryItem;

        }

        public void UpdateItem(InventoryItems inventoryItem)
        {
            _context.InventoryItems.Add(inventoryItem);
            _context.SaveChanges();

        }
    }
}

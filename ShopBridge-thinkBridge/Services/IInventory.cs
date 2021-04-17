using ShopBridge_thinkBridge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopBridge_thinkBridge.Services
{
    public interface IInventory
    {
        IList<InventoryItems> GetInventoryItems();
        InventoryItems GetInventoryItemById(int ID);
        void SaveInventoryItem(InventoryItems inventoryItem);
        InventoryItems DeleteInventoryItem(int ID);
        void UpdateItem(InventoryItems inventoryItem);
    }
}

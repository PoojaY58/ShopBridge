using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ShopBridge_thinkBridge.Models;
using ShopBridge_thinkBridge.Services;

namespace ShopBridge_thinkBridge.Controllers
{
    public class InventoryController : ApiController
    {
        private readonly Inventory _inventoryService;

        public InventoryController(IInventory _inventoryService)
        {
            _inventoryService = _inventoryService;
        }

        //GET: api/InventoryItem/userId
        [HttpGet]
        [ResponseType(typeof(InventoryItems))]
        public IHttpActionResult GetInventoryItem(int id)
        {
           InventoryItems inventoryItem = _inventoryService.GetInventoryItemById(id);
           if (inventoryItem == null)
           {
                return NotFound();
           }

           return Ok(inventoryItem);
        }


        //GET: api/InventoryItems
        [HttpGet]
        public IList<InventoryItems> GetAllInventoryItem()
        {
            try
            {
                var inventoryItems = _inventoryService.GetInventoryItems();
                return inventoryItems;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        //POST: api/InventoryItem/User
        [HttpPost]
        [ResponseType(typeof(InventoryItems))]
        public IHttpActionResult CreateInventoryItem([FromBody]InventoryItems inventoryItem)
        {
            try
            {
                if (inventoryItem != null && ModelState.IsValid)
                {
                    _inventoryService.SaveInventoryItem(inventoryItem);
                    return CreatedAtRoute("DefaultApi", new { id = inventoryItem.Id }, inventoryItem);
                }
                else
                {
                    var message = "Invalid Inventory Item details";
                    //return BadRequest(message);
                    return Content(HttpStatusCode.BadRequest, message);
                }
            }
            catch (Exception ex) 
            {
                return InternalServerError(ex);
            }

        }

        //DELETE: api/InventoryItem/userId
        [HttpDelete]
        [ResponseType(typeof(InventoryItems))]
        public IHttpActionResult DeleteInventoryItem(int id)
        {
            try
            {
                InventoryItems deleteInventoryItem = _inventoryService.DeleteInventoryItem(id);
                return Ok(deleteInventoryItem);
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }

        //UPDATE: api/InventoryItem/User
        [HttpPut]
        [Route("updateItem")]
        public void UpdateItem([FromBody] InventoryItems inventoryItem)
        {
            try
            {
               var result= _inventoryService.GetInventoryItemById(inventoryItem.Id);
               var itemToBeUpdated = new InventoryItems()
               {
                   Name = result.Name,
                   Description = result.Description,
                   Price = Convert.ToDecimal(result.Price)
               };
               _inventoryService.UpdateItem(itemToBeUpdated);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
    }
}

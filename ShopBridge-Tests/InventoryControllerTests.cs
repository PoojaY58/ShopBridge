using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Moq;
using NUnit.Framework;
using ShopBridge_thinkBridge.Controllers;
using ShopBridge_thinkBridge.Services;
using ShopBridge_thinkBridge.Models;
using System.Web.Http;
using System.Web.Http.Hosting;
using System.Web.Http.Results;



namespace ShopBridge_Tests
{
    [TestFixture]
    public class InventoryControllerTests 
    {
        private Mock<Inventory> itemService;
        [SetUp]
        public void SetUp()
        {
            itemService = new Mock<Inventory>();
        }

        [Test]
        public void GetAllInventoryItemsReturnsListOfInventoryItems()
        {
            // Arrange
            IList<InventoryItems> dummyItems = GetFakeInventoryItems();
            itemService.Setup(x => x.GetInventoryItems()).Returns(dummyItems);
            InventoryController controller = new InventoryController(itemService.Object)
            {
                Request = new HttpRequestMessage()
                {
                    Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }
                }
            };

            // Act
            var invItems = controller.GetAllInventoryItem();

            // Assert
            Assert.IsNotNull(invItems, "Result is null");
            Assert.IsInstanceOf(typeof(IEnumerable<InventoryItems>), invItems, "Wrong Model");
            Assert.AreEqual(3, invItems.Count(), "Got wrong number of Inventory Items");
        }

       
        [Test]
        public void CreateSetsLocationHeader()
        {
            // Arrange
            InventoryController controller = new InventoryController(itemService.Object);

            // Act
            IHttpActionResult actionResult = controller.CreateInventoryItem(new InventoryItems() { Id = 4, Name = "TestItem4", Description = "TestDesc4", Price = 40});
            var createdResult = actionResult as CreatedAtRouteNegotiatedContentResult<InventoryItems>;

            // Assert
            Assert.IsNotNull(createdResult);
            Assert.AreEqual("DefaultApi", createdResult.RouteName);
            Assert.AreEqual(4, createdResult.RouteValues["id"]);
        }

        [Test]
        public void CreateItemNullInventoryItemReturnsBadRequest()
        {
            // Arrange
            InventoryController controller = new InventoryController(itemService.Object);

            // Act
            IHttpActionResult actionResult = controller.CreateInventoryItem(null);
            var createdResult = actionResult as NegotiatedContentResult<string>;

            // Assert            
            Assert.IsInstanceOf<NegotiatedContentResult<string>>(createdResult);
            Assert.AreEqual(HttpStatusCode.BadRequest, createdResult.StatusCode);
            Assert.AreEqual("Invalid Request", createdResult.Content);
        }

        [Test]
        public void DeleteReturnsItemOk()
        {
            // Arrange    
            IList<InventoryItems> dummyItems = GetFakeInventoryItems();
            itemService.Setup(x => x.DeleteInventoryItem(1)).Returns(dummyItems.Where(x => x.Id == 1).FirstOrDefault());

            InventoryController controller = new InventoryController(itemService.Object)
            {
                Request = new HttpRequestMessage()
                {
                    Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }
                }
            };

            // Act
            IHttpActionResult actionResult = controller.DeleteInventoryItem(1);
            var contentResult = actionResult as OkNegotiatedContentResult<InventoryItems>;

            // Assert
            Assert.IsInstanceOf<InventoryItems>(contentResult.Content);
        }

        private static IList<InventoryItems> GetFakeInventoryItems()
        {
            IList<InventoryItems> dummyItems = new List<InventoryItems>
            {
                    new InventoryItems {Id= 1,Name="TestItem1",Description="TestDesc1",Price=10},
                    new InventoryItems {Id=2,Name="TestItem2",Description="TestDesc2",Price=20},
                    new InventoryItems {Id =3,Name="TestItem3",Description="TestDesc3",Price=30 }
            }.ToList();
            return dummyItems;
        }
    }

} 

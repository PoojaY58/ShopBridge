using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Hosting;
using System.Web.Http.Results;
using NUnit.Framework;
using Moq;
using ShopBridge_thinkBridge.Controllers;
using ShopBridge_thinkBridge.Services;
using ShopBridge_thinkBridge.Models;
namespace ShopBridge_Tests
{
   
    [TestFixture]
    public class InventoryServiceTests
    {
        private Mock<Inventory> itemService;
        [SetUp]
        public void SetUp()
        {
            itemService = new Mock<Inventory>();
        }

        [Test]
        public void GetInventoryItems_Returns_AllInventoryItems()
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
            var inventoryItems = controller.GetAllInventoryItem();

            // Assert
            Assert.IsNotNull(inventoryItems, "Result is null");
            Assert.IsInstanceOf(typeof(IList<InventoryItems>), inventoryItems, "Wrong Model");
            Assert.AreEqual(3, inventoryItems.Count(), "Got wrong number of Inventory Items");
        }

        [Test]
        public void Get_CorrectInventoryItemID_Returns_InventoryItem()
        {
            // Arrange   
            IList<InventoryItems> dummyItems = GetFakeInventoryItems();
            itemService.Setup(x => x.GetInventoryItemById(1)).Returns(dummyItems.Where(x => x.Id== 1).FirstOrDefault());

            InventoryController controller = new InventoryController(itemService.Object)
            {
                Request = new HttpRequestMessage()
                {
                    Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }
                }
            };

            // Act
            var actionResult = controller.GetInventoryItem(1);
            var contentResult = actionResult as OkNegotiatedContentResult<InventoryItems>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(1, contentResult.Content.Id, "Got wrong number of Inventory Items");
        }

        [Test]
        public void Get_InValidInventoryItemID_Returns_NotFound()
        {
            // Arrange   
            IList<InventoryItems> dummyItems = GetFakeInventoryItems();
            itemService.Setup(x => x.GetInventoryItemById(5)).Returns(dummyItems.Where(x => x.Id == 5).FirstOrDefault());

            InventoryController controller = new InventoryController(itemService.Object)
            {
                Request = new HttpRequestMessage()
                {
                    Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }
                }
            };

            // Act
            var actionResult = controller.GetInventoryItem(5);
            var contentResult = actionResult as OkNegotiatedContentResult<InventoryItems>;

            // Assert
            Assert.IsNull(contentResult);
            Assert.IsInstanceOf<NotFoundResult>(actionResult);
        }

        [Test]
        public void PostSetsLocationHeader()
        {
            // Arrange
            InventoryController controller = new InventoryController(itemService.Object);

            // Act
            IHttpActionResult actionResult = controller.CreateInventoryItem(new InventoryItems { Id = 4, Name = "TestItem4", Description = "TestDesc4", Price = 40 });
            var createdResult = actionResult as CreatedAtRouteNegotiatedContentResult<InventoryItems>;

            // Assert
            Assert.IsNotNull(createdResult);
            Assert.AreEqual("DefaultApi", createdResult.RouteName);
            Assert.AreEqual(4, createdResult.RouteValues["id"]);
        }

        [Test]
        public void Post_NullInventoryItem_Returns_BadRequest()
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
        public void DeleteReturnsOk()
        {
            // Arrange    
            IList<InventoryItems> fakeItems = GetFakeInventoryItems();
            itemService.Setup(x => x.DeleteInventoryItem(1)).Returns(fakeItems.Where(x => x.Id == 1).FirstOrDefault());

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
                    new InventoryItems {Id=1,Name="TestItem1",Description="TestDesc1",Price=10 },
                    new InventoryItems {Id=2,Name="TestItem2",Description="TestDesc2",Price=20 },
                    new InventoryItems {Id=3,Name="TestItem3",Description="TestDesc3",Price=30 }
            }.ToList();
            return dummyItems;
        }
    }
}

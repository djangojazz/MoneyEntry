using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ExpensesAPI;
using ExpensesAPI.Controllers;
using System.Threading.Tasks;

namespace ExpensesAPI.Tests.Controllers
{
    [TestClass]
    public class ValuesControllerTest
    {
        [TestMethod]
        public async void Get()
        {
            // Arrange
            TypesController controller = new TypesController();

            // Act
            var result = await controller.Get();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("value1", result.ElementAt(0));
            Assert.AreEqual("value2", result.ElementAt(1));
        }

        //[TestMethod]
        //public void GetById()
        //{
        //    // Arrange
        //    TypesController controller = new TypesController();

        //    // Act
        //    string result = controller.Get();

        //    // Assert
        //    Assert.AreEqual("value", result);
        //}

        //[TestMethod]
        //public void Post()
        //{
        //    // Arrange
        //    TypesController controller = new TypesController();

        //    // Act
        //    controller.Post("value");

        //    // Assert
        //}

        //[TestMethod]
        //public void Put()
        //{
        //    // Arrange
        //    TypesController controller = new TypesController();

        //    // Act
        //    controller.Put(5, "value");

        //    // Assert
        //}

        //[TestMethod]
        //public void Delete()
        //{
        //    // Arrange
        //    TypesController controller = new TypesController();

        //    // Act
        //    controller.Delete(5);

        //    // Assert
        //}
    }
}

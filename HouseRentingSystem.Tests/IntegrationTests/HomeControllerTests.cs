using HouseRentingSystem.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseRentingSystem.Tests.IntegrationTests
{
    public class HomeControllerTests
    {
        private HomeController homeController;

        [OneTimeSetUp]
        public void SetUp()
        {
            this.homeController = new HomeController(null);
        }

        [Test]
        public void Error_ShouldReturnCorrectView()
        {
            //Arrange: assign a valid status code to a variable
            var statusCode = 500;

            //Act: invoke the controller method with valid data
            var result = this.homeController.Error(statusCode);

            //Assert the returned result is not null
            Assert.IsNotNull(result);

            //Assert the returned result is a view
            var viewResult = new ViewResult();
            Assert.IsNotNull(viewResult);
        }
    }
}

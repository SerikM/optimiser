//using Xunit;
//using Moq;
//using Optimiser.Services;
//using Optimiser.Controllers;
//using Microsoft.AspNetCore.Mvc;
//using System.Threading.Tasks;

//namespace Optimiser.Tests
//{
//    public class DefaultControllerTests
//    {
//        [Fact]
//        public void TestGetOptimiser()
//        {
//            var serviceMock = new Mock<ICalculationService>();
//            serviceMock.Setup(p => p.GetOptimiser(It.IsAny<string>(), It.IsAny<string>())).Returns(() => new Task<int>(null));
//            var mockController = new DefaultController(serviceMock.Object);
//            var response = mockController.GetData("","").Result as BadRequestObjectResult;
//            Assert.NotNull(response);
//            Assert.Equal("failed to process request", response.Value);
//        }

//        [Fact]
//        public void TestGetOptimiserSuccess()
//        {
//            var serviceMock = new Mock<ICalculationService>();
//            serviceMock.Setup(p => p.GetOptimiser(It.IsAny<string>(), It.IsAny<string>())).Returns(() => Task.FromResult(20));
//            var mockController = new DefaultController(serviceMock.Object);
//            var response = mockController.GetData("01/02/2020", "01/03/2020").Result as OkObjectResult;
//            Assert.NotNull(response);
//            Assert.Equal("20", response.Value);
//        }
//    }
//}

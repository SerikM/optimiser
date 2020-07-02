using Xunit;
using Moq;
using Optimiser.Services;
using Optimiser.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using Optimiser.Models;

namespace Optimiser.Tests
{
    public class DefaultControllerTests
    {
        [Fact]
        public void TestOptimise()
        {
            var serviceMock = new Mock<IProcessingService>();
            serviceMock.Setup(p => p.GetOptimalRatings(It.IsAny<List<Break>>())).Returns(() => new Task<List<Break>>(null));
            var mockController = new DefaultController(serviceMock.Object);
            var response = mockController.Optimise(null).Result as BadRequestObjectResult;
            Assert.NotNull(response);
            Assert.Equal("failed to process request", response.Value);
        }

        [Fact]
        public void TestGetOptimiserSuccess()
        {
            var serviceMock = new Mock<IProcessingService>();
            serviceMock.Setup(p => p.GetOptimalRatings(It.IsAny<List<Break>>())).Returns(() => Task.FromResult(MockData.GetDefaultBreaks()));
            var mockController = new DefaultController(serviceMock.Object);
            var response = mockController.Optimise(MockData.GetBreaksWithOrderedCommercials()).Result as OkObjectResult;
            Assert.NotNull(response);
            Assert.Equal(MockData.GetSerializedResponse(), response.Value);
        }
    }
}

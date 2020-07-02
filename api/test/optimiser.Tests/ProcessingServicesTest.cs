using Xunit;
using Moq;
using Optimiser.Models;
using System.Threading.Tasks;
using Optimiser.Services;
using System.Linq;

namespace Optimiser.Tests
{
    public class ProcessingServicesTest
    {
        [Fact]
        public void TestStartSatEndSunday()
        {
            var dataMock = new Mock<IDbDataService<IData>>();
            var brksWithRatings = MockData.GetDefaultBreaks();
            var expectedOutput = MockData.GetBreaksWithOrderedCommercials();
            var defaultCommercials = MockData.GetDefaultCommercials();
            dataMock.Setup(p => p.GetItems<Commercial>()).Returns(Task.FromResult(defaultCommercials));
            var service = new ProcessingService(dataMock.Object);
            var result = service.GetOptimalRatings(brksWithRatings).Result;
            
            //brk 1
            Assert.Equal(expectedOutput.ElementAt(0).Id, result.ElementAt(0).Id);
            Assert.Equal(expectedOutput.ElementAt(0).Commercials.Count, result.ElementAt(0).Commercials.Count);
            //brk 1 comm1
            Assert.Equal(expectedOutput.ElementAt(0).Commercials.ElementAt(0).Id, result.ElementAt(0).Commercials.ElementAt(0).Id);
            Assert.Equal(expectedOutput.ElementAt(0).Commercials.ElementAt(0).TargetDemo, result.ElementAt(0).Commercials.ElementAt(0).TargetDemo);
            Assert.Equal(expectedOutput.ElementAt(0).Commercials.ElementAt(0).CommercialType, result.ElementAt(0).Commercials.ElementAt(0).CommercialType);
            Assert.Equal(expectedOutput.ElementAt(0).Commercials.ElementAt(0).TargetDemoName, result.ElementAt(0).Commercials.ElementAt(0).TargetDemoName);
            //brk 1 comm2
            Assert.Equal(expectedOutput.ElementAt(0).Commercials.ElementAt(1).Id, result.ElementAt(0).Commercials.ElementAt(1).Id);
            Assert.Equal(expectedOutput.ElementAt(0).Commercials.ElementAt(1).TargetDemo, result.ElementAt(0).Commercials.ElementAt(1).TargetDemo);
            Assert.Equal(expectedOutput.ElementAt(0).Commercials.ElementAt(1).CommercialType, result.ElementAt(0).Commercials.ElementAt(1).CommercialType);
            Assert.Equal(expectedOutput.ElementAt(0).Commercials.ElementAt(1).TargetDemoName, result.ElementAt(0).Commercials.ElementAt(1).TargetDemoName);
            //brk 1 comm3
            Assert.Equal(expectedOutput.ElementAt(0).Commercials.ElementAt(2).Id, result.ElementAt(0).Commercials.ElementAt(2).Id);
            Assert.Equal(expectedOutput.ElementAt(0).Commercials.ElementAt(2).TargetDemo, result.ElementAt(0).Commercials.ElementAt(2).TargetDemo);
            Assert.Equal(expectedOutput.ElementAt(0).Commercials.ElementAt(2).CommercialType, result.ElementAt(0).Commercials.ElementAt(2).CommercialType);
            Assert.Equal(expectedOutput.ElementAt(0).Commercials.ElementAt(2).TargetDemoName, result.ElementAt(0).Commercials.ElementAt(2).TargetDemoName);
            //brk 2
            Assert.Equal(expectedOutput.ElementAt(1).Id, result.ElementAt(1).Id);
            Assert.Equal(expectedOutput.ElementAt(1).Commercials.Count, result.ElementAt(1).Commercials.Count);
            //brk 2 comm1
            Assert.Equal(expectedOutput.ElementAt(1).Commercials.ElementAt(0).Id, result.ElementAt(1).Commercials.ElementAt(0).Id);
            Assert.Equal(expectedOutput.ElementAt(1).Commercials.ElementAt(0).TargetDemo, result.ElementAt(1).Commercials.ElementAt(0).TargetDemo);
            Assert.Equal(expectedOutput.ElementAt(1).Commercials.ElementAt(0).CommercialType, result.ElementAt(1).Commercials.ElementAt(0).CommercialType);
            Assert.Equal(expectedOutput.ElementAt(1).Commercials.ElementAt(0).TargetDemoName, result.ElementAt(1).Commercials.ElementAt(0).TargetDemoName);
            //brk 2 comm2
            Assert.Equal(expectedOutput.ElementAt(1).Commercials.ElementAt(1).Id, result.ElementAt(1).Commercials.ElementAt(1).Id);
            Assert.Equal(expectedOutput.ElementAt(1).Commercials.ElementAt(1).TargetDemo, result.ElementAt(1).Commercials.ElementAt(1).TargetDemo);
            Assert.Equal(expectedOutput.ElementAt(1).Commercials.ElementAt(1).CommercialType, result.ElementAt(1).Commercials.ElementAt(1).CommercialType);
            Assert.Equal(expectedOutput.ElementAt(1).Commercials.ElementAt(1).TargetDemoName, result.ElementAt(1).Commercials.ElementAt(1).TargetDemoName);
            //brk 2 comm3
            Assert.Equal(expectedOutput.ElementAt(1).Commercials.ElementAt(2).Id, result.ElementAt(1).Commercials.ElementAt(2).Id);
            Assert.Equal(expectedOutput.ElementAt(1).Commercials.ElementAt(2).TargetDemo, result.ElementAt(1).Commercials.ElementAt(2).TargetDemo);
            Assert.Equal(expectedOutput.ElementAt(1).Commercials.ElementAt(2).CommercialType, result.ElementAt(1).Commercials.ElementAt(2).CommercialType);
            Assert.Equal(expectedOutput.ElementAt(1).Commercials.ElementAt(2).TargetDemoName, result.ElementAt(1).Commercials.ElementAt(2).TargetDemoName);
            //brk 3
            Assert.Equal(expectedOutput.ElementAt(2).Id, result.ElementAt(2).Id);
            Assert.Equal(expectedOutput.ElementAt(2).Commercials.Count, result.ElementAt(2).Commercials.Count);
            //brk 3 comm1
            Assert.Equal(expectedOutput.ElementAt(2).Commercials.ElementAt(0).Id, result.ElementAt(2).Commercials.ElementAt(0).Id);
            Assert.Equal(expectedOutput.ElementAt(2).Commercials.ElementAt(0).TargetDemo, result.ElementAt(2).Commercials.ElementAt(0).TargetDemo);
            Assert.Equal(expectedOutput.ElementAt(2).Commercials.ElementAt(0).CommercialType, result.ElementAt(2).Commercials.ElementAt(0).CommercialType);
            Assert.Equal(expectedOutput.ElementAt(2).Commercials.ElementAt(0).TargetDemoName, result.ElementAt(2).Commercials.ElementAt(0).TargetDemoName);
            //brk 3 comm2
            Assert.Equal(expectedOutput.ElementAt(2).Commercials.ElementAt(1).Id, result.ElementAt(2).Commercials.ElementAt(1).Id);
            Assert.Equal(expectedOutput.ElementAt(2).Commercials.ElementAt(1).TargetDemo, result.ElementAt(2).Commercials.ElementAt(1).TargetDemo);
            Assert.Equal(expectedOutput.ElementAt(2).Commercials.ElementAt(1).CommercialType, result.ElementAt(2).Commercials.ElementAt(1).CommercialType);
            Assert.Equal(expectedOutput.ElementAt(2).Commercials.ElementAt(1).TargetDemoName, result.ElementAt(2).Commercials.ElementAt(1).TargetDemoName);
            //brk 3 comm3
            Assert.Equal(expectedOutput.ElementAt(2).Commercials.ElementAt(2).Id, result.ElementAt(2).Commercials.ElementAt(2).Id);
            Assert.Equal(expectedOutput.ElementAt(2).Commercials.ElementAt(2).TargetDemo, result.ElementAt(2).Commercials.ElementAt(2).TargetDemo);
            Assert.Equal(expectedOutput.ElementAt(2).Commercials.ElementAt(2).CommercialType, result.ElementAt(2).Commercials.ElementAt(2).CommercialType);
            Assert.Equal(expectedOutput.ElementAt(2).Commercials.ElementAt(2).TargetDemoName, result.ElementAt(2).Commercials.ElementAt(2).TargetDemoName);
            //totals
            Assert.Equal(expectedOutput.ElementAt(1).DisallowedCommTypes.Count, result.ElementAt(1).DisallowedCommTypes.Count);
            Assert.Equal(1970, result.Sum(d => d?.Commercials?.Sum(p => p.CurrentRating.Score)));
        }

        [Fact]
        public void TestStartSundayEndSunday()
        {
            var dataMock = new Mock<IDbDataService<IData>>();
            var unordered = MockData.GetBreaksWithUnorderedCommercials();
            var ordered = MockData.GetBreaksWithOrderedCommercials();
            var service = new ProcessingService(dataMock.Object);
            var result = service.OrderCommercials(unordered);

            //brk 1
            Assert.Equal(ordered.ElementAt(0).Commercials.ElementAt(0).CommercialType, result.ElementAt(0).Commercials.ElementAt(0).CommercialType);
            Assert.Equal(ordered.ElementAt(0).Commercials.ElementAt(1).CommercialType, result.ElementAt(0).Commercials.ElementAt(1).CommercialType);
            Assert.Equal(ordered.ElementAt(0).Commercials.ElementAt(2).CommercialType, result.ElementAt(0).Commercials.ElementAt(2).CommercialType);
            //brk 1
            Assert.Equal(ordered.ElementAt(1).Commercials.ElementAt(0).CommercialType, result.ElementAt(1).Commercials.ElementAt(0).CommercialType);
            Assert.Equal(ordered.ElementAt(1).Commercials.ElementAt(1).CommercialType, result.ElementAt(1).Commercials.ElementAt(1).CommercialType);
            Assert.Equal(ordered.ElementAt(1).Commercials.ElementAt(2).CommercialType, result.ElementAt(1).Commercials.ElementAt(2).CommercialType);
            //brk 1
            Assert.Equal(ordered.ElementAt(2).Commercials.ElementAt(0).CommercialType, result.ElementAt(2).Commercials.ElementAt(0).CommercialType);
            Assert.Equal(ordered.ElementAt(2).Commercials.ElementAt(1).CommercialType, result.ElementAt(2).Commercials.ElementAt(1).CommercialType);
            Assert.Equal(ordered.ElementAt(2).Commercials.ElementAt(2).CommercialType, result.ElementAt(2).Commercials.ElementAt(2).CommercialType);
        }
    }
}

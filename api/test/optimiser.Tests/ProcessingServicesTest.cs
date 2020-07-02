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
        public void TestGetOptimalRatings()
        {
            var dataMock = new Mock<IDbDataService<IData>>();
            var brksWithRatings = MockData.GetDefaultBreaks();
            var expectedOutput = MockData.GetBreaksWithOrderedCommercials();
            var defaultCommercials = MockData.GetDefaultCommercials();
            dataMock.Setup(p => p.GetItems<Commercial>()).Returns(Task.FromResult(defaultCommercials));
            var service = new ProcessingService(dataMock.Object);
            var result = service.GetOptimalRatings(brksWithRatings).Result;

            for (int i = 0; i < expectedOutput.Count; i++)
            {
                Assert.Equal(expectedOutput.ElementAt(i).Id, result.ElementAt(i).Id);
                Assert.Equal(expectedOutput.ElementAt(i).Commercials.Count, result.ElementAt(i).Commercials.Count);
                for (int j = 0; j < expectedOutput.ElementAt(i).Commercials.Count; j++)
                {
                    Assert.Equal(expectedOutput.ElementAt(i).Commercials.ElementAt(j).Id, result.ElementAt(i).Commercials.ElementAt(j).Id);
                    Assert.Equal(expectedOutput.ElementAt(i).Commercials.ElementAt(j).TargetDemo, result.ElementAt(i).Commercials.ElementAt(j).TargetDemo);
                    Assert.Equal(expectedOutput.ElementAt(i).Commercials.ElementAt(j).CommercialType, result.ElementAt(i).Commercials.ElementAt(j).CommercialType);
                    Assert.Equal(expectedOutput.ElementAt(i).Commercials.ElementAt(j).TargetDemoName, result.ElementAt(i).Commercials.ElementAt(j).TargetDemoName);
                }
            }
            Assert.Equal(expectedOutput.ElementAt(1).DisallowedCommTypes.Count, result.ElementAt(1).DisallowedCommTypes.Count);
            Assert.Equal(1970, result.Sum(d => d?.Commercials?.Sum(p => p.CurrentRating.Score)));
        }

        [Fact]
        public void TestOrderCommercials()
        {
            var dataMock = new Mock<IDbDataService<IData>>();
            var unordered = MockData.GetBreaksWithUnorderedCommercials();
            var ordered = MockData.GetBreaksWithOrderedCommercials();
            var service = new ProcessingService(dataMock.Object);
            var result = service.OrderCommercials(unordered);


            for (int i = 0; i < ordered.Count; i++)
            {
                Assert.Equal(ordered.ElementAt(i).Id, result.ElementAt(i).Id);
                Assert.Equal(ordered.ElementAt(i).Commercials.Count, result.ElementAt(i).Commercials.Count);
                for (int j = 0; j < ordered.ElementAt(i).Commercials.Count; j++)
                {
                    Assert.Equal(ordered.ElementAt(i).Commercials.ElementAt(j).CommercialType, result.ElementAt(i).Commercials.ElementAt(j).CommercialType);
                    Assert.Equal(ordered.ElementAt(i).Commercials.ElementAt(j).CommercialType, result.ElementAt(i).Commercials.ElementAt(j).CommercialType);
                    Assert.Equal(ordered.ElementAt(i).Commercials.ElementAt(j).CommercialType, result.ElementAt(i).Commercials.ElementAt(j).CommercialType);
                }
            }
        }

        [Fact]
        public void TestGetBestBreak()
        {
            var unordered = MockData.GetDefaultBreaks();
            var commercial = MockData.GetDefaultCommercials().ElementAt(3);
            var service = new ProcessingService(null);
            var br = service.GetBestBreak(unordered, commercial);
            Assert.NotNull(br);
            Assert.Equal(3, br.Id);
        }
    }
}
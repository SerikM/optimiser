using Optimiser.Enums;
using Optimiser.Models;
using System.Collections.Generic;

namespace Optimiser.Services
{
    public class DefaultData
    {
        public static List<Commercial> GetDefaultCommercials()
        {
            return new List<Commercial> {
                new Commercial() { Id = 1, CommercialType = CommercialType.Automotive, TargetDemo = DemographicType.Women },
                new Commercial() { Id = 2, CommercialType = CommercialType.Travel, TargetDemo = DemographicType.Men },
                new Commercial() { Id = 3, CommercialType = CommercialType.Travel, TargetDemo = DemographicType.Total },
                new Commercial() { Id = 4, CommercialType = CommercialType.Automotive, TargetDemo = DemographicType.Men },
                new Commercial() { Id = 5, CommercialType = CommercialType.Automotive, TargetDemo = DemographicType.Men },
                new Commercial() { Id = 6, CommercialType = CommercialType.Finance, TargetDemo = DemographicType.Women },
                new Commercial() { Id = 7, CommercialType = CommercialType.Finance, TargetDemo = DemographicType.Men },
                new Commercial() { Id = 8, CommercialType = CommercialType.Automotive, TargetDemo = DemographicType.Total },
                new Commercial() { Id = 9, CommercialType = CommercialType.Travel, TargetDemo = DemographicType.Women }
                };
        }

        public static List<Break> GetDefaultBreaks()
        {
            return new List<Break> {
                    new Break() { Id = 1, Ratings = new List<Rating>{
                    new Rating() { Score = 80, DemoType = DemographicType.Women },
                    new Rating() { Score = 100, DemoType = DemographicType.Men },
                    new Rating() { Score = 250, DemoType = DemographicType.Total }}},
                    new Break() { Id = 2, DisallowedCommTypes =
                    new List<CommercialType> { CommercialType.Finance }, Ratings = new List<Rating>{
                    new Rating() { Score = 50, DemoType = DemographicType.Women },
                    new Rating() { Score = 120, DemoType = DemographicType.Men },
                    new Rating() { Score = 200, DemoType = DemographicType.Total }}},
                    new Break() { Id = 3, Ratings = new List<Rating>{
                    new Rating() { Score = 350, DemoType = DemographicType.Women },
                    new Rating() { Score = 150, DemoType = DemographicType.Men },
                    new Rating() { Score = 500, DemoType = DemographicType.Total }}}
                };
        }
    }
}

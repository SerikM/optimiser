using Optimiser.Models;
using System.Collections.Generic;
using Optimiser.Enums;

namespace Optimiser.Tests
{
    public static class MockData
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

        public static List<Break> GetBreaksWithOrderedCommercials()
        {
            return new List<Break> {
                    new Break() {
                    Id = 1,
                    Commercials = new List<Commercial>()
                    {
                        new Commercial() { Id = 6, CommercialType = CommercialType.Finance, TargetDemo = DemographicType.Women },
                        new Commercial() { Id = 9, CommercialType = CommercialType.Travel, TargetDemo = DemographicType.Women },
                        new Commercial() { Id = 7, CommercialType = CommercialType.Finance, TargetDemo = DemographicType.Men }
                    },
                    Ratings = new List<Rating>{
                    new Rating() { Score = 80, DemoType = DemographicType.Women },
                    new Rating() { Score = 100, DemoType = DemographicType.Men },
                    new Rating() { Score = 250, DemoType = DemographicType.Total }}},

                    new Break() {
                    Id = 2,
                    Commercials = new List<Commercial>()
                    {
                        new Commercial() { Id = 5, CommercialType = CommercialType.Automotive, TargetDemo = DemographicType.Men },
                        new Commercial() { Id = 2, CommercialType = CommercialType.Travel, TargetDemo = DemographicType.Men },
                        new Commercial() { Id = 4, CommercialType = CommercialType.Automotive, TargetDemo = DemographicType.Men },
                    },
                    DisallowedCommTypes =
                    new List<CommercialType> { CommercialType.Finance },
                    Ratings = new List<Rating>{
                    new Rating() { Score = 50, DemoType = DemographicType.Women },
                    new Rating() { Score = 120, DemoType = DemographicType.Men },
                    new Rating() { Score = 200, DemoType = DemographicType.Total }}},

                    new Break() {
                    Id = 3,
                    Commercials = new List<Commercial>()
                    {
                       new Commercial() { Id = 8, CommercialType = CommercialType.Automotive, TargetDemo = DemographicType.Total },
                       new Commercial() { Id = 3, CommercialType = CommercialType.Travel, TargetDemo = DemographicType.Total },
                       new Commercial() { Id = 1, CommercialType = CommercialType.Automotive, TargetDemo = DemographicType.Women }

                    },
                    Ratings = new List<Rating>{
                    new Rating() { Score = 350, DemoType = DemographicType.Women },
                    new Rating() { Score = 150, DemoType = DemographicType.Men },
                    new Rating() { Score = 500, DemoType = DemographicType.Total }}}
                };
        }

        public static List<Break> GetBreaksWithUnorderedCommercials()
        {
            return new List<Break>
            {
                    new Break()
                    {
                    Id = 1,
                    Commercials = new List<Commercial>()
                    {
                        new Commercial() { Id = 6, CommercialType = CommercialType.Finance, TargetDemo = DemographicType.Women },
                        new Commercial() { Id = 7, CommercialType = CommercialType.Finance, TargetDemo = DemographicType.Men },
                        new Commercial() { Id = 9, CommercialType = CommercialType.Travel, TargetDemo = DemographicType.Women }
                    }
                    },
                    new Break()
                    {
                    Id = 2,
                    Commercials = new List<Commercial>()
                    {
                        new Commercial() { Id = 5, CommercialType = CommercialType.Automotive, TargetDemo = DemographicType.Men },
                        new Commercial() { Id = 4, CommercialType = CommercialType.Automotive, TargetDemo = DemographicType.Men },
                        new Commercial() { Id = 2, CommercialType = CommercialType.Travel, TargetDemo = DemographicType.Men }
                    }
                    },
                    new Break()
                    {
                    Id = 3,
                    Commercials = new List<Commercial>()
                    {
                       new Commercial() { Id = 3, CommercialType = CommercialType.Travel, TargetDemo = DemographicType.Total },
                       new Commercial() { Id = 1, CommercialType = CommercialType.Automotive, TargetDemo = DemographicType.Women },
                       new Commercial() { Id = 8, CommercialType = CommercialType.Automotive, TargetDemo = DemographicType.Total },
                    }
                }
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

        public static string GetSerializedResponse()
        {
            return "{\"breaksWithCommercials\":[{\"Id\":1,\"Ratings\":[{\"Id\":0,\"DemoType\":2,\"DemoName\":\"Women\",\"Score\":80},{\"Id\":0,\"DemoType\":1,\"DemoName\":\"Men\",\"Score\":100},{\"Id\":0,\"DemoType\":3,\"DemoName\":\"Total\",\"Score\":250}],\"DisallowedCommTypes\":null,\"Commercials\":null},{\"Id\":2,\"Ratings\":[{\"Id\":0,\"DemoType\":2,\"DemoName\":\"Women\",\"Score\":50},{\"Id\":0,\"DemoType\":1,\"DemoName\":\"Men\",\"Score\":120},{\"Id\":0,\"DemoType\":3,\"DemoName\":\"Total\",\"Score\":200}],\"DisallowedCommTypes\":[3],\"Commercials\":null},{\"Id\":3,\"Ratings\":[{\"Id\":0,\"DemoType\":2,\"DemoName\":\"Women\",\"Score\":350},{\"Id\":0,\"DemoType\":1,\"DemoName\":\"Men\",\"Score\":150},{\"Id\":0,\"DemoType\":3,\"DemoName\":\"Total\",\"Score\":500}],\"DisallowedCommTypes\":null,\"Commercials\":null}],\"total\":0}";
        }
    }
}

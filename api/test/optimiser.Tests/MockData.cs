//using Optimiser.Models;
//using System.Collections.Generic;
//using System;
//using DemographicType = Optimiser.Models.DemographicType;

//namespace Optimiser.Tests
//{
//    public static class MockData
//    {
//        public static List<Break> GetMockHolidays()
//        {
//            return new List<Break>() {
//                new Break()
//                {
//                    Name = "New Years Day",
//                    Date =  new DateTime(2020, 1, 1),
//                    Type = DemographicType.Two
//                },
//                 new Break()
//                {
//                    Name = "Australia Day",
//                    Date =  new DateTime(2020, 1, 26),
//                    Type = DemographicType.Two
//                },
//                 new Break()
//                {
//                    Name = "Good Friday",
//                    Date =  new DateTime(2020,4,19),
//                    Type = DemographicType.Four
//                },
//                 new Break()
//                {
//                    Name = "Eater Monday",
//                    Date =  new DateTime(2020,4,22),
//                    Type = DemographicType.Four
//                },
//                new Break()
//                {
//                    Name = "Anzac Day",
//                    Date =  new DateTime(2020, 4, 25),
//                    Type =  DemographicType.One
//                 },
//                new Break()
//                {
//                    Name = "Queens Birthday",
//                    Date =  new DateTime(2020, 6, 8),
//                    Type =  DemographicType.Three
//                 },
//                 new Break()
//                {
//                    Name = "Labor Day",
//                    Date =  new DateTime(2020, 10, 5),
//                    Type =  DemographicType.Three
//                 },
//                 new Break()
//                {
//                    Name = "Christmas Day",
//                    Date =  new DateTime(2020, 12, 25),
//                    Type =  DemographicType.One
//                 },
//                 new Break()
//                {
//                    Name = "Boxing Day",
//                    Date =  new DateTime(2020, 12, 26),
//                    Type =  DemographicType.One
//                 }
//            };
//        }

//        public static List<Break> GetHolidaysMultipleYears()
//        {
//            return new List<Break>() { new Break() { Date = new DateTime(2019, 12, 26), Type = DemographicType.One },
//                new Break() { Date = new DateTime(2020, 1, 1), Type = DemographicType.Two }};
//        }
//    }
//}

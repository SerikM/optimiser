using Optimiser.Models;
using System.Collections.Generic;
using System;
using DemographicType = Optimiser.Models.DemographicType;

namespace Optimiser.Tests
{
    public static class MockData
    {
        public static List<DataStructure> GetMockHolidays()
        {
            return new List<DataStructure>() {
                new DataStructure()
                {
                    Name = "New Years Day",
                    Date =  new DateTime(2020, 1, 1),
                    Type = DemographicType.Two
                },
                 new DataStructure()
                {
                    Name = "Australia Day",
                    Date =  new DateTime(2020, 1, 26),
                    Type = DemographicType.Two
                },
                 new DataStructure()
                {
                    Name = "Good Friday",
                    Date =  new DateTime(2020,4,19),
                    Type = DemographicType.Four
                },
                 new DataStructure()
                {
                    Name = "Eater Monday",
                    Date =  new DateTime(2020,4,22),
                    Type = DemographicType.Four
                },
                new DataStructure()
                {
                    Name = "Anzac Day",
                    Date =  new DateTime(2020, 4, 25),
                    Type =  DemographicType.One
                 },
                new DataStructure()
                {
                    Name = "Queens Birthday",
                    Date =  new DateTime(2020, 6, 8),
                    Type =  DemographicType.Three
                 },
                 new DataStructure()
                {
                    Name = "Labor Day",
                    Date =  new DateTime(2020, 10, 5),
                    Type =  DemographicType.Three
                 },
                 new DataStructure()
                {
                    Name = "Christmas Day",
                    Date =  new DateTime(2020, 12, 25),
                    Type =  DemographicType.One
                 },
                 new DataStructure()
                {
                    Name = "Boxing Day",
                    Date =  new DateTime(2020, 12, 26),
                    Type =  DemographicType.One
                 }
            };
        }

        public static List<DataStructure> GetHolidaysMultipleYears()
        {
            return new List<DataStructure>() { new DataStructure() { Date = new DateTime(2019, 12, 26), Type = DemographicType.One },
                new DataStructure() { Date = new DateTime(2020, 1, 1), Type = DemographicType.Two }};
        }
    }
}

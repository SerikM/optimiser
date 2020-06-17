//using Xunit;
//using Moq;
//using Optimiser.Services;
//using Optimiser.Models;
//using System;
//using System.Threading.Tasks;
//using DemographicType = Optimiser.Models.DemographicType;

//namespace Optimiser.Tests
//{
//    public class CalculationServicesTest
//    {
//        [Fact]
//        public void TestStartSatEndSunday()
//        {
//            var dataMock = new Mock<IDBDataService<IData>>();
//            var holidays = MockData.GetMockHolidays();
//            dataMock.Setup(p => p.GetDatedItems<DataStructure>(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(Task.FromResult(holidays));
//            var service = new CalculationService(dataMock.Object);
//            var result = (int)service.GetOptimiser("01/02/2020", "01/03/2020").Result;
//            Assert.Equal(20, result);
//        }

//        [Fact]
//        public void TestStartSundayEndSunday()
//        {
//            var dataMock = new Mock<IDBDataService<IData>>();
//            var holidays = MockData.GetMockHolidays();
//            dataMock.Setup(p => p.GetDatedItems<DataStructure>(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(Task.FromResult(holidays));
//            var service = new CalculationService(dataMock.Object);
//            var result = (int)service.GetOptimiser("02/02/2020", "01/03/2020").Result;
//            Assert.Equal(20, result);
//        }

//        [Fact]
//        public void TestStartMondayEndSunday()
//        {
//            var dataMock = new Mock<IDBDataService<IData>>();
//            var holidays = MockData.GetMockHolidays();
//            dataMock.Setup(p => p.GetDatedItems<DataStructure>(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(Task.FromResult(holidays));
//            var service = new CalculationService(dataMock.Object);
//            var result = (int)service.GetOptimiser("03/02/2020", "01/03/2020").Result;
//            Assert.Equal(19, result);
//        }

//        [Fact]
//        public void TestStartAfterEnd()
//        {
//            var dataMock = new Mock<IDBDataService<IData>>();
//            var holidays = MockData.GetMockHolidays();
//            dataMock.Setup(p => p.GetDatedItems<DataStructure>(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(Task.FromResult(holidays));
//            var service = new CalculationService(dataMock.Object);
//            var result = (int)service.GetOptimiser("03/04/2020", "01/03/2020").Result;
//            Assert.Equal(-1, result);
//        }

//        [Fact]
//        public void TestStartSameAsEnd()
//        {
//            var dataMock = new Mock<IDBDataService<IData>>();
//            var holidays = MockData.GetMockHolidays();
//            dataMock.Setup(p => p.GetDatedItems<DataStructure>(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(Task.FromResult(holidays));
//            var service = new CalculationService(dataMock.Object);
//            var result = (int)service.GetOptimiser("03/04/2020", "03/04/2020").Result;
//            Assert.Equal(-1, result);
//        }

//        [Fact]
//        public void TestEndOneDayAfterStart()
//        {
//            var dataMock = new Mock<IDBDataService<IData>>();
//            var holidays = MockData.GetMockHolidays();
//            dataMock.Setup(p => p.GetDatedItems<DataStructure>(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(Task.FromResult(holidays));
//            var service = new CalculationService(dataMock.Object);
//            var result = (int)service.GetOptimiser("03/04/2020", "04/04/2020").Result;
//            Assert.Equal(0, result);
//        }

//        [Fact]
//        public void TestEndTwoDaysAfterStart()
//        {
//            var dataMock = new Mock<IDBDataService<IData>>();
//            var holidays = MockData.GetMockHolidays();
//            dataMock.Setup(p => p.GetDatedItems<DataStructure>(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(Task.FromResult(holidays));
//            var service = new CalculationService(dataMock.Object);
//            var result = (int)service.GetOptimiser("02/04/2020", "04/04/2020").Result;
//            Assert.Equal(1, result);
//        }

//        [Fact]
//        public void TestEndMondayStartFridayWith1PublicHoliday()
//        {
//            var dataMock = new Mock<IDBDataService<IData>>();
//            var holidays = MockData.GetMockHolidays();
//            dataMock.Setup(p => p.GetDatedItems<DataStructure>(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(Task.FromResult(holidays));
//            var service = new CalculationService(dataMock.Object, new HolidaysCalculationService());
//            var result = (int)service.GetOptimiser("08/06/2020", "12/06/2020").Result;
//            Assert.Equal(3, result);
//        }

//        [Fact]
//        public void TestStartFridayEndMonday()
//        {
//            var dataMock = new Mock<IDBDataService<IData>>();
//            var holidays = MockData.GetMockHolidays();
//            dataMock.Setup(p => p.GetDatedItems<DataStructure>(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(Task.FromResult(holidays));
//            var service = new CalculationService(dataMock.Object, new HolidaysCalculationService());
//            var result = (int)service.GetOptimiser("12/06/2020", "15/06/2020").Result;
//            Assert.Equal(0, result);
//        }

//        [Fact]
//        public void TestVeryLongTimeSpan()
//        {
//            var dataMock = new Mock<IDBDataService<IData>>();
//            var holidays = MockData.GetMockHolidays();
//            dataMock.Setup(p => p.GetDatedItems<DataStructure>(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(Task.FromResult(holidays));
//            var service = new CalculationService(dataMock.Object, new HolidaysCalculationService());
//            var result = service.GetOptimiser("01/01/2019", "31/12/2019").Result;
//            Assert.Equal(251, result);
//            result = service.GetOptimiser("01/10/2010", "31/12/2019").Result;
//            Assert.Equal(2335, result);
//        }

//        [Fact]
//        public void TestIsTypeOneHolMatchWeekend()
//        {
//            var service = new HolidaysCalculationService();
//            var hol = new DataStructure() { Date = new DateTime(2020, 4, 25), Type = DemographicType.One };
//            var result = service.IsTypeOneMatch(hol, new DateTime(2020, 3, 25), new DateTime(2020, 5, 25));
//            Assert.False(result);
//        }

//        [Fact]
//        public void TestIsTypeOneHolMatchWeekday()
//        {
//            var service = new HolidaysCalculationService();
//            var hol = new DataStructure() { Date = new DateTime(2020, 1, 27), Type = DemographicType.One };
//            var result = service.IsTypeOneMatch(hol, new DateTime(2020, 1, 15), new DateTime(2020, 3, 12));
//            Assert.True(result);
//        }

//        [Fact]
//        public void TestIsTypeTwoMatch()
//        {
//            var service = new HolidaysCalculationService();
//            var hol = new DataStructure() { Date = new DateTime(2020, 05, 17), Type = DemographicType.Two };
//            var result = service.IsTypeTwoMatch(hol, new DateTime(2020, 05, 18), new DateTime(2020, 05, 25));
//            Assert.False(result);
//        }

//        [Fact]
//        public void TestIsTypeTwoMatch2()
//        {
//            var service = new HolidaysCalculationService();
//            var hol = new DataStructure() { Date = new DateTime(2020, 05, 17), Type = DemographicType.Two };
//            var result = service.IsTypeTwoMatch(hol, new DateTime(2020, 05, 17), new DateTime(2020, 05, 25));
//            Assert.True(result);
//        }

//        [Fact]
//        public void TestIsTypeThreeMatchQueensBirthday()
//        {
//            var service = new HolidaysCalculationService();
//            var hol = new DataStructure() { Date = new DateTime(2020, 06, 8), Type = DemographicType.Three };
//            var result = service.IsTypeThreeMatch(hol, new DateTime(2022, 05, 17), new DateTime(2022, 06, 25));
//            Assert.True(result);
//        }

//        [Fact]
//        public void TestIsTypeThreeMatchLaborDay()
//        {
//            var service = new HolidaysCalculationService();
//            var hol = new DataStructure() { Date = new DateTime(2020, 10, 5), Type = DemographicType.Three };
//            var result = service.IsTypeThreeMatch(hol, new DateTime(2021, 09, 25), new DateTime(2021, 10, 15));
//            Assert.True(result);
//        }

//        [Fact]
//        public void TestHandleMultipleJointYears()
//        {
//            var service = new HolidaysCalculationService();
//            var hols = MockData.GetHolidaysMultipleYears();
//            var result = service.HandleMultipleJointYears(new DateTime(2019, 12, 26), new DateTime(2020, 01, 10), hols);
//            Assert.Equal(1, result);
//        }

//        [Fact]
//        public void TestHandleMultipleJointYearsIncludeBoxingDay()
//        {
//            var service = new HolidaysCalculationService();
//            var hols = MockData.GetHolidaysMultipleYears();
//            var result = service.HandleMultipleJointYears(new DateTime(2019, 12, 25), new DateTime(2020, 01, 10), hols);
//            Assert.Equal(2, result);
//        }

//        [Fact]
//        public void TestHandleMultipleYears()
//        {
//            var service = new HolidaysCalculationService();
//            var hols = MockData.GetMockHolidays();
//            var result = service.HandleYears(new DateTime(2019, 02, 02), new DateTime(2021, 12, 29), hols);
//            Assert.Equal(20, result);
//        }

//        [Fact]
//        public void TestGetEasterSunday()
//        {
//            var service = new HolidaysCalculationService();
//            var result2018 = service.GetEasterSunday(2018);
//            var result2019 = service.GetEasterSunday(2019);
//            var result2020 = service.GetEasterSunday(2020);
//            var result2021 = service.GetEasterSunday(2021);
//            var result2022 = service.GetEasterSunday(2022);

//            Assert.Equal(new DateTime(2018,04,01), result2018);
//            Assert.Equal(new DateTime(2019, 04, 21), result2019);
//            Assert.Equal(new DateTime(2020, 04, 12), result2020);
//            Assert.Equal(new DateTime(2021, 04, 4), result2021);
//            Assert.Equal(new DateTime(2022, 04, 17), result2022);
//        }
//    }
//}

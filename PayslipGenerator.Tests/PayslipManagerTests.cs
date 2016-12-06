using System;
using System.Collections.Generic;
using Autofac.Features.Indexed;
using Moq;
using NUnit.Framework;
using PayslipGenerator.DataReader;
using PayslipGenerator.Lib;
using PayslipGenerator.Lib.Mapper;

namespace PayslipGenerator.Tests
{
    [TestFixture]
    public class PayslipManagerTests
    {
        [Test]
        public void CanProducePayslips()
        {
            var data = new List<InputData>
            {
                new InputData { FirstName = "Joe", LastName = "Bald", AnnualSalary = 100000, Superannuation = 10, StartDate = DateTime.Parse("01 March"), EndDate = DateTime.Parse("31 March") }
            };

            var mockReader = new Mock<ISalaryDataReader>();
            var mockIndexProvider = new Mock<IIndex<InputType, ISalaryDataReader>>();
            mockIndexProvider.SetupGet(p => p[InputType.Csv]).Returns(mockReader.Object);
            var mockMapper = new Mock<IDataMapper>();
            var producer = new Mock<IPayslipProducer>();
            var manager = new PayslipsManager(mockIndexProvider.Object, mockMapper.Object, producer.Object);

        }
    }
}

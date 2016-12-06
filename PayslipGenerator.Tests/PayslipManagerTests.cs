using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Features.Indexed;
using Moq;
using NUnit.Framework;
using PayslipGenerator.DataReader;
using PayslipGenerator.Lib;
using PayslipGenerator.Lib.Mapper;
using PayslipGenerator.Utils;

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

            const string filename = "A valid filename";
            var mockReader = new Mock<ISalaryDataReader>();
            mockReader.Setup(x => x.GetData(filename))
                .Returns(Response<IList<SalaryInfo>>.From(default(IList<SalaryInfo>)));
            var mockIndexProvider = new Mock<IIndex<InputType, ISalaryDataReader>>();
            mockIndexProvider.SetupGet(p => p[InputType.Csv]).Returns(mockReader.Object);
            var mockMapper = new Mock<IDataMapper>();
            mockMapper.Setup(x => x.Map<IList<SalaryInfo>, IList<InputData>>(It.IsAny<IList<SalaryInfo>>()))
                .Returns(Response<IList<InputData>>.From(data));

            var producer = new Mock<IPayslipProducer>();
            var manager = new PayslipsManager(mockIndexProvider.Object, mockMapper.Object, producer.Object);
            
            var request = new PayslipRequest {InputType = InputType.Csv, Filename = filename};
            var result = manager.Execute(request);

            Assert.AreEqual(ResponseCode.Ok, result.Code);
            producer.Verify(x => x.GenerateSlip(data.First()), Times.Once);
            mockMapper.Verify(x => x.Map<IList<SalaryInfo>, IList<InputData>>(It.IsAny<IList<SalaryInfo>>()), Times.Once);
        }
    }
}

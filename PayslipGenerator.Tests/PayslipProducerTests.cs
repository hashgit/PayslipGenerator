using System;
using System.Globalization;
using Moq;
using NUnit.Framework;
using PayslipGenerator.Lib;
using PayslipGenerator.Lib.Calculators;
using PayslipGenerator.Utils;

namespace PayslipGenerator.Tests
{
    [TestFixture]
    public class PayslipProducerTests
    {
        [TestCase("Joe", "Bald", 9, 60050, "01 March", "31 March")]
        [TestCase("Joe", "Bald", 9, 120000, "01 March", "31 March")]
        public void CanProduceAValidPayslip(string firstName, string lastName,
            decimal super, decimal annualSalary, string startDate, string endDate)
        {
            var sDate = DateTime.ParseExact(startDate, "dd MMMM", CultureInfo.InvariantCulture, DateTimeStyles.None);
            var eDate = DateTime.ParseExact(endDate, "dd MMMM", CultureInfo.InvariantCulture, DateTimeStyles.None);

            var data = new InputData
            {
                Superannuation = super,
                AnnualSalary = annualSalary,
                FirstName = firstName,
                LastName = lastName,
                StartDate = sDate,
                EndDate = eDate
            };

            var taxCalculatorMock = new Mock<ITaxCalculator>();
            taxCalculatorMock.Setup(x => x.Calculate(data)).Returns(10000);
            var superCalculatorMock = new Mock<ISuperCalculator>();
            superCalculatorMock.Setup(x => x.Calculate(data)).Returns(4000);

            var producer = new PayslipProducer(taxCalculatorMock.Object, superCalculatorMock.Object);
            var response = producer.GenerateSlip(data);

            Assert.IsNotNull(response);
            Assert.AreEqual(response.Code, ResponseCode.Ok);
            Assert.AreEqual(data.FirstName, response.Data.FirstName);
            Assert.AreEqual(data.LastName, response.Data.LastName);
            Assert.AreEqual(response.Data.GrossIncome, decimal.Round(data.AnnualSalary / 12));
            Assert.AreEqual(response.Data.NetIncome, decimal.Round(data.AnnualSalary / 12 - (10000m / 12)));
        }

        [TestCase("Joe", "", 9, 60050, "01 March", "31 March")]
        [TestCase("Joe", "Bald", 0, 120000, "01 March", "31 March")]
        [TestCase("Joe", "Bald", 60, 120000, "01 March", "31 March")]
        [TestCase("Joe", "Bald", 11, 0, "01 March", "31 March")]
        [TestCase("Joe", "Bald", 11, 60000, "01 March", "31 May")]
        [TestCase("Joe", "Bald", 11, 60000, "01 March", "20 March")]
        [TestCase("Joe", "Bald", 11, 60000, "02 March", "31 March")]
        public void CanGetErrorsForInvalidSalaryData(string firstName, string lastName,
            decimal super, decimal annualSalary, DateTime startDate, DateTime endDate)
        {
            var data = new InputData
            {
                Superannuation = super,
                AnnualSalary = annualSalary,
                FirstName = firstName,
                LastName = lastName,
                StartDate = startDate,
                EndDate = endDate
            };

            var taxCalculatorMock = new Mock<ITaxCalculator>();
            taxCalculatorMock.Setup(x => x.Calculate(data)).Returns(10000);
            var superCalculatorMock = new Mock<ISuperCalculator>();
            superCalculatorMock.Setup(x => x.Calculate(data)).Returns(4000);

            var producer = new PayslipProducer(taxCalculatorMock.Object, superCalculatorMock.Object);
            var response = producer.GenerateSlip(data);

            Assert.IsNotNull(response);
            Assert.AreEqual(ResponseCode.Error, response.Code);
        }
    }
}
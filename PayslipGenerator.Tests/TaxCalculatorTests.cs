using NUnit.Framework;
using PayslipGenerator.Lib;
using PayslipGenerator.Lib.Calculators;

namespace PayslipGenerator.Tests
{
    [TestFixture]
    public class TaxCalculatorTests
    {
        [TestCase(18205, 1)]
        [TestCase(5000, 0)]
        [TestCase(10000, 0)]
        [TestCase(180001, 54547)]
        [TestCase(60500, 11209)]
        public void CanCalculateCorrectTax(decimal annualSalary, decimal annualTax)
        {
            var calculator = new TaxCalculator();
            var tax = calculator.Calculate(new InputData { AnnualSalary = annualSalary });
            Assert.AreEqual(annualTax, tax);
        }
    }
}
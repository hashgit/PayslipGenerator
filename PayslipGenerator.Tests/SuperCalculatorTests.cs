using NUnit.Framework;
using PayslipGenerator.Lib;
using PayslipGenerator.Lib.Calculators;

namespace PayslipGenerator.Tests
{
    [TestFixture]
    public class SuperCalculatorTests
    {
        [TestCase(60050, 9, 5404)]
        public void CanCalculateSuperannuation(decimal annualSalary, decimal superRate, decimal super)
        {
            var superCalculator = new SuperCalculator();
            var superResult = superCalculator.Calculate(new InputData {AnnualSalary = annualSalary, Superannuation = superRate});
            Assert.AreEqual(super, superResult);
        }
    }
}
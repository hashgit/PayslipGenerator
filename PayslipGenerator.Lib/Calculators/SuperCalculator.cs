namespace PayslipGenerator.Lib.Calculators
{
    public class SuperCalculator : ISuperCalculator
    {
        public decimal Calculate(InputData data)
        {
            // We assume the super is always on top of the annual salary
            var super = decimal.Round(data.AnnualSalary*(data.Superannuation/100));
            return super;
        }
    }
}
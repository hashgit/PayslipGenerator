namespace PayslipGenerator.Lib.Calculators
{
    public class SuperCalculator : ISuperCalculator
    {
        public decimal Calculate(InputData data)
        {
            var super = decimal.Round(data.AnnualSalary*(data.Superannuation/100));
            return super;
        }
    }
}
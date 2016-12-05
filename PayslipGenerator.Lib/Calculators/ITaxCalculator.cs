namespace PayslipGenerator.Lib.Calculators
{
    public interface ITaxCalculator
    {
        decimal Calculate(InputData data);
    }
}
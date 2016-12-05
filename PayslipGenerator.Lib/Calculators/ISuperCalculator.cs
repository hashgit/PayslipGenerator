namespace PayslipGenerator.Lib.Calculators
{
    public interface ISuperCalculator
    {
        decimal Calculate(InputData data);
    }
}
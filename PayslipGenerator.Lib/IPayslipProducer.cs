using PayslipGenerator.Utils;

namespace PayslipGenerator.Lib
{
    /// <summary>
    /// Interface that exposes functionality to generate a single salary slip
    /// </summary>
    public interface IPayslipProducer
    {
        Response<SalarySlip> GenerateSlip(InputData salaryData);
    }
}
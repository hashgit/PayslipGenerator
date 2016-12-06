using System.Web;
using PayslipGenerator.Utils;

namespace PayslipGenerator.Lib
{
    public interface IPayslipProducer
    {
        Response<SalarySlip> GenerateSlip(InputData salaryData);
    }
}
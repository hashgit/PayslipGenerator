using PayslipGenerator.Utils;

namespace PayslipGenerator.Lib
{
    public interface IPayslipProducer
    {
        Response<bool> Execute(PayslipRequest request);
    }
}
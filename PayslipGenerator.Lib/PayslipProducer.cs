using PayslipGenerator.DataReader;

namespace PayslipGenerator.Lib
{
    public class PayslipProducer : IPayslipProducer
    {
        private readonly ISalaryDataReader _salaryDataReader;

        public PayslipProducer(ISalaryDataReader salaryDataReader)
        {
            _salaryDataReader = salaryDataReader;
        }
    }
}

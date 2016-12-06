using PayslipGenerator.Lib.Calculators;
using PayslipGenerator.Utils;

namespace PayslipGenerator.Lib
{
    public class PayslipProducer : IPayslipProducer
    {
        private readonly ITaxCalculator _taxCalculator;
        private readonly ISuperCalculator _superCalculator;

        public PayslipProducer(ITaxCalculator taxCalculator, ISuperCalculator superCalculator)
        {
            _taxCalculator = taxCalculator;
            _superCalculator = superCalculator;
        }

        public Response<SalarySlip> GenerateSlip(InputData salaryData)
        {
            var sanityErrors = ValidateInput(salaryData);
            if (sanityErrors != null) return sanityErrors;

            var annualTax = _taxCalculator.Calculate(salaryData);
            var annualSuper = _superCalculator.Calculate(salaryData);
            var monthlyTax = decimal.Round(annualTax/12);

            var salarySlip = new SalarySlip
            {
                FirstName = salaryData.FirstName,
                LastName = salaryData.LastName,
                Tax = monthlyTax,
                Super = decimal.Round(annualSuper / 12),
                GrossIncome = decimal.Round(salaryData.AnnualSalary / 12),
                PayPeriod = $"{salaryData.StartDate:dd MMMM} - {salaryData.EndDate:dd MMMM}",
            };

            salarySlip.NetIncome = salarySlip.GrossIncome - monthlyTax;
            return Response<SalarySlip>.From(salarySlip);
        }

        private Response<SalarySlip> ValidateInput(InputData salaryData)
        {
            if (salaryData == null)
                return Response<SalarySlip>.Error("Salary data not provided");

            if (string.IsNullOrWhiteSpace(salaryData.FirstName))
                return Response<SalarySlip>.Error($"{nameof(salaryData.FirstName)} not provided");

            if (string.IsNullOrWhiteSpace(salaryData.LastName))
                return Response<SalarySlip>.Error($"{nameof(salaryData.LastName)} not provided");

            if (salaryData.AnnualSalary <= 0)
                return Response<SalarySlip>.Error($"{nameof(salaryData.AnnualSalary)} should be greater than zero");

            if (salaryData.Superannuation < 9 || salaryData.Superannuation > 50)
                return Response<SalarySlip>.Error($"{salaryData.Superannuation} should be between 9-50% inclusive");

            if (salaryData.StartDate.Month != salaryData.EndDate.Month)
                return Response<SalarySlip>.Error($"Salary period should lie within a month");

            return null;
        }
    }
}
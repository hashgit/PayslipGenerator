using System;
using PayslipGenerator.Lib.Calculators;
using PayslipGenerator.Utils;

namespace PayslipGenerator.Lib
{
    /// <summary>
    /// Implementation of a single salary slip generator
    /// </summary>
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
            // First we make sure all data is correct as we expect
            var sanityErrors = ValidateInput(salaryData);
            if (sanityErrors != null) return sanityErrors;

            // calculate the salary slip data
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
            // sanity check
            if (salaryData == null)
                return Response<SalarySlip>.Error("Salary data not provided");

            // data which should be available as per requirements
            if (string.IsNullOrWhiteSpace(salaryData.FirstName))
                return Response<SalarySlip>.Error($"{nameof(salaryData.FirstName)} not provided");

            if (string.IsNullOrWhiteSpace(salaryData.LastName))
                return Response<SalarySlip>.Error($"{nameof(salaryData.LastName)} not provided");

            if (salaryData.AnnualSalary <= 0)
                return Response<SalarySlip>.Error($"{nameof(salaryData.AnnualSalary)} should be greater than zero");

            if (salaryData.Superannuation < 9 || salaryData.Superannuation > 50)
                return Response<SalarySlip>.Error($"{salaryData.Superannuation} should be between 9-50% inclusive");

            // TODO: This is an assumption
            // Pay period should fall within a month and should consist of one full month
            if (salaryData.StartDate.Month != salaryData.EndDate.Month)
                return Response<SalarySlip>.Error($"Salary period should lie within a single month");

            if (salaryData.StartDate.Day != 1)
                return Response<SalarySlip>.Error($"Salary period should start from the first of the month");

            if (salaryData.EndDate.Day != DateTime.DaysInMonth(salaryData.EndDate.Year, salaryData.EndDate.Month))
                return Response<SalarySlip>.Error($"Salary period should end at the last day of the month");

            return null;
        }
    }
}
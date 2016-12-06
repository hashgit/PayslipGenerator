using System;

namespace PayslipGenerator.Lib
{
    /// <summary>
    /// Transformed input data for ease of use
    /// </summary>
    public class InputData
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal AnnualSalary { get; set; }
        public decimal Superannuation { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
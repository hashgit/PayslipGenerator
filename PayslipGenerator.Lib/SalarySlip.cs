namespace PayslipGenerator.Lib
{
    public class SalarySlip
    {
        public decimal Tax { get; set; }
        public decimal Super { get; set; }
        public decimal GrossIncome { get; set; }
        public string PayPeriod { get; set; }
        public decimal NetIncome { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
    }
}
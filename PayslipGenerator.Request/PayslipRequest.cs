namespace PayslipGenerator.Request
{
    public class PayslipRequest : ICsvRequest
    {
        public string Filename { get; set; }
    }
}

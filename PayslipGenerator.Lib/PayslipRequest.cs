using PayslipGenerator.DataReader;

namespace PayslipGenerator.Lib
{
    /// <summary>
    /// Information that constitutes a request
    /// </summary>
    public class PayslipRequest
    {
        public InputType InputType { get; set; }

        // A cvs file
        public string Filename { get; set; }
    }
}
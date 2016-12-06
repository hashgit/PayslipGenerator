using System.Collections.Generic;
using PayslipGenerator.Utils;

namespace PayslipGenerator.Lib
{
    /// <summary>
    /// interface that exposes functionality to generate list of salary slips given a request object
    /// </summary>
    public interface IPayslipsManager
    {
        Response<IList<Response<SalarySlip>>> Execute(PayslipRequest request);
    }
}
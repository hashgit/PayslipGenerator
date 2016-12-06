using System.Collections.Generic;
using PayslipGenerator.Utils;

namespace PayslipGenerator.Lib
{
    public interface IPayslipsManager
    {
        Response<IList<Response<SalarySlip>>> Execute(PayslipRequest request);
    }
}
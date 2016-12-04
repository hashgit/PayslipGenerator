using System.Collections.Generic;
using PayslipGenerator.Utils;

namespace PayslipGenerator.DataReader
{
    public interface ISalaryDataReader
    {
        Response<IList<SalaryInfo>> GetData(string fileName);
    }
}
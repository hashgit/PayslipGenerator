using PayslipGenerator.Utils;

namespace PayslipGenerator.Lib.Mapper
{
    public interface IDataMapper
    {
        Response<TOut> Map<TIn, TOut>(TIn data);
    }
}
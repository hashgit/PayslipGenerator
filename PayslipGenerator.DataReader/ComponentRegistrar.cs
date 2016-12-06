using Autofac;

namespace PayslipGenerator.DataReader
{
    public static class ComponentRegistrar
    {
        public static void RegisterComponents(ContainerBuilder builder)
        {
            builder.RegisterType<CsvSalaryDataReader>().Keyed<ISalaryDataReader>(InputType.Csv);
        }
    }
}
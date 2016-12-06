using Autofac;
using PayslipGenerator.Lib.Calculators;
using PayslipGenerator.Lib.Mapper;

namespace PayslipGenerator.Lib
{
    /// <summary>
    // ReSharper disable once CommentTypo
    /// Use this class to add components from this library into your Autofac container
    /// </summary>
    public static class ComponentRegistrar
    {
        public static void RegisterComponents(ContainerBuilder builder)
        {
            builder.RegisterType<PayslipProducer>().As<IPayslipProducer>();
            builder.RegisterType<PayslipsManager>().As<IPayslipsManager>();
            builder.RegisterType<DataReaderToInputData>().As<IDataMapper>();
            builder.RegisterType<SuperCalculator>().As<ISuperCalculator>();
            builder.RegisterType<TaxCalculator>().As<ITaxCalculator>();

            DataReader.ComponentRegistrar.RegisterComponents(builder);
        }
    }
}
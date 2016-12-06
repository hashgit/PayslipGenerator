using System;
using System.IO;
using Autofac;
using PayslipGenerator.DataReader;
using PayslipGenerator.Lib;
using PayslipGenerator.Utils;
using ComponentRegistrar = PayslipGenerator.Lib.ComponentRegistrar;

namespace PayslipGenerator
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            ComponentRegistrar.RegisterComponents(builder);
            var container = builder.Build();

            var payslipManager = container.Resolve<IPayslipsManager>();

            var fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData", "Data1.txt");
            var result = payslipManager.Execute(new PayslipRequest { InputType = InputType.Csv, Filename = fileName });

            // Display the results in the console for a quick view
            if (result.Code == ResponseCode.Ok)
            {
                // correctly generated salary slips will be displayed in a coma separated CSV format
                // errors will be displayed verbatim
                result.Data.Foreach(d =>
                {
                    if (d.Code == ResponseCode.Error)
                    {
                        Console.WriteLine(d.Message);
                    }
                    else
                    {
                        Console.WriteLine($"{d.Data.FirstName},{d.Data.LastName},{d.Data.PayPeriod},{d.Data.GrossIncome},{d.Data.Tax},{d.Data.NetIncome},{d.Data.Super}");
                    }
                });
            }
            else
            {
                Console.WriteLine(result.Message);
            }

            Console.ReadKey();
        }
    }
}

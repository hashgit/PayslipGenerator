using System;
using PayslipGenerator.Lib;

namespace PayslipGenerator
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            ComponentRegistrar.RegisterComponents();

            Console.ReadKey();
        }
    }
}

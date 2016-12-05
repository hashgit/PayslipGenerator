using System;
using System.Globalization;
using AutoMapper;
using PayslipGenerator.DataReader;

namespace PayslipGenerator.Lib.Mapper
{
    public class DataReaderToInputData : IDataMapper
    {
        public MapperConfiguration Configure()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<SalaryInfo, InputData>()
                .ForMember(d => d.AnnualSalary, opt => opt.ResolveUsing<StringToDecimalResolver, string>(s => s.AnnualSalary))
                .ForMember(d => d.Superannuation, opt => opt.ResolveUsing<StringToDecimalResolver, string>(s => s.Superannuation))
                .ForMember(d => d.StartDate, opt => opt.ResolveUsing<StringToStartDateResolver, string>(s => s.Period))
                .ForMember(d => d.EndDate, opt => opt.ResolveUsing<StringToEndDateResolver, string>(s => s.Period)));

            return config;
        }

        public class StringToStartDateResolver : IMemberValueResolver<SalaryInfo, InputData, string, DateTime>
        {
            public DateTime Resolve(SalaryInfo source, InputData destination, string sourceMember, DateTime destMember,
                ResolutionContext context)
            {
                if (string.IsNullOrWhiteSpace(sourceMember))
                    throw new ArgumentException("Salary period is required");

                var parts = sourceMember.Split(new[] {"-"}, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length < 1)
                    throw new FormatException($"Invalid salary period '{sourceMember}'");

                DateTime value;
                if (DateTime.TryParseExact(parts[0].Trim(), "DD Mon", CultureInfo.InvariantCulture, DateTimeStyles.None,
                    out value))
                    return value;

                throw new FormatException($"Invalid salary period '{sourceMember}'");
            }
        }

        public class StringToEndDateResolver : IMemberValueResolver<SalaryInfo, InputData, string, DateTime>
        {
            public DateTime Resolve(SalaryInfo source, InputData destination, string sourceMember, DateTime destMember,
                ResolutionContext context)
            {
                if (string.IsNullOrWhiteSpace(sourceMember))
                    throw new ArgumentException("Salary period is required");

                var parts = sourceMember.Split(new[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length < 2)
                    throw new FormatException($"Invalid salary period '{sourceMember}'");

                DateTime value;
                if (DateTime.TryParseExact(parts[1].Trim(), "DD Mon", CultureInfo.InvariantCulture, DateTimeStyles.None,
                    out value))
                    return value;

                throw new FormatException($"Invalid salary period '{sourceMember}'");
            }
        }

        public class StringToDecimalResolver : IMemberValueResolver<SalaryInfo, InputData, string, decimal>
        {
            public decimal Resolve(SalaryInfo source, InputData destination, string sourceMember, decimal destMember,
                ResolutionContext context)
            {
                decimal value;
                if (decimal.TryParse(sourceMember, out value))
                    return value;

                throw new FormatException($"{sourceMember} is not a valid decimal value.");
            }
        }
    }
}
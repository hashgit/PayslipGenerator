using System;
using System.Globalization;
using AutoMapper;
using PayslipGenerator.DataReader;
using PayslipGenerator.Utils;

namespace PayslipGenerator.Lib.Mapper
{
    public class DataReaderToInputData : IDataMapper
    {
        private MapperConfiguration _configuration;

        public Response<TOut> Map<TIn, TOut>(TIn data)
        {
            var mappingConfig = Configure();
            var mapper = mappingConfig.CreateMapper();
            try
            {
                var inputs = mapper.Map<TOut>(data);
                return Response<TOut>.From(inputs);
            }
            catch (Exception e)
            {
                return Response<TOut>.Error(e.Message);
            }
        }

        private MapperConfiguration Configure()
        {
            if (_configuration == null)
            {
                _configuration = new MapperConfiguration(cfg => cfg.CreateMap<SalaryInfo, InputData>()
                    .ForMember(d => d.AnnualSalary, opt => opt.ResolveUsing<StringToDecimalResolver, string>(s => s.AnnualSalary))
                    .ForMember(d => d.Superannuation, opt => opt.ResolveUsing<StringToDecimalResolver, string>(s => s.Superannuation))
                    .ForMember(d => d.StartDate, opt => opt.ResolveUsing<StringToStartDateResolver, string>(s => s.Period))
                    .ForMember(d => d.EndDate, opt => opt.ResolveUsing<StringToEndDateResolver, string>(s => s.Period)));
            }

            return _configuration;
        }

        public abstract class StringToDateResolver : IMemberValueResolver<SalaryInfo, InputData, string, DateTime>
        {
            protected abstract int GetDatePart();
            public DateTime Resolve(SalaryInfo source, InputData destination, string sourceMember, DateTime destMember,
                ResolutionContext context)
            {
                if (string.IsNullOrWhiteSpace(sourceMember))
                    throw new ArgumentException("Salary period is required");

                var parts = sourceMember.Split(new[] { "–", "-" }, StringSplitOptions.RemoveEmptyEntries);
                var datePart = GetDatePart();
                if (parts.Length < datePart)
                    throw new FormatException($"Invalid salary period '{sourceMember}'");

                DateTime value;
                if (DateTime.TryParseExact(parts[datePart-1].Trim(), "dd MMMM", CultureInfo.InvariantCulture, DateTimeStyles.None,
                    out value))
                    return value;

                throw new FormatException($"Invalid salary period '{sourceMember}'");
            }
        }

        public class StringToStartDateResolver : StringToDateResolver
        {
            protected override int GetDatePart()
            {
                return 1;
            }
        }

        public class StringToEndDateResolver : StringToDateResolver
        {
            protected override int GetDatePart()
            {
                return 2;
            }
        }

        public class StringToDecimalResolver : IMemberValueResolver<SalaryInfo, InputData, string, decimal>
        {
            public decimal Resolve(SalaryInfo source, InputData destination, string sourceMember, decimal destMember,
                ResolutionContext context)
            {
                decimal value;
                if (decimal.TryParse(sourceMember.TrimEnd('%', ' '), out value))
                    return value;

                throw new FormatException($"'{sourceMember}' is not a valid decimal value.");
            }
        }
    }
}
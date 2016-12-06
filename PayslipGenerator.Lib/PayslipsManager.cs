using System.Collections.Generic;
using System.Linq;
using Autofac.Features.Indexed;
using PayslipGenerator.DataReader;
using PayslipGenerator.Lib.Mapper;
using PayslipGenerator.Utils;

namespace PayslipGenerator.Lib
{
    /// <summary>
    /// Implementation of a payslip manager
    /// Reads data using ISalaryDataReader and generates salary slip for each data object
    /// using IPayslipProducer
    /// </summary>
    public class PayslipsManager : IPayslipsManager
    {
        private readonly IIndex<InputType, ISalaryDataReader> _salaryDataReader;
        private readonly IDataMapper _dataMapper;
        private readonly IPayslipProducer _payslipProducer;

        public PayslipsManager(IIndex<InputType, ISalaryDataReader> salaryDataReader, IDataMapper dataMapper, IPayslipProducer payslipProducer)
        {
            _salaryDataReader = salaryDataReader;
            _dataMapper = dataMapper;
            _payslipProducer = payslipProducer;
        }

        public Response<IList<Response<SalarySlip>>> Execute(PayslipRequest request)
        {
            // sanity checks
            if (request.InputType == InputType.Unknown)
                return Response<IList<Response<SalarySlip>>>.Error("Unknown input data type");

            // Get input data and transform it into a more useful information
            var inputDataResponse = GetMappedInputData(request);
            if (inputDataResponse.Code == ResponseCode.Error)
                return Response<IList<Response<SalarySlip>>>.Error(inputDataResponse.Message);

            var salarySlips = new List<Response<SalarySlip>>();
            // process all salary data one by one, write an error for invalid data
            if (inputDataResponse.Data != null)
            {
                salarySlips.AddRange(inputDataResponse
                    .Data.Select(inputData => _payslipProducer.GenerateSlip(inputData)));
            }

            return Response<IList<Response<SalarySlip>>>.From(salarySlips);
        }

        private Response<IList<InputData>> GetMappedInputData(PayslipRequest request)
        {
            // we use a data reader based on the input request
            var dataReader = _salaryDataReader[request.InputType];
            var info = dataReader.GetData(request.Filename);
            if (info.Code == ResponseCode.Error)
                return Response<IList<InputData>>.Error(info.Message);

            // transform the input data into a meaningful form
            var input = _dataMapper.Map<IList<SalaryInfo>, IList<InputData>>(info.Data);
            return input;
        }
    }
}

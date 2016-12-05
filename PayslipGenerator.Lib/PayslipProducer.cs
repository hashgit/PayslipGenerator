using System;
using System.Collections.Generic;
using Autofac.Features.Indexed;
using PayslipGenerator.DataReader;
using PayslipGenerator.Lib.Mapper;
using PayslipGenerator.Utils;

namespace PayslipGenerator.Lib
{
    public class PayslipProducer : IPayslipProducer
    {
        private readonly IIndex<InputType, ISalaryDataReader> _salaryDataReader;
        private readonly IDataMapper _dataMapper;

        public PayslipProducer(IIndex<InputType, ISalaryDataReader> salaryDataReader, IDataMapper dataMapper)
        {
            _salaryDataReader = salaryDataReader;
            _dataMapper = dataMapper;
        }

        public Response<bool> Execute(PayslipRequest request)
        {
            if (request.InputType == InputType.Unknown)
                return Response<bool>.Error("Unknown input data type");

            var inputDataResponse = GetMappedInputData(request);
            if (inputDataResponse.Code == ResponseCode.Error)
                return Response<bool>.Error(inputDataResponse.Message);

            // process all salary data one by one, write an error for invalid data
            if (inputDataResponse.Data != null)
            {
                foreach (var inputData in inputDataResponse.Data)
                {
                    
                }
            }

            return Response<bool>.From(true);
        }

        private Response<IList<InputData>> GetMappedInputData(PayslipRequest request)
        {
            var dataReader = _salaryDataReader[request.InputType];
            var info = dataReader.GetData(request.Filename);

            var mappingConfig = _dataMapper.Configure();
            var mapper = mappingConfig.CreateMapper();
            try
            {
                var inputs = mapper.Map<IList<InputData>>(info);
                return Response<IList<InputData>>.From(inputs);
            }
            catch (Exception e)
            {
                return Response<IList<InputData>>.Error(e.Message);
            }
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Features.Indexed;
using PayslipGenerator.DataReader;
using PayslipGenerator.Lib.Mapper;
using PayslipGenerator.Utils;

namespace PayslipGenerator.Lib
{
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
            if (request.InputType == InputType.Unknown)
                return Response<IList<Response<SalarySlip>>>.Error("Unknown input data type");

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

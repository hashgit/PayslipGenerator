using System;
using System.Collections.Generic;
using NUnit.Framework;
using PayslipGenerator.DataReader;
using PayslipGenerator.Lib;
using PayslipGenerator.Lib.Mapper;
using PayslipGenerator.Utils;

namespace PayslipGenerator.Tests
{
    [TestFixture]
    public class MapperTests
    {
        [TestCase("TestData\\Data1.txt", 2)]
        public void CanMapDataCorrectly(string filename, int rowCount)
        {
            var reader = new CsvSalaryDataReader();
            var dataResponse = reader.GetData($"{AppDomain.CurrentDomain.BaseDirectory}\\{filename}");
            Assert.AreEqual(ResponseCode.Ok, dataResponse.Code);
            Assert.AreEqual(rowCount, dataResponse.Data.Count);

            var mapper = new DataReaderToInputData();
            var inputData = mapper.Map<IList<SalaryInfo>, IList<InputData>>(dataResponse.Data);
            Assert.AreEqual(ResponseCode.Ok, inputData.Code);
            Assert.AreEqual(rowCount, inputData.Data.Count);
        }

        [TestCase("TestData\\Data6.txt", 1)]
        public void CanErrorOnBadData(string filename, int rowCount)
        {
            var reader = new CsvSalaryDataReader();
            var dataResponse = reader.GetData($"{AppDomain.CurrentDomain.BaseDirectory}\\{filename}");
            Assert.AreEqual(ResponseCode.Ok, dataResponse.Code);
            Assert.AreEqual(rowCount, dataResponse.Data.Count);

            var mapper = new DataReaderToInputData();
            var inputData = mapper.Map<IList<SalaryInfo>, IList<InputData>>(dataResponse.Data);
            Assert.AreEqual(ResponseCode.Error, inputData.Code);
        }
    }
}
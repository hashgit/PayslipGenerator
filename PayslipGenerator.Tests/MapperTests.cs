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
            var config = mapper.Configure();
            var automapper = config.CreateMapper();
            var inputData = automapper.Map<IList<InputData>>(dataResponse.Data);
            Assert.AreEqual(rowCount, inputData.Count);
        }
    }
}
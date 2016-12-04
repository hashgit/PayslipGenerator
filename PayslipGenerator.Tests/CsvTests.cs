using System;
using NUnit.Framework;
using PayslipGenerator.DataReader;
using PayslipGenerator.Utils;

namespace PayslipGenerator.Tests
{
    [TestFixture]
    public class CsvTests
    {
        [TestCase("A bad file")]
        [TestCase("A Non Existing Csv File")]
        public void GetsErrorForNonExistingCsvFile(string fileName)
        {
            var reader = new CsvSalaryDataReader();
            var dataResponse = reader.GetData(fileName);
            Assert.AreEqual(ResponseCode.Error, dataResponse.Code);
            Assert.IsTrue(dataResponse.Message.StartsWith("Could not find file"));
        }

        [TestCase("TestData\\Data1.txt", 2, "David", "Ryan")]
        public void GetsDataForExistingCsvFile(string fileName, int rowCount, string name1, string name2)
        {
            var reader = new CsvSalaryDataReader();
            var dataResponse = reader.GetData($"{AppDomain.CurrentDomain.BaseDirectory}\\{fileName}");
            Assert.AreEqual(ResponseCode.Ok, dataResponse.Code);
            Assert.AreEqual(rowCount, dataResponse.Data.Count);
            Assert.AreEqual(name1, dataResponse.Data[0].FirstName);
            Assert.AreEqual(name2, dataResponse.Data[1].FirstName);
        }

        [TestCase("TestData\\Data5.txt", 2, "David", "Ryan")]
        [TestCase("TestData\\Data4.txt", 3, "David", "Ryan", "Chen")]
        [TestCase("TestData\\Data3.txt", 0)]
        [TestCase("TestData\\Data2.txt", 3, "David", "Ryan", "Chen")]
        public void GetsDataForBadCsvFile(string fileName, int rowCount, params string[] args)
        {
            var reader = new CsvSalaryDataReader();
            var dataResponse = reader.GetData($"{AppDomain.CurrentDomain.BaseDirectory}\\{fileName}");
            Assert.AreEqual(ResponseCode.Ok, dataResponse.Code);
            Assert.AreEqual(rowCount, dataResponse.Data.Count);

            for (var i = 0; i < args.Length; i++)
            {
                Assert.AreEqual(args[i], dataResponse.Data[i].FirstName);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using PayslipGenerator.Utils;

namespace PayslipGenerator.DataReader
{
    public class CsvSalaryDataReader : ISalaryDataReader
    {
        public Response<IList<SalaryInfo>> GetData(string fileName)
        {
            TextReader fileHandle;
            try
            {
                fileHandle = File.OpenText(fileName);
            }
            catch (FileNotFoundException e)
            {
                return Response<IList<SalaryInfo>>.Error(e.Message);
            }
            catch (Exception e)
            {
                return Response<IList<SalaryInfo>>.Error(e.Message);
            }

            // We read all data here and let the library decide how to deal with bad data
            var reader = new CsvReader(fileHandle,
                new CsvConfiguration
                {
                    HasHeaderRecord = false,
                    DetectColumnCountChanges = false,
                    ThrowOnBadData = false,
                    SkipEmptyRecords = true,
                    WillThrowOnMissingField = false,
                    IgnoreBlankLines = true,
                    IgnoreReadingExceptions = true
                });

            var data = new List<SalaryInfo>();
            while (reader.Read())
            {
                data.Add(reader.GetRecord<SalaryInfo>());
            }

            return Response<IList<SalaryInfo>>.From(data);
        }
    }
}

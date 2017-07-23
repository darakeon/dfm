using System;
using System.Collections.Generic;
using FileExchange.Importer;

namespace FileExchange.Domain
{
    public class DataControl
    {
        public Int32 ID { get; set; }

        public String FileName { get; set; }
        public DateTime Import { get; set; }
        public String User { get; set; }

        public FileStatus Status { get; set; }

        public void MakeRecords()
        {
            var importer = BaseImporter.GetImporter(FileName);

            TestColumns(importer.Columns);

            for(var r = 0; r < importer.RowCount; r++)
            {
                var record = new Record(importer, r);
            }
        }


        private void TestColumns(IList<String> columns)
        {
            if (!columns.Contains("AccountIn"))
                throw new FileExchangeException();

            if (!columns.Contains("AccountOut"))
                throw new FileExchangeException();

            if (!columns.Contains("Category"))
                throw new FileExchangeException();

            if (!columns.Contains("Description"))
                throw new FileExchangeException();

            if (!columns.Contains("Date"))
                throw new FileExchangeException();

            if (!columns.Contains("Value"))
                throw new FileExchangeException();
        }

    }

}

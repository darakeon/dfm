using System;
using System.Collections.Generic;

namespace FileExchange.Importer
{
    public class MoneyImporter : IImporter
    {
        private MoneyImporter()
        {
            
        }

        public MoneyImporter(String fullFilePath) : this()
        {
            //var csv = new CsvStream()
        }


        public string this[string columnName, int rowIndex]
        {
            get { throw new NotImplementedException(); }
        }

        public int RowCount
        {
            get { throw new NotImplementedException(); }
        }

        public IList<string> Columns
        {
            get { throw new NotImplementedException(); }
        }

    }
}

using System;

namespace FileExchange.Importer
{
    public class XmlImporter : IImporter
    {
        private XmlImporter()
        {
            
        }

        public XmlImporter(String fullFilePath) : this()
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

        public bool IsFilled(string columnName, int rowIndex)
        {
            throw new NotImplementedException();
        }
    }
}

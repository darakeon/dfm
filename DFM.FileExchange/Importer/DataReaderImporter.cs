using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FileExchange.Importer
{
    public class DataReaderImporter : IImporter
    {
        protected DataReaderImporter()
        {
            data = new Dictionary<String, IList<String>>();
            Columns = new List<String>();
        }


        
        private readonly IDictionary<String, IList<String>> data;
        public IList<String> Columns { get; private set; }



        protected void Read(IDataReader dr)
        {
            createColumns(dr);

            var isLastFilled = insertData(dr);

            if (!isLastFilled)
                cleanLastLine();
        }



        private Boolean insertData(IDataReader dr)
        {
            var hasNext = true;

            while (dr.Read() && hasNext)
            {
                hasNext = false;

                for (var i = 0; i < data.Count; i++)
                {
                    var name = dr.GetName(i);
                    var value = dr[i].ToString();

                    if (!String.IsNullOrEmpty(value))
                        hasNext = true;

                    data[name].Add(value);
                }

                RowCount++;
            }

            return hasNext;
        }



        private void createColumns(IDataReader dr)
        {
            for (var c = 0; c < dr.FieldCount; c++)
            {
                var name = dr.GetName(c);

                Columns.Add(name);
                data.Add(name, new List<String>());
            }
        }



        private void cleanLastLine()
        {
            for (var c = 0; c < data.Count; c++)
            {
                data[Columns[c]].Remove(data[Columns[c]].Last());
            }
        }



        public String this[String columnName, Int32 rowIndex]
        {
            get
            {
                return RowCount > rowIndex
                        && Columns.Contains(columnName)
                    ? data[columnName][rowIndex]
                    : null;
            }
        }



        public Int32 RowCount { get; private set; }

        public Boolean IsFilled(String columnName, Int32 rowIndex)
        {
            return !String.IsNullOrEmpty(this[columnName, rowIndex]);
        }

    }
}

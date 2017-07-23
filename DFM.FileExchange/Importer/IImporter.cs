using System;
using System.Collections.Generic;
using System.IO;

namespace FileExchange.Importer
{
    public interface IImporter
    {
        String this[String columnName, Int32 rowIndex] { get; }
        Int32 RowCount { get; }
        IList<String> Columns { get; }
    }

    public class BaseImporter
    {
        public static IImporter GetImporter(String fullFileName)
        {
            var info = new FileInfo(fullFileName);

            switch (info.Extension)
            {
                case "xls":
                    return new ExcelImporter(fullFileName);
                case "csv":
                    return new CsvImporter(fullFileName);
                case "xml":
                    return new XmlImporter(fullFileName);
                case "mny":
                    return new MoneyImporter(fullFileName);
            }
        }

    }
}
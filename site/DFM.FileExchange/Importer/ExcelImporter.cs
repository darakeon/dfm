using System;
using System.Data.Common;

namespace FileExchange.Importer
{
    public class ExcelImporter : DataReaderImporter
    {
        public ExcelImporter(String fullFilePath)
        {
            var factory = DbProviderFactories.GetFactory("System.Data.OleDb");

            using (var excelConnection = factory.CreateConnection())
            {
                var command = connect(excelConnection, fullFilePath);

                using (var dr = command.ExecuteReader())
                {
                    Read(dr);
                }
            }
        }


        private static DbCommand connect(DbConnection excelConnection, String fullFilePath)
        {
            if (excelConnection == null)
                throw new Exception("File not found");

            var connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fullFilePath + ";Extended Properties='Excel 8.0;HDR=Yes;'";
            excelConnection.ConnectionString = connectionString;
            excelConnection.Open();

            var command = excelConnection.CreateCommand();
            var dataTable = excelConnection.GetSchema("Tables");
            var sheetName = dataTable.Rows[0]["TABLE_NAME"].ToString();

            command.CommandText = "SELECT * FROM [" + sheetName + "]";
            return command;
        }

    }
}

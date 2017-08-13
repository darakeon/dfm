using System;
using System.Data;
using System.Data.Odbc;

namespace FileExchange.Importer
{
    public class CsvImporter : DataReaderImporter
    {
        public CsvImporter(String fullFilePath)
        {
            using (var conn = getConnection(fullFilePath))
            {
                using (var cmd = getCommand(conn))
                {
                    conn.Open();

                    using (var dr = getDataReader(cmd))
                    {
                        while (dr.Read())
                        {
                            Read(dr);
                        }
                    }
                }
            }
        }

        private static OdbcConnection getConnection(string fullFilePath)
        {
            return new OdbcConnection("Driver={Microsoft Text Driver (*.txt; *.csv)};Dbq=" + fullFilePath + ";Extensions=asc,csv,tab,txt");
        }

        private static OdbcCommand getCommand(OdbcConnection conn)
        {
            return new OdbcCommand("SELECT * FROM verylarge.csv", conn);
        }

        private static OdbcDataReader getDataReader(OdbcCommand cmd)
        {
            return cmd.ExecuteReader(CommandBehavior.SequentialAccess);
        }

    }
}

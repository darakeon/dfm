using System;
using System.Data;

namespace DFM.PageLog
{
    static class CommandExtension
    {
        public static void AddParameter(this IDbCommand command, String name, object value)
        {
            var parameter = command.CreateParameter();

            parameter.ParameterName = name;
            parameter.Value = value;

            command.Parameters.Add(parameter);
        }

    }
}

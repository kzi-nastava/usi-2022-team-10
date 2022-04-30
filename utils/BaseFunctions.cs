﻿using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCareInfromationSystem.utils
{
    public class BaseFunctions
    {
        public static string GenerateId(string table, string idName)
        {
            using (OleDbConnection connection = new OleDbConnection(Constants.connectionString))
            {
                Random random = new Random();
                string id;
                connection.Open();
                while (true)
                {
                    id = random.Next(1, 1000).ToString();
                    string query = $"select * from {table} where {idName} = \"{id}\"";
                    OleDbCommand command = new OleDbCommand(query, connection);
                    OleDbDataReader reader = command.ExecuteReader();
                    if (!reader.HasRows)
                        break;
                }
                return id;
            }
        }
    }
}

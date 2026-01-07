using ArchivingTool.Model.Arms;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace ArchivingTool.Service.Arms.Services.Common
{
    public class ExportService
    {
        public List<Dictionary<string, object>> ExportData(string sqlConnectionString, string storedProcedureName, Guid agencyKey)
        {
            var records = new List<Dictionary<string, object>>();

            using (var connection = new SqlConnection(sqlConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 180;
                    command.Parameters.AddWithValue("@AgencyKey", agencyKey);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var json = reader.GetString(0);
                            if (!string.IsNullOrWhiteSpace(json))
                            {
                                var record = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                                records.Add(record);
                            }
                        }
                    }
                }
            }

            return records;
        }

        public List<Dictionary<string, object>> ExportData(
    string connectionString,
    string storedProcedureName,
    Guid agencyKey,
    int pageNumber,
    int pageSize)
        {
            var result = new List<Dictionary<string, object>>();

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(storedProcedureName, connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@AgencyKey", agencyKey);
                command.Parameters.AddWithValue("@PageNumber", pageNumber);
                command.Parameters.AddWithValue("@PageSize", pageSize);

                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read()) 
                    {
                        var rawJson = reader.IsDBNull(0) ? null : reader.GetString(0);

                        if (!string.IsNullOrWhiteSpace(rawJson))
                        {
                            // Deserialize single object per row into a dictionary
                            var caseDict = Newtonsoft.Json.JsonConvert
                                .DeserializeObject<Dictionary<string, object>>(rawJson);

                            if (caseDict != null)
                                result.Add(caseDict);
                        }
                    }
                }
            }

            return result;
        }

    }

}

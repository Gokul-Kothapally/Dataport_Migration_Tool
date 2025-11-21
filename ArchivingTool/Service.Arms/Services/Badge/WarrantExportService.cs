using ArchivingTool.Model.Arms;
using ArchivingTool.Models;
using ArchivingTool.Service.Arms.Services.Common;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using Newtonsoft.Json;

namespace ArchivingTool.Service.Arms.Services.Badge
{
    public class WarrantExportService
    {

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
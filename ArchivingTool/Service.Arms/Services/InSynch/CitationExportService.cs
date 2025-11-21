using ArchivingTool.Model.Arms;
using ArchivingTool.Models;
using ArchivingTool.Service.Arms.Services.Common;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace ArchivingTool.Service.Arms.Services.InSynch
{
    public class CitationExportService
    {
        public List<CitationImportRequest> ExportData(string sqlConnectionString, string storedProcedureName, Guid agencyKey)
        {
            var records = new List<CitationImportRequest>();

            using (var connection = new SqlConnection(sqlConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@AgencyKey", agencyKey);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var record = new CitationImportRequest
                            {
                                Citation = reader["Citation"]?.ToString(),
                                Data = new Dictionary<string, object>()
                            };

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                if (columnName != "Citation") 
                                {
                                    record.Data[columnName] = reader[i]?.ToString();
                                }
                            }

                            records.Add(record);
                        }
                    }
                }
            }

            return records;
        }
    }

}

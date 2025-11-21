using ArchivingTool.Model.Arms;
using ArchivingTool.Models;
using ArchivingTool.Service.Arms.Services.Common;
using MongoDB.Bson;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Form1 = System.Windows.Forms.Form;
namespace ArchivingTool.Service.Arms.Services.LawTrak
{
    public class EvidenceImportService
    {

        private readonly HttpClientService _httpClientService;

        public EvidenceImportService()
        {
            _httpClientService = new HttpClientService();
        }

        public async Task<(bool Success, System.Net.HttpStatusCode? StatusCode, string Message)>
ImportEvidencesAsync(
    string apiToken,
    Dictionary<string, object> evidence,
    Guid? historyId = null,
    string baseFolderPath = "",
    Guid agencyKey = default)
        {
            if (evidence == null || evidence.Count == 0)
                return (false, null, "No data");

            var attachmentService = new AttachmentService(_httpClientService);
            var module = "Evidences";

            var evidenceJson = JObject.FromObject(evidence);

            string evidenceType = evidenceJson["Document"]?["CitationsData"]?["Citation"]?.ToString() ?? "";

            // deciding which field to use
            string evidenceNumber;
            if (evidenceType == "Main Entry")
            {
                evidenceNumber = evidenceJson["Document"]?["EvidencesMainData"]?["Incident"]?.ToString() ?? "Unknown";
            }
            else if (evidenceType == "Lost And Found")
            {
                evidenceNumber = evidenceJson["Document"]?["LostandFoundsData"]?["Control"]?.ToString() ?? "Unknown";
            }
            else
            {
                evidenceNumber = evidenceJson["Document"]?["EvidenceLogsData"]?["Case Number"]?.ToString() ?? "Unknown";
            }

            try
            {
                // Process attachments if any
                await attachmentService.ProcessAttachmentsRecursiveAsync(
                    apiToken,
                    agencyKey.ToString(),
                    module,
                    evidenceNumber,
                    baseFolderPath,
                    evidenceJson,
                    "",
                    "LawTrak"
                );

                var json = JsonConvert.SerializeObject(evidenceJson);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var requestModel = new CommonRequestModel
                {
                    Endpoint = "Document/Evidences",
                    RequestMethod = HttpMethod.Post,
                    RequestAuthMethod = Enums.AuthorizationMethod.Token,
                    ApiToken = apiToken,
                    PostContent = content
                };

                var response = await _httpClientService.SendApiRequest<object>(requestModel);

                if (response == null)
                    return (false, null, $"{evidenceNumber}: No response from server");

                if ((int)response.StatusCode < 200 || (int)response.StatusCode >= 300)
                    return (false, response.StatusCode, $"{evidenceNumber}: {response.StatusCode} - {response.Message}");
            }
            catch (Exception ex)
            {
                return (false, null, $"{evidenceNumber}: Exception - {ex.Message}");
            }

            return (true, System.Net.HttpStatusCode.OK, null);
        }



    }
}

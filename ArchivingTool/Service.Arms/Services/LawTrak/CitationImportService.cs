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
    public class CitationImportService
    {

        private readonly HttpClientService _httpClientService;

        public CitationImportService()
        {
            _httpClientService = new HttpClientService();
        }

        public async Task<(bool Success, System.Net.HttpStatusCode? StatusCode, string Message)>
ImportCitationsAsync(
    string apiToken,
    Dictionary<string, object> citation,
    Guid? historyId = null,
    string baseFolderPath = "",
    Guid agencyKey = default)
        {
            if (citation == null || citation.Count == 0)
                return (false, null, "No data");

            var attachmentService = new AttachmentService(_httpClientService);
            var module = "Citations";

            var citationJson = JObject.FromObject(citation);

            string citationType = citationJson["Document"]?["CitationsData"]?["Citation"]?.ToString() ?? "";

            // deciding which field to use
            string citationNumber;
            if (citationType == "Warning Ticket")
            {
                citationNumber = citationJson["Document"]?["CitationsData"]?["Ticket Number"]?.ToString() ?? "Unknown";
            }
            else
            {
                citationNumber = citationJson["Document"]?["CitationsData"]?["Citation Number"]?.ToString() ?? "Unknown";
            }


            try
            {
                // Process attachments if any
                await attachmentService.ProcessAttachmentsRecursiveAsync(
                    apiToken,
                    agencyKey.ToString(),
                    module,
                    citationNumber,
                    baseFolderPath,
                    citationJson,
                    "",
                    "LawTrak"
                );

                var json = JsonConvert.SerializeObject(citationJson);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var requestModel = new CommonRequestModel
                {
                    Endpoint = "Document/Citations",
                    RequestMethod = HttpMethod.Post,
                    RequestAuthMethod = Enums.AuthorizationMethod.Token,
                    ApiToken = apiToken,
                    PostContent = content
                };

                var response = await _httpClientService.SendApiRequest<object>(requestModel);

                if (response == null)
                    return (false, null, $"{citationNumber}: No response from server");

                if ((int)response.StatusCode < 200 || (int)response.StatusCode >= 300)
                    return (false, response.StatusCode, $"{citationNumber}: {response.StatusCode} - {response.Message}");
            }
            catch (Exception ex)
            {
                return (false, null, $"{citationNumber}: Exception - {ex.Message}");
            }

            return (true, System.Net.HttpStatusCode.OK, null);
        }



    }
}

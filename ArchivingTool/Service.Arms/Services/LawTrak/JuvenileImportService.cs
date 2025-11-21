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
    public class JuvenileImportService
    {

        private readonly HttpClientService _httpClientService;

        public JuvenileImportService()
        {
            _httpClientService = new HttpClientService();
        }

        public async Task<(bool Success, System.Net.HttpStatusCode? StatusCode, string Message)>
ImportJuvenileAsync(
    string apiToken,
    Dictionary<string, object> juveniles,
    Guid? historyId = null,
    string baseFolderPath = "",
    Guid agencyKey = default)
        {
            if (juveniles == null || juveniles.Count == 0)
                return (false, null, "No data");

            var attachmentService = new AttachmentService(_httpClientService);
            var module = "Juveniles";

            var juvenileJson = JObject.FromObject(juveniles);

            string juvenileNumber = juvenileJson["Document"]?["JuvenilesData"]?["Unique Number"]?.ToString() ?? "Unknown";


            try
            {
                // Process attachments if any
                await attachmentService.ProcessAttachmentsRecursiveAsync(
                    apiToken,
                    agencyKey.ToString(),
                    module,
                    juvenileNumber,
                    baseFolderPath,
                    juvenileJson,
                    "",
                    "LawTrak"
                );

                var json = JsonConvert.SerializeObject(juvenileJson);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var requestModel = new CommonRequestModel
                {
                    Endpoint = "Document/Juvenile",
                    RequestMethod = HttpMethod.Post,
                    RequestAuthMethod = Enums.AuthorizationMethod.Token,
                    ApiToken = apiToken,
                    PostContent = content
                };

                var response = await _httpClientService.SendApiRequest<object>(requestModel);

                if (response == null)
                    return (false, null, $"{juvenileJson}: No response from server");

                if ((int)response.StatusCode < 200 || (int)response.StatusCode >= 300)
                    return (false, response.StatusCode, $"{juvenileJson}: {response.StatusCode} - {response.Message}");
            }
            catch (Exception ex)
            {
                return (false, null, $"{juvenileJson}: Exception - {ex.Message}");
            }

            return (true, System.Net.HttpStatusCode.OK, null);
        }

    }
}

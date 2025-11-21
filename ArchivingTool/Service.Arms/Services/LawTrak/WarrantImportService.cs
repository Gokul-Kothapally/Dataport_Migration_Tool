using ArchivingTool.Model.Arms;
using ArchivingTool.Service.Arms.Services.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace ArchivingTool.Service.Arms.Services.LawTrak
{
    public class WarrantImportService
    {

        private readonly HttpClientService _httpClientService;

        public WarrantImportService()
        {
            _httpClientService = new HttpClientService();
        }

        public async Task<(bool Success, System.Net.HttpStatusCode? StatusCode, string Message)>
ImportWarrantsAsync(string apiToken, Dictionary<string, object> warrant, Guid? historyId = null, string baseFolderPath = "", Guid agencyKey = default)
        {
            if (warrant == null || warrant.Count == 0)
                return (false, null, "No data");

            var attachmentService = new AttachmentService(_httpClientService);
            var module = "Warrants";

            var warrantJson = JObject.FromObject(warrant);
            string moduleNumber = warrantJson["Document"]?["WarrantsData"]?["Warrant Number"]?.ToString() ?? "Unknown";

            try
            {
                await attachmentService.ProcessAttachmentsRecursiveAsync(
                    apiToken,
                    agencyKey.ToString(),
                    module,
                    moduleNumber,
                    baseFolderPath,
                    warrantJson,
                    "",
                    "LawTrak"
                );

                var json = JsonConvert.SerializeObject(warrantJson);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var requestModel = new CommonRequestModel
                {
                    Endpoint = "Document/Warrants",
                    RequestMethod = HttpMethod.Post,
                    RequestAuthMethod = Enums.AuthorizationMethod.Token,
                    ApiToken = apiToken,
                    PostContent = content
                };

                var response = await _httpClientService.SendApiRequest<object>(requestModel);

                if (response == null)
                    return (false, null, $"{moduleNumber}: No response from server");

                if ((int)response.StatusCode < 200 || (int)response.StatusCode >= 300)
                    return (false, response.StatusCode, $"{moduleNumber}: {response.StatusCode} - {response.Message}");
            }
            catch (Exception ex)
            {
                return (false, null, $"{moduleNumber}: Exception - {ex.Message}");
            }

            return (true, System.Net.HttpStatusCode.OK, null);
        }

    }
}

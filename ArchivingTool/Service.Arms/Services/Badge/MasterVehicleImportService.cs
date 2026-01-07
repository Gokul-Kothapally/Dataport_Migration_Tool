using ArchivingTool.Model.Arms;
using ArchivingTool.Service.Arms.Services.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace ArchivingTool.Service.Arms.Services.Badge
{
    public class MasterVehicleImportService
    {
        private readonly BlobUploadHelper blobUploadHelper;
        private readonly HttpClientService _httpClientService;

        public MasterVehicleImportService()
        {
            blobUploadHelper = new BlobUploadHelper();

            _httpClientService = new HttpClientService();
        }

        public async Task<(bool Success, System.Net.HttpStatusCode? StatusCode, string Message)>
ImportMasterVehicleAsync(
    string apiToken,
    JObject masterVehicleJson,
    string moduleNumber,
    Guid? historyId = null,
    string baseFolderPath = "",
    Guid agencyKey = default)
        {
            if (masterVehicleJson == null || masterVehicleJson.Count == 0)
                return (false, null, "No data");

            var attachmentService = new AttachmentService(_httpClientService, blobUploadHelper);
            var module = "MasterVehicles";

            try
            {
                await attachmentService.ProcessAttachmentsRecursiveAsync(
                    apiToken,
                    agencyKey.ToString(),
                    module,
                    moduleNumber,
                    baseFolderPath,
                    masterVehicleJson,
                    "",
                    "Badge"
                );

                var json = JsonConvert.SerializeObject(masterVehicleJson);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var requestModel = new CommonRequestModel
                {
                    Endpoint = "Document/MasterVehicles",
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

                return (true, response.StatusCode, string.Empty);
            }
            catch (Exception ex)
            {
                return (false, null, $"{moduleNumber}: Exception - {ex.Message}");
            }
        }
    }

}


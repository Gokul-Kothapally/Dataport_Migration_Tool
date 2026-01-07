using ArchivingTool.Model.Arms;
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
    public class PropertyCheckImportService
    {
        private readonly BlobUploadHelper blobUploadHelper;
        private readonly HttpClientService _httpClientService;

        public PropertyCheckImportService()
        {
            blobUploadHelper = new BlobUploadHelper();
            _httpClientService = new HttpClientService();
        }

        public async Task<(bool Success, System.Net.HttpStatusCode? StatusCode, string Message)>
ImportPropertyCheckAsync(string apiToken, JObject PropertyCheckJson, string moduleNumber, Guid? historyId = null, string baseFolderPath = "", Guid agencyKey = default)
        {
            if (PropertyCheckJson == null || PropertyCheckJson.Count == 0)
                return (false, null, "No data");

            var attachmentService = new AttachmentService(_httpClientService, blobUploadHelper);
            var module = "PropertyCheck";

            try
            {
                // Process attachments if any
                await attachmentService.ProcessAttachmentsRecursiveAsync(
                    apiToken,
                    agencyKey.ToString(),
                    module,
                    moduleNumber,
                    baseFolderPath,
                    PropertyCheckJson,
                    "",
                    "LawTrak"
                );

                var json = JsonConvert.SerializeObject(PropertyCheckJson);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var requestModel = new CommonRequestModel
                {
                    Endpoint = "Document/PropertyChecks",
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

            return (true, System.Net.HttpStatusCode.OK, string.Empty);
        }



    }
}

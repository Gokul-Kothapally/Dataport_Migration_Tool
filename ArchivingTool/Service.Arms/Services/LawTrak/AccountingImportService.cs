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
    public class AccountingImportService
    {

        private readonly HttpClientService _httpClientService;

        public AccountingImportService()
        {
            _httpClientService = new HttpClientService();
        }

        public async Task<(bool Success, System.Net.HttpStatusCode? StatusCode, string Message)>
ImportAccountingAsync(
    string apiToken,
    Dictionary<string, object> accounting,
    Guid? historyId = null,
    string baseFolderPath = "",
    Guid agencyKey = default)
        {
            if (accounting == null || accounting.Count == 0)
                return (false, null, "No data");

            var attachmentService = new AttachmentService(_httpClientService);
            var module = "Accounting";

            var accountingJson = JObject.FromObject(accounting);

            string accountingNumber = accountingJson["Document"]?["AccountingMonthlyData"]?["Accounting Date"]?.ToString() ?? "Unknown";


            try
            {
                // Process attachments if any
                await attachmentService.ProcessAttachmentsRecursiveAsync(
                    apiToken,
                    agencyKey.ToString(),
                    module,
                    accountingNumber,
                    baseFolderPath,
                    accountingJson,
                    "",
                    "LawTrak"
                );

                var json = JsonConvert.SerializeObject(accountingJson);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var requestModel = new CommonRequestModel
                {
                    Endpoint = "Document/Accounting",
                    RequestMethod = HttpMethod.Post,
                    RequestAuthMethod = Enums.AuthorizationMethod.Token,
                    ApiToken = apiToken,
                    PostContent = content
                };

                var response = await _httpClientService.SendApiRequest<object>(requestModel);

                if (response == null)
                    return (false, null, $"{accountingJson}: No response from server");

                if ((int)response.StatusCode < 200 || (int)response.StatusCode >= 300)
                    return (false, response.StatusCode, $"{accountingJson}: {response.StatusCode} - {response.Message}");
            }
            catch (Exception ex)
            {
                return (false, null, $"{accountingJson}: Exception - {ex.Message}");
            }

            return (true, System.Net.HttpStatusCode.OK, null);
        }

    }
}

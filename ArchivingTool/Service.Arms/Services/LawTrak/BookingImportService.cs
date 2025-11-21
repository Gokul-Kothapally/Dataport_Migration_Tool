using ArchivingTool.Model.Arms;
using ArchivingTool.Service.Arms.Services.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace ArchivingTool.Service.Arms.Services.LawTrak
{
    public class BookingImportService
    {
        private readonly HttpClientService _httpClientService;
        public BookingImportService()
        {
            _httpClientService = new HttpClientService();

        }

        public async Task<(bool Success, System.Net.HttpStatusCode? StatusCode, string Message)>
ImportBookingAsync(
    string apiToken,
    Dictionary<string, object> bookingData,
    Guid? historyId = null,
    string baseFolderPath = "",
    Guid agencyKey = default)
        {
            if (bookingData == null || bookingData.Count == 0)
                return (false, null, "No data");

            var attachmentService = new AttachmentService(_httpClientService);
            var module = "Booking";

            var bookingJson = JObject.FromObject(bookingData);
            string moduleNumber = bookingJson["Document"]?["BookingsData"]?["Booking Number"]?.ToString() ?? "Unknown";

            try
            {
                await attachmentService.ProcessAttachmentsRecursiveAsync(
                    apiToken,
                    agencyKey.ToString(),
                    module,
                    moduleNumber,
                    baseFolderPath,
                    bookingJson,
                    "",
                    "LawTrak"
                );

                var json = JsonConvert.SerializeObject(bookingJson);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var requestModel = new CommonRequestModel
                {
                    Endpoint = "Document/Bookings",
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

                return (true, System.Net.HttpStatusCode.OK, null);
            }
            catch (Exception ex)
            {
                return (false, null, $"{moduleNumber}: Exception - {ex.Message}");
            }
        }


    }
}

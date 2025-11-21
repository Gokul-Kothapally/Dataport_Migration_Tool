using ArchivingTool.Model.Arms;
using Microsoft.VisualBasic.ApplicationServices;
using System.Net.Http.Json;
using System.Text;

namespace ArchivingTool.Service.Arms.Services.Common
{
    public class ApiTokenService
    {
        private readonly HttpClientService httpClientService;

        public ApiTokenService()
        {
            httpClientService = new HttpClientService();
        }

        public async Task<CommonResponse<TokenResponse>> GetToken(string agencyKey, string apiKey)
        {
            var commonRequestModel = new CommonRequestModel
            {
                Username = agencyKey,
                Password = apiKey,
                Endpoint = "Authorize/CreateToken",
                RequestMethod = HttpMethod.Get,
                RequestAuthMethod = Enums.AuthorizationMethod.Basic
            };

            return await httpClientService.SendApiRequest<TokenResponse>(commonRequestModel);
        }
    }
}

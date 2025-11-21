using ArchivingTool.Model.Arms;
using ArchivingTool.Models;
using ArchivingTool.Service.Arms.Services.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;


namespace ArchivingTool.Service.Arms.Services.InSynch
{
    public class CitationImportService
    {
        private readonly HttpClientService httpClientService;

        public CitationImportService()
        {
            httpClientService = new HttpClientService();
        }

        public async Task<CommonResponse<CitationImportResponse>> ImportCitationsAsync(
    string apiToken,
    List<CitationImportRequest> citationData,
    Guid? historyId = null)
        {
            var json = JsonConvert.SerializeObject(citationData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var apiEndpoint = "External/Citation/Import";

            if (historyId.HasValue)
            {
                apiEndpoint += "?historyId=" + historyId.Value.ToString();
            }

            var commonRequestModel = new CommonRequestModel
            {
                ApiToken = apiToken,
                Endpoint = apiEndpoint,
                RequestMethod = HttpMethod.Post,
                RequestAuthMethod = Enums.AuthorizationMethod.Token,
                PostContent = content
            };

            return await httpClientService.SendApiRequest<CitationImportResponse>(commonRequestModel);
        }

    }
}

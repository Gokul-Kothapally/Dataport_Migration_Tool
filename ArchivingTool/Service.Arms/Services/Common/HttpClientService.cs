using Microsoft.Extensions.Configuration;
using ArchivingTool.Model.Arms;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime;
using System.Text;
using System.Text.Json.Serialization;

namespace ArchivingTool.Service.Arms.Services.Common
{
    public class HttpClientService
    {
        private readonly string _apiBaseUrl;

        public HttpClientService() {
            var mySettings = Program.Configuration?.GetSection("ApplicationSettings").Get<ApplicationSetting>();
            _apiBaseUrl = mySettings?.ApiBaseUrl ?? string.Empty;
        }

        /*public async Task<CommonResponse<T>> SendApiRequest<T>(CommonRequestModel commonRequestModel)
        {
            try
            {
                using var httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(_apiBaseUrl);

                if (commonRequestModel.RequestAuthMethod == Enums.AuthorizationMethod.Basic)
                {
                    var authenticationString = $"{commonRequestModel.Username}:{commonRequestModel.Password}";
                    var base64String = Convert.ToBase64String(Encoding.ASCII.GetBytes(authenticationString));
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + base64String);
                }
                else if (commonRequestModel.RequestAuthMethod == Enums.AuthorizationMethod.Token)
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", commonRequestModel.ApiToken);
                }

                if (commonRequestModel.RequestMethod == HttpMethod.Get)
                {
                    using var response = await httpClient.GetAsync(commonRequestModel.Endpoint);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadFromJsonAsync<CommonResponse<T>>();
                        if (responseBody != null)
                        {
                            responseBody.StatusCode = response.StatusCode;
                            return responseBody;
                        }
                    }
                    else
                        return CommonResponse<T>.CreateError(response.StatusCode, "Unknown Error Occurred, Please, contact administrator.");
                }
                else if (commonRequestModel.RequestMethod == HttpMethod.Post)
                {
                    using var response = await httpClient.PostAsync(commonRequestModel.Endpoint, commonRequestModel.PostContent);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadFromJsonAsync<CommonResponse<T>>();
                        if (responseBody != null)
                        {
                            responseBody.StatusCode = response.StatusCode;
                            return responseBody;
                        }
                    }
                    else
                       return CommonResponse<T>.CreateError(response.StatusCode, "Unknown Error Occurred, Please, contact administrator.");
                }
                else
                    return CommonResponse<T>.CreateError(HttpStatusCode.BadRequest, "Please select request method.");

                return CommonResponse<T>.CreateError(HttpStatusCode.InternalServerError, "Unknown Error Occurred, Please, contact administrator.");
            }
            catch (Exception ex)
            {
                return CommonResponse<T>.CreateError(HttpStatusCode.InternalServerError, ex.Message);
            }
        }*/

        public async Task<CommonResponse<T>> SendApiRequest<T>(CommonRequestModel commonRequestModel)
        {
            try
            {
                using var httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(_apiBaseUrl);

                // Authentication
                if (commonRequestModel.RequestAuthMethod == Enums.AuthorizationMethod.Basic)
                {
                    var authenticationString = $"{commonRequestModel.Username}:{commonRequestModel.Password}";
                    var base64String = Convert.ToBase64String(Encoding.ASCII.GetBytes(authenticationString));
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + base64String);
                }
                else if (commonRequestModel.RequestAuthMethod == Enums.AuthorizationMethod.Token)
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", commonRequestModel.ApiToken);
                }

                HttpResponseMessage response;

                if (commonRequestModel.RequestMethod == HttpMethod.Get)
                {
                    response = await httpClient.GetAsync(commonRequestModel.Endpoint);
                }
                else if (commonRequestModel.RequestMethod == HttpMethod.Post)
                {
                    response = await httpClient.PostAsync(commonRequestModel.Endpoint, commonRequestModel.PostContent);
                }
                else
                {
                    return CommonResponse<T>.CreateError(HttpStatusCode.BadRequest, "Please select request method.");
                }

                // Try to read the body as CommonResponse<T> (works for success and error JSON)
                var responseBody = await response.Content.ReadFromJsonAsync<CommonResponse<T>>();
                if (responseBody != null)
                {
                    responseBody.StatusCode = response.StatusCode;
                    return responseBody; // will include the API's "message"
                }

                // fallback if API body isn't in expected format
                var rawError = await response.Content.ReadAsStringAsync();
                return CommonResponse<T>.CreateError(response.StatusCode,
                    $"{response.StatusCode} {response.ReasonPhrase}. {rawError}");
            }
            catch (Exception ex)
            {
                return CommonResponse<T>.CreateError(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

    }
}

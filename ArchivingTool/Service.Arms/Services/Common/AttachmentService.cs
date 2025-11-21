using ArchivingTool.Model.Arms;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ArchivingTool.Service.Arms.Services.Common
{
    public class AttachmentService
    {
        private readonly HttpClientService _httpClientService;

        public AttachmentService(HttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
        }

        /// <summary>
        /// Uploading a single file to the API.
        /// </summary>
        private async Task<string> UploadFileAsync(string apiToken, string agencyKey, string logicalFolderPath, string filePath)
        {
            using var content = new MultipartFormDataContent();
            content.Add(new StringContent(agencyKey), "agencyKey");
            content.Add(new StringContent(logicalFolderPath), "folderpath");
            content.Add(new StreamContent(File.OpenRead(filePath)), "file", Path.GetFileName(filePath));

            var requestModel = new CommonRequestModel
            {
                Endpoint = "Files/upload",
                RequestMethod = HttpMethod.Post,
                RequestAuthMethod = Enums.AuthorizationMethod.Token,
                ApiToken = apiToken,
                PostContent = content
            };

            CommonResponse<string> response = await _httpClientService.SendApiRequest<string>(requestModel);

            if (response != null && response.Success && !string.IsNullOrEmpty(response.Data))
                return response.Data;

            return null;
        }

        /// <summary>
        /// Recursively processing any JSON object and uploading attachments.
        /// </summary>
        public async Task ProcessAttachmentsRecursiveAsync(
    string apiToken,
    string agencyKey,
    string module,
    string moduleNumber,
    string baseFolderPath,
    JToken token,
    string currentPath = "",
    string rms = "")
        {
            if (token == null)
                return;

            // If this token is an "Attachments" array
            if (token.Type == JTokenType.Array && token.Path.EndsWith("Attachments"))
            {
                var attachments = token as JArray;

                // 🔹 Precompute LawTrak subfolders (only if needed)
                string videosFolderPath = null;
                string ltPicsFolderPath1 = null;
                string ltPicsFolderPath2= null;
                string ltPicsFolderPath3 = null;

                if (rms.Equals("LawTrak", StringComparison.OrdinalIgnoreCase))
                {
                    videosFolderPath = Path.Combine(baseFolderPath, "videos");
                    ltPicsFolderPath1 = Path.Combine(baseFolderPath, "ltpics01");
                    ltPicsFolderPath2 = Path.Combine(baseFolderPath, "ltpics02");
                    ltPicsFolderPath3 = Path.Combine(baseFolderPath, "ltpics03");
                }

                for (int i = 0; i < attachments.Count; i++)
                {
                    var attachment = (JObject)attachments[i];
                    string fileName = attachment["File Name"]?.ToString();

                    if (!string.IsNullOrEmpty(fileName))
                    {
                        string targetFolder = baseFolderPath;

                        if (rms.Equals("LawTrak", StringComparison.OrdinalIgnoreCase))
                        {
                            string attachmentType = attachment["AttachmentType"]?.ToString();

                            if (attachmentType == "Incident" || attachmentType == "Offenses")
                                targetFolder = ltPicsFolderPath3;
                            else if (attachmentType == "MUG")
                                targetFolder = ltPicsFolderPath1;
                            else if (attachmentType == "VID")
                                targetFolder = videosFolderPath;
                            else
                                targetFolder = ltPicsFolderPath3; // default
                        }


                        string filePath = Path.Combine(targetFolder, fileName);

                        if (File.Exists(filePath))
                        {
                            // Build logical path (module + case number + optional currentPath)
                            string logicalFolderPath = string.IsNullOrEmpty(currentPath)
                                ? $"{module},{moduleNumber}"
                                : $"{module},{moduleNumber},{currentPath.Replace('.', ',')}";

                            string file = await UploadFileAsync(apiToken, agencyKey, logicalFolderPath, filePath);

                            if (!string.IsNullOrEmpty(file))
                            {
                                // Inject FileURL back into the attachment
                                attachment["File"] = file;
                            }
                        }
                    }
                }
            }

            // Recurse into child objects/arrays
            foreach (var child in token.Children())
            {
                string newPath = currentPath;

                if (child is JProperty prop)
                {
                    // Add this property name to path (only once)
                    string propName = prop.Name;

                    // Avoid repeating the same node name consecutively (e.g., Document.Document)
                    if (!string.IsNullOrEmpty(currentPath) &&
                        currentPath.Split('.').Last().Equals(propName, StringComparison.OrdinalIgnoreCase))
                    {
                        newPath = currentPath;
                    }
                    else
                    {
                        newPath = string.IsNullOrEmpty(currentPath)
                            ? propName
                            : $"{currentPath}.{propName}";
                    }

                    // Recurse into the property’s VALUE, not the property itself
                    await ProcessAttachmentsRecursiveAsync(
                        apiToken, agencyKey, module, moduleNumber,
                        baseFolderPath, prop.Value, newPath, rms
                    );
                }
                else if (child is JObject obj)
                {
                    // Recurse into nested object without changing the path
                    await ProcessAttachmentsRecursiveAsync(
                        apiToken, agencyKey, module, moduleNumber,
                        baseFolderPath, obj, newPath, rms
                    );
                }
                else if (child is JArray arr)
                {
                    // Recurse into array without changing the path
                    await ProcessAttachmentsRecursiveAsync(
                        apiToken, agencyKey, module, moduleNumber,
                        baseFolderPath, arr, newPath, rms
                    );
                }
            }


        }

    }
}

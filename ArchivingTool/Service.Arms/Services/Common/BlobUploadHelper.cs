using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ArchivingTool.Service.Arms.Services.Common
{
    public class BlobUploadHelper
    {
        private readonly BlobContainerClient _containerClient;

        // Container shuld always be "attachments"
        public BlobUploadHelper()
        {
#if DEBUG
            // DEBUG MODE: Use connection string
            var connectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    "AZURE_STORAGE_CONNECTION_STRING is not set for DEBUG mode.");
            }

            _containerClient = new BlobContainerClient(
                connectionString,
                "attachments"
            );
#else
            var containerUri = new Uri("https://dataportsatx.blob.core.usgovcloudapi.net/attachments");

            _containerClient = new BlobContainerClient(containerUri, new DefaultAzureCredential(
                new DefaultAzureCredentialOptions
                {
                    AuthorityHost = AzureAuthorityHosts.AzureGovernment
                }
            ));
#endif
        }

        public async Task<string> UploadAsync(
            string filePath,
            string blobFolderPath,
            CancellationToken cancellationToken = default)
        {
            string fileName = Path.GetFileName(filePath);

            // Build blob path (folder structure)
            string blobName = $"{blobFolderPath}/{fileName}".Replace("\\", "/");

            var blobClient = _containerClient.GetBlockBlobClient(blobName);

            using var stream = File.OpenRead(filePath);

            await blobClient.UploadAsync(stream, cancellationToken: cancellationToken);

            return blobClient.Uri.ToString();
        }

    }
}

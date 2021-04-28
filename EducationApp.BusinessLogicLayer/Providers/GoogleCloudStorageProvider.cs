using EducationApp.BusinessLogicLayer.Providers.Interfaces;
using EducationApp.Shared.Configs;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.IO;
using System.Threading.Tasks;

namespace EducationApp.BusinessLogicLayer.Providers
{
    public class GoogleCloudStorageProvider : ICloudStorageProvider
    {
        private readonly GoogleCredential _googleCredential;
        private readonly StorageClient _storageClient;
        private readonly string _bucketName;

        public GoogleCloudStorageProvider(IOptions<GoogleStorageConfig> config)
        {
            _googleCredential = GoogleCredential.FromFile(config.Value.CredentialFile);
            _storageClient = StorageClient.Create(_googleCredential);
            _bucketName = config.Value.CloudStorageBucketName;
        }

        public async Task<string> UploadFileAsync(IFormFile imageFile, string fileNameForStorage)
        {
            using (var memoryStream = new MemoryStream())
            {
                await imageFile.CopyToAsync(memoryStream);
                var dataObject = await _storageClient.UploadObjectAsync(_bucketName, fileNameForStorage, null, memoryStream);
                return dataObject.MediaLink;
            }
        }

        public async Task DeleteFileAsync(string fileNameForStorage)
        {
            await _storageClient.DeleteObjectAsync(_bucketName, fileNameForStorage);
        }
    }
}

using EducationApp.BusinessLogicLayer.Providers.Interfaces;
using EducationApp.Shared.Configs;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
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

        public async Task<string> UploadFileAsync(string imageString, string fileNameForStorage)
        {
            string base64Image = imageString.Split(',')[1];
            byte[] bytes = Convert.FromBase64String(base64Image);
            using MemoryStream fileStream = new(bytes);
            var dataObject = await _storageClient.UploadObjectAsync(_bucketName, fileNameForStorage, null, fileStream);
            return dataObject.MediaLink;
        }

        public async Task DeleteFileAsync(string fileNameForStorage)
        {
            await _storageClient.DeleteObjectAsync(_bucketName, fileNameForStorage);
        }
    }
}

using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace EducationApp.BusinessLogicLayer.Providers.Interfaces
{
    public interface ICloudStorageProvider
    {
        public Task<string> UploadFileAsync(string imageString, string fileNameForStorage);
        Task DeleteFileAsync(string fileNameForStorage);
    }
}

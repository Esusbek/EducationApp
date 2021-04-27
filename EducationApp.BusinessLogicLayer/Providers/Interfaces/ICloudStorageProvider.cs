using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace EducationApp.BusinessLogicLayer.Providers.Interfaces
{
    public interface ICloudStorageProvider
    {
        Task<string> UploadFileAsync(IFormFile imageFile, string fileNameForStorage);
        Task DeleteFileAsync(string fileNameForStorage);
    }
}

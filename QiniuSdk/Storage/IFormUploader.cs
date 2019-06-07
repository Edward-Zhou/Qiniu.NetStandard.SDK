using System.IO;
using System.Threading.Tasks;
using QiniuSdk.Http;

namespace QiniuSdk.Storage
{
    public interface IFormUploader
    {
        Task<HttpResult> UploadData(byte[] data, string key, string token, PutExtra extra);
        Task<HttpResult> UploadFile(string localFile, string key, string token, PutExtra extra);
        Task<HttpResult> UploadStream(Stream stream, string key, string token, PutExtra putExtra);
    }
}
using System.IO;
using System.Threading.Tasks;
using QiniuSdk.Http;

namespace QiniuSdk.Storage
{
    public interface IResumableUploader
    {
        Task<HttpResult> UploadFile(string localFile, string key, string token, PutExtra putExtra);
        Task<HttpResult> UploadStream(Stream stream, string key, string upToken, PutExtra putExtra);
    }
}
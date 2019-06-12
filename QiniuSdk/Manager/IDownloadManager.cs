using System.Threading.Tasks;
using QiniuSdk.Http;

namespace QiniuSdk.Storage
{
    public interface IDownloadManager
    {
        string CreatePrivateUrl(Mac mac, string domain, string fileName, int expireInSeconds = 3600);
        string CreatePublishUrl(string domain, string fileName);
        Task<HttpResult> Download(string url, string saveasFile);
    }
}
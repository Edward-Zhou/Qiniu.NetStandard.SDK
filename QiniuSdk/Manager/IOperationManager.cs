using System.IO;
using System.Threading.Tasks;
using QiniuSdk.Http;

namespace QiniuSdk.Storage
{
    public interface IOperationManager
    {
        Task<HttpResult> Dfop(string fop, string uri);
        Task<HttpResult> DfopData(string fop, string localFile);
        Task<HttpResult> DfopFile(string fop, string localFile, string mimeType = "application/octet-stream");
        Task<HttpResult> DfopStream(string fop, Stream stream, string mimeType, string fileName);
        Task<HttpResult> DfopText(string fop, string text);
        Task<HttpResult> DfopTextFile(string fop, string textFile);
        Task<HttpResult> DfopUrl(string fop, string url);
        Task<PfopResult> Pfop(string bucket, string key, string fops, string pipeline, string notifyUrl, bool force);
        Task<PfopResult> Pfop(string bucket, string key, string[] fops, string pipeline, string notifyUrl, bool force);
        Task<PrefopResult> Prefop(string persistentId);
    }
}
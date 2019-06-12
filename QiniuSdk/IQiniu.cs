using QiniuSdk.Http;
using QiniuSdk.Storage;
using System;
using System.IO;
using System.Threading.Tasks;

namespace QiniuSdk
{
    public interface IQiniu
    {
        Task<HttpResult> UploadStream(Stream stream, PutPolicy putPolicy, string key, PutExtra putExtra);
    }
}

using Microsoft.Extensions.Options;
using QiniuSdk.Http;
using QiniuSdk.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace QiniuSdk
{
    public class Qiniu: IQiniu
    {
        private readonly IFormUploader _formUploader;
        private readonly IAuth _auth;
        public Qiniu(IFormUploader formUploader
            , IAuth auth)
        {
            _formUploader = formUploader;
            _auth = auth;
        }

        #region Storage
        public async Task<HttpResult> UploadStream(Stream stream, string key, PutExtra putExtra)
        {
            var putPolicy = new PutPolicy
            {
                Scope = $"goldhouse",
                DeleteAfterDays = 1
            };
            putPolicy.SetExpires(3600);
            var token = _auth.CreateUploadToken(putPolicy.ToJsonString());

            var result = await _formUploader.UploadStream(stream, "test.png", token, null);
            return result;
        }

        #endregion
    }
}

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
    public class Qiniu : IQiniu
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
        public async Task<HttpResult> UploadStream(Stream stream, PutPolicy putPolicy, string key, PutExtra putExtra)
        {
            var token = _auth.CreateUploadToken(putPolicy.ToJsonString());
            var result = await _formUploader.UploadStream(stream, key, token, putExtra);
            return result;
        }
        #endregion
    }
}

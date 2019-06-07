using QiniuSdk.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace QiniuSdk.Storage
{
    internal class QiniuException : Exception
    {
        public HttpResult HttpResult;

        public QiniuException(HttpResult httpResult, string message) : base(message)
        {
            HttpResult = httpResult ?? new HttpResult();
        }
    }
}

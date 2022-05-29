using System;
using System.Collections.Generic;
using System.Text;

namespace QiniuSdk
{
    public class QiniuOptions
    {
        public Mac Mac { get; set; }
        public HttpOptions HttpOptions { get; set; }
        public string BucketName { get; set; }
        public string DomainName { get; set; }

    }
    public class HttpOptions
    {
        public bool AllowAutoRedirect { get; set; } = false;
        public string BaseUrl { get; set; }
    }
}

﻿using QiniuSdk.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace QiniuSdk
{
    /// <summary>
    ///     Authentication/Authorization
    /// </summary>
    public class Auth : IAuth
    {
        private readonly Signature _signature;

        /// <summary>
        ///     一般初始化
        /// </summary>
        /// <param name="mac">账号(密钥)</param>
        public Auth(Signature signature)
        {
            _signature = signature;
        }

        /// <summary>
        ///     生成管理凭证
        ///     有关管理凭证请参阅
        ///     http://developer.qiniu.com/article/developer/security/access-token.html
        /// </summary>
        /// <param name="url">请求的URL</param>
        /// <param name="body">请求的主体内容</param>
        /// <returns>生成的管理凭证</returns>
        public string CreateManageToken(string url, byte[] body)
        {
            return $"QBox {_signature.SignRequest(url, body)}";
        }

        /// <summary>
        ///     生成管理凭证-不包含body
        /// </summary>
        /// <param name="url">请求的URL</param>
        /// <returns>生成的管理凭证</returns>
        public string CreateManageToken(string url)
        {
            return CreateManageToken(url, null);
        }

        /// <summary>
        ///     生成上传凭证
        /// </summary>
        /// <param name="jsonStr">上传策略对应的JSON字符串</param>
        /// <returns>生成的上传凭证</returns>
        public string CreateUploadToken(string jsonStr)
        {
            return _signature.SignWithData(jsonStr);
        }

        /// <summary>
        ///     生成下载凭证
        /// </summary>
        /// <param name="url">原始链接</param>
        /// <returns></returns>
        public string CreateDownloadToken(string url)
        {
            return _signature.Sign(url);
        }

        /// <summary>
        ///     生成推流地址使用的凭证
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string CreateStreamPublishToken(string path)
        {
            return _signature.Sign(path);
        }

        /// <summary>
        ///     生成流管理凭证
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string CreateStreamManageToken(string data)
        {
            return $"Qiniu {_signature.SignWithData(data)}";
        }
    }
}

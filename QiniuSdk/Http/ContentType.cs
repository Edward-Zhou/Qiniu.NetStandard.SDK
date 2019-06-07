﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace QiniuSdk.Http
{
    /// <summary>
    ///     HTTP 内容类型(Content-Type)
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class ContentType
    {
        /// <summary>
        ///     资源类型：普通文本
        /// </summary>
        public static readonly string TEXT_PLAIN = "text/plain";

        /// <summary>
        ///     资源类型：JSON字符串
        /// </summary>
        public static readonly string APPLICATION_JSON = "application/json";

        /// <summary>
        ///     资源类型：未知类型(数据流)
        /// </summary>
        public static readonly string APPLICATION_OCTET_STREAM = "application/octet-stream";

        /// <summary>
        ///     资源类型：表单数据(键值对)
        /// </summary>
        public static readonly string WWW_FORM_URLENC = "application/x-www-form-urlencoded";

        /// <summary>
        ///     资源类型：多分部数据
        /// </summary>
        public static readonly string MULTIPART_FORM_DATA = "multipart/form-data";
    }
}

﻿using System;
using System.Collections.Generic;
using System.Text;

namespace QiniuSdk.Storage
{
    /// <summary>
    ///     目前已支持的区域：华东/华北/华南/北美/新加坡
    /// </summary>
    public class Zone
    {
        /// <summary>
        ///     华东
        /// </summary>
        public static Zone ZoneCnEast = new Zone
        {
            RsHost = "rs.qbox.me",
            RsfHost = "rsf.qbox.me",
            ApiHost = "api.qiniu.com",
            IovipHost = "iovip.qbox.me",
            SrcUpHosts = new[]
            {
                "up.qiniup.com",
                "up-nb.qiniup.com", "up-xs.qiniup.com"
            },
            CdnUpHosts = new[]
            {
                "upload.qiniup.com",
                "upload-nb.qiniup.com", "upload-xs.qiniup.com"
            }
        };

        /// <summary>
        ///     华北
        /// </summary>
        public static Zone ZoneCnNorth = new Zone
        {
            RsHost = "rs-z1.qbox.me",
            RsfHost = "rsf-z1.qbox.me",
            ApiHost = "api-z1.qiniu.com",
            IovipHost = "iovip-z1.qbox.me",
            SrcUpHosts = new[] { "up-z1.qiniup.com" },
            CdnUpHosts = new[] { "upload-z1.qiniup.com" }
        };

        /// <summary>
        ///     华南
        /// </summary>
        public static Zone ZoneCnSouth = new Zone
        {
            RsHost = "rs-z2.qbox.me",
            RsfHost = "rsf-z2.qbox.me",
            ApiHost = "api-z2.qiniu.com",
            IovipHost = "iovip-z2.qbox.me",
            SrcUpHosts = new[]
            {
                "up-z2.qiniup.com",
                "up-gz.qiniup.com", "up-fs.qiniup.com"
            },
            CdnUpHosts = new[]
            {
                "upload-z2.qiniup.com",
                "upload-gz.qiniup.com", "upload-fs.qiniup.com"
            }
        };

        /// <summary>
        ///     北美
        /// </summary>
        public static Zone ZoneUsNorth = new Zone
        {
            RsHost = "rs-na0.qbox.me",
            RsfHost = "rsf-na0.qbox.me",
            ApiHost = "api-na0.qiniu.com",
            IovipHost = "iovip-na0.qbox.me",
            SrcUpHosts = new[] { "up-na0.qiniup.com" },
            CdnUpHosts = new[] { "upload-na0.qiniup.com" }
        };

        /// <summary>
        ///     新加坡
        /// </summary>
        public static Zone ZoneAsSingapore = new Zone
        {
            RsHost = "rs-as0.qbox.me",
            RsfHost = "rsf-as0.qbox.me",
            ApiHost = "api-as0.qiniu.com",
            IovipHost = "iovip-as0.qbox.me",
            SrcUpHosts = new[] { "up-as0.qiniup.com" },
            CdnUpHosts = new[] { "upload-as0.qiniup.com" }
        };

        /// <summary>
        ///     资源管理
        /// </summary>
        public string RsHost { set; get; }

        /// <summary>
        ///     源列表
        /// </summary>
        public string RsfHost { set; get; }

        /// <summary>
        ///     数据处理
        /// </summary>
        public string ApiHost { set; get; }

        /// <summary>
        ///     镜像刷新、资源抓取
        /// </summary>
        public string IovipHost { set; get; }

        /// <summary>
        ///     资源上传
        /// </summary>
        public string[] SrcUpHosts { set; get; }

        /// <summary>
        ///     CDN加速
        /// </summary>
        public string[] CdnUpHosts { set; get; }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace QiniuSdk.Storage
{
    /// <summary>
    ///     配置信息，主要包括Zone配置(另请参阅Zone模块)
    ///     目前已支持的机房包括：
    ///     华东(CN_East), 华北(CN_North), 华南(CN_South), 北美(US_North), 新加坡（AS_Singapore）
    ///     默认设置为华东机房(CN_East)
    /// </summary>
    public class Config
    {
        private readonly ZoneHelper _zoneHelper;
        /// <summary>
        ///     默认高级资源管理域名
        /// </summary>
        public static string DefaultRsHost = "rs.qiniu.com";

        /// <summary>
        ///     默认数据处理域名
        /// </summary>
        public static string DefaultApiHost = "api.qiniu.com";

        /// <summary>
        ///     分片上传时，片的大小，默认为4MB，以提高上传效率
        /// </summary>
        public ChunkUnit ChunkSize = ChunkUnit.U4096K;

        /// <summary>
        ///     重试请求次数
        /// </summary>
        public int MaxRetryTimes = 3;

        /// <summary>
        ///     分片上传的阈值，超过该大小采用分片上传的方式
        /// </summary>
        public int PutThreshold = ResumeChunk.GetChunkSize(ChunkUnit.U1024K) * 10;

        /// <summary>
        ///     是否采用CDN加速域名，对上传有效
        /// </summary>
        public bool UseCdnDomains = false;

        /// <summary>
        ///     是否采用https域名
        /// </summary>
        public bool UseHttps = false;

        /// <summary>
        ///     空间所在的区域(Zone)
        /// </summary>
        public Zone Zone = null;

        public Config(ZoneHelper zoneHelper)
        {
            _zoneHelper = zoneHelper;
        }
        /// <summary>
        ///     获取资源管理域名
        /// </summary>
        /// <param name="ak"></param>
        /// <param name="bucket"></param>
        /// <returns></returns>
        public async Task<string> RsHost(string ak, string bucket)
        {
            var scheme = UseHttps ? "https://" : "http://";
            var z = Zone;
            if (z == null)
            {
                z = await _zoneHelper.QueryZone(ak, bucket);
            }

            return $"{scheme}{z.RsHost}";
        }

        /// <summary>
        ///     获取资源列表域名
        /// </summary>
        /// <param name="ak"></param>
        /// <param name="bucket"></param>
        /// <returns></returns>
        public async Task<string> RsfHost(string ak, string bucket)
        {
            var scheme = UseHttps ? "https://" : "http://";
            var z = Zone;
            if (z == null)
            {
                z = await _zoneHelper.QueryZone(ak, bucket);
            }

            return $"{scheme}{z.RsfHost}";
        }

        /// <summary>
        ///     获取数据处理域名
        /// </summary>
        /// <param name="ak"></param>
        /// <param name="bucket"></param>
        /// <returns></returns>
        public async Task<string> ApiHost(string ak, string bucket)
        {
            var scheme = UseHttps ? "https://" : "http://";
            var z = Zone;
            if (z == null)
            {
                z = await _zoneHelper.QueryZone(ak, bucket);
            }

            return $"{scheme}{z.ApiHost}";
        }

        /// <summary>
        ///     获取资源抓取更新域名
        /// </summary>
        /// <param name="ak"></param>
        /// <param name="bucket"></param>
        /// <returns></returns>
        public async Task<string> IovipHost(string ak, string bucket)
        {
            var scheme = UseHttps ? "https://" : "http://";
            var z = Zone;
            if (z == null)
            {
                z = await _zoneHelper.QueryZone(ak, bucket);
            }

            return $"{scheme}{z.IovipHost}";
        }

        /// <summary>
        ///     获取文件上传域名
        /// </summary>
        /// <param name="ak"></param>
        /// <param name="bucket"></param>
        /// <returns></returns>
        public async Task<string> UpHost(string ak, string bucket)
        {
            var scheme = UseHttps ? "https://" : "http://";
            var z = Zone;
            if (z == null)
            {
                z = await _zoneHelper.QueryZone(ak, bucket);
            }

            var upHost = z.SrcUpHosts[0];
            if (UseCdnDomains)
            {
                upHost = z.CdnUpHosts[0];
            }

            return $"{scheme}{upHost}";
        }
    }
}

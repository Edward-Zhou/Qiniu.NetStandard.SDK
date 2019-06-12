using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QiniuSdk.Http;
using QiniuSdk.Util;

namespace QiniuSdk.Storage
{
    /// <summary>
    ///     空间(资源)管理/操作
    /// </summary>
    public class BucketManager : IBucketManager
    {
        private readonly IAuth _auth;
        private readonly Config _config;
        private readonly HttpManager _httpManager;
        private readonly Mac _mac;

        /// <summary>
        /// </summary>
        /// <param name="mac"></param>
        /// <param name="config"></param>
        public BucketManager(Mac mac, IAuth auth, Config config)
        {
            _mac = mac;
            _auth = auth;
            _httpManager = new HttpManager();
            _config = config;
        }

        /// <summary>
        ///     获取空间文件信息
        /// </summary>
        /// <param name="bucket">空间名称</param>
        /// <param name="key">文件key</param>
        /// <returns>文件信息获取结果</returns>
        public async Task<StatResult> Stat(string bucket, string key)
        {
            var result = new StatResult();

            try
            {
                var host = await _config.RsHost(_mac.AccessKey, bucket);
                var statUrl = $"{host}{StatOp(bucket, key)}";
                var token = _auth.CreateManageToken(statUrl);

                var hr = await _httpManager.GetAsync(statUrl, token);
                result.Shadow(hr);
            }
            catch (QiniuException ex)
            {
                var sb = new StringBuilder();
                sb.Append($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.ffff}] [stat] Error:  ");
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }

                sb.AppendLine();

                result.Code = ex.HttpResult.Code;
                result.RefCode = ex.HttpResult.Code;
                result.Text = ex.HttpResult.Text;
                result.RefText += sb.ToString();
            }

            return result;
        }

        /// <summary>
        ///     获取空间(bucket)列表
        /// </summary>
        /// <param name="shared">是否列出被授权访问的空间</param>
        /// <returns>空间列表获取结果</returns>
        public async Task<BucketsResult> Buckets(bool shared)
        {
            var result = new BucketsResult();

            try
            {
                var scheme = _config.UseHttps ? "https://" : "http://";
                var rsHost = $"{scheme}{Config.DefaultRsHost}";
                var sharedStr = "false";
                if (shared)
                {
                    sharedStr = "true";
                }

                var bucketsUrl = $"{rsHost}/buckets?shared={sharedStr}";
                var token = _auth.CreateManageToken(bucketsUrl);

                var hr = await _httpManager.GetAsync(bucketsUrl, token);
                result.Shadow(hr);
            }
            catch (QiniuException ex)
            {
                var sb = new StringBuilder();
                sb.Append($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.ffff}] [buckets] Error:  ");
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }

                sb.AppendLine();

                result.Code = ex.HttpResult.Code;
                result.RefCode = ex.HttpResult.Code;
                result.Text = ex.HttpResult.Text;
                result.RefText += sb.ToString();
            }

            return result;
        }


        /// <summary>
        ///     删除文件
        /// </summary>
        /// <param name="bucket">空间名称</param>
        /// <param name="key">文件key</param>
        /// <returns>状态码为200时表示OK</returns>
        public async Task<HttpResult> Delete(string bucket, string key)
        {
            var result = new HttpResult();

            try
            {
                var host = await _config.RsHost(_mac.AccessKey, bucket);
                var deleteUrl = $"{host}{DeleteOp(bucket, key)}";
                var token = _auth.CreateManageToken(deleteUrl);

                result = await _httpManager.PostAsync(deleteUrl, token);
            }
            catch (QiniuException ex)
            {
                var sb = new StringBuilder();
                sb.Append($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.ffff}] [delete] Error:  ");
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }

                sb.AppendLine();

                result.Code = ex.HttpResult.Code;
                result.RefCode = ex.HttpResult.Code;
                result.Text = ex.HttpResult.Text;
                result.RefText += sb.ToString();
            }

            return result;
        }

        /// <summary>
        ///     复制文件
        /// </summary>
        /// <param name="srcBucket">源空间</param>
        /// <param name="srcKey">源文件key</param>
        /// <param name="dstBucket">目标空间</param>
        /// <param name="dstKey">目标key</param>
        /// <returns>状态码为200时表示OK</returns>
        public Task<HttpResult> Copy(string srcBucket, string srcKey, string dstBucket, string dstKey)
        {
            return Copy(srcBucket, srcKey, dstBucket, dstKey, false);
        }

        /// <summary>
        ///     复制文件 (with 'force' param)
        /// </summary>
        /// <param name="srcBucket">源空间</param>
        /// <param name="srcKey">源文件key</param>
        /// <param name="dstBucket">目标空间</param>
        /// <param name="dstKey">目标key</param>
        /// <param name="force">force标志,true/false</param>
        /// <returns>状态码为200时表示OK</returns>
        public async Task<HttpResult> Copy(string srcBucket, string srcKey, string dstBucket, string dstKey, bool force)
        {
            var result = new HttpResult();

            try
            {
                var host = await _config.RsHost(_mac.AccessKey, srcBucket);
                var copyUrl = $"{host}{CopyOp(srcBucket, srcKey, dstBucket, dstKey, force)}";
                var token = _auth.CreateManageToken(copyUrl);

                result = await _httpManager.PostAsync(copyUrl, token);
            }
            catch (QiniuException ex)
            {
                var sb = new StringBuilder();
                sb.Append($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.ffff}] [copy] Error:  ");
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }

                sb.AppendLine();

                result.Code = ex.HttpResult.Code;
                result.RefCode = ex.HttpResult.Code;
                result.Text = ex.HttpResult.Text;
                result.RefText += sb.ToString();
            }

            return result;
        }

        /// <summary>
        ///     移动文件
        /// </summary>
        /// <param name="srcBucket">源空间</param>
        /// <param name="srcKey">源文件key</param>
        /// <param name="dstBucket">目标空间</param>
        /// <param name="dstKey">目标key</param>
        /// <returns>状态码为200时表示OK</returns>
        public Task<HttpResult> Move(string srcBucket, string srcKey, string dstBucket, string dstKey)
        {
            return Move(srcBucket, srcKey, dstBucket, dstKey, false);
        }

        /// <summary>
        ///     移动文件 (with 'force' param)
        /// </summary>
        /// <param name="srcBucket">源空间</param>
        /// <param name="srcKey">源文件key</param>
        /// <param name="dstBucket">目标空间</param>
        /// <param name="dstKey">目标key</param>
        /// <param name="force">force标志,true/false</param>
        /// <returns>状态码为200时表示OK</returns>
        public async Task<HttpResult> Move(string srcBucket, string srcKey, string dstBucket, string dstKey, bool force)
        {
            var result = new HttpResult();

            try
            {
                var host = await _config.RsHost(_mac.AccessKey, srcBucket);
                var moveUrl = $"{host}{MoveOp(srcBucket, srcKey, dstBucket, dstKey, force)}";
                var token = _auth.CreateManageToken(moveUrl);

                result = await _httpManager.PostAsync(moveUrl, token);
            }
            catch (QiniuException ex)
            {
                var sb = new StringBuilder();
                sb.Append($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.ffff}] [move] Error:  ");
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }

                sb.AppendLine();

                result.Code = ex.HttpResult.Code;
                result.RefCode = ex.HttpResult.Code;
                result.Text = ex.HttpResult.Text;
                result.RefText += sb.ToString();
            }

            return result;
        }

        /// <summary>
        ///     修改文件MimeType
        /// </summary>
        /// <param name="bucket">空间名称</param>
        /// <param name="key">文件key</param>
        /// <param name="mimeType">修改后的MIME Type</param>
        /// <returns>状态码为200时表示OK</returns>
        public async Task<HttpResult> ChangeMime(string bucket, string key, string mimeType)
        {
            var result = new HttpResult();

            try
            {
                var host = await _config.RsHost(_mac.AccessKey, bucket);
                var chgmUrl = $"{host}{ChangeMimeOp(bucket, key, mimeType)}";
                var token = _auth.CreateManageToken(chgmUrl);
                result = await _httpManager.PostAsync(chgmUrl, token);
            }
            catch (QiniuException ex)
            {
                var sb = new StringBuilder();
                sb.Append($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.ffff}] [chgm] Error:  ");
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }

                sb.AppendLine();

                result.Code = ex.HttpResult.Code;
                result.RefCode = ex.HttpResult.Code;
                result.Text = ex.HttpResult.Text;
                result.RefText += sb.ToString();
            }

            return result;
        }

        /// <summary>
        ///     修改文件存储类型
        /// </summary>
        /// <param name="bucket">空间名称</param>
        /// <param name="key">文件key</param>
        /// <param name="fileType">修改后的文件存储类型，0表示普通存储，1表示低频存储</param>
        /// <returns>状态码为200时表示OK</returns>
        public async Task<HttpResult> ChangeType(string bucket, string key, int fileType)
        {
            var result = new HttpResult();

            try
            {
                var host = await _config.RsHost(_mac.AccessKey, bucket);
                var chtypeUrl = $"{host}{ChangeTypeOp(bucket, key, fileType)}";
                var token = _auth.CreateManageToken(chtypeUrl);
                result = await _httpManager.PostAsync(chtypeUrl, token);
            }
            catch (QiniuException ex)
            {
                var sb = new StringBuilder();
                sb.Append($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.ffff}] [chtype] Error:  ");
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }

                sb.AppendLine();

                result.Code = ex.HttpResult.Code;
                result.RefCode = ex.HttpResult.Code;
                result.Text = ex.HttpResult.Text;
                result.RefText += sb.ToString();
            }

            return result;
        }

        /// <summary>
        ///     批处理
        /// </summary>
        /// <param name="batchOps">批量操作的操作字符串</param>
        /// <returns>状态码为200时表示OK</returns>
        private async Task<BatchResult> Batch(string batchOps)
        {
            var result = new BatchResult();

            try
            {
                var scheme = _config.UseHttps ? "https://" : "http://";
                var rsHost = $"{scheme}{Config.DefaultRsHost}";
                var batchUrl = rsHost + "/batch";
                var data = Encoding.UTF8.GetBytes(batchOps);
                var token = _auth.CreateManageToken(batchUrl, data);

                var hr = await _httpManager.PostFormAsync(batchUrl, data, token);
                result.Shadow(hr);
            }
            catch (QiniuException ex)
            {
                var sb = new StringBuilder();
                sb.Append($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.ffff}] [batch] Error:  ");
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }

                sb.AppendLine();

                result.Code = ex.HttpResult.Code;
                result.RefCode = ex.HttpResult.Code;
                result.Text = ex.HttpResult.Text;
                result.RefText += sb.ToString();
            }

            return result;
        }

        /// <summary>
        ///     批处理，字符串数组拼接后与另一形式等价
        /// </summary>
        /// <param name="ops">批量操作的操作字符串数组</param>
        /// <returns>状态码为200时表示OK</returns>
        public Task<BatchResult> Batch(IList<string> ops)
        {
            return Batch(string.Join("&", ops.Select(op => $"op={op}")));
        }

        /// <summary>
        ///     抓取文件
        /// </summary>
        /// <param name="resUrl">资源URL</param>
        /// <param name="bucket">空间名称</param>
        /// <param name="key">文件key</param>
        /// <returns>状态码为200时表示OK</returns>
        public async Task<FetchResult> Fetch(string resUrl, string bucket, string key)
        {
            var result = new FetchResult();

            try
            {
                var host = await _config.IovipHost(_mac.AccessKey, bucket);
                var fetchUrl = $"{host}{FetchOp(resUrl, bucket, key)}";
                var token = _auth.CreateManageToken(fetchUrl);

                var httpResult = await _httpManager.PostAsync(fetchUrl, token);
                result.Shadow(httpResult);
            }
            catch (QiniuException ex)
            {
                var sb = new StringBuilder();
                sb.Append($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.ffff}] [fetch] Error:  ");
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }

                sb.AppendLine();

                result.Code = ex.HttpResult.Code;
                result.RefCode = ex.HttpResult.Code;
                result.Text = ex.HttpResult.Text;
                result.RefText += sb.ToString();
            }

            return result;
        }

        /// <summary>
        ///     更新文件，适用于"镜像源站"设置的空间
        /// </summary>
        /// <param name="bucket">空间名称</param>
        /// <param name="key">文件key</param>
        /// <returns>状态码为200时表示OK</returns>
        public async Task<HttpResult> Prefetch(string bucket, string key)
        {
            var result = new HttpResult();

            try
            {
                var host = await _config.IovipHost(_mac.AccessKey, bucket);
                var prefetchUrl = $"{host}{PrefetchOp(bucket, key)}";
                var token = _auth.CreateManageToken(prefetchUrl);

                result = await _httpManager.PostAsync(prefetchUrl, token);
            }
            catch (QiniuException ex)
            {
                var sb = new StringBuilder();
                sb.Append($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.ffff}] [prefetch] Error:  ");
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }

                sb.AppendLine();

                result.Code = ex.HttpResult.Code;
                result.RefCode = ex.HttpResult.Code;
                result.Text = ex.HttpResult.Text;
                result.RefText += sb.ToString();
            }

            return result;
        }

        /// <summary>
        ///     获取空间的域名
        /// </summary>
        /// <param name="bucket">空间名称</param>
        /// <returns>空间对应的域名</returns>
        public async Task<DomainsResult> Domains(string bucket)
        {
            var result = new DomainsResult();

            try
            {
                var scheme = _config.UseHttps ? "https://" : "http://";
                var rsHost = $"{scheme}{Config.DefaultApiHost}";
                var domainsUrl = $"{rsHost}/v6/domain/list";
                var body = $"tbl={bucket}";
                var data = Encoding.UTF8.GetBytes(body);
                var token = _auth.CreateManageToken(domainsUrl, data);

                var hr = await _httpManager.PostFormAsync(domainsUrl, data, token);
                result.Shadow(hr);
            }
            catch (QiniuException ex)
            {
                var sb = new StringBuilder();
                sb.Append($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.ffff}] [domains] Error:  ");
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }

                sb.AppendLine();

                result.Code = ex.HttpResult.Code;
                result.RefCode = ex.HttpResult.Code;
                result.Text = ex.HttpResult.Text;
                result.RefText += sb.ToString();
            }

            return result;
        }

        /// <summary>
        ///     获取空间文件列表
        ///     listFiles(bucket, prefix, marker, limit, delimiter)
        ///     bucket:    目标空间名称
        ///     prefix:    返回指定文件名前缀的文件列表(prefix可设为null)
        ///     marker:    考虑到设置limit后返回的文件列表可能不全(需要重复执行listFiles操作)
        ///     执行listFiles操作时使用marker标记来追加新的结果
        ///     特别注意首次执行listFiles操作时marker为null
        ///     limit:     每次返回结果所包含的文件总数限制(limit最大值1000，建议值100)
        ///     delimiter: 分隔符，比如-或者/等等，可以模拟作为目录结构(参考下述示例)
        ///     假设指定空间中有2个文件 fakepath/1.txt fakepath/2.txt
        ///     现设置分隔符delimiter = / 得到返回结果items =[]，commonPrefixes = [fakepath/]
        ///     然后调整prefix = fakepath/ delimiter = null 得到所需结果items = [1.txt,2.txt]
        ///     于是可以在本地先创建一个目录fakepath,然后在该目录下写入items中的文件
        /// </summary>
        /// <param name="bucket">空间名称</param>
        /// <param name="prefix">前缀</param>
        /// <param name="marker">标记</param>
        /// <param name="limit">数量限制</param>
        /// <param name="delimiter">分隔符</param>
        /// <returns>文件列表获取结果</returns>
        public async Task<ListResult> ListFiles(string bucket, string prefix, string marker, int limit, string delimiter)
        {
            var result = new ListResult();

            try
            {
                var sb = new StringBuilder($"/list?bucket={bucket}");

                if (!string.IsNullOrEmpty(marker))
                {
                    sb.Append($"&marker={marker}");
                }

                if (!string.IsNullOrEmpty(prefix))
                {
                    sb.Append($"&prefix={prefix}");
                }

                if (!string.IsNullOrEmpty(delimiter))
                {
                    sb.Append($"&delimiter={delimiter}");
                }

                if (limit > 1000 || limit < 1)
                {
                    sb.Append("&limit=1000");
                }
                else
                {
                    sb.Append($"&limit={limit}");
                }

                var host = await _config.RsfHost(_mac.AccessKey, bucket);
                var listUrl = $"{host}{sb}";
                var token = _auth.CreateManageToken(listUrl);

                var hr = await _httpManager.PostAsync(listUrl, token);
                result.Shadow(hr);
            }
            catch (QiniuException ex)
            {
                var sb = new StringBuilder();
                sb.Append($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.ffff}] [listFiles] Error:  ");
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }

                sb.AppendLine();

                result.Code = ex.HttpResult.Code;
                result.RefCode = ex.HttpResult.Code;
                result.Text = ex.HttpResult.Text;
                result.RefText += sb.ToString();
            }

            return result;
        }

        /// <summary>
        ///     更新文件生命周期
        /// </summary>
        /// <param name="bucket">空间名称</param>
        /// <param name="key">文件key</param>
        /// <param name="deleteAfterDays">多少天后删除</param>
        /// <returns>状态码为200时表示OK</returns>
        public async Task<HttpResult> DeleteAfterDays(string bucket, string key, int deleteAfterDays)
        {
            var result = new HttpResult();

            try
            {
                var host = await _config.RsHost(_mac.AccessKey, bucket);
                var updateUrl = $"{host}{DeleteAfterDaysOp(bucket, key, deleteAfterDays)}";
                var token = _auth.CreateManageToken(updateUrl);
                result = await _httpManager.PostAsync(updateUrl, token);
            }
            catch (QiniuException ex)
            {
                var sb = new StringBuilder();
                sb.Append($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.ffff}] [deleteAfterDays] Error:  ");
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }

                sb.AppendLine();

                result.Code = ex.HttpResult.Code;
                result.RefCode = ex.HttpResult.Code;
                result.Text = ex.HttpResult.Text;
                result.RefText += sb.ToString();
            }

            return result;
        }

        /// <summary>
        ///     生成stat操作字符串
        /// </summary>
        /// <param name="bucket">空间名称</param>
        /// <param name="key">文件key</param>
        /// <returns>stat操作字符串</returns>
        public string StatOp(string bucket, string key)
        {
            return $"/stat/{Base64.UrlSafeBase64Encode(bucket, key)}";
        }

        /// <summary>
        ///     生成delete操作字符串
        /// </summary>
        /// <param name="bucket">空间名称</param>
        /// <param name="key">文件key</param>
        /// <returns>delete操作字符串</returns>
        public string DeleteOp(string bucket, string key)
        {
            return $"/delete/{Base64.UrlSafeBase64Encode(bucket, key)}";
        }

        /// <summary>
        ///     生成copy操作字符串
        /// </summary>
        /// <param name="srcBucket">源空间</param>
        /// <param name="srcKey">源文件key</param>
        /// <param name="dstBucket">目标空间</param>
        /// <param name="dstKey">目标文件key</param>
        /// <returns>copy操作字符串</returns>
        public string CopyOp(string srcBucket, string srcKey, string dstBucket, string dstKey)
        {
            return CopyOp(srcBucket, srcKey, dstBucket, dstKey, false);
        }

        /// <summary>
        ///     生成copy(with 'force' param)操作字符串
        /// </summary>
        /// <param name="srcBucket">源空间</param>
        /// <param name="srcKey">源文件key</param>
        /// <param name="dstBucket">目标空间</param>
        /// <param name="dstKey">目标文件key</param>
        /// <param name="force">force标志,true/false</param>
        /// <returns>copy操作字符串</returns>
        public string CopyOp(string srcBucket, string srcKey, string dstBucket, string dstKey, bool force)
        {
            var fx = force ? "force/true" : "force/false";
            return $"/copy/{Base64.UrlSafeBase64Encode(srcBucket, srcKey)}/{Base64.UrlSafeBase64Encode(dstBucket, dstKey)}/{fx}";
        }

        /// <summary>
        ///     生成move操作字符串
        /// </summary>
        /// <param name="srcBucket">源空间</param>
        /// <param name="srcKey">源文件key</param>
        /// <param name="dstBucket">目标空间</param>
        /// <param name="dstKey">目标文件key</param>
        /// <returns>move操作字符串</returns>
        public string MoveOp(string srcBucket, string srcKey, string dstBucket, string dstKey)
        {
            return MoveOp(srcBucket, srcKey, dstBucket, dstKey, false);
        }

        /// <summary>
        ///     生成copy(with 'force' param)操作字符串
        /// </summary>
        /// <param name="srcBucket">源空间</param>
        /// <param name="srcKey">源文件key</param>
        /// <param name="dstBucket">目标空间</param>
        /// <param name="dstKey">目标文件key</param>
        /// <param name="force">force标志,true/false</param>
        /// <returns>move操作字符串</returns>
        public string MoveOp(string srcBucket, string srcKey, string dstBucket, string dstKey, bool force)
        {
            var fx = force ? "force/true" : "force/false";
            return $"/move/{Base64.UrlSafeBase64Encode(srcBucket, srcKey)}/{Base64.UrlSafeBase64Encode(dstBucket, dstKey)}/{fx}";
        }

        /// <summary>
        ///     生成chgm操作字符串
        /// </summary>
        /// <param name="bucket">空间名称</param>
        /// <param name="key">文件key</param>
        /// <param name="mimeType">修改后MIME Type</param>
        /// <returns>chgm操作字符串</returns>
        public string ChangeMimeOp(string bucket, string key, string mimeType)
        {
            return $"/chgm/{Base64.UrlSafeBase64Encode(bucket, key)}/mime/{Base64.UrlSafeBase64Encode(mimeType)}";
        }

        /// <summary>
        ///     生成chtype操作字符串
        /// </summary>
        /// <param name="bucket">空间名称</param>
        /// <param name="key">文件key</param>
        /// <param name="fileType">修改后文件类型</param>
        /// <returns>chtype操作字符串</returns>
        public string ChangeTypeOp(string bucket, string key, int fileType)
        {
            return $"/chtype/{Base64.UrlSafeBase64Encode(bucket, key)}/type/{fileType}";
        }

        /// <summary>
        ///     生成fetch操作字符串
        /// </summary>
        /// <param name="url">资源URL</param>
        /// <param name="bucket">空间名称</param>
        /// <param name="key">文件key</param>
        /// <returns>fetch操作字符串</returns>
        public string FetchOp(string url, string bucket, string key)
        {
            string entry;
            if (key == null)
            {
                entry = Base64.UrlSafeBase64Encode(bucket);
            }
            else
            {
                entry = Base64.UrlSafeBase64Encode(bucket, key);
            }

            return $"/fetch/{Base64.UrlSafeBase64Encode(url)}/to/{entry}";
        }

        /// <summary>
        ///     生成prefetch操作字符串
        /// </summary>
        /// <param name="bucket">空间名称</param>
        /// <param name="key">文件key</param>
        /// <returns>prefetch操作字符串</returns>
        public string PrefetchOp(string bucket, string key)
        {
            return $"/prefetch/{Base64.UrlSafeBase64Encode(bucket, key)}";
        }

        /// <summary>
        ///     生成updateLifecycle操作字符串
        /// </summary>
        /// <param name="bucket">空间名称</param>
        /// <param name="key">文件key</param>
        /// <param name="deleteAfterDays">多少天后删除(设为0表示取消)</param>
        /// <returns>updateLifecycle操作字符串</returns>
        public string DeleteAfterDaysOp(string bucket, string key, int deleteAfterDays)
        {
            return $"/deleteAfterDays/{Base64.UrlSafeBase64Encode(bucket, key)}/{deleteAfterDays}";
        }
    }
}

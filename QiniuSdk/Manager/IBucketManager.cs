using System.Collections.Generic;
using System.Threading.Tasks;
using QiniuSdk.Http;

namespace QiniuSdk.Storage
{
    public interface IBucketManager
    {
        Task<BatchResult> Batch(IList<string> ops);
        Task<BucketsResult> Buckets(bool shared);
        Task<HttpResult> ChangeMime(string bucket, string key, string mimeType);
        string ChangeMimeOp(string bucket, string key, string mimeType);
        Task<HttpResult> ChangeType(string bucket, string key, int fileType);
        string ChangeTypeOp(string bucket, string key, int fileType);
        Task<HttpResult> Copy(string srcBucket, string srcKey, string dstBucket, string dstKey);
        Task<HttpResult> Copy(string srcBucket, string srcKey, string dstBucket, string dstKey, bool force);
        string CopyOp(string srcBucket, string srcKey, string dstBucket, string dstKey);
        string CopyOp(string srcBucket, string srcKey, string dstBucket, string dstKey, bool force);
        Task<HttpResult> Delete(string bucket, string key);
        Task<HttpResult> DeleteAfterDays(string bucket, string key, int deleteAfterDays);
        string DeleteAfterDaysOp(string bucket, string key, int deleteAfterDays);
        string DeleteOp(string bucket, string key);
        Task<DomainsResult> Domains(string bucket);
        Task<FetchResult> Fetch(string resUrl, string bucket, string key);
        string FetchOp(string url, string bucket, string key);
        Task<ListResult> ListFiles(string bucket, string prefix, string marker, int limit, string delimiter);
        Task<HttpResult> Move(string srcBucket, string srcKey, string dstBucket, string dstKey);
        Task<HttpResult> Move(string srcBucket, string srcKey, string dstBucket, string dstKey, bool force);
        string MoveOp(string srcBucket, string srcKey, string dstBucket, string dstKey);
        string MoveOp(string srcBucket, string srcKey, string dstBucket, string dstKey, bool force);
        Task<HttpResult> Prefetch(string bucket, string key);
        string PrefetchOp(string bucket, string key);
        Task<StatResult> Stat(string bucket, string key);
        string StatOp(string bucket, string key);
    }
}
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QiniuSdk.Http;
using QiniuSdk.Storage;
using QiniuSdk.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace QiniuSdk
{
    public static class QiniuServiceCollectionExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configurationSection">
        /// configuration.GetSection("Qiniu")
        /// </param>
        /// <returns></returns>
        public static IServiceCollection AddQiniu(this IServiceCollection services, IConfigurationSection configurationSection)
        {
            QiniuOptions qiniuOptions = new QiniuOptions();
            configurationSection.Bind(qiniuOptions);
            services.Configure<QiniuOptions>(configurationSection);

            services.AddHttpClient<HttpManager>();
            services.AddScoped<IAuth, Auth>();
            services.AddScoped<Signature>();
            services.Configure<Mac>(mac => {
                mac.AccessKey = qiniuOptions.Mac.AccessKey;
                mac.SecretKey = qiniuOptions.Mac.SecretKey;
            });
            services.AddScoped<IFormUploader, FormUploader>();
            services.AddScoped<Config>();
            services.AddScoped<ZoneHelper>();
            services.AddScoped<IQiniu, Qiniu>();
            services.AddScoped<IBucketManager, BucketManager>();
            services.AddScoped<IDownloadManager, DownloadManager>();
            services.AddScoped<IOperationManager, OperationManager>();

            return services;
        }


        [Obsolete("replace this with AddQiniu(IConfigurationSection)")]
        public static IServiceCollection AddQiniu(this IServiceCollection services, QiniuOptions qiniuOptions)
        {
            services.AddHttpClient<HttpManager>();
            services.AddScoped<IAuth, Auth>();
            services.AddScoped<Signature>();
            services.Configure<Mac>(mac => {
                mac.AccessKey = qiniuOptions.Mac.AccessKey;
                mac.SecretKey = qiniuOptions.Mac.SecretKey;
            });
            services.AddScoped<IFormUploader, FormUploader>();
            services.AddScoped<Config>();
            services.AddScoped<ZoneHelper>();
            services.AddScoped<IQiniu, Qiniu>();
            services.AddScoped<IBucketManager, BucketManager>();
            services.AddScoped<IDownloadManager, DownloadManager>();
            services.AddScoped<IOperationManager, OperationManager>();

            return services;
        }
    }
}

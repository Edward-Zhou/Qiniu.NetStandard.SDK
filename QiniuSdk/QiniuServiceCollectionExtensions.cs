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
            return services;
        }
    }
}

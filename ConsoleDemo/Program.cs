// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using QiniuSdk;
using QiniuSdk.Storage;

Console.WriteLine("Hello, World!");

var builder = new ConfigurationBuilder()
   .SetBasePath(AppContext.BaseDirectory)
   .AddJsonFile("appsettings.json", true, true);
IConfiguration configuration = builder.Build();
IConfigurationSection? config = configuration.GetSection("Qiniu");

IServiceCollection servicesCollection = new ServiceCollection();
servicesCollection.AddQiniu(config);

var services = servicesCollection.BuildServiceProvider();

var bucketManager = services.GetRequiredService<IBucketManager>();
var result = await bucketManager.Buckets(true);
Console.WriteLine(result.ToString()); ;


var qiniu = services.GetRequiredService<IQiniu>();

// 存储空间名
string bucketName = "dao1-test";
PutPolicy putPolicy = new PutPolicy();
// 设置要上传的目标空间
putPolicy.Scope = bucketName;


await using var stream = new FileStream("./dapr.png", FileMode.Open);
await qiniu.UploadStream(stream, putPolicy, "image/dapr.png");

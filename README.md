# Qiniu.NetStandard.SDK
.Net Standard SDK For Qiniu

# Install Package

`install-package Edward.QiniuSdk`

# Use   

## `appsettings.json`   

```
{
  "Qiniu": {
    "Mac": {
      "AccessKey": "xx",
      "SecretKey": "xx"
    }
  },
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "System": "Information",
      "Microsoft": "Information"
    }
  }
}

```

## `Startup.cs`   

```
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<QiniuOptions>(Configuration.GetSection("Qiniu"));
            var QiniuOptions = services.BuildServiceProvider().GetRequiredService<IOptions<QiniuOptions>>().Value;
            services.AddQiniu(QiniuOptions);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }
    }
```

## `Controller`   

```
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class QiniuController : ControllerBase
    {
        private readonly IQiniu _qiniu;
        public QiniuController(IQiniu qiniu)
        {
            _qiniu = qiniu;
        }
        public async Task<IActionResult> Upload(IFormFile file)
        {
            var putPolicy = new PutPolicy
            {
                Scope = $"goldhouse",
                DeleteAfterDays = 1
            };

            var result = await _qiniu.UploadStream(file.OpenReadStream(), putPolicy, "Test.txt", null);
            return Ok(result);
        }
    }

```

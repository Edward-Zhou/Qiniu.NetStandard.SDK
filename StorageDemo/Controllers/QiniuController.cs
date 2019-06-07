using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QiniuSdk;

namespace StorageDemo.Controllers
{
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
            var result = await _qiniu.UploadStream(file.OpenReadStream(), "", null);
            return Ok(result);
        }
    }
}
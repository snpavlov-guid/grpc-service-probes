using GrpcServiceApp.Application;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.IO;
using System.Text;

namespace GrpcServiceApp.Controllers
{
    public class ProtosController : Controller
    {
        readonly IServiceProvider _provider;

        public ProtosController(IServiceProvider provider)
        {
            _provider = provider;
        }

        [HttpGet]
        [Route("[controller]/{name}")]
        [Route("[controller]/{version}/{name}")]
        public IActionResult Index(string version, string name)
        {
            var protosService = _provider.GetService<IProtosService>();

            var (fileName, content) = protosService.GetProto(version, name);

            if (string.IsNullOrEmpty(content))
            {
                return NotFound(string.IsNullOrEmpty(version) ?
                     $"Protobuf file '{name}.proto' not found!" :
                     $"Protobuf file '{Path.Combine(version, fileName)}' not found!");
            }

            return Content(content, "text/plain");
        }

        [HttpGet]
        [Route("[controller]/[action]")]
        public IActionResult GetProto(string version, string name)
        {
            var protosService = _provider.GetService<IProtosService>();

            var (fileName, content) = protosService.GetProto(version, name);

            return File(Encoding.UTF8.GetBytes(content), "text/plain", fileName);

        }

    }


}

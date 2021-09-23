using GrpcServiceApp.Application;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace GrpcServiceApp.Controllers
{
    public class HomeController : Controller
    {
        readonly IServiceProvider _provider;

        public HomeController(IServiceProvider provider)
        {
            _provider = provider;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var protosService = _provider.GetService<IProtosService>();

            return View(protosService.GetProtoList());
        }

        [HttpGet]
        public IActionResult Files()
        {
            var fileRepoService = _provider.GetService<IFileRepositoryService>();

            return View(fileRepoService.ListFiles());
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcServiceApp.Application
{
    public class ApplicationUrlService : IApplicationUrlService
    {
        private readonly IUrlHelper _urlHelper;

        public ApplicationUrlService(IUrlHelperFactory urlHelperFactory, IActionContextAccessor actionContextAccessor)
        {
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
        }

        public string ApplicationUrl => GetRootUrl();

        public string GetProtobuftUrl(string name, string version)
        {
            var protoUrl = $"{GetRootUrl()}{_urlHelper.Action("Index", "Protos")}";

            return !string.IsNullOrEmpty(version) ?
                $"{protoUrl}/{version}/{name}" :
                $"{protoUrl}/{name}";

        }

        protected string GetRootUrl()
        {
            var absurl = _urlHelper.Action("Action", "Controller", null, _urlHelper.ActionContext.HttpContext.Request.Scheme);
            return absurl.Substring(0, absurl.IndexOf(_urlHelper.Action("Action", "Controller")));
        }
    }
}

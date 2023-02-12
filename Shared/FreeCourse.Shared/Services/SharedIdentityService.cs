using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeCourse.Shared.Services
{
    public class SharedIdentityService : ISharedIdentityService
    {
        private IHttpContextAccessor _contextAccessor;

        public SharedIdentityService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
        //httpcontext accessor kullanabilmek için kullanılacak microservis içinde program.cs de tanımlatmak gerekir.addhttpcontextaccessor()
        //public string GetUserId => _contextAccessor.HttpContext.User.Claims.Where(x => x.Type == "sub").FirstOrDefault().Value;
        public string GetUserId => _contextAccessor.HttpContext.User.FindFirst("sub").Value;
    }
}

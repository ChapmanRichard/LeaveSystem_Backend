using Microsoft.AspNetCore.Http;

namespace Api.Common.Helper
{
    public class AppContextHelper
    {
        private static IHttpContextAccessor _httpContextAccessor;
        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public static HttpContext Current
        {
            get
            {
                return _httpContextAccessor.HttpContext;
            }
        }
    }
}

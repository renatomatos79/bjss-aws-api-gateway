namespace eventsapi.Helpers
{
    public static class CacheControlHelper
    {
        public static void UpdateMaxAge(this IHttpContextAccessor httpContextAccessor, long maxAgeInSeconds)
        {
            var headers = httpContextAccessor.HttpContext?.Response.GetTypedHeaders();
            if (headers != null)
            {
                headers.CacheControl = new Microsoft.Net.Http.Headers.CacheControlHeaderValue
                {
                    Public = true,
                    MaxAge = TimeSpan.FromSeconds(maxAgeInSeconds)
                };
            }
        }
    }
}

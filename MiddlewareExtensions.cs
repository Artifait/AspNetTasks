namespace AspNetTasks
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomHeaders(this IApplicationBuilder app)
        {
            return app.Use(async (context, next) =>
            {
                context.Response.Headers.ContentLanguage = "ru-RU";
                context.Response.Headers.ContentType = "text/html; charset=utf-8";

                await next();
            });
        }
    }

}

namespace WebApiAutores1.Middleware
{
    public static class LoguearrespuestaHTMLMiddlewareExtensions
    {
        public static IApplicationBuilder UseLoguearrespuestaHTTP(this IApplicationBuilder app)
        {
            return app.UseMiddleware<LoguearrespuestaHTTPMiddleware>();
        }
    }

    public class LoguearrespuestaHTTPMiddleware
    {
        private readonly RequestDelegate siguiente;
        private readonly ILogger<LoguearrespuestaHTTPMiddleware> logger;

        public LoguearrespuestaHTTPMiddleware(RequestDelegate siguiente, ILogger<LoguearrespuestaHTTPMiddleware> logger)
        {
            this.siguiente = siguiente;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext contexto)
        {
            using (var ms = new MemoryStream())
            {
                var cuerpoOriginalRespuesta = contexto.Response.Body;
                contexto.Response.Body = ms;

                await siguiente(contexto);

                ms.Seek(0, SeekOrigin.Begin);
                string respuesta = new StreamReader(ms).ReadToEnd();
                ms.Seek(0, SeekOrigin.Begin);

                await ms.CopyToAsync(cuerpoOriginalRespuesta);
                contexto.Response.Body = cuerpoOriginalRespuesta;

                logger.LogInformation(respuesta);
            }
        }
    }
}

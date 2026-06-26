namespace MVC_FinalProject
{
    public class SessionMiddleware
    {
        private readonly RequestDelegate _next;

        public SessionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value?.ToLower();

            // Admin zonası üçün session yoxla
            if (path.StartsWith("/admin"))
            {
                var token = context.Session.GetString("AuthToken");

                // Session yoxdursa, login səhifəsinə yönləndir
                if (string.IsNullOrEmpty(token))
                {
                    context.Response.Redirect("/account/login");
                    return;
                }
            }

            // Növbəti middleware-ə keç
            await _next(context);
        }
    }
}

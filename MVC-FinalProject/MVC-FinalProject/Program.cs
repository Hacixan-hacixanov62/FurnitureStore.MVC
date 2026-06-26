using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Session;
using MVC_FinalProject;
using Service;
using Stripe;
using SessionMiddleware = MVC_FinalProject.SessionMiddleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication("MyCookieAuth")
    .AddCookie("MyCookieAuth", options =>
    {
        options.LoginPath = "/Account/Login";

        options.Events.OnRedirectToAccessDenied = context =>
        {
            context.Response.StatusCode = 401;
            context.Response.Redirect("/Unauthorized/Index");
            return Task.CompletedTask;
        };

        options.AccessDeniedPath = "/Unauthorized/Index";
    });


builder.Services.AddAuthorization();

builder.Services.AddDistributedMemoryCache();


builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromDays(1);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient();

builder.Services.Configure<IdentityOptions>(opt =>
{
    opt.User.RequireUniqueEmail = true;
    opt.Password.RequiredUniqueChars = 1;
    opt.Password.RequireNonAlphanumeric = true;
    opt.Password.RequireDigit = true;
    opt.Password.RequireLowercase = true;
    opt.Password.RequireUppercase = true;
    opt.SignIn.RequireConfirmedEmail = true;
});
builder.Services.AddServiceLayer();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseMiddleware<GlobalExceptionHandler>();

app.UseStaticFiles();

app.UseSession();
app.UseMiddleware<SessionMiddleware>();
app.UseRouting();
StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();

app.Use(async (context, next) =>
{
    var token = context.Session.GetString("AuthToken");
    if (!string.IsNullOrEmpty(token))
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        var claims = jwtToken.Claims.Select(c =>
        {
            if (c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")
                return new Claim(ClaimTypes.NameIdentifier, c.Value);

            if (c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")
                return new Claim(ClaimTypes.Role, c.Value);

            return new Claim(c.Type, c.Value);
        });

        var identity = new ClaimsIdentity(claims, "jwt");
        var principal = new ClaimsPrincipal(identity);
        context.User = principal;

        Thread.CurrentPrincipal = principal;
    }

    await next();
});

app.UseAuthentication();

app.UseStatusCodePages(async context =>
{
    var response = context.HttpContext.Response;

    if (response.StatusCode == 401 || response.StatusCode == 403)
    {
        response.Redirect("/Unauthorized/Index");
    }

    await Task.CompletedTask; // await use
});


app.UseAuthorization();


app.UseStatusCodePages(async context =>
{
    var response = context.HttpContext.Response;

    if (response.StatusCode == 404)
    {
        response.Redirect("/NotFound/Index");
    }

    await Task.CompletedTask; // await use
});

app.UseStatusCodePages(async context =>
{
    var response = context.HttpContext.Response;

    if (response.StatusCode == 500)
    {
        response.Redirect("/ServerError/Index");
    }

    await Task.CompletedTask; // await use
});



app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

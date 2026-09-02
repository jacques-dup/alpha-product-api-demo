using Microsoft.AspNetCore.HttpOverrides;
using Product.ApplicationRoot;
using Product.Bff;
using Product.WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddConsole();

var applicationInsightsConnectionString = builder.Configuration["ApplicationInsights:ConnectionString"];
if (!string.IsNullOrWhiteSpace(applicationInsightsConnectionString))
{
    builder.Services.AddApplicationInsightsTelemetry();
}

builder.Services.AddProductModules(builder.Configuration, builder.Environment);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // Vite (HTTPS :5173) proxies /bff and /signin-oidc here. Honor X-Forwarded-*
    // so the OIDC redirect_uri is https://localhost:5173/signin-oidc, not Kestrel's host.
    var forwarded = new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor
            | ForwardedHeaders.XForwardedProto
            | ForwardedHeaders.XForwardedHost
    };
    forwarded.KnownNetworks.Clear();
    forwarded.KnownProxies.Clear();
    app.UseForwardedHeaders(forwarded);
}

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseProductWebApi();

app.UseAuthentication();
app.UseProductBff();
app.UseAuthorization();

app.MapProductBff();
app.MapProductWebApi();

app.Run();

public partial class Program;

using Blazor_LettuceEncrypt.Data;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddLettuceEncrypt();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

builder.WebHost.ConfigureKestrel((context, options) =>
{
    var appServices = options.ApplicationServices;
    options.Listen(IPAddress.Any, 5443);
    options.Listen(IPAddress.Any, 7080,
        o => o.UseHttps(h =>
        {
            h.ClientCertificateMode = ClientCertificateMode.AllowCertificate;
            h.UseLettuceEncrypt(appServices);
        }));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHttpsRedirection();
    app.UseHsts();
}

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

using System.Net;
using IdentityWebApplication.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
//string certificateFileName = "testcert.pfx";
//string completeFilePath = Path.Join("certificate", certificateFileName);
////string completeFilePath = @"C:\Users\subhash1202\source\repos\IdentityWebApplication\ssl-iam-identity-docker.pfx";
//if (File.Exists(completeFilePath))
//{
//    Console.Write($"File found at path: {completeFilePath}");
//    builder.WebHost.ConfigureKestrel((context, serverOptions) =>
//    {
//        serverOptions.Listen(IPAddress.Any, 5005, listenOptions =>
//        {
//            listenOptions.UseHttps(completeFilePath, "testbar");
//        });
//    });
//}
//else if (File.Exists(certificateFileName))
//{
//    Console.Write($"File found at path: {certificateFileName}");
//    builder.WebHost.ConfigureKestrel((context, serverOptions) =>
//    {
//        serverOptions.Listen(IPAddress.Any, 5005, listenOptions =>
//        {
//            listenOptions.UseHttps(certificateFileName, "eEtNrrgzIApOTWglaySv");
//        });
//    });
//}
//else
//{
//Console.Write($"Default Kestrel server is setup");
//builder.WebHost.UseKestrel();
//}




// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();

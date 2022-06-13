using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using WhatWasThatBlog.Data;
using WhatWasThatBlog.Helpers;
using WhatWasThatBlog.Models;
using WhatWasThatBlog.Services;
using WhatWasThatBlog.Services.Interfaces;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var connectionString = ConnectionService.GetConnectionString(builder.Configuration);


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString)); 


builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//Jason thinks this should be updated in order to specify BOTH the User and Role types
builder.Services.AddIdentity<BlogUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddDefaultTokenProviders().AddDefaultUI().AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddTransient<DataService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<DisplayService>();
builder.Services.AddScoped<IEmailSender, BasicEmailService>();
builder.Services.AddScoped<SearchService>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "What Was That Blog API",
        Version = "v1",
        Description = "Serving Up Blog data for you using .Net 6",
        Contact = new OpenApiContact
        {
            Name = "Tricia Chitwood",
            Email = "TChitwoodCoding@gmail.com",
            Url = new Uri("https://www.TriciaChitwoodArt.com")
        }
    });
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI(c =>
{

    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Public API");
    c.InjectStylesheet("~/css/swaggerr.css");
    c.InjectJavascript("~/js/swagger.js");
    c.DocumentTitle = "WWT Blog Public API";
});

var shell = app.Services.CreateScope();
var dataService = shell.ServiceProvider.GetRequiredService<DataService>();
await dataService.SetupDBAsync();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "details",
    pattern: "PostDetails/{slug}",
    defaults: new { controller = "BlogPosts", action="Details" }
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();

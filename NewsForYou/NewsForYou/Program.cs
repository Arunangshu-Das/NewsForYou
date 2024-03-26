using Microsoft.EntityFrameworkCore;
using NewsForYou.Models;
using NewsForYou.Business;
using NewsForYou.DAL;
using NewsForYou.DAL.Models;
using Microsoft.Extensions.Options;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options => {
    options.AddPolicy("AllowOrigin",
        builder =>
        {
            builder.WithOrigins("http://127.0.0.1:5500", "https://127.0.0.1:5500")
                   .AllowAnyMethod()  // You might need this to allow any method, or you can specify specific methods.
                   .AllowAnyHeader()  // Allow any header, or you can specify specific headers.
                   .AllowCredentials();  // If your request includes credentials like cookies, you need to allow credentials.
        });
});

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSession();

string loggingFolderPath = builder.Configuration.GetValue<string>("LoggingFolderPath");

builder.Services.AddScoped<IService, Service>();
builder.Services.AddScoped<IDataAccess, DataAccess>();
builder.Services.AddSingleton<NewsForYou.Logger.ILogger>(new NewsForYou.Logger.Logger(loggingFolderPath));

builder.Services.AddDbContext<NewsForYouContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DBCS")));


builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen();


var app = builder.Build();

app.UseCors("AllowOrigin");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.UseSwagger();
app.UseSwaggerUI(
    options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
        options.DocumentTitle = "Login";
    }
    );


app.Run();

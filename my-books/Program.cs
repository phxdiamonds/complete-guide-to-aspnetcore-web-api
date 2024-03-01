using Microsoft.OpenApi.Models;
using my_books.Data;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using my_books.Data.Services;
using my_books.Exceptions;

using Serilog;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<AppDbContext>(option => option.UseSqlServer(@"Data Source=LAPTOP-RGC0BD9H\SQLEXPRESS;Initial Catalog=my-books-db;Integrated Security=True;Pooling=False;Encrypt=True;Trust Server Certificate=True;"));
builder.Services.AddTransient<BooksServices>();
builder.Services.AddTransient<AuthorsService>();
builder.Services.AddTransient<PublishersService>();
builder.Services.AddTransient<LogsService>();

builder.Services.AddApiVersioning(config =>
{
    config.DefaultApiVersion = new ApiVersion(1,0);
    config.AssumeDefaultVersionWhenUnspecified = true;
    //config.ApiVersionReader = new HeaderApiVersionReader("custom-version-header");  //for custom header
    //config.ApiVersionReader = new MediaTypeApiVersionReader(); //for custom media type
});

//Log.Logger = new LoggerConfiguration().WriteTo.File(@"Logs\log.txt",rollingInterval:RollingInterval.Day).CreateLogger();

var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();// used this so that it can read form the appsettings.json which defined tha above configuration
builder.Services.AddLogging().AddSerilog();//for logging


builder.Services.AddSwaggerGen(c=>
{
    c.SwaggerDoc("v2", new OpenApiInfo { Title = "my_books_updated_title", Version = "v2" });
});

var app = builder.Build();
app.UseSerilogRequestLogging();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v2/swagger.json", "my_books_ui_updated v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();



//Exception Handling

app.ConfigureBuildiInExceptions(loggerFactory: app.Services.GetRequiredService<ILoggerFactory>());


app.ConfigureCustomExceptionHandler();



app.MapControllers();

//AppDbInitializer.Seed(app);

app.Run();

using Microsoft.OpenApi.Models;
using my_books.Data;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using my_books.Data.Services;
using my_books.Exceptions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<AppDbContext>(option => option.UseSqlServer(@"Data Source=LAPTOP-RGC0BD9H\SQLEXPRESS;Initial Catalog=my-books-db;Integrated Security=True;Pooling=False;Encrypt=True;Trust Server Certificate=True;"));
builder.Services.AddTransient<BooksServices>();
builder.Services.AddTransient<AuthorsService>();
builder.Services.AddTransient<PublishersService>();
builder.Services.AddSwaggerGen(c=>
{
    c.SwaggerDoc("v2", new OpenApiInfo { Title = "my_books_updated_title", Version = "v2" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v2/swagger.json", "my_books_ui_updated v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

//Exception Handling
app.ConfigureBuildiInExceptions();
//app.ConfigureCustomExceptionHandler();



app.MapControllers();

//AppDbInitializer.Seed(app);

app.Run();

using Coravel;
using CoravelProject.Data;
using CoravelProject.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Coravel's services
builder.Services.AddScheduler();
builder.Services.AddQueue();
builder.Services.AddEvents();
//builder.Services.AddNotification();

builder.Services.AddTransient<TodoItemBackgroundTask>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.ConfigureWarnings(builder =>
    {
    });
    options.UseSqlServer(
        connectionString: builder.Configuration.GetConnectionString("DefaultConnection"));
},
                contextLifetime: ServiceLifetime.Scoped,
                optionsLifetime: ServiceLifetime.Scoped);

var app = builder.Build();

app.Services.UseScheduler(scheduler =>
           scheduler
               .Schedule<TodoItemBackgroundTask>()
               .EveryFifteenSeconds()
       );

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

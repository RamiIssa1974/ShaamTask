using ShamTask.Api.Application;
using ShamTask.Api.Infrastructure;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ITaskRepository, JsonTaskRepository>();
builder.Services.AddSingleton<TaskService>();

var allowedOrigins = new[] {
    "https://stackblitz.com",
    "https://*.stackblitz.io"
};

// CORS (מאפשר מה-StackBlitz)
builder.Services.AddCors(options =>
{
    options.AddPolicy("StackBlitz", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .SetIsOriginAllowed(origin =>
                    origin == "http://shaam.creativehandsco.com" ||
                    origin == "http://localhost:4200"||
                    origin == "https://stackblitz.com" ||
                    origin.EndsWith(".stackblitz.io"))
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();
 
app.UseHttpsRedirection();
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("StackBlitz");
app.MapControllers();
app.Run();

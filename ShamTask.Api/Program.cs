using ShamTask.Api.Application;
using ShamTask.Api.Infrastructure;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services
    .AddControllers()
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        opt.JsonSerializerOptions.PropertyNameCaseInsensitive = true;    
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ITaskRepository, JsonTaskRepository>();
builder.Services.AddSingleton<TaskService>();

var allowedOrigins = new[] {
    "https://stackblitz.com",
    "https://*.stackblitz.io"
};

builder.Services.AddCors(options =>
{
    options.AddPolicy("StackBlitz", policy =>
    {
        policy
            .SetIsOriginAllowed(origin =>
            {
                try
                {
                    var host = new Uri(origin).Host.ToLowerInvariant();
                    return host.EndsWith("stackblitz.io")
                        || host.EndsWith("webcontainer.io")
                        || host.EndsWith("ngrok-free.app")
                        || host.EndsWith("ngrok.app")
                        || host == "shaam.creativehandsco.com"
                        || host == "localhost";
                }
                catch { return false; }
            })
            .AllowAnyHeader()
            .AllowAnyMethod();
        // הימנע מ-AllowCredentials אלא אם באמת חייבים cookies cross-origin
    });
});

 

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();
 
app.UseHttpsRedirection();
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("StackBlitz");
app.MapControllers().RequireCors("StackBlitz");
app.Run();

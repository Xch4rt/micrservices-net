using Polly;
using HelloDotnet5;
var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllers();
builder.Services.Configure<ServiceSettings>(builder.Configuration.GetSection(nameof(HelloDotnet5.ServiceSettings)));
builder.Services.AddHttpClient<WeatherClient>()
    .AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(10, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))) //exponential backoff
    .AddTransientHttpErrorPolicy(builder => builder.CircuitBreakerAsync(3, TimeSpan.FromSeconds(15)));//circuit breaker

builder.Services.AddHealthChecks()
    .AddCheck<ExternalEndpointHealthCheck>("OpenWeather");

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");
app.Run();

using Microsoft.AspNetCore.Mvc;

namespace HelloDotnet5.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly WeatherClient client;
    public WeatherForecastController(ILogger<WeatherForecastController> logger, WeatherClient client)
    {
        _logger = logger;
        this.client = client;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    [Route("{city}")]
    public async Task<WeatherForecast> Get(string city)
    {
        var forecast = await client.GetCurrentWeatherAsync(city);

        return new WeatherForecast{
            Summary = forecast.weather[0].description,
            TemperatureC = (int)forecast.main.temp,
            Date = DateTimeOffset.FromUnixTimeSeconds(forecast.dt).DateTime
        };
    }
}

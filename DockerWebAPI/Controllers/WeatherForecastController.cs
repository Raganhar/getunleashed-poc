using Dapper;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Unleash;

namespace DockerWebAPI.Controllers;

[ApiController]
[Route("")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;
    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public bool Get([FromServices]IUnleash featureToggle)
    {
        return featureToggle.IsEnabled(Consts.FeatureToggle.SomeKey);
    }
}
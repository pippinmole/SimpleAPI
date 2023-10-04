using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleAPI.Database;

namespace SimpleAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase {
    private static readonly string[] Summaries = new[] {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly DatabaseContext _context;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, DatabaseContext context)
    {
        _logger = logger;
        _context = context;
    }
    
    [HttpPost]
    public async Task<IActionResult> AddTestModel([FromBody] TestModel model)
    {
        await _context.TestModels.AddAsync(model);
        await _context.SaveChangesAsync();
        return Ok();
    }
    
    [HttpGet]
    public async Task<IActionResult> GetTestModels()
    {
        var data = await _context.TestModels.ToListAsync();
        return Ok(data);
    }
    
    // [HttpGet(Name = "GetWeatherForecast")]
    // public IEnumerable<WeatherForecast> Get() {
    //     return Enumerable.Range(1, 5).Select(index => new WeatherForecast {
    //             Date = DateTime.Now.AddDays(index),
    //             TemperatureC = Random.Shared.Next(-20, 55),
    //             Summary = Summaries[Random.Shared.Next(Summaries.Length)]
    //         })
    //         .ToArray();
    // }

    [HttpGet("/GetHostName")]
    public string GetHostName() => Environment.MachineName;
}
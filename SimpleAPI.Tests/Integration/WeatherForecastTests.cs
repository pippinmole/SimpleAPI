using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using SimpleAPI.Database;
using Testcontainers.PostgreSql;

namespace SimpleAPI.Tests.Integration;

public class GreetingControllerIntegrationTest {
    private WebApplicationFactory<Program> _factory = null!;
    private HttpClient _client = null!;
    private readonly PostgreSqlContainer _postgresContainer = new PostgreSqlBuilder().Build();

    [SetUp]
    public async Task SetupAsync()
    {
        await _postgresContainer.StartAsync();
        
        _factory = new CustomWebApplicationFactory<Program>(_postgresContainer.GetConnectionString());
        _client = _factory.CreateClient();
    }

    [Test]
    public async Task CreateGreeting_ReturnsOk()
    {
        // Arrange

        // Act
        var model = new TestModel { Value = "Hello World" };
        _ = await _client.PostAsync("/WeatherForecast",
            new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"));

        var response2 = await _client.GetAsync("/WeatherForecast");
        var json = await response2.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<List<TestModel>>(json);

        // Assert
        Assert.That(response2.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(data, Is.Not.Empty);
    }
    
    [OneTimeTearDown]
    public async Task OneTimeTearDownAsync()
    {
        await _postgresContainer.StopAsync();
        await _postgresContainer.DisposeAsync();
    }

    [Test]
    public async Task GetGreeting_ReturnsHelloWorld()
    {
        // Act
        var response = await _client.GetAsync("/WeatherForecast");

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [TearDown]
    public void TearDown()
    {
        _client?.Dispose();
        _factory?.Dispose();
    }
}
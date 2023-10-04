using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using SimpleAPI.Database;

namespace SimpleAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    private readonly IValidator<TestModel> _validator;

    public TestController(IValidator<TestModel> validator)
    {
        _validator = validator;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var model = new TestModel
        {
            Id = 1,
            // Value = "Test"
        };

        var context = new ValidationContext<TestModel>(model);
        var result = _validator.Validate(context);

        return Ok(result);
    }
}
using FluentValidation;
using SimpleAPI.Database;

namespace SimpleAPI.Models;

public class TestModelValidator : AbstractValidator<TestModel>
{
    public TestModelValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithSeverity(Severity.Info);
        RuleFor(x => x.Value)
            .NotEmpty()
            .WithSeverity(Severity.Info);
    }
}
using FluentValidation;

namespace webapi.Core.Application.Products;


public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Dto.Description).MaximumLength(500);
    }
}

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Dto.Id).GreaterThan(0);
        RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(100);
    }
}
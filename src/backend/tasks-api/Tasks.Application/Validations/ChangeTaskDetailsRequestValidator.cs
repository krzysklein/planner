using FluentValidation;
using Tasks.Application.Dto;

namespace Tasks.Application.Validations
{
    public class ChangeTaskDetailsRequestValidator : AbstractValidator<ChangeTaskDetailsRequest>
    {
        public ChangeTaskDetailsRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);
        }
    }
}

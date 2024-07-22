using FluentValidation;
using Tasks.Application.Dto;
using Tasks.Domain;

namespace Tasks.Application.Validations
{
    public class CreateTaskRequestValidator : AbstractValidator<CreateTaskRequest>
    {
        public CreateTaskRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.State)
                .IsEnumName(typeof(TaskState), caseSensitive: false);
        }
    }

}

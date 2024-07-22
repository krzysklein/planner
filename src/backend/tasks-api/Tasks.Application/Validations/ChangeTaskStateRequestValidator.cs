using FluentValidation;
using Tasks.Application.Dto;
using Tasks.Domain;

namespace Tasks.Application.Validations
{
    public class ChangeTaskStateRequestValidator : AbstractValidator<ChangeTaskStateRequest>
    {
        public ChangeTaskStateRequestValidator()
        {
            RuleFor(x => x.State)
                .IsEnumName(typeof(TaskState), caseSensitive: false);
        }
    }

}

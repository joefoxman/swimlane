using FluentValidation;
using Shared.ViewModels;

namespace Shared.Validators {
    public class ServiceModelValidator : AbstractValidator<ServiceInputModel> {
        public ServiceModelValidator() {
            RuleFor(x => x.IpOrDomain)
                .NotEmpty().NotNull()
                .WithMessage("IP Address or Domain Name is required.")
                .MaximumLength(150)
                .WithMessage("max length is 150 characters.");

            RuleFor(x => x.Services)
                .Must(serviceTypes => serviceTypes.IsInServiceType())
                .WithMessage("Services must be from the list.");
        }
    }
}

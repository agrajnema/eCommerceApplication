using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementApi.Clean.Application.Features.Orders.Commands.CheckoutOrderCommand
{
    public class CheckoutOrderCommandValidator : AbstractValidator<CheckoutOrderCommand>
    {
        public CheckoutOrderCommandValidator()
        {
            RuleFor(o => o.UserName).NotNull().NotEmpty().WithMessage("{UserName} is required").MaximumLength(50).WithMessage("{UserName} cannot exceed 50 characters");
            RuleFor(o => o.EmailAddress).NotNull().NotEmpty().WithMessage("{EmailAddress} is required");
            RuleFor(o => o.TotalPrice).NotNull().WithMessage("{TotalPrice} is required").GreaterThan(0).WithMessage("{TotalPrice} should be greater than zero");
        }
    }
}

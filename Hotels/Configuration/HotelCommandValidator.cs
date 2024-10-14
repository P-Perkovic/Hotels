using FluentValidation;
using Hotels.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hotels.Configuration
{
    public class HotelCommandValidator : AbstractValidator<HotelCommand>
    {
        public HotelCommandValidator()
        {
            RuleFor(h => h)
                .NotNull()
                .WithMessage("Hotel details missing.");

            RuleFor(h => h.Name)
                .NotEmpty()
                .WithMessage("Hotel name is required.")
                .MaximumLength(255)
                .WithMessage("Max hotel name length is 255.");

            RuleFor(h => h.Price)
                .NotEqual(0m)
                .WithMessage("Hotel price is required.");

            RuleFor(h => h.Longitude)
                .Must(h => h > -180d && h < 180d)
                .WithMessage("Hotel longitude range is from -180 to 180");

            RuleFor(h => h.Latitude)
                .Must(h => h > -90d && h < 90d)
                .WithMessage("Hotel latitude range is from -90 to 90");
        }
    }
}

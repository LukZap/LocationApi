using FluentValidation;
using LocationApi.Enums;
using LocationApi.Helpers;
using LocationApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LocationApi.Validators
{
    public class GeolocationViewModelValidator : AbstractValidator<GeolocationViewModel>
    {
        public GeolocationViewModelValidator()
        {
            RuleFor(x => x.City).NotEmpty();
            RuleFor(x => x.Continent).NotEmpty();
            RuleFor(x => x.Continent).IsEnumName(typeof(Continent));

            RuleFor(x => x.Country).NotEmpty();

            RuleFor(x => x.Lat).NotEmpty();
            RuleFor(x => x.Lat).LessThanOrEqualTo(180);
            RuleFor(x => x.Lat).GreaterThanOrEqualTo(-180);

            RuleFor(x => x.Lng).NotEmpty();
            RuleFor(x => x.Lng).LessThanOrEqualTo(180);
            RuleFor(x => x.Lng).GreaterThanOrEqualTo(-180);

            When(x => !string.IsNullOrWhiteSpace(x.IP), () =>
            {
                RuleFor(x => x.IP).NotEmpty();
                RuleFor(x => x.IP).Must(x => IPHelper.TryParseIPAddress(x, out System.Net.IPAddress _));
                RuleFor(x => x.IPType).NotEmpty();
                RuleFor(x => x.IPType).IsEnumName(typeof(IPType));
            });

            When(x => !string.IsNullOrWhiteSpace(x.Url), () =>
            {
                RuleFor(x => x.Url).NotEmpty();
                RuleFor(x => x.Url)
                    .Must(x => x.Split('.').Length >= 2)
                    .WithMessage("Must be valid Url authority name");
            });

            RuleFor(x => x.IP).NotEmpty().When(x => string.IsNullOrEmpty(x.Url));
            RuleFor(x => x.Url).NotEmpty().When(x => string.IsNullOrEmpty(x.IP));
        }
    }
}
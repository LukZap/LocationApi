using LocationApi.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationApi.Test.Fixtures
{
    public class GeolocationViewModelValidatorFixture
    {
        public GeolocationViewModelValidator Validator { get; }

        public GeolocationViewModelValidatorFixture()
        {
            Validator = new GeolocationViewModelValidator();
        }
    }
}

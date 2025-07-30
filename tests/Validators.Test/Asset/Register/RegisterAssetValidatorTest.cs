using InventarioEscolar.Application.UsesCases.AssetCase.Register;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validators.Test.Asset.Register
{
    public class RegisterAssetValidatorTest
    {
        [Fact]
        public void Validate_WhenAllFieldsAreValid_ShouldNotHaveAnyValidationErrors()
        {
            var validator = new RegisterAssetValidator();

        }
    }
}

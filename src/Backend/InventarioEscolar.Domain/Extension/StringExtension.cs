using InventarioEscolar.Api.Extension;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioEscolar.Domain.Extension
{
    public static class StringExtension
    {
        public static bool NotEmpty([NotNullWhen(true)] this string? value) => string.IsNullOrWhiteSpace(value).IsFalse();

    }
}

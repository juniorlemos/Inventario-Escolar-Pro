using InventarioEscolar.Api.Extension;
using System.Diagnostics.CodeAnalysis;

namespace InventarioEscolar.Domain.Extension
{
    public static class StringExtension
    {
        public static bool NotEmpty([NotNullWhen(true)] this string? value) => string.IsNullOrWhiteSpace(value).IsFalse();

    }
}

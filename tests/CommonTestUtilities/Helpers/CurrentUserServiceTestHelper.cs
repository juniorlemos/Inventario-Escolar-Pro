using CommonTestUtilities.Services;
using InventarioEscolar.Application.Services.Interfaces;

namespace CommonTestUtilities.Helpers
{
    public static class CurrentUserServiceTestHelper
    {
        public static ICurrentUserService CreateCurrentUserService(bool isAuthenticated, long? schoolId = null)
        {
            var builder = new CurrentUserServiceBuilder().IsAuthenticated(isAuthenticated);

            if (isAuthenticated && schoolId.HasValue)
            {
                builder = builder.WithSchoolId(schoolId.Value);
            }

            return builder.Build();
        }
    }
}
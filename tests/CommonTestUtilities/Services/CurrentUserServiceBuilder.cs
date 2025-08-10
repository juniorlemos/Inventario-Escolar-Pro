using InventarioEscolar.Application.Services.Interfaces;
using NSubstitute;

namespace CommonTestUtilities.Services
{
    public class CurrentUserServiceBuilder
    {
        private readonly ICurrentUserService _currentUser;

        public CurrentUserServiceBuilder()
        {
            _currentUser = Substitute.For<ICurrentUserService>();
        }

        public CurrentUserServiceBuilder WithSchoolId(long schoolId)
        {
            _currentUser.SchoolId.Returns(schoolId);
            return this;
        }
        public CurrentUserServiceBuilder IsAuthenticatedTrue()
        {
            _currentUser.IsAuthenticated.Returns(true);
            return this;
        }

        public CurrentUserServiceBuilder IsAuthenticatedFalse()
        {
            _currentUser.IsAuthenticated.Returns(false);
            return this;
        }

        public CurrentUserServiceBuilder IsAuthenticated(bool isAuthenticated)
        {
            _currentUser.IsAuthenticated.Returns(isAuthenticated);
            return this;
        }
        public ICurrentUserService Build() => _currentUser;
    }
}
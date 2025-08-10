using InventarioEscolar.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using NSubstitute;

namespace CommonTestUtilities.Services
{
    public class UserManagerBuilder
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserManagerBuilder()
        {
            _userManager = Substitute.For<UserManager<ApplicationUser>>(
                Substitute.For<IUserStore<ApplicationUser>>(),
                null, // IOptions<IdentityOptions>
                null, // IPasswordHasher<ApplicationUser>
                null, // IEnumerable<IUserValidator<ApplicationUser>>
                null, // IEnumerable<IPasswordValidator<ApplicationUser>>
                null, // ILookupNormalizer
                null, // IdentityErrorDescriber
                null, // IServiceProvider
                null  // ILogger<UserManager<ApplicationUser>>
            );
        }

        public UserManagerBuilder WithUserFoundByEmail(string email, ApplicationUser user)
        {
            _userManager.FindByEmailAsync(email).Returns(user);
            return this;
        }

        public UserManagerBuilder WithNoUserFoundByEmail(string email)
        {
            _userManager.FindByEmailAsync(email).Returns((ApplicationUser?)null);
            return this;
        }

        public UserManagerBuilder WithPasswordResetToken(ApplicationUser user, string token)
        {
            _userManager.GeneratePasswordResetTokenAsync(user).Returns(token);
            return this;
        }

        public UserManager<ApplicationUser> Build()
        {
            return _userManager;
        }
    }
}
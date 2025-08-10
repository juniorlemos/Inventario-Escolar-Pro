using Bogus;
using InventarioEscolar.Domain.Entities;

namespace CommonTestUtilities.Entities
{
    public static class ApplicationUserBuilder
    {
        public static Faker<ApplicationUser> CreateFake()
        {
            return new Faker<ApplicationUser>("pt_BR")
                .RuleFor(u => u.Id, f => f.Random.Long(1, 10000))
                .RuleFor(u => u.UserName, f => f.Internet.UserName())
                .RuleFor(u => u.Email, f => f.Internet.Email())
                .RuleFor(u => u.NormalizedEmail, (f, u) => u.Email!.ToUpperInvariant())
                .RuleFor(u => u.NormalizedUserName, (f, u) => u.UserName!.ToUpperInvariant())
                .RuleFor(u => u.PhoneNumber, f => f.Phone.PhoneNumber())
                .RuleFor(u => u.EmailConfirmed, f => true)
                .RuleFor(u => u.PhoneNumberConfirmed, f => f.Random.Bool())
                .RuleFor(u => u.AccessFailedCount, f => f.Random.Int(0, 5))
                .RuleFor(u => u.LockoutEnabled, f => f.Random.Bool())
                .RuleFor(u => u.School, f => SchoolBuilder.Build())
                .RuleFor(u => u.SchoolId, (f, u) => u.School.Id);
        }
        public static ApplicationUser Build() => CreateFake().Generate();
    }
}
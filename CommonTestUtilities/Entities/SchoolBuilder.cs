using Bogus;
using InventarioEscolar.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTestUtilities.Entities
{
    public static class SchoolBuilder
    {
        public static Faker<School> CreateFake()
        {
            return new Faker<School>("pt_BR")
                .RuleFor(s => s.Id, f => f.Random.Long(1, 10000))
                .RuleFor(s => s.CreatedOn, f => f.Date.Past())
                .RuleFor(s => s.Active, f => true)
                .RuleFor(s => s.Name, f => f.Company.CompanyName())
                .RuleFor(s => s.Inep, f => f.Random.String2(8, "0123456789"))
                .RuleFor(s => s.Address, f => f.Address.StreetAddress())
                .RuleFor(s => s.City, f => f.Address.City())
                .RuleFor(s => s.RoomLocations, _ => new List<RoomLocation>())
                .RuleFor(s => s.Categories, _ => new List<Category>())
                .RuleFor(s => s.Assets, _ => new List<Asset>())
                .RuleFor(s => s.Users, _ => new List<ApplicationUser>());
        }

        public static School Build() => CreateFake().Generate();
        public static List<School> BuildList(int quantity) => CreateFake().Generate(quantity);

    }
}

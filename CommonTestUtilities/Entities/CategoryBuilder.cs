using Bogus;
using InventarioEscolar.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTestUtilities.Entities
{
    public static class CategoryBuilder
    {
        public static Faker<Category> CreateFake()
        {
            return new Faker<Category>("pt_BR")
                .RuleFor(x => x.Id, f => f.Random.Long(1, 10000))
                .RuleFor(x => x.CreatedOn, f => f.Date.Past())
                .RuleFor(x => x.Active, f => true)
                .RuleFor(x => x.Name, f => f.Commerce.Categories(1)[0])
                .RuleFor(x => x.Description, f => f.Lorem.Sentence())
                .RuleFor(r => r.School, f => SchoolBuilder.Build())
                .RuleFor(r => r.SchoolId, (f, rl) => rl.School.Id)
                .RuleFor(x => x.Assets, _ => new List<Asset>());
        }

        public static Category Build() => CreateFake().Generate();
        public static List<Category> BuildList(int quantity) => CreateFake().Generate(quantity);

    }
}
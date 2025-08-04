using Bogus;
using InventarioEscolar.Communication.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTestUtilities.Dtos
{
    public static class AssetMovementDtoBuilder
    {
        public static AssetMovementDto Build()
        {
            var faker = new Faker<AssetMovementDto>("pt_BR")
                .RuleFor(x => x.Id, f => f.Random.Long(1, 1000))
                .RuleFor(x => x.AssetId, f => f.Random.Long(1, 1000))
                .RuleFor(x => x.FromRoomId, f => f.Random.Long(1, 10))
                .RuleFor(x => x.ToRoomId, f => f.Random.Long(11, 20))
                .RuleFor(x => x.MovedAt, f => f.Date.PastOffset(1).DateTime.ToUniversalTime())
                .RuleFor(x => x.Responsible, f => f.Person.FullName)
                .RuleFor(x => x.IsCanceled, f => false) 
                .RuleFor(x => x.CancelReason, f => null as string)
                .RuleFor(x => x.CanceledAt, f => null as DateTime?);

            return faker.Generate();
        }

        public static AssetMovementDto BuildCanceled()
        {
            var faker = new Faker<AssetMovementDto>("pt_BR")
                .RuleFor(x => x.Id, f => f.Random.Long(1, 1000))
                .RuleFor(x => x.AssetId, f => f.Random.Long(1, 1000))
                .RuleFor(x => x.FromRoomId, f => f.Random.Long(1, 10))
                .RuleFor(x => x.ToRoomId, f => f.Random.Long(11, 20))
                .RuleFor(x => x.MovedAt, f => f.Date.PastOffset(1).DateTime.ToUniversalTime())
                .RuleFor(x => x.Responsible, f => f.Person.FullName)
                .RuleFor(x => x.IsCanceled, true)
                .RuleFor(x => x.CancelReason, f => f.Lorem.Sentence())
                .RuleFor(x => x.CanceledAt, f => f.Date.RecentOffset(5).DateTime.ToUniversalTime());

            return faker.Generate();
        }
    }
}

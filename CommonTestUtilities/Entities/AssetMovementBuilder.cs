using Bogus;
using InventarioEscolar.Domain.Entities;

namespace CommonTestUtilities.Entities
{
    public static class AssetMovementBuilder
    {
        public static Faker<AssetMovement> CreateFake()
        {
            return new Faker<AssetMovement>("pt_BR")
                .RuleFor(m => m.Id, f => f.Random.Long(1, 10000))
                .RuleFor(m => m.CreatedOn, f => f.Date.Past())
                .RuleFor(m => m.Active, f => true)

                .RuleFor(m => m.Asset, f => AssetBuilder.Build())
                .RuleFor(m => m.AssetId, (f, m) => m.Asset.Id)

                .RuleFor(m => m.FromRoom, f => RoomLocationBuilder.Build())
                .RuleFor(m => m.FromRoomId, (f, m) => m.FromRoom.Id)

                .RuleFor(m => m.ToRoom, f => RoomLocationBuilder.Build())
                .RuleFor(m => m.ToRoomId, (f, m) => m.ToRoom.Id)

                .RuleFor(m => m.MovedAt, f => f.Date.Recent())
                .RuleFor(m => m.Responsible, f => f.Person.FullName)

                .RuleFor(m => m.IsCanceled, f => f.Random.Bool(0.2f)) 
                .RuleFor(m => m.CancelReason, (f, m) => m.IsCanceled ? f.Lorem.Sentence() : null)
                .RuleFor(m => m.CanceledAt, (f, m) => m.IsCanceled ? f.Date.Recent() : null)

                .RuleFor(m => m.School, f => SchoolBuilder.Build())
                .RuleFor(m => m.SchoolId, (f, m) => m.School.Id);
        }

        public static AssetMovement Build() => CreateFake().Generate();
        public static List<AssetMovement> BuildList(int quantity) => CreateFake().Generate(quantity);
    }
}

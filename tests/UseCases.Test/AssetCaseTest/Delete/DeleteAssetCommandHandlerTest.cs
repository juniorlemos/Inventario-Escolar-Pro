using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.AssetRepository;
using CommonTestUtilities.Services;
using InventarioEscolar.Application.Services.Interfaces;
using InventarioEscolar.Application.UsesCases.AssetCase.Delete;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces.Repositories.Assets;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using MediatR;
using Shouldly;

namespace UseCases.Test.AssetCaseTest.Delete
{
    public class DeleteAssetCommandHandlerTest
    {
        [Fact]
        public async Task Handle_ShouldReturnUnit_WhenAssetDeletedSuccessfully()
        {
            var asset = AssetBuilder.Build();

            var assetReadRepository = CreateAssetReadRepository(true, asset);
            var assetDeleteRepository = CreateAssetDeleteRepository(true, asset.Id);
           
            var command = new DeleteAssetCommand(asset.Id);

            var currentUser = CreateCurrentUserServiceBuilder(schoolId: asset.SchoolId).Build();

            var handler = CreateHandler(assetReadRepository, assetDeleteRepository, currentUser);

            var result = await handler.Handle(command, CancellationToken.None);

            result.ShouldBe(Unit.Value);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenAssetDoesNotExist()
        {
            var asset = AssetBuilder.Build();

            var command = new DeleteAssetCommand(asset.Id);
            var assetReadRepository = CreateAssetReadRepository(false, asset);

            var assetDeleteRepository = CreateAssetDeleteRepository(false, asset.Id);

            var currentUser = CreateCurrentUserServiceBuilder(schoolId: asset.SchoolId).Build();

            var handler = CreateHandler(assetReadRepository, assetDeleteRepository , currentUser);

            var exception = await Should.ThrowAsync<NotFoundException>(
                () => handler.Handle(command, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.ASSET_NOT_FOUND);
        }

        [Fact]
        public async Task Handle_ShouldThrowBusinessException_WhenAssetDoesNotBelongToCurrentUserSchool()
        {
            var asset = AssetBuilder.Build();

            var command = new DeleteAssetCommand(asset.Id);
            var assetReadRepository = CreateAssetReadRepository(true, asset);

            var asssDeleteRepository = CreateAssetDeleteRepository(true, asset.Id);

            var currentUser = CreateCurrentUserServiceBuilder(schoolId: null).Build();

            var wrongSchoolId = asset.SchoolId + 1;

            var handler = CreateHandler(assetReadRepository,asssDeleteRepository, currentUser);

            var exception = await Should.ThrowAsync<BusinessException>(
                () => handler.Handle(command, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.ASSET_NOT_BELONG_TO_SCHOOL);
        }
        private static CurrentUserServiceBuilder CreateCurrentUserServiceBuilder(long? schoolId = null)
        {
            return new CurrentUserServiceBuilder()
                .WithSchoolId(schoolId ?? 1);
        }
        private static IAssetDeleteOnlyRepository CreateAssetDeleteRepository(
            bool deleteReturnsTrue,
            long assetId)
        {
            var assetDeleteRepositoryBuilder = new AssetDeleteOnlyRepositoryBuilder();
            return deleteReturnsTrue
                ? assetDeleteRepositoryBuilder.WithDeleteReturningTrue(assetId).Build()
                : assetDeleteRepositoryBuilder.WithDeleteReturningFalse(assetId).Build();
        }
        private static IAssetReadOnlyRepository CreateAssetReadRepository(
            bool assetExists,
            Asset asset)
        {
            var assetReadRepositoryBuilder = new AssetReadOnlyRepositoryBuilder();
            return assetExists
                ? assetReadRepositoryBuilder.WithAssetExist(asset.Id, asset).Build()
                : assetReadRepositoryBuilder.WithAssetNotFound(asset.Id).Build();
        }
        private static DeleteAssetCommandHandler CreateHandler(
            IAssetReadOnlyRepository assetReadRepository,
            IAssetDeleteOnlyRepository assetDeleteOnlyRepository,
            ICurrentUserService currentUserService)
        {
            var unitOfWork = new UnitOfWorkBuilder().Build();

            return new DeleteAssetCommandHandler(assetDeleteOnlyRepository,assetReadRepository, unitOfWork, currentUserService);
        }
    }
}
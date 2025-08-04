using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.AssetRepository;
using CommonTestUtilities.Services;
using InventarioEscolar.Application.Services.Interfaces;
using InventarioEscolar.Application.UsesCases.AssetCase.Delete;
using InventarioEscolar.Domain.Interfaces;
using InventarioEscolar.Domain.Interfaces.Repositories.Assets;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using NSubstitute;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCases.Test.AssetCaseTest.Delete
{
    public class DeleteAssetCommandHandlerTest
    {
        
        [Fact]
        public async Task Handle_ShouldReturnTrue_WhenAssetDeletedSuccessfully()
        {
            var asset = AssetBuilder.Build();

            var unitOfWork = new UnitOfWorkBuilder().Build();

            var currentUser = new CurrentUserServiceBuilder()
                .WithSchoolId(asset.SchoolId)
                .Build();

            var assetReadRepository = new AssetReadOnlyRepositoryBuilder()
                .WithAssetExist(asset.Id, asset)
                .Build();

            var deleteRepository =  new AssetDeleteOnlyRepositoryBuilder()
                .WithDeleteReturningTrue(asset.Id)
                .Build();

            var command = new DeleteAssetCommand(asset.Id);

            var handler = new DeleteAssetCommandHandler(
                deleteRepository, assetReadRepository, unitOfWork, currentUser);

            var result = await handler.Handle(command, CancellationToken.None);

            result.ShouldBe(Unit.Value);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenAssetDoesNotExist()
        {
          
            var asset = AssetBuilder.Build();

            var unitOfWork = new UnitOfWorkBuilder().Build();

            var currentUser = new CurrentUserServiceBuilder()
                .WithSchoolId(asset.SchoolId)
                .Build();

            var assetReadRepository = new AssetReadOnlyRepositoryBuilder()
                .WithAssetNotFound(asset.Id)
                .Build();

            var deleteRepository = new AssetDeleteOnlyRepositoryBuilder()
                .WithDeleteReturningTrue(asset.Id)
                .Build();

            var command = new DeleteAssetCommand(asset.Id);

            var handler = new DeleteAssetCommandHandler(
                deleteRepository, assetReadRepository, unitOfWork, currentUser);

            // Act & Assert
            var exception = await Should.ThrowAsync<NotFoundException>(
                () => handler.Handle(command, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.ASSET_NOT_FOUND);
        }

        [Fact]
        public async Task Handle_ShouldThrowBusinessException_WhenAssetDoesNotBelongToCurrentUserSchool()
        {
            
            var asset = AssetBuilder.Build();

            var unitOfWork = new UnitOfWorkBuilder().Build();

            var currentUser = new CurrentUserServiceBuilder().Build();

            var assetReadRepository = new AssetReadOnlyRepositoryBuilder()
                .WithAssetExist(asset.Id, asset)
                .Build();

            var deleteRepository = new AssetDeleteOnlyRepositoryBuilder()
                .WithDeleteReturningTrue(asset.Id)
                .Build();

            var command = new DeleteAssetCommand(asset.Id);

            var handler = new DeleteAssetCommandHandler(
                deleteRepository, assetReadRepository, unitOfWork, currentUser);

            var exception = await Should.ThrowAsync<BusinessException>(
                () => handler.Handle(command, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.ASSET_NOT_BELONG_TO_SCHOOL);
        }
    }
}
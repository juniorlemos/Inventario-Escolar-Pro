using CommonTestUtilities.Dtos;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.AssetRepository;
using CommonTestUtilities.Services;
using FluentValidation;
using InventarioEscolar.Application.Dtos;
using InventarioEscolar.Application.Services.Interfaces;
using InventarioEscolar.Application.Services.Validators;
using InventarioEscolar.Application.UsesCases.AssetCase.Register;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces;
using InventarioEscolar.Domain.Interfaces.Repositories.Assets;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using NSubstitute;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCases.Test.AssetCaseTest.Register
{
    public class RegisterAssetCommandHandlerTest
    {
            [Fact]
            public async Task Handle_ShouldRegisterAsset_WhenAllFieldsAreValidAndUserIsAuthenticated()
            {

            var assetDto = AssetDtoBuilder.Build();

            var command = new RegisterAssetCommand(assetDto);

            var unitOfWork = new UnitOfWorkBuilder().Build();

            var currentUser = new CurrentUserServiceBuilder()
                .IsAuthenticatedTrue()
                .WithSchoolId(assetDto.SchoolId)
                .Build();

            var validator = new ValidatorBuilder<AssetDto>()
                .WithValidResult()
                .Build();

            var assetWriteRepository = new AssetWriteOnlyRepositoryBuilder().Build();

            var assetReadOnlyRepository = new AssetReadOnlyRepositoryBuilder()
                .WithAssetExistenceFalse(command.AssetDto.PatrimonyCode,currentUser.SchoolId)
                .Build();
             
            var useCase = CreateUseCase(assetWriteRepository, unitOfWork, validator, assetReadOnlyRepository, currentUser);
                 

             var result = await useCase.Handle(command, CancellationToken.None);

            result.ShouldNotBeNull();
            result.PatrimonyCode.ShouldBe(assetDto.PatrimonyCode);

            await assetWriteRepository.Received(1).Insert(Arg.Any<Asset>());
            await unitOfWork.Received(1).Commit();
        }

            [Fact]
            public async Task Handle_ShouldThrowValidationException_WhenAssetDtoIsInvalid()
            {
            var assetDto = AssetDtoBuilder.Build();

            var command = new RegisterAssetCommand(assetDto);

            var unitOfWork = new UnitOfWorkBuilder().Build();

            var currentUser = new CurrentUserServiceBuilder()
                .IsAuthenticatedTrue()
                .WithSchoolId(assetDto.SchoolId)
                .Build();

            var validator = new ValidatorBuilder<AssetDto>()
                .WithInvalidResult(ResourceMessagesException.NAME_EMPTY)
                .Build();

            var assetWriteRepository = new AssetWriteOnlyRepositoryBuilder().Build();

            var assetReadOnlyRepository = new AssetReadOnlyRepositoryBuilder()
                .WithAssetExistenceFalse(command.AssetDto.PatrimonyCode, currentUser.SchoolId)
                .Build();

            var useCase = CreateUseCase(assetWriteRepository, unitOfWork, validator, assetReadOnlyRepository, currentUser);

            var exception = await Should.ThrowAsync<ErrorOnValidationException>(
               () => useCase.Handle(command, CancellationToken.None));
            exception.Message.ShouldBe(ResourceMessagesException.NAME_EMPTY);
          
        }
            

            [Fact]
            public async Task Handle_ShouldThrowBusinessException_WhenUserIsNotAuthenticated()
            {
           
            var assetDto = AssetDtoBuilder.Build();
            var command = new RegisterAssetCommand(assetDto);

            var unitOfWork = new UnitOfWorkBuilder().Build();

            var currentUser = new CurrentUserServiceBuilder()
                .IsAuthenticatedFalse()
                .Build();

            var validator = new ValidatorBuilder<AssetDto>()
                           .WithValidResult()
                           .Build();
            var assetWriteRepository = new AssetWriteOnlyRepositoryBuilder().Build();

            var assetReadOnlyRepository = new AssetReadOnlyRepositoryBuilder()
                .WithAssetExistenceFalse(assetDto.PatrimonyCode, assetDto.SchoolId)
                .Build();

            var useCase = CreateUseCase(assetWriteRepository, unitOfWork, validator, assetReadOnlyRepository, currentUser);

            var exception = await Should.ThrowAsync<BusinessException>(
                () => useCase.Handle(command, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.USER_NOT_AUTHENTICATED);
        }
            

            [Fact]
            public async Task Handle_ShouldThrowDuplicateEntityException_WhenPatrimonyCodeAlreadyExists()
            {
                var assetDto = AssetDtoBuilder.Build();
                var command = new RegisterAssetCommand(assetDto);

                var unitOfWork = new UnitOfWorkBuilder().Build();

                var currentUser = new CurrentUserServiceBuilder()
                    .IsAuthenticatedTrue()
                    .WithSchoolId(assetDto.SchoolId)
                    .Build();

                var validator = new ValidatorBuilder<AssetDto>()
                    .WithValidResult() 
                    .Build();

                var assetWriteRepository = new AssetWriteOnlyRepositoryBuilder().Build();

                var assetReadOnlyRepository = new AssetReadOnlyRepositoryBuilder()
                    .WithAssetExistenceTrue(assetDto.PatrimonyCode, currentUser.SchoolId)
                    .Build();

                var useCase = CreateUseCase(assetWriteRepository, unitOfWork, validator, assetReadOnlyRepository, currentUser);

                var exception = await Should.ThrowAsync<DuplicateEntityException>(
                    () => useCase.Handle(command, CancellationToken.None));

                exception.Message.ShouldBe(ResourceMessagesException.PATRIMONY_CODE_ALREADY_EXISTS_);
            }

        [Fact]
        public async Task Handle_ShouldCallInsertAndCommit_WhenAssetIsValid()
        {
            var assetDto = AssetDtoBuilder.Build();
            var command = new RegisterAssetCommand(assetDto);

            var unitOfWork = Substitute.For<IUnitOfWork>();
            var currentUser = new CurrentUserServiceBuilder()
                .IsAuthenticatedTrue()
                .WithSchoolId(assetDto.SchoolId)
                .Build();

            var validator = new ValidatorBuilder<AssetDto>()
                .WithValidResult()
                .Build();

            var assetWriteRepository = Substitute.For<IAssetWriteOnlyRepository>();
            var assetReadOnlyRepository = new AssetReadOnlyRepositoryBuilder()
                .WithAssetExistenceFalse(assetDto.PatrimonyCode, currentUser.SchoolId)
                .Build();

            var useCase = CreateUseCase(assetWriteRepository, unitOfWork, validator, assetReadOnlyRepository, currentUser);

            var result = await useCase.Handle(command, CancellationToken.None);

            
            await assetWriteRepository.Received(1).Insert(Arg.Any<Asset>());
            await unitOfWork.Received(1).Commit();

            result.ShouldNotBeNull();
            result.PatrimonyCode.ShouldBe(assetDto.PatrimonyCode);
        }

        private static RegisterAssetCommandHandler CreateUseCase(
     IAssetWriteOnlyRepository assetWriteOnlyRepository,
     IUnitOfWork unitOfWork,
     IValidator<AssetDto> validator,
     IAssetReadOnlyRepository assetReadOnlyRepository,
     ICurrentUserService currentUser)
        {
            return new RegisterAssetCommandHandler(
                assetWriteOnlyRepository,
                unitOfWork,
                validator,
                assetReadOnlyRepository,
                currentUser);
        }
    }
}

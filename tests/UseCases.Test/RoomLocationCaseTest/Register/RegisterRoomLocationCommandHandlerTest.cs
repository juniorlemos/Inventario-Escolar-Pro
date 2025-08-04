using CommonTestUtilities.Dtos;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.RoomLocationRepository;
using CommonTestUtilities.Services;
using FluentValidation;
using InventarioEscolar.Application.Services.Interfaces;
using InventarioEscolar.Application.UsesCases.RoomLocationCase.Register;
using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces;
using InventarioEscolar.Domain.Interfaces.Repositories.RoomLocations;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using NSubstitute;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCases.Test.RoomLocationCaseTest.Register
{
    public class RegisterRoomLocationCommandHandlerTest
    {

        [Fact]
        public async Task Handle_ShouldRegisterRoomLocation_WhenValidAndUserIsAuthenticated()
        {
            var roomLocationDto = RoomLocationDtoBuilder.Build();
            var command = new RegisterRoomLocationCommand(roomLocationDto);

            var unitOfWork = new UnitOfWorkBuilder().Build();

            var currentUser = new CurrentUserServiceBuilder()
                .IsAuthenticatedTrue()
                .WithSchoolId(roomLocationDto.SchoolId)
                .Build();

            var validator = new ValidatorBuilder<RoomLocationDto>()
                .WithValidResult()
                .Build();

            var roomLocationReadRepository = new RoomLocationReadOnlyRepositoryBuilder()
                .WithRoomLocationNameNotExists(roomLocationDto.Name, currentUser.SchoolId)
                .Build();

            var roomLocationWriteRepository = new RoomLocationWriteOnlyRepositoryBuilder().Build();

            var handler = CreateHandler(
                roomLocationWriteRepository,
                unitOfWork,
                validator,
                roomLocationReadRepository,
                currentUser
            );

            var result = await handler.Handle(command, CancellationToken.None);

            result.ShouldNotBeNull();
            result.Name.ShouldBe(roomLocationDto.Name);

            await roomLocationWriteRepository.Received(1).Insert(Arg.Any<RoomLocation>());
            await unitOfWork.Received(1).Commit();
        }

        [Fact]
        public async Task Handle_ShouldThrowValidationException_WhenDtoIsInvalid()
        {
            var roomLocationDto = RoomLocationDtoBuilder.Build();
            var command = new RegisterRoomLocationCommand(roomLocationDto);

            var unitOfWork = new UnitOfWorkBuilder().Build();

            var currentUser = new CurrentUserServiceBuilder()
                .IsAuthenticatedTrue()
                .WithSchoolId(roomLocationDto.SchoolId)
                .Build();

            var validator = new ValidatorBuilder<RoomLocationDto>()
                .WithInvalidResult(ResourceMessagesException.NAME_EMPTY)
                .Build();

            var roomLocationReadRepository = new RoomLocationReadOnlyRepositoryBuilder()
                .WithRoomLocationNameNotExists(roomLocationDto.Name, currentUser.SchoolId)
                .Build();

            var roomLocationWriteRepository = new RoomLocationWriteOnlyRepositoryBuilder().Build();

            var handler = CreateHandler(
                roomLocationWriteRepository,
                unitOfWork,
                validator,
                roomLocationReadRepository,
                currentUser
            );

            var exception = await Should.ThrowAsync<ErrorOnValidationException>(() =>
                handler.Handle(command, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.NAME_EMPTY);
        }

        [Fact]
        public async Task Handle_ShouldThrowBusinessException_WhenUserIsNotAuthenticated()
        {
            var roomLocationDto = RoomLocationDtoBuilder.Build();
            var command = new RegisterRoomLocationCommand(roomLocationDto);

            var unitOfWork = new UnitOfWorkBuilder().Build();

            var currentUser = new CurrentUserServiceBuilder()
                .IsAuthenticatedFalse()
                .Build();

            var validator = new ValidatorBuilder<RoomLocationDto>()
                .WithValidResult()
                .Build();

            var roomLocationReadRepository = new RoomLocationReadOnlyRepositoryBuilder()
                .WithRoomLocationNameNotExists(roomLocationDto.Name, roomLocationDto.SchoolId)
                .Build();

            var roomLocationWriteRepository = new RoomLocationWriteOnlyRepositoryBuilder().Build();

            var handler = CreateHandler(
                roomLocationWriteRepository,
                unitOfWork,
                validator,
                roomLocationReadRepository,
                currentUser
            );

            var exception = await Should.ThrowAsync<BusinessException>(() =>
                handler.Handle(command, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.SCHOOL_NOT_FOUND);
        }

        [Fact]
        public async Task Handle_ShouldThrowDuplicateEntityException_WhenRoomLocationNameAlreadyExists()
        {
            var roomLocationDto = RoomLocationDtoBuilder.Build();
            var command = new RegisterRoomLocationCommand(roomLocationDto);

            var unitOfWork = new UnitOfWorkBuilder().Build();

            var currentUser = new CurrentUserServiceBuilder()
                .IsAuthenticatedTrue()
                .WithSchoolId(roomLocationDto.SchoolId)
                .Build();

            var validator = new ValidatorBuilder<RoomLocationDto>()
                .WithValidResult()
                .Build();

            var roomLocationReadRepository = new RoomLocationReadOnlyRepositoryBuilder()
                .WithRoomLocationNameExists(roomLocationDto.Name, currentUser.SchoolId)
                .Build();

            var roomLocationWriteRepository = new RoomLocationWriteOnlyRepositoryBuilder().Build();

            var handler = CreateHandler(
                roomLocationWriteRepository,
                unitOfWork,
                validator,
                roomLocationReadRepository,
                currentUser
            );

            var exception = await Should.ThrowAsync<DuplicateEntityException>(() =>
                handler.Handle(command, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.ROOMLOCATION_NAME_ALREADY_EXISTS);
        }

        private static RegisterRoomLocationCommandHandler CreateHandler(
            IRoomLocationWriteOnlyRepository writeRepo,
            IUnitOfWork unitOfWork,
            IValidator<RoomLocationDto> validator,
            IRoomLocationReadOnlyRepository readRepo,
            ICurrentUserService currentUser)
        {
            return new RegisterRoomLocationCommandHandler(
                writeRepo,
                unitOfWork,
                validator,
                readRepo,
                currentUser);
        }
    }
}

using CommonTestUtilities.Dtos;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.RoomLocationRepository;
using FluentValidation;
using InventarioEscolar.Application.Services.Interfaces;
using InventarioEscolar.Application.UsesCases.RoomLocationCase.Register;
using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Interfaces.Repositories.RoomLocations;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using Shouldly;
using static CommonTestUtilities.Helpers.CurrentUserServiceTestHelper;
using static CommonTestUtilities.Helpers.ValidatorTestHelper;

namespace UseCases.Test.RoomLocationCaseTest.Register
{
    public class RegisterRoomLocationCommandHandlerTest
    {
        [Fact]
        public async Task Handle_ShouldRegisterRoomLocation_WhenValidAndUserIsAuthenticated()
        {
            var roomLocationDto = RoomLocationDtoBuilder.Build();
            var command = new RegisterRoomLocationCommand(roomLocationDto);

            var validator = CreateValidator<RoomLocationDto>(isValid: true);
            var readRepository = CreateReadOnlyRepository(nameExists: false, roomLocationDto);
            var user = CreateCurrentUserService(true, roomLocationDto.SchoolId);

            var handler = CreateHandler(validator, readRepository, user);

            var result = await handler.Handle(command, CancellationToken.None);

            result.ShouldNotBeNull();
            result.Name.ShouldBe(roomLocationDto.Name);
        }

        [Fact]
        public async Task Handle_ShouldThrowValidationException_WhenDtoIsInvalid()
        {
            var roomLocationDto = RoomLocationDtoBuilder.Build();
            var command = new RegisterRoomLocationCommand(roomLocationDto);

            var validator = CreateValidator<RoomLocationDto>(isValid: false, ResourceMessagesException.NAME_EMPTY);
            var readRepository = CreateReadOnlyRepository(nameExists: false, roomLocationDto);
            var user = CreateCurrentUserService(true, roomLocationDto.SchoolId);

            var handler = CreateHandler(validator, readRepository, user);

            var exception = await Should.ThrowAsync<ErrorOnValidationException>(() =>
                handler.Handle(command, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.NAME_EMPTY);
        }

        [Fact]
        public async Task Handle_ShouldThrowBusinessException_WhenUserIsNotAuthenticated()
        {
            var roomLocationDto = RoomLocationDtoBuilder.Build();
            var command = new RegisterRoomLocationCommand(roomLocationDto);

            var validator = CreateValidator<RoomLocationDto>(isValid: true);
            var readRepository = CreateReadOnlyRepository(nameExists: false, roomLocationDto);
            var user = CreateCurrentUserService(false);

            var handler = CreateHandler(validator, readRepository, user);

            var exception = await Should.ThrowAsync<BusinessException>(() =>
                handler.Handle(command, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.SCHOOL_NOT_FOUND);
        }

        [Fact]
        public async Task Handle_ShouldThrowDuplicateEntityException_WhenRoomLocationNameAlreadyExists()
        {
            var roomLocationDto = RoomLocationDtoBuilder.Build();
            var command = new RegisterRoomLocationCommand(roomLocationDto);

            var validator = CreateValidator<RoomLocationDto>(isValid: true);
            var readRepository = CreateReadOnlyRepository(nameExists: true, roomLocationDto);
            var user = CreateCurrentUserService(true, roomLocationDto.SchoolId);

            var handler = CreateHandler(validator, readRepository, user);

            var exception = await Should.ThrowAsync<DuplicateEntityException>(() =>
                handler.Handle(command, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.ROOMLOCATION_NAME_ALREADY_EXISTS);
        }

        private static IRoomLocationReadOnlyRepository CreateReadOnlyRepository(bool nameExists, RoomLocationDto dto)
        {
            var builder = new RoomLocationReadOnlyRepositoryBuilder();
            return nameExists
                ? builder.WithRoomLocationNameExists(dto.Name, dto.SchoolId).Build()
                : builder.WithRoomLocationNameNotExists(dto.Name, dto.SchoolId).Build();
        }

        private static RegisterRoomLocationCommandHandler CreateHandler(
            IValidator<RoomLocationDto> validator,
            IRoomLocationReadOnlyRepository readRepository,
            ICurrentUserService user)
        {
            var unitOfWork = new UnitOfWorkBuilder().Build();
            var writeRepository = new RoomLocationWriteOnlyRepositoryBuilder().Build();

            return new RegisterRoomLocationCommandHandler(
                writeRepository,
                unitOfWork,
                validator,
                readRepository,
                user);
        }
    }
}
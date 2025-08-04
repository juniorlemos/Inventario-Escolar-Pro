using CommonTestUtilities.Dtos;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.SchoolRepository;
using CommonTestUtilities.Services;
using FluentValidation;
using InventarioEscolar.Application.UsesCases.SchoolCase.Register;
using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces;
using InventarioEscolar.Domain.Interfaces.Repositories.Schools;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using Mapster;
using NSubstitute;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCases.Test.SchoolCaseTest.Register
{
    public class RegisterSchoolCommandHandlerTest
    {
        [Fact]
        public async Task Handle_ShouldRegisterSchool_WhenValid()
        {
            var dto = SchoolDtoBuilder.Build();
            var command = new RegisterSchoolCommand(dto);

            var unitOfWork = new UnitOfWorkBuilder().Build();

            var validator = new ValidatorBuilder<SchoolDto>()
                .WithValidResult()
                .Build();

            var schoolReadOnlyRepository = new SchoolReadOnlyRepositoryBuilder()
                .WithDuplicateSchool(null)
                .Build();

            var schoolWriteOnlyRepository = new SchoolWriteOnlyRepositoryBuilder().Build();

            var useCase = CreateUseCase(unitOfWork, validator, schoolReadOnlyRepository, schoolWriteOnlyRepository);

            var result = await useCase.Handle(command, CancellationToken.None);

            result.ShouldNotBeNull();
            result.Name.ShouldBe(dto.Name);

            await schoolWriteOnlyRepository.Received(1).Insert(Arg.Any<School>());
            await unitOfWork.Received(1).Commit();
        }

        [Fact]
        public async Task Handle_ShouldThrowValidationException_WhenDtoIsInvalid()
        {
            var dto = SchoolDtoBuilder.Build();
            var command = new RegisterSchoolCommand(dto);

            var unitOfWork = new UnitOfWorkBuilder().Build();

            var validator = new ValidatorBuilder<SchoolDto>()
                .WithInvalidResult(ResourceMessagesException.NAME_EMPTY)
                .Build();

            var schoolReadOnlyRepository = new SchoolReadOnlyRepositoryBuilder()
                .WithDuplicateSchool(null)
                .Build();

            var schoolWriteOnlyRepository = new SchoolWriteOnlyRepositoryBuilder().Build();

            var useCase = CreateUseCase(unitOfWork, validator, schoolReadOnlyRepository, schoolWriteOnlyRepository);

            var exception = await Should.ThrowAsync<ErrorOnValidationException>(() =>
                useCase.Handle(command, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.NAME_EMPTY);
        }

        [Fact]
        public async Task Handle_ShouldThrowDuplicateEntityException_WhenNameAlreadyExists()
        {
            var dto = SchoolDtoBuilder.Build();
            var duplicate = dto.Adapt<School>();
            duplicate.Inep = "99999999";
            duplicate.Address = "Outro Endereço";

            var command = new RegisterSchoolCommand(dto);

            var unitOfWork = new UnitOfWorkBuilder().Build();

            var validator = new ValidatorBuilder<SchoolDto>().WithValidResult().Build();

            var schoolReadOnlyRepository = new SchoolReadOnlyRepositoryBuilder()
                .WithDuplicateSchool(duplicate)
                .Build();

            var schoolWriteOnlyRepository = new SchoolWriteOnlyRepositoryBuilder().Build();

            var useCase = CreateUseCase(unitOfWork, validator, schoolReadOnlyRepository, schoolWriteOnlyRepository);

            var exception = await Should.ThrowAsync<DuplicateEntityException>(() =>
                useCase.Handle(command, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.SCHOOL_NAME_ALREADY_EXISTS);
        }

        [Fact]
        public async Task Handle_ShouldThrowDuplicateEntityException_WhenInepAlreadyExists()
        {
            var dto = SchoolDtoBuilder.Build();
            var duplicate = dto.Adapt<School>();
            duplicate.Name = "Outra escola";
            duplicate.Address = "Outro Endereço";

            var command = new RegisterSchoolCommand(dto);

            var unitOfWork = new UnitOfWorkBuilder().Build();

            var validator = new ValidatorBuilder<SchoolDto>().WithValidResult().Build();

            var schoolReadOnlyRepository = new SchoolReadOnlyRepositoryBuilder()
                .WithDuplicateSchool(duplicate)
                .Build();

            var schoolWriteOnlyRepository = new SchoolWriteOnlyRepositoryBuilder().Build();

            var useCase = CreateUseCase(unitOfWork, validator, schoolReadOnlyRepository, schoolWriteOnlyRepository);

            var exception = await Should.ThrowAsync<DuplicateEntityException>(() =>
                useCase.Handle(command, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.SCHOOL_INEP_ALREADY_EXISTS);
        }

        [Fact]
        public async Task Handle_ShouldThrowDuplicateEntityException_WhenAddressAlreadyExists()
        {
            var dto = SchoolDtoBuilder.Build();
            var duplicate = dto.Adapt<School>();
            duplicate.Name = "Outra escola";
            duplicate.Inep = "12345678";

            var command = new RegisterSchoolCommand(dto);

            var unitOfWork = new UnitOfWorkBuilder().Build();

            var validator = new ValidatorBuilder<SchoolDto>().WithValidResult().Build();

            var schoolReadOnlyRepository = new SchoolReadOnlyRepositoryBuilder()
                .WithDuplicateSchool(duplicate)
                .Build();

            var schoolWriteOnlyRepository = new SchoolWriteOnlyRepositoryBuilder().Build();

            var useCase = CreateUseCase(unitOfWork, validator, schoolReadOnlyRepository, schoolWriteOnlyRepository);

            var exception = await Should.ThrowAsync<DuplicateEntityException>(() =>
                useCase.Handle(command, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.SCHOOL_ADDRESS_ALREADY_EXISTS);
        }

        private static RegisterSchoolCommandHandler CreateUseCase(
            IUnitOfWork unitOfWork,
            IValidator<SchoolDto> validator,
            ISchoolReadOnlyRepository schoolReadOnlyRepository,
            ISchoolWriteOnlyRepository schoolWriteOnlyRepository)
        {
            return new RegisterSchoolCommandHandler(
                unitOfWork,
                validator,
                schoolReadOnlyRepository,
                schoolWriteOnlyRepository);
        }
    }
}
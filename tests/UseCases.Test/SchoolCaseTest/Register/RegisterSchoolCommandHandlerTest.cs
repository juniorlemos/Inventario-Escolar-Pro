using CommonTestUtilities.Dtos;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.SchoolRepository;
using FluentValidation;
using InventarioEscolar.Application.UsesCases.SchoolCase.Register;
using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces.Repositories.Schools;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using Mapster;
using Shouldly;
using static CommonTestUtilities.Helpers.ValidatorTestHelper;

namespace UseCases.Test.SchoolCaseTest.Register
{
    public class RegisterSchoolCommandHandlerTest
    {
        
        [Fact]
        public async Task Handle_ShouldThrowValidationException_WhenDtoIsInvalid()
        {
            var schoolDto = SchoolDtoBuilder.Build();
            var command = new RegisterSchoolCommand(schoolDto);

            var validator = CreateValidator<SchoolDto>(false, ResourceMessagesException.NAME_EMPTY);

            var schoolReadOnlyRepository = CreateSchoolReadRepository(null);


            var useCase = CreateUseCase( validator, schoolReadOnlyRepository);

            var exception = await Should.ThrowAsync<ErrorOnValidationException>(() =>
                useCase.Handle(command, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.NAME_EMPTY);
        }

        [Fact]
        public async Task Handle_ShouldThrowDuplicateEntityException_WhenNameAlreadyExists()
        {
            var schoolDto = SchoolDtoBuilder.Build();
            var schoolSecond = SchoolBuilder.Build();

            var duplicate = schoolDto.Adapt<School>();
            duplicate.Inep = schoolSecond.Inep;
            duplicate.Address = schoolSecond.Address;
            duplicate.Name = schoolDto.Name;

            var command = new RegisterSchoolCommand(schoolDto);

            var validator = CreateValidator<SchoolDto>(true);

            var schoolReadOnlyRepository = CreateSchoolReadRepository(duplicate);

            var useCase = CreateUseCase( validator, schoolReadOnlyRepository);

            var exception = await Should.ThrowAsync<DuplicateEntityException>(() =>
                useCase.Handle(command, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.SCHOOL_NAME_ALREADY_EXISTS);
        }

        [Fact]
        public async Task Handle_ShouldThrowDuplicateEntityException_WhenInepAlreadyExists()
        {
            var schoolDto = SchoolDtoBuilder.Build();
            var schoolSecond = SchoolBuilder.Build();

            var duplicate = schoolDto.Adapt<School>();
            duplicate.Name = schoolSecond.Name;
            duplicate.Inep = schoolDto.Inep;
            duplicate.Address = schoolSecond.Address;

            var command = new RegisterSchoolCommand(schoolDto);

            var validator = CreateValidator<SchoolDto>(true);

            var schoolReadOnlyRepository = CreateSchoolReadRepository(duplicate);


            var useCase = CreateUseCase( validator, schoolReadOnlyRepository);

            var exception = await Should.ThrowAsync<DuplicateEntityException>(() =>
                useCase.Handle(command, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.SCHOOL_INEP_ALREADY_EXISTS);
        }

        [Fact]
        public async Task Handle_ShouldContinue_WhenDuplicateExistsButAddressIsDifferent()
        {
            var schoolDto = SchoolDtoBuilder.Build();

            var duplicate = schoolDto.Adapt<School>();
            var schoolSecond = SchoolBuilder.Build();

            duplicate.Name = schoolSecond.Name;
            duplicate.Inep = schoolSecond.Inep;
            duplicate.Address = schoolSecond.Address;

            var command = new RegisterSchoolCommand(schoolDto);

            var validator = CreateValidator<SchoolDto>(true);
            var schoolReadOnlyRepository = CreateSchoolReadRepository(duplicate);          

            var useCase = CreateUseCase(validator, schoolReadOnlyRepository);

            var result = await useCase.Handle(command, CancellationToken.None);

            result.ShouldNotBeNull();
            result.Name.ShouldBe(schoolDto.Name);

        }

        [Fact]
        public async Task Handle_ShouldThrowDuplicateEntityException_WhenAddressAlreadyExists()
        {
            var schoolDto = SchoolDtoBuilder.Build();
            var schoolSecond = SchoolBuilder.Build();

            var duplicate = schoolDto.Adapt<School>();
            duplicate.Name = schoolSecond.Name;
            duplicate.Inep = schoolSecond.Inep;
            duplicate.Address = schoolDto.Address;

            var command = new RegisterSchoolCommand(schoolDto);

            var validator = CreateValidator<SchoolDto>(true);

            var schoolReadOnlyRepository = CreateSchoolReadRepository(duplicate);


            var useCase = CreateUseCase( validator, schoolReadOnlyRepository);

            var exception = await Should.ThrowAsync<DuplicateEntityException>(() =>
                useCase.Handle(command, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.SCHOOL_ADDRESS_ALREADY_EXISTS);
        }
        public static ISchoolReadOnlyRepository CreateSchoolReadRepository(School? duplicate)
        {
            return new SchoolReadOnlyRepositoryBuilder()
                .WithDuplicateSchool(duplicate)
                .Build();
        }
        private static RegisterSchoolCommandHandler CreateUseCase(
            IValidator<SchoolDto> validator,
            ISchoolReadOnlyRepository schoolReadOnlyRepository)
        {

            var unitOfWork = new UnitOfWorkBuilder().Build();
            var schoolWriteOnlyRepository = new SchoolWriteOnlyRepositoryBuilder().Build();

            return new RegisterSchoolCommandHandler(
                unitOfWork,
                validator,
                schoolReadOnlyRepository,
                schoolWriteOnlyRepository);
        }
    }
}
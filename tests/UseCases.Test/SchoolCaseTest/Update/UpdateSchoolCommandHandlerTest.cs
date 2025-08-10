using CommonTestUtilities.Dtos;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.SchoolRepository;
using FluentValidation;
using InventarioEscolar.Application.UsesCases.SchoolCase.Update;
using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces.Repositories.Schools;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using MediatR;
using Shouldly;
using static CommonTestUtilities.Helpers.ValidatorTestHelper;

namespace UseCases.Test.SchoolCaseTest.Update
{
    public class UpdateSchoolCommandHandlerTest
    {
        [Fact]
        public async Task Handle_ShouldUpdateSchool_WhenDtoIsValidAndSchoolExists()
        {
            var school = SchoolBuilder.Build();
            var updateSchoolDto = UpdateSchoolDtoBuilder.Build();

            var command =  new UpdateSchoolCommand(school.Id, updateSchoolDto);

            var validator = CreateValidator<UpdateSchoolDto>(true);

            var schoolReadOnlyRepository = CreateSchoolReadRepository(school.Id, school);
            
            var handler = CreateUseCase( validator, schoolReadOnlyRepository);

            var result = await handler.Handle(command, CancellationToken.None);

            result.ShouldBe(Unit.Value);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenSchoolDoesNotExist()
        {
            int id = new Random().Next(1, 1000);

            var updateSchoolDto = UpdateSchoolDtoBuilder.Build();
            var command = new UpdateSchoolCommand(id, updateSchoolDto);

            var validator = CreateValidator<UpdateSchoolDto>(true);

            var schoolReadOnlyRepository = CreateSchoolReadRepository(id, null);


            var handler = CreateUseCase( validator, schoolReadOnlyRepository);

            var exception = await Should.ThrowAsync<NotFoundException>(() =>
                handler.Handle(command, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.SCHOOL_NOT_FOUND);
        }

        [Fact]
        public async Task Handle_ShouldThrowValidationException_WhenDtoIsInvalid()
        {
            var school = SchoolBuilder.Build();
            var updateSchoolDto = UpdateSchoolDtoBuilder.Build();
            var command = new UpdateSchoolCommand(school.Id, updateSchoolDto);

            var validator = CreateValidator<UpdateSchoolDto>(false, ResourceMessagesException.NAME_EMPTY);
            
            var schoolReadOnlyRepository = CreateSchoolReadRepository(school.Id, school);
            

            var handler = CreateUseCase( validator, schoolReadOnlyRepository);

            var exception = await Should.ThrowAsync<ErrorOnValidationException>(() =>
                handler.Handle(command, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.NAME_EMPTY);
        }
        public static ISchoolReadOnlyRepository CreateSchoolReadRepository(long id, School? school)
        {
            var builder = new SchoolReadOnlyRepositoryBuilder();

            if (school == null)
                return builder.WithSchoolNotFound(id).Build();

            return builder.WithSchoolExist(id, school).Build();
        }
        private static UpdateSchoolCommandHandler CreateUseCase(
            IValidator<UpdateSchoolDto> validator,
            ISchoolReadOnlyRepository schoolReadOnlyRepository)
        {
            var unitOfWork = new UnitOfWorkBuilder().Build();
            var schoolUpdateOnlyRepository = new SchoolUpdateOnlyRepositoryBuilder().Build();

            return new UpdateSchoolCommandHandler(
                unitOfWork,
                validator,
                schoolReadOnlyRepository,
                schoolUpdateOnlyRepository);
        }
    }
}
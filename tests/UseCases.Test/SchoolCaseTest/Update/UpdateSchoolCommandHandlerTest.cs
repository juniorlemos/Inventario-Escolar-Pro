using CommonTestUtilities.Dtos;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.SchoolRepository;
using CommonTestUtilities.Services;
using FluentValidation;
using InventarioEscolar.Application.UsesCases.SchoolCase.Update;
using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Interfaces;
using InventarioEscolar.Domain.Interfaces.Repositories.Schools;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using MediatR;
using NSubstitute;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCases.Test.SchoolCaseTest.Update
{
    public class UpdateSchoolCommandHandlerTest
    {
        [Fact]
        public async Task Handle_ShouldUpdateSchool_WhenDtoIsValidAndSchoolExists()
        {
            var school = SchoolBuilder.Build();
            var dto = UpdateSchoolDtoBuilder.Build();

            var command = new UpdateSchoolCommand(school.Id, dto);

            var unitOfWork = new UnitOfWorkBuilder().Build();
            var validator = new ValidatorBuilder<UpdateSchoolDto>().WithValidResult().Build();

            var schoolReadOnlyRepository = new SchoolReadOnlyRepositoryBuilder()
                .WithSchoolExist(school.Id, school)
                .Build();

            var schoolUpdateOnlyRepository = new SchoolUpdateOnlyRepositoryBuilder().Build();

            var handler = CreateUseCase(unitOfWork, validator, schoolReadOnlyRepository, schoolUpdateOnlyRepository);

            var result = await handler.Handle(command, CancellationToken.None);

            result.ShouldBe(Unit.Value);
            schoolUpdateOnlyRepository.Received(1).Update(school);
            await unitOfWork.Received(1).Commit();
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenSchoolDoesNotExist()
        {
            var id = 999;
            var dto = UpdateSchoolDtoBuilder.Build();
            var command = new UpdateSchoolCommand(id, dto);

            var unitOfWork = new UnitOfWorkBuilder().Build();
            var validator = new ValidatorBuilder<UpdateSchoolDto>().WithValidResult().Build();

            var schoolReadOnlyRepository = new SchoolReadOnlyRepositoryBuilder()
                .WithSchoolNotFound(id)
                .Build();

            var schoolUpdateOnlyRepository = new SchoolUpdateOnlyRepositoryBuilder().Build();

            var handler = CreateUseCase(unitOfWork, validator, schoolReadOnlyRepository, schoolUpdateOnlyRepository);

            var exception = await Should.ThrowAsync<NotFoundException>(() =>
                handler.Handle(command, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.SCHOOL_NOT_FOUND);
        }

        [Fact]
        public async Task Handle_ShouldThrowValidationException_WhenDtoIsInvalid()
        {
            var school = SchoolBuilder.Build();
            var dto = UpdateSchoolDtoBuilder.Build();
            var command = new UpdateSchoolCommand(school.Id, dto);

            var unitOfWork = new UnitOfWorkBuilder().Build();
            var validator = new ValidatorBuilder<UpdateSchoolDto>()
                .WithInvalidResult(ResourceMessagesException.NAME_EMPTY)
                .Build();

            var schoolReadOnlyRepository = new SchoolReadOnlyRepositoryBuilder()
                .WithSchoolExist(school.Id, school)
                .Build();

            var schoolUpdateOnlyRepository = new SchoolUpdateOnlyRepositoryBuilder().Build();

            var handler = CreateUseCase(unitOfWork, validator, schoolReadOnlyRepository, schoolUpdateOnlyRepository);

            var exception = await Should.ThrowAsync<ErrorOnValidationException>(() =>
                handler.Handle(command, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.NAME_EMPTY);
        }

        private static UpdateSchoolCommandHandler CreateUseCase(
            IUnitOfWork unitOfWork,
            IValidator<UpdateSchoolDto> validator,
            ISchoolReadOnlyRepository schoolReadOnlyRepository,
            ISchoolUpdateOnlyRepository schoolUpdateOnlyRepository)
        {
            return new UpdateSchoolCommandHandler(
                unitOfWork,
                validator,
                schoolReadOnlyRepository,
                schoolUpdateOnlyRepository);
        }
    }
}

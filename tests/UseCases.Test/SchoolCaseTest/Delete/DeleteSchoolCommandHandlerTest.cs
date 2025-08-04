using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.SchoolRepository;
using InventarioEscolar.Application.UsesCases.SchoolCase.Delete;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using MediatR;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCases.Test.SchoolCaseTest.Delete
{
    public class DeleteSchoolCommandHandlerTest
    {
        [Fact]
        public async Task Handle_ShouldReturnUnit_WhenSchoolDeletedSuccessfully()
        {
            var school = SchoolBuilder.Build();

            var unitOfWork = new UnitOfWorkBuilder().Build();

            var readRepo = new SchoolReadOnlyRepositoryBuilder()
                .WithSchoolExist(school.Id, school)
                .Build();

            var deleteRepo = new SchoolDeleteOnlyRepositoryBuilder()
                .WithDeleteReturningTrue(school.Id)
                .Build();

            var command = new DeleteSchoolCommand(school.Id);

            var handler = new DeleteSchoolCommandHandler(deleteRepo, readRepo, unitOfWork);

            var result = await handler.Handle(command, CancellationToken.None);

            result.ShouldBe(Unit.Value);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenSchoolDoesNotExist()
        {
            var school = SchoolBuilder.Build();

            var unitOfWork = new UnitOfWorkBuilder().Build();

            var readRepo = new SchoolReadOnlyRepositoryBuilder()
                .WithSchoolNotFound(school.Id)
                .Build();

            var deleteRepo = new SchoolDeleteOnlyRepositoryBuilder()
                .WithDeleteReturningTrue(school.Id)
                .Build();

            var command = new DeleteSchoolCommand(school.Id);

            var handler = new DeleteSchoolCommandHandler(deleteRepo, readRepo, unitOfWork);

            var exception = await Should.ThrowAsync<NotFoundException>(() =>
                handler.Handle(command, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.SCHOOL_NOT_FOUND);
        }

        [Fact]
        public async Task Handle_ShouldThrowBusinessException_WhenSchoolHasAssetsOrRoomsOrCategories()
        {
            var school = SchoolBuilder.Build();
            school.RoomLocations = RoomLocationBuilder.BuildList(2);
            school.Assets = AssetBuilder.BuildList(3);
            school.Categories = CategoryBuilder.BuildList(1);

            var unitOfWork = new UnitOfWorkBuilder().Build();

            var readRepo = new SchoolReadOnlyRepositoryBuilder()
                .WithSchoolExist(school.Id, school)
                .Build();

            var deleteRepo = new SchoolDeleteOnlyRepositoryBuilder()
                .WithDeleteReturningTrue(school.Id)
                .Build();

            var command = new DeleteSchoolCommand(school.Id);

            var handler = new DeleteSchoolCommandHandler(deleteRepo, readRepo, unitOfWork);

            var exception = await Should.ThrowAsync<BusinessException>(() =>
                handler.Handle(command, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.CATEGORY_HAS_ASSETS);
        }
    }
}

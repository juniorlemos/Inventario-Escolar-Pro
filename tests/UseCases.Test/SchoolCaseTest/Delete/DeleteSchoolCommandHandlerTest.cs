using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.SchoolRepository;
using InventarioEscolar.Application.UsesCases.SchoolCase.Delete;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces.Repositories.Schools;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using MediatR;
using Shouldly;

namespace UseCases.Test.SchoolCaseTest.Delete
{
    public class DeleteSchoolCommandHandlerTest
    {
        [Fact]
        public async Task Handle_ShouldReturnUnit_WhenSchoolDeletedSuccessfully()
        {
            var school = SchoolBuilder.Build();
            var command = new DeleteSchoolCommand(school.Id);

            var readRepository = CreateSchoolReadOnlyRepository(true, school);
            var deleteRepository = CreateSchoolDeleteOnlyRepository(school.Id);

            var handler = CreateHandler(readRepository, deleteRepository);

            var result = await handler.Handle(command, CancellationToken.None);

            result.ShouldBe(Unit.Value);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenSchoolDoesNotExist()
        {
            var school = SchoolBuilder.Build();
            var command = new DeleteSchoolCommand(school.Id);

            var readRepository = CreateSchoolReadOnlyRepository(false, school);
            var deleteRepository = CreateSchoolDeleteOnlyRepository(school.Id);

            var handler = CreateHandler(readRepository, deleteRepository);

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

            var command = new DeleteSchoolCommand(school.Id);

            var readRepository = CreateSchoolReadOnlyRepository(true, school);
            var deleteRepository = CreateSchoolDeleteOnlyRepository(school.Id);

            var handler = CreateHandler(readRepository, deleteRepository);

            var exception = await Should.ThrowAsync<BusinessException>(() =>
                handler.Handle(command, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.CATEGORY_HAS_ASSETS);
        }

        private static ISchoolReadOnlyRepository CreateSchoolReadOnlyRepository(bool exists, School school)
        {
            var builder = new SchoolReadOnlyRepositoryBuilder();

            return exists
                ? builder.WithSchoolExist(school.Id, school).Build()
                : builder.WithSchoolNotFound(school.Id).Build();
        }

        private static ISchoolDeleteOnlyRepository CreateSchoolDeleteOnlyRepository(long schoolId)
        {
            return new SchoolDeleteOnlyRepositoryBuilder()
                .WithDeleteReturningTrue(schoolId)
                .Build();
        }

        private static DeleteSchoolCommandHandler CreateHandler(
            ISchoolReadOnlyRepository readRepository,
            ISchoolDeleteOnlyRepository deleteRepository)
        {
            var unitOfWork = new UnitOfWorkBuilder().Build();
            return new DeleteSchoolCommandHandler(deleteRepository, readRepository, unitOfWork);
        }
    }
}
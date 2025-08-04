using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories.SchoolRepository;
using InventarioEscolar.Application.UsesCases.SchoolCase.GetById;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCases.Test.SchoolCaseTest.GetById
{
    public class GetSchoolByIdQueryHandlerTest
    {
        [Fact]
        public async Task Handle_ShouldReturnSchoolDto_WhenSchoolExists()
        {
            // Arrange
            var school = SchoolBuilder.Build(); // já existe conforme seu padrão
            var query = new GetByIdSchoolQuery(school.Id);

            var repository = new SchoolReadOnlyRepositoryBuilder()
                .WithSchoolExist(school.Id, school)
                .Build();

            var useCase = new GetByIdSchoolQueryHandler(repository);

            // Act
            var result = await useCase.Handle(query, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(school.Id);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenSchoolDoesNotExist()
        {
            // Arrange
            var school = SchoolBuilder.Build();
            var query = new GetByIdSchoolQuery(school.Id);

            var repository = new SchoolReadOnlyRepositoryBuilder()
                .WithSchoolNotFound(school.Id)
                .Build();

            var useCase = new GetByIdSchoolQueryHandler(repository);

            // Act & Assert
            var exception = await Should.ThrowAsync<NotFoundException>(() =>
                useCase.Handle(query, CancellationToken.None));

            exception.Message.ShouldBe(ResourceMessagesException.SCHOOL_NOT_FOUND);
        }
    }
}

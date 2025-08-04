using InventarioEscolar.Application.Services.AuthService.RegisterAuth;
using InventarioEscolar.Communication.Request;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces;
using InventarioEscolar.Domain.Interfaces.Repositories.Schools;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using MediatR;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCases.Test.Services.Auth
{
    public class RegisterCommandHandlerTests
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ISchoolReadOnlyRepository _schoolReadOnlyRepository;
        private readonly ISchoolWriteOnlyRepository _schoolWriteOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly RegisterCommandHandler _handler;

        public RegisterCommandHandlerTests()
        {
            var userStore = Substitute.For<IUserStore<ApplicationUser>>();
            _userManager = Substitute.For<UserManager<ApplicationUser>>(
                userStore, null, null, null, null, null, null, null, null);

            _schoolReadOnlyRepository = Substitute.For<ISchoolReadOnlyRepository>();
            _schoolWriteOnlyRepository = Substitute.For<ISchoolWriteOnlyRepository>();
            _unitOfWork = Substitute.For<IUnitOfWork>();

            _handler = new RegisterCommandHandler(
                _userManager,
                _schoolReadOnlyRepository,
                _schoolWriteOnlyRepository,
                _unitOfWork);
        }

        private RegisterRequest CreateValidRequest() =>
            new RegisterRequest
            {
                SchoolName = "Escola A",
                Inep = "123456",
                Address = "Rua 1",
                City = "Cidade X",
                Email = "teste@teste.com",
                Password = "Senha@123"
            };

        [Fact]
        public async Task Handle_ShouldRegisterUser_WhenAllValid()
        {
            // Arrange
            var request = CreateValidRequest();

            _schoolReadOnlyRepository.GetDuplicateSchool(request.SchoolName, request.Inep, request.Address)
                .Returns(Task.FromResult<School>(null));

            _userManager.FindByEmailAsync(request.Email).Returns(Task.FromResult<ApplicationUser>(null));

            _unitOfWork.ExecuteInTransaction(Arg.Any<Func<Task>>())
                .Returns(callInfo =>
                {
                    var func = callInfo.Arg<Func<Task>>();
                    return func();
                });

            _unitOfWork.Commit().Returns(Task.CompletedTask);

            _schoolWriteOnlyRepository.Insert(Arg.Any<School>())
                .Returns(Task.CompletedTask)
                .AndDoes(ci =>
                {
                    var school = ci.Arg<School>();
                    school.Id = 99; // Simular Id gerado
                });

            _userManager.CreateAsync(Arg.Any<ApplicationUser>(), request.Password)
                .Returns(Task.FromResult(IdentityResult.Success));

            var command = new RegisterCommand(request);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(Unit.Value, result);
            await _schoolWriteOnlyRepository.Received(1).Insert(Arg.Any<School>());
            await _unitOfWork.Received(1).Commit();
            await _userManager.Received(1).CreateAsync(Arg.Any<ApplicationUser>(), request.Password);
        }

        [Theory]
        [InlineData("Name")]
        [InlineData("Inep")]
        [InlineData("Address")]
        public async Task Handle_ShouldThrowDuplicateEntityException_WhenSchoolDuplicateExists(string field)
        {
            // Arrange
            var request = CreateValidRequest();

            var duplicateSchool = new School
            {
                Name = field == "Name" ? request.SchoolName : "Outra Escola",
                Inep = field == "Inep" ? request.Inep : "000000",
                Address = field == "Address" ? request.Address : "Outra Rua"
            };

            _schoolReadOnlyRepository.GetDuplicateSchool(request.SchoolName, request.Inep, request.Address)
                .Returns(Task.FromResult(duplicateSchool));

            var command = new RegisterCommand(request);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<DuplicateEntityException>(() => _handler.Handle(command, CancellationToken.None));

            if (field == "Name")
                Assert.Equal(ResourceMessagesException.SCHOOL_NAME_ALREADY_EXISTS, ex.Message);
            else if (field == "Inep")
                Assert.Equal(ResourceMessagesException.SCHOOL_INEP_ALREADY_EXISTS, ex.Message);
            else if (field == "Address")
                Assert.Equal(ResourceMessagesException.SCHOOL_ADDRESS_ALREADY_EXISTS, ex.Message);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenEmailAlreadyInUse()
        {
            // Arrange
            var request = CreateValidRequest();

            _schoolReadOnlyRepository.GetDuplicateSchool(request.SchoolName, request.Inep, request.Address)
                .Returns(Task.FromResult<School>(null));

            _userManager.FindByEmailAsync(request.Email)
                .Returns(Task.FromResult(new ApplicationUser()));

            var command = new RegisterCommand(request);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Equal("Este e-mail já está em uso.", ex.Message);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenUserCreationFails()
        {
            // Arrange
            var request = CreateValidRequest();

            _schoolReadOnlyRepository.GetDuplicateSchool(request.SchoolName, request.Inep, request.Address)
                .Returns(Task.FromResult<School>(null));

            _userManager.FindByEmailAsync(request.Email)
                .Returns(Task.FromResult<ApplicationUser>(null));

            _unitOfWork.ExecuteInTransaction(Arg.Any<Func<Task>>())
                .Returns(callInfo =>
                {
                    var func = callInfo.Arg<Func<Task>>();
                    return func();
                });

            _unitOfWork.Commit().Returns(Task.CompletedTask);

            _schoolWriteOnlyRepository.Insert(Arg.Any<School>())
                .Returns(Task.CompletedTask)
                .AndDoes(ci =>
                {
                    var school = ci.Arg<School>();
                    school.Id = 99;
                });

            var identityErrors = new IdentityError[]
            {
            new IdentityError { Description = "Erro1" },
            new IdentityError { Description = "Erro2" }
            };

            var failedResult = IdentityResult.Failed(identityErrors);

            _userManager.CreateAsync(Arg.Any<ApplicationUser>(), request.Password)
                .Returns(Task.FromResult(failedResult));

            var command = new RegisterCommand(request);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));

            Assert.Contains("Erro ao criar usuário", ex.Message);
            Assert.Contains("Erro1", ex.Message);
            Assert.Contains("Erro2", ex.Message);
        }
    }
}
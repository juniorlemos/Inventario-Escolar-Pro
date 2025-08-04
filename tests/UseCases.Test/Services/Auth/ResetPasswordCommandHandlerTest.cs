using InventarioEscolar.Application.Services.AuthService.ResetPassword;
using InventarioEscolar.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace UseCases.Test.Services.Auth
{
    public class ResetPasswordCommandHandlerTest
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ResetPasswordCommandHandler _handler;

        public ResetPasswordCommandHandlerTest()
        {
            var userStore = Substitute.For<IUserStore<ApplicationUser>>();
            _userManager = Substitute.For<UserManager<ApplicationUser>>(userStore, null, null, null, null, null, null, null, null);

            _handler = new ResetPasswordCommandHandler(_userManager);
        }

        [Fact]
        public async Task Handle_ShouldResetPassword_WhenDataIsValid()
        {
            // Arrange
            var email = "user@test.com";
            var token = WebUtility.UrlEncode("token123");
            var newPassword = "NewPassword!123";

            var command = new ResetPasswordCommand(email, token, newPassword);

            var user = new ApplicationUser { Email = email };

            _userManager.FindByEmailAsync(email).Returns(user);
            _userManager.ResetPasswordAsync(user, "token123", newPassword).Returns(IdentityResult.Success);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(Unit.Value, result);
            await _userManager.Received(1).FindByEmailAsync(email);
            await _userManager.Received(1).ResetPasswordAsync(user, "token123", newPassword);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenUserNotFound()
        {
            // Arrange
            var email = "user@test.com";
            var token = "token123";
            var newPassword = "NewPassword!123";

            var command = new ResetPasswordCommand(email, token, newPassword);

            _userManager.FindByEmailAsync(email).Returns((ApplicationUser)null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Equal("Usuário não encontrado.", ex.Message);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenResetFails()
        {
            // Arrange
            var email = "user@test.com";
            var token = WebUtility.UrlEncode("token123");
            var newPassword = "NewPassword!123";

            var command = new ResetPasswordCommand(email, token, newPassword);

            var user = new ApplicationUser { Email = email };

            _userManager.FindByEmailAsync(email).Returns(user);

            var errors = new IdentityError[]
            {
            new IdentityError { Description = "Erro 1" },
            new IdentityError { Description = "Erro 2" }
            };

            _userManager.ResetPasswordAsync(user, "token123", newPassword)
                .Returns(IdentityResult.Failed(errors));

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Contains("Erro ao redefinir a senha: Erro 1, Erro 2", ex.Message);
        }
    }
}

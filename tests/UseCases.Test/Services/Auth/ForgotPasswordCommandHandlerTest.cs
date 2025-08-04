using InventarioEscolar.Application.Services.AuthService.ForgotPassword;
using InventarioEscolar.Application.Services.Interfaces;
using InventarioEscolar.Communication.Request;
using InventarioEscolar.Domain.Entities;
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
    public class ForgotPasswordCommandHandlerTest
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly ForgotPasswordCommandHandler _handler;

        public ForgotPasswordCommandHandlerTest()
        {
            _userManager = Substitute.For<UserManager<ApplicationUser>>(
                Substitute.For<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null
            );

            _emailService = Substitute.For<IEmailService>();
            _handler = new ForgotPasswordCommandHandler(_userManager, _emailService);
        }

        [Fact]
        public async Task Handle_ShouldSendEmail_WhenUserExists()
        {
            // Arrange
            var email = "usuario@teste.com";
            var user = new ApplicationUser { Email = email };
            var token = "resettoken";

            _userManager.FindByEmailAsync(email).Returns(user);
            _userManager.GeneratePasswordResetTokenAsync(user).Returns(token);

            var request = new ForgotPasswordCommand(new ForgotPasswordRequest { Email = email });

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal(Unit.Value, result);
            await _emailService.Received(1).SendEmailAsync(
                email,
                "Redefinir Senha",
                Arg.Is<string>(msg => msg.Contains(token) && msg.Contains(email))
            );
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenUserNotFound()
        {
            // Arrange
            var email = "naoencontrado@teste.com";

            _userManager.FindByEmailAsync(email).Returns((ApplicationUser?)null);
            var request = new ForgotPasswordCommand(new ForgotPasswordRequest { Email = email });

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(request, CancellationToken.None));
            Assert.Equal("E-mail não encontrado.", ex.Message);
        }
    }
}


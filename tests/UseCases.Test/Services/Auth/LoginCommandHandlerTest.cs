using InventarioEscolar.Application.Services.AuthService.LoginAuth;
using InventarioEscolar.Application.Services.Interfaces;
using InventarioEscolar.Application.Services.Interfaces.Auth;
using InventarioEscolar.Communication.Request;
using InventarioEscolar.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using NSubstitute;

public class LoginCommandHandlerTests
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ISignInManagerWrapper _signInManagerWrapper;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly LoginCommandHandler _handler;

    public LoginCommandHandlerTests()
    {
        // Para simplificar, criaremos UserManager mock (métodos virtuais)
        var userStore = Substitute.For<Microsoft.AspNetCore.Identity.IUserStore<ApplicationUser>>();
        _userManager = Substitute.For<UserManager<ApplicationUser>>(
            userStore, null, null, null, null, null, null, null, null);

        _signInManagerWrapper = Substitute.For<ISignInManagerWrapper>();
        _jwtTokenGenerator = Substitute.For<IJwtTokenGenerator>();

        _handler = new LoginCommandHandler(_userManager, _signInManagerWrapper, _jwtTokenGenerator);
    }

    [Fact]
    public async Task Handle_ShouldReturnToken_WhenCredentialsAreValid()
    {
        // Arrange
        var email = "teste@teste.com";
        var password = "senha123";
        var fakeUser = new ApplicationUser { Email = email };

        var loginRequest = new LoginRequest { Email = email, Password = password };
        var command = new LoginCommand(loginRequest);

        _userManager.FindByEmailAsync(email).Returns(Task.FromResult(fakeUser));
        _signInManagerWrapper.CheckPasswordSignInAsync(fakeUser, password, false)
                             .Returns(Task.FromResult(SignInResult.Success));
        _jwtTokenGenerator.GenerateToken(fakeUser).Returns("token.jwt.fake");

        // Act
        var token = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal("token.jwt.fake", token);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenUserNotFound()
    {
        // Arrange
        var email = "naoexiste@teste.com";
        var password = "qualquer";

        var loginRequest = new LoginRequest { Email = email, Password = password };
        var command = new LoginCommand(loginRequest);

        _userManager.FindByEmailAsync(email).Returns(Task.FromResult<ApplicationUser>(null));

        // Act & Assert
        var ex = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal("Usuário não encontrado", ex.Message);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenPasswordIsInvalid()
    {
        // Arrange
        var email = "teste@teste.com";
        var password = "senhaErrada";
        var fakeUser = new ApplicationUser { Email = email };

        var loginRequest = new LoginRequest { Email = email, Password = password };
        var command = new LoginCommand(loginRequest);

        _userManager.FindByEmailAsync(email).Returns(Task.FromResult(fakeUser));
        _signInManagerWrapper.CheckPasswordSignInAsync(fakeUser, password, false)
                             .Returns(Task.FromResult(SignInResult.Failed));

        // Act & Assert
        var ex = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal("Senha inválida", ex.Message);
    }
}
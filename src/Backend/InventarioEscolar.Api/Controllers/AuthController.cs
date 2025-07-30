using InventarioEscolar.Application.Services.AuthService.ForgotPassword;
using InventarioEscolar.Application.Services.AuthService.LoginAuth;
using InventarioEscolar.Application.Services.AuthService.RegisterAuth;
using InventarioEscolar.Application.Services.AuthService.ResetPassword;
using InventarioEscolar.Application.Services.Interfaces;
using InventarioEscolar.Communication.Request;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace InventarioEscolar.Api.Controllers
{
    public class AuthController : InventarioApiBaseController
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            await _mediator.Send(new RegisterCommand(request));
            return Ok(new { message = "Usuário registrado com sucesso." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var token = await _mediator.Send(new LoginCommand(request));
            return Ok(new { token });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            await _mediator.Send(new ForgotPasswordCommand(request));
            return Ok(new { message = "Se o e-mail estiver correto, um link foi enviado." });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            await _mediator.Send(new ResetPasswordCommand(request.Email, request.Token, request.NewPassword));
            return Ok(new { message = "Senha redefinida com sucesso." });
        }
    }
}
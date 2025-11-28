using InventarioEscolar.Application.UsesCases.AuthService.ForgotPassword;
using InventarioEscolar.Application.UsesCases.AuthService.LoginAuth;
using InventarioEscolar.Application.UsesCases.AuthService.RegisterAuth;
using InventarioEscolar.Application.UsesCases.AuthService.ResetPassword;
using InventarioEscolar.Communication.Response;
using InventarioEscolar.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using ForgotPasswordRequest = InventarioEscolar.Communication.Request.ForgotPasswordRequest;
using LoginRequest = InventarioEscolar.Communication.Request.LoginRequest;
using RegisterRequest = InventarioEscolar.Communication.Request.RegisterRequest;
using ResetPasswordRequest = InventarioEscolar.Communication.Request.ResetPasswordRequest;

namespace InventarioEscolar.Api.Controllers
{
    public class AuthController(IMediator mediator) : InventarioApiBaseController
    {
        [HttpPost("register")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            await mediator.Send(new RegisterCommand(request));
            return Ok (new { message = "Usuário registrado com sucesso." });
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var token = await mediator.Send(new LoginCommand(request));
            return Ok(new LoginResponse
            {
               AccessToken = token.AccessToken,
               RefreshToken = token.RefreshToken
            });
        }

        [HttpPost("forgot-password")]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            await mediator.Send(new ForgotPasswordCommand(request));
            return Ok(new { message = "Se o e-mail estiver correto, um link foi enviado." });
        }

        [HttpPost("reset-password")]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            await mediator.Send(new ResetPasswordCommand(request.Email, request.Token, request.NewPassword));
            return Ok(new { message = "Senha redefinida com sucesso." });
        }
        [Authorize]
        [HttpPost("refresh")]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest request,
                                         [FromServices] ITokenService tokenService,
                                         [FromServices] IRefreshTokenService refreshTokenService)
        {
            var refreshToken = await refreshTokenService.GetByRefreshToken(request.RefreshToken);

            if (refreshToken == null || !refreshToken.IsActive)
                return Unauthorized("Refresh token inválido ou expirado.");

            var newAccessToken = await tokenService.GenerateToken(refreshToken.User);

            var newRefreshToken = await refreshTokenService.GenerateRefreshToken(refreshToken.User);

            return Ok(new LoginResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken.Token
            });
        }
    }
}
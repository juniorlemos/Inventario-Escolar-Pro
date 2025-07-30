using InventarioEscolar.Communication.Request;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioEscolar.Application.Services.AuthService.ForgotPassword
{
    public record ForgotPasswordCommand(ForgotPasswordRequest Request) : IRequest<Unit>;
}

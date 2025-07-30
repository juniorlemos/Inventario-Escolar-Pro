using InventarioEscolar.Communication.Request;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioEscolar.Application.Services.AuthService.LoginAuth
{
    public record LoginCommand(LoginRequest Request) : IRequest<string>;
}

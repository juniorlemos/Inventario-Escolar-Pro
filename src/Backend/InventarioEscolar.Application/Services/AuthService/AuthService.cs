using InventarioEscolar.Application.Services.Interfaces;
using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Communication.Request;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces;
using InventarioEscolar.Domain.Interfaces.Repositories.Schools;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using Microsoft.AspNetCore.Identity;
using System;
using System.Net;

namespace InventarioEscolar.Application.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly ISchoolWriteOnlyRepository _schoolWriteOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;
        private readonly ISchoolReadOnlyRepository _schoolReadOnlyRepository;
        public AuthService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IJwtTokenGenerator jwtTokenGenerator,
            ISchoolWriteOnlyRepository schoolWriteOnlyRepository,
            IUnitOfWork unitOfWork,
            IEmailService emailService,
            ISchoolReadOnlyRepository schoolReadOnlyRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtTokenGenerator = jwtTokenGenerator;
            _schoolWriteOnlyRepository = schoolWriteOnlyRepository;
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _schoolReadOnlyRepository = schoolReadOnlyRepository;
        }

        public async Task<string> LoginAsync(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                throw new Exception("Usuário não encontrado");

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!result.Succeeded)
                throw new Exception("Senha inválida");

            return _jwtTokenGenerator.GenerateToken(user);
        }

        public async Task RegisterAsync(RegisterRequest request)
        {
            var schoolDuplicate = await _schoolReadOnlyRepository.GetDuplicateSchool(request.SchoolName, request.Inep, request.Address);

            if (schoolDuplicate != null)
            {
                if (string.Equals(schoolDuplicate.Name, request.SchoolName, StringComparison.OrdinalIgnoreCase))
                    throw new DuplicateEntityException(ResourceMessagesException.SCHOOL_NAME_ALREADY_EXISTS);

                if (schoolDuplicate.Inep == request.Inep)
                    throw new DuplicateEntityException(ResourceMessagesException.SCHOOL_INEP_ALREADY_EXISTS);

                if (schoolDuplicate.Address == request.Address)
                    throw new DuplicateEntityException(ResourceMessagesException.SCHOOL_ADDRESS_ALREADY_EXISTS);
            }

            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
                throw new Exception("Este e-mail já está em uso.");

            await _unitOfWork.ExecuteInTransaction(async () =>
            {
                var school = new School
                {
                    Name = request.SchoolName,
                    Inep = request.Inep,
                    Address = request.Address,
                    City = request.City
                };

                await _schoolWriteOnlyRepository.Insert(school);
                await _unitOfWork.Commit(); 

                var user = new ApplicationUser
                {
                    UserName = request.Email,
                    Email = request.Email,
                    SchoolId = school.Id
                };

                var result = await _userManager.CreateAsync(user, request.Password);
                if (!result.Succeeded)
                    throw new Exception("Erro ao criar usuário: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            });
        }


        public async Task ForgotPasswordAsync(ForgotPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                throw new Exception("E-mail não encontrado.");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = $"https://seusite.com/reset-password?token={Uri.EscapeDataString(token)}&email={user.Email}";

            var message = $"<p>Clique no link abaixo para redefinir sua senha:</p><p><a href='{resetLink}'>Redefinir senha</a></p>";
            await _emailService.SendEmailAsync(user.Email, "Redefinir Senha", message);
        }

        public async Task ResetPasswordAsync(string email, string token, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                throw new Exception("Usuário não encontrado.");

            token = WebUtility.UrlDecode(token); // <- decodificação crítica aqui

            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            if (!result.Succeeded)
                throw new Exception("Erro ao redefinir a senha: " + string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }
}
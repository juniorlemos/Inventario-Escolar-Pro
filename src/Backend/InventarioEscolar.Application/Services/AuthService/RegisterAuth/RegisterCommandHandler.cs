using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces;
using InventarioEscolar.Domain.Interfaces.Repositories.Schools;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioEscolar.Application.Services.AuthService.RegisterAuth
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand,Unit>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ISchoolReadOnlyRepository _schoolReadOnlyRepository;
        private readonly ISchoolWriteOnlyRepository _schoolWriteOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterCommandHandler(
            UserManager<ApplicationUser> userManager,
            ISchoolReadOnlyRepository schoolReadOnlyRepository,
            ISchoolWriteOnlyRepository schoolWriteOnlyRepository,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _schoolReadOnlyRepository = schoolReadOnlyRepository;
            _schoolWriteOnlyRepository = schoolWriteOnlyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var r = request.Request;

            var schoolDuplicate = await _schoolReadOnlyRepository.GetDuplicateSchool(r.SchoolName, r.Inep, r.Address);
            if (schoolDuplicate != null)
            {
                if (string.Equals(schoolDuplicate.Name, r.SchoolName, StringComparison.OrdinalIgnoreCase))
                    throw new DuplicateEntityException(ResourceMessagesException.SCHOOL_NAME_ALREADY_EXISTS);

                if (schoolDuplicate.Inep == r.Inep)
                    throw new DuplicateEntityException(ResourceMessagesException.SCHOOL_INEP_ALREADY_EXISTS);

                if (schoolDuplicate.Address == r.Address)
                    throw new DuplicateEntityException(ResourceMessagesException.SCHOOL_ADDRESS_ALREADY_EXISTS);
            }

            var existingUser = await _userManager.FindByEmailAsync(r.Email);
            if (existingUser != null)
                throw new Exception("Este e-mail já está em uso.");

            await _unitOfWork.ExecuteInTransaction(async () =>
            {
                var school = new School
                {
                    Name = r.SchoolName,
                    Inep = r.Inep,
                    Address = r.Address,
                    City = r.City
                };

                await _schoolWriteOnlyRepository.Insert(school);
                await _unitOfWork.Commit();

                var user = new ApplicationUser
                {
                    UserName = r.Email,
                    Email = r.Email,
                    SchoolId = school.Id
                };

                var result = await _userManager.CreateAsync(user, r.Password);
                if (!result.Succeeded)
                    throw new Exception("Erro ao criar usuário: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            });

            return Unit.Value;
        }
    }
}

using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces;
using InventarioEscolar.Domain.Interfaces.Repositories.Schools;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace InventarioEscolar.Application.UsesCases.AuthService.RegisterAuth
{
    public class RegisterCommandHandler(
        UserManager<ApplicationUser> userManager,
        ISchoolReadOnlyRepository schoolReadOnlyRepository,
        ISchoolWriteOnlyRepository schoolWriteOnlyRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<RegisterCommand,Unit>
    {
        public async Task<Unit> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var r = request.Request;

            var schoolDuplicate = await schoolReadOnlyRepository.GetDuplicateSchool(r.SchoolName, r.Inep, r.Address);
            
            if (schoolDuplicate != null)
            {
                if (string.Equals(schoolDuplicate.Name, r.SchoolName, StringComparison.OrdinalIgnoreCase))
                    throw new DuplicateEntityException(ResourceMessagesException.SCHOOL_NAME_ALREADY_EXISTS);

                if (schoolDuplicate.Inep == r.Inep)
                    throw new DuplicateEntityException(ResourceMessagesException.SCHOOL_INEP_ALREADY_EXISTS);

                if (schoolDuplicate.Address == r.Address)
                    throw new DuplicateEntityException(ResourceMessagesException.SCHOOL_ADDRESS_ALREADY_EXISTS);
            }

            var existingUser = await userManager.FindByEmailAsync(r.Email);
           
            if (existingUser != null)
                throw new Exception(ResourceMessagesException.THIS_EMAIL_IS_ALREADY_IN_USE);

            await unitOfWork.ExecuteInTransaction(async () =>
            {
                var school = new School
                {
                    Name = r.SchoolName,
                    Inep = r.Inep,
                    Address = r.Address,
                    City = r.City
                };

                await schoolWriteOnlyRepository.Insert(school);
                await unitOfWork.Commit();

                var user = new ApplicationUser
                {
                    UserName = r.Email,
                    Email = r.Email,
                    SchoolId = school.Id
                };

                var result = await userManager.CreateAsync(user, r.Password);
               
                if (!result.Succeeded)
                    throw new Exception("Erro ao criar usuário: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            });

            return Unit.Value;
        }
    }
}
using InventarioEscolar.Domain.Interfaces;
using InventarioEscolar.Domain.Interfaces.Repositories.Schools;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioEscolar.Application.UsesCases.SchoolCase.Delete
{
    public class DeleteSchoolCommandHandler : IRequestHandler<DeleteSchoolCommand,Unit>
    {
        private readonly ISchoolDeleteOnlyRepository _schoolDeleteOnlyRepository;
        private readonly ISchoolReadOnlyRepository _schoolReadOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteSchoolCommandHandler(
            ISchoolDeleteOnlyRepository schoolDeleteOnlyRepository,
            ISchoolReadOnlyRepository schoolReadOnlyRepository,
            IUnitOfWork unitOfWork)
        {
            _schoolDeleteOnlyRepository = schoolDeleteOnlyRepository;
            _schoolReadOnlyRepository = schoolReadOnlyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteSchoolCommand request, CancellationToken cancellationToken)
        {
            var school = await _schoolReadOnlyRepository.GetById(request.SchoolId)
                ?? throw new NotFoundException(ResourceMessagesException.SCHOOL_NOT_FOUND);

            if (school.RoomLocations.Any() || school.Assets.Any() || school.Categories.Any())
                throw new BusinessException(ResourceMessagesException.CATEGORY_HAS_ASSETS);

            await _schoolDeleteOnlyRepository.Delete(school.Id);

            await _unitOfWork.Commit();

            return Unit.Value;
        }
    }
}

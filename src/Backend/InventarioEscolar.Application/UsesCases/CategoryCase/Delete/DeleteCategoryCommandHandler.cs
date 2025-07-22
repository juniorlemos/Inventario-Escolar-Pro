using InventarioEscolar.Application.Services.Interfaces;
using InventarioEscolar.Domain.Interfaces;
using InventarioEscolar.Domain.Interfaces.Repositories.Categories;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioEscolar.Application.UsesCases.CategoryCase.Delete
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, Unit>
    {
        private readonly ICategoryDeleteOnlyRepository _categoryDeleteOnlyRepository;
        private readonly ICategoryReadOnlyRepository _categoryReadOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public DeleteCategoryCommandHandler(
            ICategoryDeleteOnlyRepository categoryDeleteOnlyRepository,
            ICategoryReadOnlyRepository categoryReadOnlyRepository,
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _categoryDeleteOnlyRepository = categoryDeleteOnlyRepository;
            _categoryReadOnlyRepository = categoryReadOnlyRepository;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<Unit> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _categoryReadOnlyRepository.GetById(request.categoryId)
                ?? throw new NotFoundException(ResourceMessagesException.CATEGORY_NOT_FOUND);

            if (category.Assets.Any())
                throw new BusinessException(ResourceMessagesException.CATEGORY_HAS_ASSETS);

            if (category.SchoolId != _currentUser.SchoolId)
                throw new BusinessException(ResourceMessagesException.CATEGORY_NOT_BELONG_TO_SCHOOL);

            await _categoryDeleteOnlyRepository.Delete(category.Id);
            await _unitOfWork.Commit();

            return Unit.Value;
        }
    }
}
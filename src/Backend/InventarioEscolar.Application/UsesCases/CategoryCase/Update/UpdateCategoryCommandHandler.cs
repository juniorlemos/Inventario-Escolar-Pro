using FluentValidation;
using InventarioEscolar.Application.Services.Interfaces;
using InventarioEscolar.Application.Services.Validators;
using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Interfaces;
using InventarioEscolar.Domain.Interfaces.Repositories.Categories;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioEscolar.Application.UsesCases.CategoryCase.Update
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<UploadCategoryDto> _validator;
        private readonly ICategoryReadOnlyRepository _categoryReadOnlyRepository;
        private readonly ICategoryUpdateOnlyRepository _categoryUpdateOnlyRepository;
        private readonly ICurrentUserService _currentUser;

        public UpdateCategoryCommandHandler(
            IUnitOfWork unitOfWork,
            IValidator<UploadCategoryDto> validator,
            ICategoryReadOnlyRepository categoryReadOnlyRepository,
            ICategoryUpdateOnlyRepository categoryUpdateOnlyRepository,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
            _categoryReadOnlyRepository = categoryReadOnlyRepository;
            _categoryUpdateOnlyRepository = categoryUpdateOnlyRepository;
            _currentUser = currentUser;
        }

        public async Task<Unit> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowIfInvalid(request.CategoryDto);

            var category = await _categoryReadOnlyRepository.GetById(request.Id);

            if (category is null)
                throw new NotFoundException(ResourceMessagesException.CATEGORY_NOT_FOUND);

            if (category.SchoolId != _currentUser.SchoolId)
                throw new BusinessException(ResourceMessagesException.CATEGORY_NOT_BELONG_TO_SCHOOL);

            request.CategoryDto.Adapt(category);

            _categoryUpdateOnlyRepository.Update(category);
            await _unitOfWork.Commit();

            return Unit.Value;
        }
    }
}

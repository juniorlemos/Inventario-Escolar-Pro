using FluentValidation;
using InventarioEscolar.Application.Dtos;
using InventarioEscolar.Application.Services.Interfaces;
using InventarioEscolar.Application.Services.Validators;
using InventarioEscolar.Application.UsesCases.CategoryCase.Register;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces;
using InventarioEscolar.Domain.Interfaces.Repositories.Categories;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using Mapster;
using MediatR;

public class RegisterCategoryCommandHandler : IRequestHandler<RegisterCategoryCommand, CategoryDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CategoryDto> _validator;
    private readonly ICategoryReadOnlyRepository _categoryReadOnlyRepository;
    private readonly ICategoryWriteOnlyRepository _categoryWriteOnlyRepository;
    private readonly ICurrentUserService _currentUser;

    public RegisterCategoryCommandHandler(
        IUnitOfWork unitOfWork,
        IValidator<CategoryDto> validator,
        ICategoryReadOnlyRepository categoryReadOnlyRepository,
        ICategoryWriteOnlyRepository categoryWriteOnlyRepository,
        ICurrentUserService currentUser)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
        _categoryReadOnlyRepository = categoryReadOnlyRepository;
        _categoryWriteOnlyRepository = categoryWriteOnlyRepository;
        _currentUser = currentUser;
    }

    public async Task<CategoryDto> Handle(RegisterCategoryCommand request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowIfInvalid(request.CategoryDto);

        if (!_currentUser.IsAuthenticated)
             throw new BusinessException(ResourceMessagesException.SCHOOL_NOT_FOUND);

        var exists = await _categoryReadOnlyRepository
            .ExistCategoryName(request.CategoryDto.Name, _currentUser.SchoolId);

        if (exists)
            throw new DuplicateEntityException(ResourceMessagesException.CATEGORY_NAME_ALREADY_EXISTS);

        var category = request.CategoryDto.Adapt<Category>();
        category.SchoolId = _currentUser.SchoolId;

        await _categoryWriteOnlyRepository.Insert(category);
        await _unitOfWork.Commit();

        return category.Adapt<CategoryDto>();
    }
}

﻿using AutoMapper;
using InventarioEscolar.Api.Extension;
using InventarioEscolar.Communication.Request;
using InventarioEscolar.Communication.Response;
using InventarioEscolar.Domain.Repositories;
using InventarioEscolar.Domain.Repositories.Asset;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;

namespace InventarioEscolar.Application.UsesCases.Asset.Register
{
    public class RegisterAssetUseCase :IRegisterAssetUseCase
    {
        private readonly IAssetWriteOnlyRepository _writeOnlyRepository;
        private readonly IAssetReadOnlyRepository _repositoryReadOnly;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterAssetUseCase(
            IAssetWriteOnlyRepository writeOnlyRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IAssetReadOnlyRepository repositoryReadOnly)
        {
            _writeOnlyRepository = writeOnlyRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _repositoryReadOnly = repositoryReadOnly;
        }

        public async Task<ResponseRegisterAssetJson> Execute(RequestRegisterAssetJson request)
        {
            var exist = await _repositoryReadOnly.ExistPatrimonyCode(request.PatrimonyCode);

            if (exist)
                throw new DuplicateEntityException(ResourceMessagesException.PATRIMONYCODE_ALREADY_EXISTS_);

            await Validate(request);

            var asset = _mapper.Map<Domain.Entities.Asset>(request);

            await _writeOnlyRepository.Add(asset);
            await _unitOfWork.Commit();

            return new ResponseRegisterAssetJson
            {
                Name = request.Name
            };

        }

        private async Task Validate(RequestRegisterAssetJson request)
        {
            var validator = new RegisterAssetValidator();
            var result = await validator.ValidateAsync(request);

            if (result.IsValid.IsFalse())
            {
                var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errorMessages);
            }

        }
    }
}

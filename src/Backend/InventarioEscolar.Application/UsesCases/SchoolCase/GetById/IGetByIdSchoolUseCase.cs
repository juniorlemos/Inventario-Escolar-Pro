using InventarioEscolar.Communication.Dtos;

namespace InventarioEscolar.Application.UsesCases.SchoolCase.GetById
{
    public interface IGetByIdSchoolUseCase
    {
        Task<SchoolDto> Execute(long schoolId);
    }
}

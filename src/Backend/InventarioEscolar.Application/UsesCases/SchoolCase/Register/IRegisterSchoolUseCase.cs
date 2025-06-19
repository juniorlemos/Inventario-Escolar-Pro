using InventarioEscolar.Communication.Dtos;

namespace InventarioEscolar.Application.UsesCases.SchoolCase.Register
{
    public interface IRegisterSchoolUseCase  
    {
        Task<SchoolDto> Execute(SchoolDto schoolDto);
    }
}

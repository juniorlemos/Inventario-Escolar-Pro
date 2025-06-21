using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Entities;

namespace InventarioEscolar.Application.UsesCases.SchoolCase.Update
{
    public interface IUpdateSchoolUseCase
    {
      Task Execute(long id, SchoolDto schooDto);
    }
}

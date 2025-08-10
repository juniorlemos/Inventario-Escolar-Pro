using InventarioEscolar.Domain.Interfaces;
using NSubstitute;

namespace CommonTestUtilities.Repositories
{
    public class UnitOfWorkBuilder 
    {
        private readonly IUnitOfWork _unitOfWork;
        public UnitOfWorkBuilder()
        {
            _unitOfWork = Substitute.For<IUnitOfWork>();
        }
        public IUnitOfWork Build() => _unitOfWork;
    }
}
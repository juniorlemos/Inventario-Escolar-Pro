using InventarioEscolar.Domain.Interfaces;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

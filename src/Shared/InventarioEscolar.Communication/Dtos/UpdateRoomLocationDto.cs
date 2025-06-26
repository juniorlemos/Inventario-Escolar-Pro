using InventarioEscolar.Communication.Dtos.Interfaces;

namespace InventarioEscolar.Communication.Dtos
{
    public class UpdateRoomLocationDto : IRoomLocationBaseDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Building { get; set; }
    }
}

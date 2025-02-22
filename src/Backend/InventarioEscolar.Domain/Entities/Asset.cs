﻿using InventarioEscolar.Domain.Enums;

namespace InventarioEscolar.Domain.Entities
{
    public class Asset : EntityBase
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public long? PatrimonyCode { get; set; }
        public decimal? AcquisitionValue{ get; set;} 
        public ConservationState ConservationState { get; set; }
        public string? SerieNumber { get; set; }

        public long CategoryId { get; set; }
        public  Category Category { get; set; } = null!;

        public long? RoomLocationId { get; set; }
        public RoomLocation? RoomLocation { get; set; }

    }
}

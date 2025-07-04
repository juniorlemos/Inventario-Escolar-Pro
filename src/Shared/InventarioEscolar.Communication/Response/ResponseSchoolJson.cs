﻿using InventarioEscolar.Application.Dtos;
using InventarioEscolar.Communication.Dtos;

namespace InventarioEscolar.Communication.Response
{
    public record ResponseSchoolJson
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Inep { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }

        public ICollection<RoomLocationSummaryDto> RoomLocations { get; set; } = [];
        public ICollection<AssetSummaryDto> Assets { get; set; } = [];
        public ICollection<CategorySummaryDto> Categories { get; set; } = [];

    }
}

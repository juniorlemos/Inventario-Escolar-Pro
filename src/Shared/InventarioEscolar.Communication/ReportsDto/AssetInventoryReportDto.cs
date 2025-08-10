namespace InventarioEscolar.Communication.ReportsDto
{
    public record AssetInventoryReportDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string RoomName { get; set; } = string.Empty;
        public DateTime EntryDate { get; set; }
    }
}
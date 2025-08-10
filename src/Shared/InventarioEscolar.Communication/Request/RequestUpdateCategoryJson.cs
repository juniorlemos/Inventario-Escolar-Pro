namespace InventarioEscolar.Communication.Request
{
    public record RequestUpdateCategoryJson
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
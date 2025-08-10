namespace InventarioEscolar.Communication.Request
{
    public class RequestRegisterCategoryJson
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public long SchoolId { get; set; }
    }
}
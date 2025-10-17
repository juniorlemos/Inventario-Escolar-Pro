namespace InventarioEscolar.Communication.Request
{
    public class RequestRegisterSchoolJson
    {
        public string Name { get; set; } = string.Empty;
        public string? Inep { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
    }
}
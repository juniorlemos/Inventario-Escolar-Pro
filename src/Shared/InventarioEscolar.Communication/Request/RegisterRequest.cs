namespace InventarioEscolar.Communication.Request
{
    public record RegisterRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string SchoolName { get; set; } = string.Empty;
        public string? Inep { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
    }
}
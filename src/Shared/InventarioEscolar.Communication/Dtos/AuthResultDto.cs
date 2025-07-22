namespace InventarioEscolar.Communication.Dtos
{
    public record AuthResultDto
    {
        public bool IsSuccess { get; set; }
        public string? Token { get; set; } 
        public string? ErrorMessage { get; set; }
    }
}

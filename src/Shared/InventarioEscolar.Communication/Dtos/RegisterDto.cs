﻿namespace InventarioEscolar.Communication.Dtos
{
    public record RegisterDto
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public long SchoolId { get; set; }
    }
}

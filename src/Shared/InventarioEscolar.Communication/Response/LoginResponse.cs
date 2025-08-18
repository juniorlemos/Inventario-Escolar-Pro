﻿namespace InventarioEscolar.Communication.Response
{
    public record LoginResponse
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}

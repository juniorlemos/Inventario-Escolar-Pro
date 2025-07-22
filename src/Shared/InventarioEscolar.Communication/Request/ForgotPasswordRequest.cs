namespace InventarioEscolar.Communication.Request
{
    public record ForgotPasswordRequest
    {
        public string Email { get; set; } = string.Empty;
    }
}

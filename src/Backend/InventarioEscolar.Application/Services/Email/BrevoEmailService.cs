using InventarioEscolar.Application.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;

namespace InventarioEscolar.Application.Services.Email
{
    public class BrevoEmailService(IConfiguration configuration) : IEmailService
    {
        private readonly HttpClient _httpClient = new();
        private readonly string _apiKey = configuration["Brevo:ApiKey"];

        public async Task SendEmailAsync(string toEmail, string subject, string htmlContent)
        {
            var request = new
            {
                sender = new { name = "Inventário Escolar", email = "juniorlemosoi@gmail.com" },
                to = new[] { new { email = toEmail } },
                subject = subject,
                htmlContent = htmlContent
            };

            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            
            _httpClient.DefaultRequestHeaders.Add("api-key", _apiKey);

            var response = await _httpClient.PostAsync("https://api.brevo.com/v3/smtp/email", content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Erro ao enviar email: {error}");
            }
        }
    }
}
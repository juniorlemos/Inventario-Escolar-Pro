using InventarioEscolar.Application.Services.Interfaces;
using NSubstitute;

namespace CommonTestUtilities.Services
{
    public class EmailServiceBuilder
    {
        private readonly IEmailService emailService;
        public EmailServiceBuilder()
        {
            emailService = Substitute.For<IEmailService>();
        }
        public EmailServiceBuilder WithSendEmailAction(Action<string, string, string> action)
        {
            emailService.SendEmailAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>())
                .Returns(callInfo =>
                {
                    var to = callInfo.Arg<string>();
                    var subject = callInfo.Arg<string>();
                    var body = callInfo.Arg<string>();
                    action(to, subject, body);
                    return Task.CompletedTask;
                });
            return this;
        }
        public IEmailService Build() => emailService;
    }
}
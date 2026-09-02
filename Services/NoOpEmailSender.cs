using Microsoft.AspNetCore.Identity.UI.Services;

namespace TutorBridge.Services;

/// <summary>
/// Fallback used whenever no Brevo:ApiKey is configured — most notably, on a
/// marker's machine after cloning the repo fresh, since the real API key is
/// never committed to source control. Logs the email content instead of
/// sending it, so register/resend/forgot-password all stay demonstrable
/// without real email infrastructure.
///
/// NOTE: currently only logs (visible via `dotnet run` console output or the
/// VS Debug window). If you'd rather this surface directly in the UI, that's
/// a small follow-on change once we're editing the relevant Razor pages.
/// </summary>
public class NoOpEmailSender(ILogger<NoOpEmailSender> logger) : IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        logger.LogInformation(
            "[NoOpEmailSender] No Brevo API key configured. Email to {Email} — Subject: {Subject}\n{Body}",
            email, subject, htmlMessage);
        return Task.CompletedTask;
    }
}
using System.Net.Http.Json;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;

namespace TutorBridge.Services;

/// <summary>
/// Sends real email via Brevo's transactional email API (https://api.brevo.com/v3/smtp/email).
/// Registered in DI only when Brevo:ApiKey is present in configuration — see Program.cs.
/// Requires an HttpClient registered via builder.Services.AddHttpClient&lt;BrevoEmailSender&gt;().
/// </summary>
public class BrevoEmailSender(
    HttpClient httpClient,
    IOptions<BrevoOptions> options,
    ILogger<BrevoEmailSender> logger) : IEmailSender
{
    private readonly BrevoOptions _options = options.Value;

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var payload = new
        {
            sender = new { email = _options.SenderEmail, name = _options.SenderName },
            to = new[] { new { email } },
            subject,
            htmlContent = htmlMessage
        };

        using var request = new HttpRequestMessage(HttpMethod.Post, "https://api.brevo.com/v3/smtp/email")
        {
            Content = JsonContent.Create(payload)
        };
        request.Headers.Add("api-key", _options.ApiKey);

        var response = await httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync();
            logger.LogError(
                "Brevo email send failed ({StatusCode}) for {Email}: {Body}",
                response.StatusCode, email, body);

            // Deliberately not throwing: a Brevo outage/misconfiguration shouldn't
            // block registration. The user can still hit Resend.
        }
    }
}
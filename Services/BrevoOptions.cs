namespace TutorBridge.Services;

/// <summary>
/// Bound from configuration section "Brevo". ApiKey is expected to come from
/// user secrets locally (dotnet user-secrets set "Brevo:ApiKey" "...") or from
/// environment variables/host configuration in any deployed environment —
/// never from appsettings.json or appsettings.Development.json.
/// </summary>
public class BrevoOptions
{
    public const string SectionName = "Brevo";

    public string ApiKey { get; set; } = string.Empty;
    public string SenderEmail { get; set; } = string.Empty;
    public string SenderName { get; set; } = "TutorBridge";
}
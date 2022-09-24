namespace Authentication;

public class JwtSettings
{
    public const string SectionName = "Jwt";
    public string Issuer { get; set; } = "localhost";
    public string Audience { get; set; } = "localhost";
    public int? ExpirationMinutes { get; set; }
    public string CertificatePath { get; set; } = string.Empty;
    public string CertificatePassword { get; set; } = string.Empty;
}


namespace Authentication;

public class JwtSettings
{
    public const string SectionName = "Jwt";
    public string? Secret { get; set; }
    public string Issuer { get; set; } = "localhost";
    public string Audience { get; set; } = "localhost";
    public int? ExpirationMinutes { get; set; }
}


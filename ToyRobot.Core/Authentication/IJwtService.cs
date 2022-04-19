namespace Authentication;

public interface IJwtService
{
    public IDictionary<string, object> Decode(string token);
    public string CreateToken(IDictionary<string, object> claims);
}

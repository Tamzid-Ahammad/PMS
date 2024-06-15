namespace Api
{
    public class JwtSettings
    {
        public string Issuer { get; set; }
        public string Secret { get; set; }
        public int ExpirationInDays { get; set; }
        public int RefreshTokenValidityInDays { get; set; }
    }
}

namespace EducationApp.Shared.Configs
{
    public class JwtConfig
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Key { get; set; }
        public int AccessLifetime { get; set; }
        public int RefreshLifetime { get; set; }
        public int ClockSkew { get; set; }
    }
}

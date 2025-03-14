namespace ExamProj.Auth
{
    public class TokenConfig
    {
        public string Key { get; set; } 
        public string Issuer { get; set; }
        public string Audience { get; set; } 
        public int ExpirationDurationInMinutes { get; set; }
    }

}

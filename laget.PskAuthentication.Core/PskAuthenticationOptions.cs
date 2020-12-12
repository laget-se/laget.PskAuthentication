namespace laget.PskAuthentication.Core
{
    public interface IPskAuthenticationOptions
    {
        string RijndaelKey { get; set; }
        string RijndaelIV { get; set; }
        string Salt { get; set; }
        string Secret { get; set; }
        int Ttl { get; set; }
        string HeaderName { get; set; }
    }

    public class PskAuthenticationOptions : IPskAuthenticationOptions
    {
        public string RijndaelKey { get; set; }
        public string RijndaelIV { get; set; }
        public string Salt { get; set; }
        public string Secret { get; set; }
        public int Ttl { get; set; } = 900;
        public string HeaderName { get; set; } = "X-PSK-Authorization";

        public bool IsValid => !string.IsNullOrWhiteSpace(RijndaelKey) &&
                               !string.IsNullOrWhiteSpace(RijndaelIV) &&
                               !string.IsNullOrWhiteSpace(Salt) &&
                               !string.IsNullOrWhiteSpace(Secret);

    }
}

namespace laget.PskAuthentication.Core
{
    public interface IPskAuthenticationOptions
    {
        string Key { get; set; }
        string IV { get; set; }
        string Salt { get; set; }
        string Secret { get; set; }
        int Ttl { get; set; }
        string HeaderName { get; set; }
    }

    public class PskAuthenticationOptions : IPskAuthenticationOptions
    {
        public string Key { get; set; }
        public string IV { get; set; }
        public string Salt { get; set; }
        public string Secret { get; set; }
        public int Ttl { get; set; } = 900;
        public string HeaderName { get; set; } = "X-PSK-Authorization";

        public bool IsValid => !string.IsNullOrWhiteSpace(Key) &&
                               !string.IsNullOrWhiteSpace(IV) &&
                               !string.IsNullOrWhiteSpace(Salt) &&
                               !string.IsNullOrWhiteSpace(Secret);

    }
}

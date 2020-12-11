namespace laget.PskAuthentication.Core
{
    public class PskAuthenticationOptions
    {
        public string RijndaelKey { get; set; }
        public string RijndaelIV { get; set; }
        public string Salt { get; set; }
        public string Secret { get; set; }
    }
}

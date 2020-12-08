namespace laget.PskAuthentication.Extensions
{
    public static class PskExtensions
    {
        public static bool IsEqualTo(this Psk psk, string hash)
        {
            return psk.Hash == hash;
        }
    }
}

using laget.PskAuthentication.Core;
using System.Net.Http;

namespace laget.PskAuthentication.Client
{
    public static class HttpClientExtensions
    {
        public static HttpClient AddPskAuthentication(this HttpClient client, string iv, string key, string salt, string secret, int ttl = 900)
        {
            var hash = new PskGenerator(iv, key).Generate(salt, secret, ttl);

            client.DefaultRequestHeaders.Add("X-PSK-Authorization", hash);

            return client;
        }

        public static HttpClient AddPskAuthentication(this HttpClient client, PskAuthenticationOptions options)
        {
            var hash = new PskGenerator(options).Generate();

            client.DefaultRequestHeaders.Add("X-PSK-Authorization", hash);

            return client;
        }
    }
}

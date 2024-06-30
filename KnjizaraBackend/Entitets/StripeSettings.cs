using Sieve.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Security.Permissions;

namespace Knjizara.Entitets
{
    public class StripeSettings
    {
        public string PublishableKey { get; set; }
        public string SecretKey { get; set; }
        public string WhSecret { get; set; }
    }
}

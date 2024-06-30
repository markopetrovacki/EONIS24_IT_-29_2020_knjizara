using Knjizara.Entitets;
using Knjizara.Entitets.Confirmation;
using Knjizara.Models.Korisnik;

namespace Knjizara.Data
{
    public interface IPayment
    {

        public Task<Porudzbina> CreateOrUpdatePaymentIntent(Guid porudzbinaId);

    }
}

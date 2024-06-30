using AutoMapper;
using Knjizara.Entitets;
using Knjizara.Entitets.Confirmation;
using Knjizara.Models.Dostava;
using Microsoft.Extensions.Options;
using Stripe;

namespace Knjizara.Data
{
    public class PaymentRepository : IPayment
    {
        private readonly IPorudzbinaRepository _porudzbinaRepository;
        private readonly StripeSettings _stripeSettings;

        public PaymentRepository(IPorudzbinaRepository porudzbinaRepository, IOptions<StripeSettings> stripeSettings)
        {
            _porudzbinaRepository = porudzbinaRepository;
            _stripeSettings = stripeSettings.Value;
        }

        public async Task<Porudzbina> CreateOrUpdatePaymentIntent(Guid porudzbinaId)
        {
            StripeConfiguration.ApiKey = _stripeSettings.SecretKey;

            var porudzbina =  _porudzbinaRepository.GetPorudzbinaId(porudzbinaId);

            if(porudzbina == null)
            {
                throw new KeyNotFoundException(nameof(porudzbina));
            }

            var service = new PaymentIntentService();

            PaymentIntent intent;

          /*  if (string.IsNullOrEmpty(porudzbina.)
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)porudzbina.ukupna_cena.Sum(i => indexer.Quantity * (i.Price * 100)),
                    Currency = "rsd",
                    PaymentMethodTypes = new List<string> { "card" },
                };

                intent = await service.CreateAsync(options);
                
                porudzbina.ClientSecret = intent.ClientSecret;
            }
            else
            {
                var option = new PaymentIntentUpdateOptions
                {
                    Amount = (long)porudzbina.ukupna_cena
                }
            }*/

            _porudzbinaRepository.SaveChanges();
            return porudzbina;
        }
    }
}

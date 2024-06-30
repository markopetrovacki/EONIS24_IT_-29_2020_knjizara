using AutoMapper;
using Knjizara.Data;
using Knjizara.Entitets;
using Knjizara.Entitets.Confirmation;
using Knjizara.Models.Dobavljac;
using Knjizara.Models.Knjiga;
using Knjizara.Models.Porudzbina;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Sieve.Models;
using Sieve.Services;
using Stripe;
using Stripe.Checkout;

namespace Knjizara.Controllers
{
    [ApiController]
    [Route("api/create-checkout-session")]
    [Produces("application/json", "application/xml")]
    public class PaymentController : Controller
    {
      
        private readonly IMapper mapper;
        private readonly IPorudzbinaRepository porudzbinaRepository;
        private readonly IPorudzbinaKnjigaRepository porudzbinaKnjigaRepository;
        private readonly IKnjigaRepository knjigaRepository;
        private readonly string _whSecret;

        private readonly ILogger<PaymentController> _logger;

        public PaymentController(IMapper mapper, IPorudzbinaRepository porudzbinaRepository, IPorudzbinaKnjigaRepository porudzbinaKnjigaRepository, IKnjigaRepository knjigaRepository, IConfiguration config, ILogger<PaymentController> logger)
        {
            this.porudzbinaRepository = porudzbinaRepository;
            this.mapper = mapper;
            this.porudzbinaKnjigaRepository = porudzbinaKnjigaRepository;
            this.knjigaRepository = knjigaRepository;
            _whSecret = config.GetSection("StripeSettings:WhSecret").Value;
            this._logger = logger;
        }

        /*
        [Authorize]
        [HttpPost("{porudzbinaId}")] 
        public async Task<ActionResult<PorudzbinaDto>> CreateOrUpdatePaymentIntent(Guid porudzbinaId)
        {
            var porudzbina = await _paymentService.CreateOrUpdatePaymentIntent(porudzbinaId);

            if (porudzbina == null) return BadRequest();

            return mapper.Map<PorudzbinaDto>(porudzbina);
        }
        */


        [HttpPost("{id_porudzbina}")]
        public async Task<ActionResult<CreateSessionResponse>> CreateOrUpdatePaymentIntent( Guid id_porudzbina )
        {
                       
            var domain = "http://localhost:4242";

            
            var porudzbina = this.porudzbinaRepository.GetPorudzbinaId(id_porudzbina);
            Console.WriteLine(porudzbina);

            if (porudzbina == null)
            {
              
                throw new KeyNotFoundException(nameof(porudzbina));
            }

            var lineItems = new List<SessionLineItemOptions>();

            var porudzbine = porudzbinaKnjigaRepository.GetPorudzbinaKnjigaByPorudzbinaId(id_porudzbina);

            foreach (var porudzbinaItem in porudzbine)
            {
                Knjiga knjiga = knjigaRepository.GetKnjigaId(porudzbinaItem.id_knjige);

                Console.WriteLine(knjiga);
                lineItems.Add(new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {

                        Currency = "rsd",
                        UnitAmount = (long)(knjiga.cena * 100),

                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = knjiga.naziv_knjige,
                            Images = new List<string> { knjiga.slika },

                        },

                    },
                    Quantity = porudzbinaItem.kolicina,
                });
            }

            var options = new SessionCreateOptions
            {
                LineItems = lineItems,
                Mode = "payment",
                SuccessUrl = "http://localhost:4200/products",
                CancelUrl = "http://localhost:4200/products",
                PaymentIntentData = new SessionPaymentIntentDataOptions
                {
                    Metadata = new Dictionary<string, string>
                    {
                        {"id_porudzbina", id_porudzbina.ToString()},
                    }

                }
            };

            var service = new SessionService();
            Session session = service.Create(options);

            
            var response = new CreateSessionResponse { SessionId = session.Id };

            return Ok(response);
        }


        [HttpPost("webhook")]
        public async Task<IActionResult> WebHook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            // Console.WriteLine(json);
           // _logger.LogInformation("Received JSON: {json}", json);
            string endpointSecret = _whSecret;

            try
            {
           
                var stripeEvent = EventUtility.ParseEvent(json);
                var signatureHeader = Request.Headers["Stripe-Signature"];

                //_logger.LogInformation("Signature Header: {signatureHeader}", signatureHeader);

                stripeEvent = EventUtility.ConstructEvent(json,
                        signatureHeader, endpointSecret);

                //_logger.LogInformation("Signature Event: {signatureHeader}", stripeEvent);

                if (stripeEvent.Type == Events.PaymentIntentSucceeded)
                {
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                   // Console.WriteLine("A successful payment for {0} was made.", paymentIntent.Amount);
                    _logger.LogInformation("A successful payment for {0} was made.", paymentIntent.Amount);
                    Console.WriteLine(stripeEvent);

                    var id_porudzbina = paymentIntent.Metadata["id_porudzbina"]; // Assuming OrderId is stored in metadata
                    Console.WriteLine(id_porudzbina);

                    if (Guid.TryParse(id_porudzbina, out Guid parsedPorudzbinaId))
                    {
                        Porudzbina porudzbinaModel = porudzbinaRepository.GetPorudzbinaId(parsedPorudzbinaId);
                        //Console.WriteLine(porudzbinaModel);
                        /*var updateOrderCommand = new PorudzbinaUpdateDto
                        {
                            id_porudzbina = parsedPorudzbinaId,
                            status_placanja = "Placeno"
                        };*/
                        porudzbinaModel.status_placanja = "Placeno";

                        // await mediator.Send(updateOrderCommand);
                        Porudzbina mappedPorudzbina = mapper.Map<Porudzbina>(porudzbinaModel);

                        // Call the repository method to update the backlog
                        var updatedPorudzbina = porudzbinaRepository.UpdatePorudzbina(mappedPorudzbina);
                        porudzbinaRepository.SaveChanges();
                    }          


                }
                else if (stripeEvent.Type == Events.PaymentMethodAttached)
                {
                    var paymentMethod = stripeEvent.Data.Object as PaymentMethod;
                    //Console.WriteLine("PAYMENT INTENT WIThhhhH ID: {0} CANCELED.");
                    _logger.LogInformation("PAYMENT INTENT WIThhhhH ID: {0} CANCELED.");

                }
                else if (stripeEvent.Type == Events.PaymentIntentPaymentFailed)
                {
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                   // Console.WriteLine("PAYMENT INTENT WITH ID: {0} failed.", paymentIntent.Id);
                    _logger.LogInformation("PAYMENT INTENT WITH ID: {0} failed.", paymentIntent.Id);
                    // Handle the payment_intent.payment_failed event here
                }
                else if (stripeEvent.Type == Events.PaymentIntentCanceled)
                {
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                   // Console.WriteLine("PAYMENT INTENT WITH ID: {0} CANCELED.", paymentIntent.Id);
                    _logger.LogInformation("PAYMENT INTENT WITH ID: {0} CANCELED.", paymentIntent.Id);
                    // Handle the payment_intent.payment_failed event here
                }
                else
                {
                  //  Console.WriteLine("UNHANDELED!! event type: {0}", stripeEvent.Type);
                    _logger.LogInformation("UNHANDELED!! event type: {0}", stripeEvent.Type);
                }
                return Ok();
            }
            
            catch (StripeException e)
            {
               // Console.WriteLine("Error: {0}", e.Message);
                _logger.LogInformation("Error: {0}", e.Message);
                return BadRequest();
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }

    }
}

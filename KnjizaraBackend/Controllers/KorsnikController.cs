using AutoMapper;
using Knjizara.Data;
using Knjizara.Entitets;
using Knjizara.Entitets.Confirmation;
using Knjizara.Models.Dobavljac;
using Knjizara.Models.Korisnik;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using Sieve.Services;

namespace Knjizara.Controllers
{
    [ApiController]
    [Route("api/knjizara/korisnik")]
    [Produces("application/json", "application/xml")]
    public class KorsnikController : Controller
    {
        private readonly IMapper mapper;
        private readonly IKorisnikRepository korisnikRepository;
        private readonly LinkGenerator linkGenerator;
        private readonly SieveProcessor sieveProcessor;


        public KorsnikController(IMapper mapper, IKorisnikRepository korisnikRepository, LinkGenerator linkGenerator, SieveProcessor sieveProcessor)
        {
            this.mapper = mapper;
            this.korisnikRepository = korisnikRepository;
            this.linkGenerator = linkGenerator;
            this.sieveProcessor = sieveProcessor;
        }

        [HttpGet]
        [HttpHead] //Podržavamo i HTTP head zahtev koji nam vraća samo zaglavlja u odgovoru    
        [ProducesResponseType(StatusCodes.Status200OK)] //Eksplicitno definišemo šta sve može ova akcija da vrati
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<Korisnik>>> GetKorisnik([FromQuery] SieveModel model)
        {
            List<Korisnik> korisnik = korisnikRepository.GetKorisnik();
            if (korisnik == null || korisnik.Count == 0)
            {
                NoContent();
            }

            korisnik = sieveProcessor.Apply<Korisnik>(model, korisnik.AsQueryable()).ToList();
            return Ok(mapper.Map<List<KorisnikDto>>(korisnik));

        }


        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{id_korisnik}")]
        public ActionResult<Korisnik> GetKorisnikId(Guid id_korisnik)
        {
            Korisnik korisnikModel = korisnikRepository.GetKorisnikId(id_korisnik);
            if (korisnikModel == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<KorisnikDto>(korisnikModel));

        }


        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<KorisnikConfirmation> CreateKorisnik([FromBody] KorisnikCreationDto korisnik)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState); // Vraćajte 400 Bad Request za validacione greške
                }


                Korisnik mappedKorisnik = mapper.Map<Korisnik>(korisnik);
                KorisnikConfirmation confirmationKorisnik = korisnikRepository.AddKorisnik(mappedKorisnik);
                korisnikRepository.SaveChanges();

                string location = linkGenerator.GetPathByAction("GetKorisnikId", "Korisnik", new { id_korisnik = confirmationKorisnik.id_korisnik });

                return Created(location, mapper.Map<KorisnikConfirmationDto>(confirmationKorisnik));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Create Error");
            }
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{id_korisnik}")]
        public IActionResult DeleteKorisnik(Guid id_korisnik)
        {
            try
            {
                Korisnik korisnik = korisnikRepository.GetKorisnikId(id_korisnik);
                if (korisnik == null)
                {
                    return NotFound();
                }

                korisnikRepository.RemoveKorisnik(id_korisnik);
                korisnikRepository.SaveChanges();
                return StatusCode(StatusCodes.Status204NoContent, "Uspesno obrisan korisnik!");
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete Error");
            }
        }

        [HttpPut]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<KorisnikDto> UpdateKorisnik(KorisnikUpdateDto korisnik)
        {
            try
            {
                // Map the DTO to the domain model
                Korisnik mappedKorisnik = mapper.Map<Korisnik>(korisnik);

                // Call the repository method to update the backlog
                var updatedKorisnik = korisnikRepository.UpdateKorisnik(mappedKorisnik);

                // Map the updated epic to DTO
                KorisnikDto updatedKorisnikDto = mapper.Map<KorisnikDto>(updatedKorisnik);

                // Return the updated resource
                return Ok(UpdateKorisnik);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Update Error");
            }
        }
    }
}

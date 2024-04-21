using AutoMapper;
using Knjizara.Data;
using Knjizara.Entitets;
using Knjizara.Entitets.Confirmation;
using Knjizara.Models.Dobavljac;
using Knjizara.Models.Knjiga;
using Knjizara.Models.Porudzbina;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Sieve.Models;
using Sieve.Services;

namespace Knjizara.Controllers
{
    [ApiController]
    [Route("api/knjizara/knjiga")]
    [Produces("application/json", "application/xml")]
    public class KnjigaController : Controller
    {
        private readonly IMapper mapper;
        private readonly IKnjigaRepository knjigaRepository;
        private readonly LinkGenerator linkGenerator;
        private readonly SieveProcessor sieveProcessor;


        public KnjigaController(IMapper mapper, IKnjigaRepository knjigaRepository, LinkGenerator linkGenerator, SieveProcessor sieveProcessor)
        {
            this.mapper = mapper;
            this.knjigaRepository = knjigaRepository;
            this.linkGenerator = linkGenerator;
            this.sieveProcessor = sieveProcessor;
        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet]
        [HttpHead] //Podržavamo i HTTP head zahtev koji nam vraća samo zaglavlja u odgovoru    
        [ProducesResponseType(StatusCodes.Status200OK)] //Eksplicitno definišemo šta sve može ova akcija da vrati
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<Knjiga>>> GetKnjiga([FromQuery] SieveModel model)
        {
            List<Knjiga> knjiga = knjigaRepository.GetKnjiga();
            if (knjiga == null || knjiga.Count == 0)
            {
                NoContent();
            }

            knjiga = sieveProcessor.Apply<Knjiga>(model, knjiga.AsQueryable()).ToList();
            return Ok(mapper.Map<List<KnjigaDto>>(knjiga));
        }

        [Authorize(Roles = "Admin,User")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{id_knjige}")]
        public ActionResult<Knjiga> GetKnjigaId(Guid id_knjige)
        {
            Knjiga knjigaModel = knjigaRepository.GetKnjigaId(id_knjige);
            if (knjigaModel == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<KnjigaDto>(knjigaModel));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<KnjigaConfirmation> CreateKnjiga([FromBody] KnjigaCreationDto knjiga)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState); // Vraćajte 400 Bad Request za validacione greške
                }


                Knjiga mappedKnjiga = mapper.Map<Knjiga>(knjiga);
                KnjigaConfirmation confirmationKnjiga = knjigaRepository.AddKnjiga(mappedKnjiga);
                knjigaRepository.SaveChanges();

                return Ok(confirmationKnjiga);
                //string location = linkGenerator.GetPathByAction("GetKnjigaId", "Knjiga", new { id_knjige = confirmationKnjiga.id_knjige });
                //return Created(location, mapper.Map<KnjigaConfirmationDto>(confirmationKnjiga));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Create Error");
            }
        }

        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{id_knjige}")]
        public IActionResult DeleteKnjiga(Guid id_knjige)
        {
            try
            {
                Knjiga knjiga = knjigaRepository.GetKnjigaId(id_knjige);
                if (knjiga == null)
                {
                    return NotFound();
                }

                knjigaRepository.RemoveKnjiga(id_knjige);
                knjigaRepository.SaveChanges();
                return StatusCode(StatusCodes.Status204NoContent, "Uspesno obrisana knjiga!");
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete Error");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<KnjigaDto> UpdateKnjiga(KnjigaUpdateDto knjiga)
        {
            try
            {
                // Map the DTO to the domain model
                Knjiga mappedKnjga = mapper.Map<Knjiga>(knjiga);

                // Call the repository method to update the backlog
                var updatedKnjiga = knjigaRepository.UpdateKnjiga(mappedKnjga);

                // Map the updated epic to DTO
                KnjigaDto updatedKnjigaDto = mapper.Map<KnjigaDto>(updatedKnjiga);

                // Return the updated resource
                return Ok(UpdateKnjiga);
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

        [Authorize(Roles = "Admin,User")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("dobavljac/{id_dobavljac}")]
        public ActionResult<KnjigaDto> GetKnjigaByDobavljacId(Guid id_dobavljac)
        {
            var Knjiga = knjigaRepository.GetKnjigaByDobavljacId(id_dobavljac);

            if (Knjiga == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<KnjigaDto>(Knjiga));
        }
    }
}

using AutoMapper;
using Knjizara.Data;
using Knjizara.Entitets;
using Knjizara.Entitets.Confirmation;
using Knjizara.Models.Dobavljac;
using Knjizara.Models.Dostava;
using Knjizara.Models.PorudzbinaKnjiga;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using Sieve.Services;

namespace Knjizara.Controllers
{
    [ApiController]
    [Route("api/knjizara/porudzbinaKnjiga")]
    [Produces("application/json", "application/xml")]
    public class PorudzbinaKnjigaController :Controller
    {
        private readonly IMapper mapper;
        private readonly IPorudzbinaKnjigaRepository porudzbinaKnjigaRepository;
        private readonly LinkGenerator linkGenerator;
        private readonly SieveProcessor sieveProcessor;


        public PorudzbinaKnjigaController(IMapper mapper, IPorudzbinaKnjigaRepository porudzbinaKnjigaRepository, LinkGenerator linkGenerator, SieveProcessor sieveProcessor)
        {
            this.mapper = mapper;
            this.porudzbinaKnjigaRepository = porudzbinaKnjigaRepository;
            this.linkGenerator = linkGenerator;
            this.sieveProcessor = sieveProcessor;
        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet]
        [HttpHead] //Podržavamo i HTTP head zahtev koji nam vraća samo zaglavlja u odgovoru    
        [ProducesResponseType(StatusCodes.Status200OK)] //Eksplicitno definišemo šta sve može ova akcija da vrati
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<PorudzbinaKnjiga>>> GetPorudzbinaKnjiga([FromQuery] SieveModel model)
        {
            List<PorudzbinaKnjiga> porudzbinaKnjiga = porudzbinaKnjigaRepository.GetPorudzbinaKnjiga();
            if (porudzbinaKnjiga == null || porudzbinaKnjiga.Count == 0)
            {
                NoContent();
            }

            porudzbinaKnjiga = sieveProcessor.Apply<PorudzbinaKnjiga>(model, porudzbinaKnjiga.AsQueryable()).ToList();
            return Ok(mapper.Map<List<PorudzbinaKnjigaDto>>(porudzbinaKnjiga));
        }

        [Authorize(Roles = "Admin,User")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{id_knjige,id_porudzbina}")]
        public ActionResult<PorudzbinaKnjiga> GetPorudzbinaKnjigaId(Guid id_knjige, Guid id_porudzbina)
        {
            PorudzbinaKnjiga porudzbinaKnjigaModel = porudzbinaKnjigaRepository.GetPorudzbinaKnjigaId(id_knjige, id_porudzbina);
            if (porudzbinaKnjigaModel == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<PorudzbinaKnjigaDto>(porudzbinaKnjigaModel));
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<PorudzbinaKnjigaConfirmation> CreatePorudzbinaKnjiga([FromBody] PorudzbinaKnjigaCreationDto porudzbinaKnjiga)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState); // Vraćajte 400 Bad Request za validacione greške
                }


                PorudzbinaKnjiga mappedPorudzbinaknjiga = mapper.Map<PorudzbinaKnjiga>(porudzbinaKnjiga);
                PorudzbinaKnjigaConfirmation confirmationPorudzbinaKnjiga = porudzbinaKnjigaRepository.AddPorudzbinaKnjiga(mappedPorudzbinaknjiga);
                porudzbinaKnjigaRepository.SaveChanges();

                return Ok(confirmationPorudzbinaKnjiga);
                //string location = linkGenerator.GetPathByAction("GetPorudzbinaKnjigaId", "PorudzbinaKnjiga", new { id_knjige = confirmationPorudzbinaKnjiga.id_knjige , id_porudzbina = confirmationPorudzbinaKnjiga.id_porudzbina });
               // return Created(location, mapper.Map<PorudzbinaKnjigaConfirmationDto>(confirmationPorudzbinaKnjiga));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Create Error");
            }
        }

        [Authorize(Roles = "Admin,User")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[HttpDelete("{id_knjige,id_porudzbina}")]
        [HttpDelete]
        public IActionResult DeletePorudzbinaKnjiga([FromQuery] Guid id_knjige, [FromQuery] Guid id_porudzbina)
        {
            try
            {
                PorudzbinaKnjiga porudzbinaKnjiga = porudzbinaKnjigaRepository.GetPorudzbinaKnjigaId(id_knjige, id_porudzbina);
                if (porudzbinaKnjiga == null)
                {
                    return NotFound();
                }

                porudzbinaKnjigaRepository.RemovePorudzbinaKnjiga(id_knjige, id_porudzbina);
                porudzbinaKnjigaRepository.SaveChanges();
                return StatusCode(StatusCodes.Status204NoContent, "Uspesno obrisana porudzbina knjiga!");
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete Error");
            }
        }

        /*[HttpPut]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<PorudzbinaKnjigaDto> UpdatePorudzbinaKnjiga(PorudzbinaKnjigaUpdateDto porudzbinaKnjiga)
        {
            try
            {
                // Map the DTO to the domain model
                PorudzbinaKnjiga mappedPorudzbinaKnjiga = mapper.Map<PorudzbinaKnjiga>(porudzbinaKnjiga);

                // Call the repository method to update the backlog
                var updatedPorudzbinaKnjiga = porudzbinaKnjigaRepository.(mappedDostava);

                // Map the updated epic to DTO
                DostavaDto updatedDostavaDto = mapper.Map<DostavaDto>(UpdateDostava);

                // Return the updated resource
                return Ok(updatedDostava);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Update Error");
            }
        }*/

        [Authorize(Roles = "Admin,User")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("porudzbina/{id_porudzbina}")]
        public ActionResult<List<PorudzbinaKnjiga>> GetPorudzbinaKnjigaByPorudzbinaId(Guid id_porudzbina)
        {
            List<PorudzbinaKnjiga> porudzbinaKnjiga = porudzbinaKnjigaRepository.GetPorudzbinaKnjigaByPorudzbinaId(id_porudzbina);

            if (porudzbinaKnjiga == null || porudzbinaKnjiga.Count == 0)
            {
                return NotFound();
            }
            // return Ok(mapper.Map<PorudzbinaDto>(Porudzbina));

            //porudzbina = sieveProcessor.Apply<Porudzbina>(id_korisnik, porudzbina.AsQueryable()).ToList();
            return Ok(mapper.Map<List<PorudzbinaKnjiga>>(porudzbinaKnjiga));
        }


        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("porudzbina/{id_porudzbina}")]
        public IActionResult DeletePorudzbineByPorudzbinaId(Guid id_porudzbina)
        {
            try
            {
                var porudzbine = porudzbinaKnjigaRepository.GetPorudzbinaKnjigaByPorudzbinaId(id_porudzbina);
                if (porudzbine == null || porudzbine.Count == 0)
                {
                    return NotFound();
                }

                porudzbinaKnjigaRepository.RemovePorudzbineByPorudzbinaId(id_porudzbina);
                porudzbinaKnjigaRepository.SaveChanges();
                return StatusCode(StatusCodes.Status204NoContent, "Uspesno obrisane porudzbine knjiga!");
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete Error");
            }
        }

    }
}

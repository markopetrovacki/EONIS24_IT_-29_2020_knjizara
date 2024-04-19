using AutoMapper;
using Knjizara.Data;
using Knjizara.Entitets;
using Knjizara.Entitets.Confirmation;
using Knjizara.Models.Dobavljac;
using Knjizara.Models.Dostava;
using Knjizara.Models.PorudzbinaKnjiga;
using Microsoft.AspNetCore.Mvc;

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


        public PorudzbinaKnjigaController(IMapper mapper, IPorudzbinaKnjigaRepository porudzbinaKnjigaRepository, LinkGenerator linkGenerator)
        {
            this.mapper = mapper;
            this.porudzbinaKnjigaRepository = porudzbinaKnjigaRepository;
            this.linkGenerator = linkGenerator;
        }

        [HttpGet]
        [HttpHead] //Podržavamo i HTTP head zahtev koji nam vraća samo zaglavlja u odgovoru    
        [ProducesResponseType(StatusCodes.Status200OK)] //Eksplicitno definišemo šta sve može ova akcija da vrati
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<PorudzbinaKnjiga>>> GetPorudzbinaKnjiga()
        {
            List<PorudzbinaKnjiga> porudzbinaKnjiga = porudzbinaKnjigaRepository.GetPorudzbinaKnjiga();
            if (porudzbinaKnjiga == null || porudzbinaKnjiga.Count == 0)
            {
                NoContent();
            }

            return Ok(mapper.Map<List<PorudzbinaKnjigaDto>>(porudzbinaKnjiga));
        }

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

                string location = linkGenerator.GetPathByAction("GetPorudzbinaKnjigaId", "PorudzbinaKnjiga", new { id_knjige = confirmationPorudzbinaKnjiga.id_knjige , id_porudzbina = confirmationPorudzbinaKnjiga.id_porudzbina });

                return Created(location, mapper.Map<PorudzbinaKnjigaConfirmationDto>(confirmationPorudzbinaKnjiga));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Create Error");
            }
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{id_knjige,id_porudzbina}")]
        public IActionResult DeletePorudzbinaKnjiga(Guid id_knjige, Guid id_porudzbina)
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

    }
}

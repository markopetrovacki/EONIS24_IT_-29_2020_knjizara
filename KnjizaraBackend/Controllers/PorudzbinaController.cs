﻿using AutoMapper;
using Knjizara.Data;
using Knjizara.Entitets;
using Knjizara.Entitets.Confirmation;
using Knjizara.Models.Dobavljac;
using Knjizara.Models.Dostava;
using Knjizara.Models.Korisnik;
using Knjizara.Models.Porudzbina;
using Microsoft.AspNetCore.Mvc;

namespace Knjizara.Controllers
{
    [ApiController]
    [Route("api/knjizara/porudzbina")]
    [Produces("application/json", "application/xml")]
    public class PorudzbinaController : Controller
    {
        private readonly IMapper mapper;
        private readonly IPorudzbinaRepository porudzbinaRepository;
        private readonly LinkGenerator linkGenerator;


        public PorudzbinaController(IMapper mapper, IPorudzbinaRepository porudzbinaRepository, LinkGenerator linkGenerator)
        {
            this.mapper = mapper;
            this.porudzbinaRepository = porudzbinaRepository;
            this.linkGenerator = linkGenerator;
        }

        [HttpGet]
        [HttpHead] //Podržavamo i HTTP head zahtev koji nam vraća samo zaglavlja u odgovoru    
        [ProducesResponseType(StatusCodes.Status200OK)] //Eksplicitno definišemo šta sve može ova akcija da vrati
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<Porudzbina>>> GetPorudzbina()
        {
            List<Porudzbina> porudzbina = porudzbinaRepository.GetPorudzbina();
            if (porudzbina == null || porudzbina.Count == 0)
            {
                NoContent();
            }
           
            return Ok(mapper.Map<List<PorudzbinaDto>>(porudzbina));
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{id_porudzbina}")]
        public ActionResult<Porudzbina> GetPorudzbinaId(Guid id_porudzbina)
        {
            Porudzbina porudzbinaModel = porudzbinaRepository.GetPorudzbinaId(id_porudzbina);
            if (porudzbinaModel == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<PorudzbinaDto>(porudzbinaModel));
        }


        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<PorudzbinaConfirmation> CreatePorudzbina([FromBody] PorudzbinaCreationDto porudzbina)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState); // Vraćajte 400 Bad Request za validacione greške
                }


                Porudzbina mappedPorudzbina = mapper.Map<Porudzbina>(porudzbina);
                PorudzbinaConfirmation confirmationPorudzbina = porudzbinaRepository.AddPorudzbina(mappedPorudzbina);
                porudzbinaRepository.SaveChanges();

                string location = linkGenerator.GetPathByAction("GetPorudzbinaId", "Porudzbina", new { id_porudzbina = confirmationPorudzbina.id_porudzbina });

                //await loggerService.Log(LogLevel.Information, "CreateProductBacklog", "Product backlog successfully created.");
                return Created(location, mapper.Map<PorudzbinaConfirmationDto>(confirmationPorudzbina));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Create Error");
            }
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{id_porudzbina}")]
        public IActionResult DeletePorudzbina(Guid id_porudzbina)
        {
            try
            {
                Porudzbina porudzbina = porudzbinaRepository.GetPorudzbinaId(id_porudzbina);
                if (porudzbina == null)
                {
                    return NotFound();
                }

                porudzbinaRepository.RemovePorudzbina(id_porudzbina);
                porudzbinaRepository.SaveChanges();
                return StatusCode(StatusCodes.Status204NoContent, "Uspesno obrisana porudzbina!");
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
        public ActionResult<PorudzbinaDto> UpdatePorudzbina(PorudzbinaUpdateDto porudzbina)
        {
            try
            {
                // Map the DTO to the domain model
                Porudzbina mappedPorudzbina = mapper.Map<Porudzbina>(porudzbina);

                // Call the repository method to update the backlog
                var updatedPorudzbina = porudzbinaRepository.UpdatePorudzbina(mappedPorudzbina);

                // Map the updated epic to DTO
                PorudzbinaDto updatedPorudzbinaDto = mapper.Map<PorudzbinaDto>(updatedPorudzbina);

                // Return the updated resource
                return Ok(UpdatePorudzbina);
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

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("dostava/{id_dostava}")]
        public ActionResult<PorudzbinaDto> GetPorudzbinaByDostavaId(Guid id_dostava)
        {
            var Porudzbina = porudzbinaRepository.GetPorudzbinaByDostavaId(id_dostava);

            if (Porudzbina == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<PorudzbinaDto>(Porudzbina));
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("korisnik/{id_korisnik}")]
        public ActionResult<PorudzbinaDto> GetPorudzbinaByKorisnikId(Guid id_korisnik)
        {
            var Porudzbina = porudzbinaRepository.GetPorudzbinaByKorisnikId(id_korisnik);

            if (Porudzbina == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<PorudzbinaDto>(Porudzbina));
        }
    }
}
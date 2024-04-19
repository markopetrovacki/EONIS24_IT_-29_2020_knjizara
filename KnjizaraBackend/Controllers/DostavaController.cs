using AutoMapper;
using Knjizara.Data;
using Knjizara.Entitets;
using Knjizara.Entitets.Confirmation;
using Knjizara.Models.Dobavljac;
using Knjizara.Models.Dostava;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Knjizara.Controllers
{
    [ApiController]
    [Route("api/knjizara/dostava")]
    [Produces("application/json", "application/xml")]
    public class DostavaController : Controller
    {
        private readonly IMapper mapper;
        private readonly IDostavaRepository dostavaRepository;
        private readonly LinkGenerator linkGenerator;


        public DostavaController(IMapper mapper, IDostavaRepository dostavaRepository, LinkGenerator linkGenerator)
        {
            this.mapper = mapper;
            this.dostavaRepository = dostavaRepository;
            this.linkGenerator = linkGenerator;
        }

        [HttpGet]
        [HttpHead] //Podržavamo i HTTP head zahtev koji nam vraća samo zaglavlja u odgovoru    
        [ProducesResponseType(StatusCodes.Status200OK)] //Eksplicitno definišemo šta sve može ova akcija da vrati
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<Dostava>>> GetDostava()
        {
            List<Dostava> dostava = dostavaRepository.GetDostava();
            if (dostava == null || dostava.Count == 0)
            {
                NoContent();
            }

            return Ok(mapper.Map<List<DostavaDto>>(dostava));
        }


        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{id_dostava}")]
        public ActionResult<Dostava> GetDostavaId(Guid id_dostava)
        {
            Dostava dostavaModel = dostavaRepository.GetDostavaId(id_dostava);
            if (dostavaModel == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<DostavaDto>(dostavaModel));
        
        }

        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<DostavaConfirmation> CreateDostava([FromBody] DostavaCreationDto dostava)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState); // Vraćajte 400 Bad Request za validacione greške
                }


                Dostava mappedDostava = mapper.Map<Dostava>(dostava);
                DostavaConfirmation confirmationDostava = dostavaRepository.AddDostava(mappedDostava);
                dostavaRepository.SaveChanges();

                string location = linkGenerator.GetPathByAction("GetDostavaId", "Dostava", new { id_dostava = confirmationDostava.id_dostava });

                //await loggerService.Log(LogLevel.Information, "CreateProductBacklog", "Product backlog successfully created.");
                return Created(location, mapper.Map<DostavaConfirmationDto>(confirmationDostava));
            }
            catch
            {
                // await loggerService.Log(LogLevel.Error, "CreateProductBacklog", "Error creating product backlog. ");
                return StatusCode(StatusCodes.Status500InternalServerError, "Create Error");
            }
        }


        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{id_dostava}")]
        public IActionResult DeleteDostava(Guid id_dostava)
        {
            try
            {
                Dostava dostava = dostavaRepository.GetDostavaId(id_dostava);
                if (dostava == null)
                {
                    return NotFound();
                }

                dostavaRepository.RemoveDostava(id_dostava);
                dostavaRepository.SaveChanges();
                return StatusCode(StatusCodes.Status204NoContent, "Uspesno obrisana dostava!");
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
        public ActionResult<DostavaDto> UpdateDostava(DostavaUpdateDto dostava)
        {
            try
            {
                // Map the DTO to the domain model
                Dostava mappedDostava = mapper.Map<Dostava>(dostava);

                // Call the repository method to update the backlog
                var updatedDostava = dostavaRepository.UpdateDostava(mappedDostava);

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
        }
    }
}

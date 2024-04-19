﻿using AutoMapper;
using Knjizara.Data;
using Knjizara.Entitets;
using Knjizara.Entitets.Confirmation;
using Knjizara.Models.Dobavljac;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Knjizara.Controllers
{
    [ApiController]
    [Route("api/knjizara/dobavljac")]
    [Produces("application/json", "application/xml")]
    public class DobavljacController : Controller
    {
        private readonly IMapper mapper;
        private readonly IDobavljacRepository dobavljacRepository;
        private readonly LinkGenerator linkGenerator;

        public DobavljacController(IMapper mapper, IDobavljacRepository dobavljacRepository, LinkGenerator linkGenerator)
        {
            this.mapper = mapper;
            this.dobavljacRepository = dobavljacRepository;
            this.linkGenerator = linkGenerator;
        }
        

        [HttpGet]
        [HttpHead] //Podržavamo i HTTP head zahtev koji nam vraća samo zaglavlja u odgovoru    
        [ProducesResponseType(StatusCodes.Status200OK)] //Eksplicitno definišemo šta sve može ova akcija da vrati
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<List<Dobavljac>> GetDobavljac()
        {
            List<Dobavljac> dobavljac = dobavljacRepository.GetDobavljac();
            if (dobavljac == null || dobavljac.Count == 0)
            {
               // await loggerService.Log(LogLevel.Warning, "GetProductBacklogs", "Product backlog not found.");
                NoContent();
            }
          //  Console.WriteLine(await loggerService.Log(LogLevel.Information, "GetProductBacklogs", "Product backlog successfully returned."));
            return Ok(mapper.Map<List<DobavljacDto>>(dobavljac));
        }


        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{id_dobavljaca}")]
        public ActionResult<Dobavljac> GetDobavljacId(Guid id_dobavljaca)
        {
            Dobavljac dobavljacModel = dobavljacRepository.GetDobavljacId(id_dobavljaca);
            if (dobavljacModel == null)
            {
               // await loggerService.Log(LogLevel.Warning, "GetProductBacklog", "Product backlog with id: " + productBacklogId + " not found.");
                return NotFound();
            }

           // await loggerService.Log(LogLevel.Information, "GetProductBacklog", "Product backlog with id: " + productBacklogId + " successfully returned.");
            return Ok(mapper.Map<DobavljacDto>(dobavljacModel));
        }


        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public  ActionResult<DobavljacConfirmation> CreateDobavljac([FromBody] DobavljacCreationDto dobavljac)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState); // Vraćajte 400 Bad Request za validacione greške
                }


                Dobavljac mappedDobavljac = mapper.Map<Dobavljac>(dobavljac);
                DobavljacConfirmation confirmationDobavljac = dobavljacRepository.AddDobavljac(mappedDobavljac);
                dobavljacRepository.SaveChanges();

                string location = linkGenerator.GetPathByAction("GetDobavljacId", "Dobavljac", new { id_dobavljac = confirmationDobavljac.id_dobavljaca });

                //await loggerService.Log(LogLevel.Information, "CreateProductBacklog", "Product backlog successfully created.");
                return Created(location, mapper.Map<DobavljacConfirmationDto>(confirmationDobavljac));
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
        [HttpDelete("{id_dobavljaca}")]
        public IActionResult DeleteDobavljac(Guid id_dobavljaca)
        {
            try
            {
                Dobavljac dobavljac = dobavljacRepository.GetDobavljacId(id_dobavljaca);
                if (dobavljac == null)
                {
                    //await loggerService.Log(LogLevel.Warning, "DeleteProductBacklog", "Product backlog with id " + productBacklogId + " not found.");
                    return NotFound();
                }

                dobavljacRepository.RemoveDobavljac(id_dobavljaca);
                dobavljacRepository.SaveChanges();
                return StatusCode(StatusCodes.Status204NoContent, "Uspesno obrisan dobavljac!");
            }
            catch
            {
                //await loggerService.Log(LogLevel.Error, "DeleteProductBacklog", "Error deleting product backlog with id " + productBacklogId + ".");
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete Error");
            }
        }

        [HttpPut]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<DobavljacDto> UpdateDobavljac(DobavljacUpdateDto dobavljac)
        {
            try
            {
                // Map the DTO to the domain model
                Dobavljac mappedDobavljac = mapper.Map<Dobavljac>(dobavljac);

                // Call the repository method to update the backlog
                var updatedDobavljac = dobavljacRepository.UpdateDobavljac(mappedDobavljac);

                // Map the updated epic to DTO
                DobavljacDto updatedDobaljvacDto = mapper.Map<DobavljacDto>(updatedDobavljac);

                //await loggerService.Log(LogLevel.Information, "UpdateProductBacklog", "Product backlog with id " + productBacklog + " successfully updated.");
                // Return the updated resource
                return Ok(updatedDobavljac);
            }
            catch (KeyNotFoundException)
            {
               // await loggerService.Log(LogLevel.Warning, "UpdateProductBacklog", "Product backlog with id " + productBacklog + " not found.");
                return NotFound();
            }
            catch (Exception)
            {
               // await loggerService.Log(LogLevel.Error, "UpdateProductBacklog", "Error updating product backlog with id " + productBacklog + ".");
                return StatusCode(StatusCodes.Status500InternalServerError, "Update Error");
            }
        }

    }
}
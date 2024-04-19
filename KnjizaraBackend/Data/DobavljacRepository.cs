using AutoMapper;
using Knjizara.Entitets;
using Knjizara.Entitets.Confirmation;
using Knjizara.Models.Dobavljac;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Knjizara.Data
{
    public class DobavljacRepository : IDobavljacRepository
    {
        private readonly KnjizaraDBContext context;
        private readonly IMapper mapper;

        public DobavljacRepository (KnjizaraDBContext context, IMapper mapper)
        {
            this.context = context; 
            this.mapper = mapper;
        }

        public DobavljacConfirmation AddDobavljac(Dobavljac dobavljac)
        {
            var createDobavljac = context.Add(dobavljac);
            return mapper.Map<DobavljacConfirmation>(createDobavljac.Entity);
        }

        public List<Dobavljac> GetDobavljac()
        {
           List<Dobavljac> dobavljac = context.dobavljac.ToList();
            return dobavljac;
        }

        public Dobavljac GetDobavljacId(Guid Id)
        {
            return context.dobavljac.FirstOrDefault(e => e.id_dobavljaca == Id);
        }

        public void RemoveDobavljac(Guid Id)
        {
           var dobavljac = GetDobavljacId(Id);
            context.Remove(dobavljac);
        }

        public Dobavljac UpdateDobavljac(Dobavljac dobavljac)
        {
            try
            {
                var existingDobavljac = context.dobavljac.FirstOrDefault(e => e.id_dobavljaca == dobavljac.id_dobavljaca);
                if (existingDobavljac != null)
                {
                    existingDobavljac.naziv_dobavljaca = dobavljac.naziv_dobavljaca;
                    existingDobavljac.pib = dobavljac.pib;
                    existingDobavljac.telefon_dobavljaca = dobavljac.telefon_dobavljaca;
                    existingDobavljac.adresa_dobavljaca = dobavljac.adresa_dobavljaca;
                    
                    context.SaveChanges();  

                    return existingDobavljac;
                }
                else
                {
                    throw new KeyNotFoundException($"Dobaljac with ID {dobavljac.id_dobavljaca} not found");
                }
            }
            catch (Exception ex) 
            {
                // Log the exception or handle it appropriately
                throw new Exception("Error updating dobavljac", ex);
            }
        }

        public bool SaveChanges()
        {
            return context.SaveChanges() > 0;
        }
    }
}

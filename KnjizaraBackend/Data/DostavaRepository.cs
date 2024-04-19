using AutoMapper;
using Knjizara.Entitets;
using Knjizara.Entitets.Confirmation;
using Knjizara.Models.Dostava;

namespace Knjizara.Data
{
    public class DostavaRepository : IDostavaRepository
    {
        private readonly KnjizaraDBContext context;
        private readonly IMapper mapper;

        public DostavaRepository (KnjizaraDBContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public DostavaConfirmation AddDostava(Dostava dostava)
        {
            var createdDostava = context.Add(dostava);
            return mapper.Map<DostavaConfirmation>(createdDostava.Entity);
        }

        public List<Dostava> GetDostava()
        {
            List<Dostava> dostava = context.dostava.ToList();
            return dostava;
        }

        public Dostava GetDostavaId(Guid Id)
        {
            return context.dostava.FirstOrDefault(e => e.id_dostava == Id);
        }

        public void RemoveDostava(Guid Id)
        {
            var dostava = GetDostavaId(Id);
            context.Remove(dostava);
        }

        public bool SaveChanges()
        {
            return context.SaveChanges() > 0;
        }

        public Dostava UpdateDostava(Dostava dostava)
        {
            try
            {
                var existingDostava = context.dostava.FirstOrDefault(e => e.id_dostava == dostava.id_dostava);

                if (existingDostava != null)
                {
                    // Update the existing backlog with the new values
                    existingDostava.adresa_dostave = dostava.adresa_dostave;
                    existingDostava.datum_dostave = dostava.datum_dostave;
                    existingDostava.grad = dostava.grad;
                    existingDostava.drzava = dostava.drzava;
                    existingDostava.postanski_broj = dostava.postanski_broj;
                    existingDostava.broj_telefona = dostava.broj_telefona;
                    existingDostava.ime = dostava.ime;

                    context.SaveChanges(); // Save changes to the database

                    return existingDostava;
                }
                else
                {
                    throw new KeyNotFoundException($"Dostava with ID {dostava.id_dostava} not found");
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                throw new Exception("Error updating dostava", ex);
            };
        }
    }
}

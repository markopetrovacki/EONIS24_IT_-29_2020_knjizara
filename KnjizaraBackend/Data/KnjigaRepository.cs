using AutoMapper;
using AutoMapper.Configuration.Annotations;
using Knjizara.Entitets;
using Knjizara.Entitets.Confirmation;
using Knjizara.Models.Knjiga;

namespace Knjizara.Data
{
    public class KnjigaRepository : IKnjigaRepository
    {
        private readonly KnjizaraDBContext context;
        private readonly IMapper mapper;

        public KnjigaRepository(KnjizaraDBContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public KnjigaConfirmation AddKnjiga(Knjiga knjiga)
        {
            var createdKnjiga = context.Add(knjiga);
            return mapper.Map<KnjigaConfirmation>(createdKnjiga.Entity);
        }

        public List<Knjiga> GetKnjiga()
        {
            List<Knjiga> knjiga = context.knjiga.ToList();
            return knjiga;
        }

        public Knjiga GetKnjigaId(Guid Id)
        {
            return context.knjiga.FirstOrDefault(e => e.id_knjige == Id);
        }

        public void RemoveKnjiga(Guid Id)
        {
            var knjiga = GetKnjigaId(Id);
            context.Remove(knjiga);
        }

        public bool SaveChanges()
        {
            return context.SaveChanges() > 0;
        }

        public Knjiga UpdateKnjiga(Knjiga knjiga)
        {
            try
            {
                var existingKnjiga = context.knjiga.FirstOrDefault(e => e.id_knjige == knjiga.id_knjige);

                if (existingKnjiga != null)
                {
                    // Update the existing backlog with the new values
                    existingKnjiga.naziv_knjige = knjiga.naziv_knjige;
                    existingKnjiga.opis = knjiga.opis;
                    existingKnjiga.stanje_na_lageru = knjiga.stanje_na_lageru;
                    existingKnjiga.cena = knjiga.cena;
                    existingKnjiga.zanr = knjiga.zanr;
                    existingKnjiga.ime_autora = knjiga.ime_autora;
                    existingKnjiga.prezime_autora = knjiga.prezime_autora;
                    existingKnjiga.slika = knjiga.slika;
                    existingKnjiga.id_dobavljac = knjiga.id_dobavljac;

                    context.SaveChanges(); // Save changes to the database

                    return existingKnjiga;
                }
                else
                {
                    throw new KeyNotFoundException($"Knjiga with ID {knjiga.id_knjige} not found");
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                throw new Exception("Error updating knjiga", ex);
            };

        }

        public Knjiga GetKnjigaByDobavljacId(Guid Id)
        {
            return context.knjiga.FirstOrDefault(e => e.id_dobavljac == Id);
        }
    }
}

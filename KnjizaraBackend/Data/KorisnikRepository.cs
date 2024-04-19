using AutoMapper;
using Knjizara.Entitets;
using Knjizara.Entitets.Confirmation;
using Knjizara.Models.Korisnik;

namespace Knjizara.Data
{
    public class KorisnikRepository : IKorisnikRepository
    {
        private readonly KnjizaraDBContext context;
        private readonly IMapper mapper;

        public KorisnikRepository(KnjizaraDBContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public KorisnikConfirmation AddKorisnik(Korisnik korisnik)
        {
            var createdKorisnik = context.Add(korisnik);
            return mapper.Map<KorisnikConfirmation>(createdKorisnik.Entity);
        }

        public List<Korisnik> GetKorisnik()
        {
            List<Korisnik> korisnik = context.korisnik.ToList();
            return korisnik;
        }

        public Korisnik GetKorisnikId(Guid Id)
        {
            return context.korisnik.FirstOrDefault(e => e.id_korisnik == Id);
        }

        public void RemoveKorisnik(Guid Id)
        {
            var korisnik = GetKorisnikId(Id);
            context.Remove(korisnik);
        }

        public bool SaveChanges()
        {
            return context.SaveChanges() > 0;
        }

        public Korisnik UpdateKorisnik(Korisnik korisnik)
        {
            try
            {
                var existingKorisnik = context.korisnik.FirstOrDefault(e => e.id_korisnik == korisnik.id_korisnik);

                if (existingKorisnik != null)
                {
                    // Update the existing backlog with the new values
                    existingKorisnik.ime_korisnika = korisnik.ime_korisnika;
                    existingKorisnik.prezime_korisnika = korisnik.prezime_korisnika;
                    existingKorisnik.adresa_korisnika = korisnik.adresa_korisnika;
                    existingKorisnik.grad_korisnika = korisnik.grad_korisnika;
                    existingKorisnik.kontakt_telefon = korisnik.kontakt_telefon;
                    existingKorisnik.status_korisnika = korisnik.status_korisnika;

                    context.SaveChanges(); // Save changes to the database

                    return existingKorisnik;
                }
                else
                {
                    throw new KeyNotFoundException($"Korisnik with ID {korisnik.id_korisnik} not found");
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                throw new Exception("Error updating korisnik", ex);
            };
        }
    }
}

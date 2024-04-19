using AutoMapper;
using Knjizara.Entitets;
using Knjizara.Entitets.Confirmation;
using Knjizara.Models.Porudzbina;

namespace Knjizara.Data
{
    public class PorudzbinaRepository : IPorudzbinaRepository
    {
        private readonly KnjizaraDBContext context;
        private readonly IMapper mapper;

        public PorudzbinaRepository(KnjizaraDBContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public PorudzbinaConfirmation   AddPorudzbina(Porudzbina porudzbina)
        {
            var createdPorudzbina= context.Add(porudzbina);
            return mapper.Map<PorudzbinaConfirmation>(createdPorudzbina.Entity);
        }

        public List<Porudzbina> GetPorudzbina()
        {
            List<Porudzbina> porudzbina = context.porudzbina.ToList();
            return porudzbina;
        }

        public Porudzbina GetPorudzbinaId(Guid Id)
        {
            return context.porudzbina.FirstOrDefault(e => e.id_porudzbina == Id);
        }

        public void RemovePorudzbina(Guid Id)
        {
            var porudzbina = GetPorudzbinaId(Id);
            context.Remove(porudzbina);
        }

        public bool SaveChanges()
        {
            return context.SaveChanges() > 0;
        }

        public Porudzbina UpdatePorudzbina(Porudzbina porudzbina)
        {
            try
            {
                var existingPorudzbina = context.porudzbina.FirstOrDefault(e => e.id_porudzbina == porudzbina.id_porudzbina);

                if (existingPorudzbina != null)
                {
                    // Update the existing backlog with the new values
                    existingPorudzbina.datum_kreiranja = porudzbina.datum_kreiranja;
                    existingPorudzbina.ukupna_cena = porudzbina.ukupna_cena;
                    existingPorudzbina.status_porudzbine = porudzbina.status_porudzbine;
                    existingPorudzbina.status_placanja = porudzbina.status_placanja;
                    existingPorudzbina.dobavljac = porudzbina.dobavljac;
                    existingPorudzbina.broj_posiljke = porudzbina.broj_posiljke;
                    existingPorudzbina.rok_isporuke = porudzbina.rok_isporuke;
                    existingPorudzbina.id_korisnik = porudzbina.id_korisnik;
                    existingPorudzbina.id_dostava = porudzbina.id_dostava;


                    context.SaveChanges(); // Save changes to the database

                    return existingPorudzbina;
                }
                else
                {
                    throw new KeyNotFoundException($"Porudzbina with ID {porudzbina.id_porudzbina} not found");
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                throw new Exception("Error updating porudzbina", ex);
            };
        }

        public Porudzbina GetPorudzbinaByDostavaId(Guid Id)
        {
            return context.porudzbina.FirstOrDefault(e => e.id_dostava == Id);
        }

       /* public Porudzbina GetPorudzbinaByKnjigaId(Guid Id)
        {
            return context.porudzbina.FirstOrDefault(e => e.id == Id);
        }*/

        public Porudzbina GetPorudzbinaByKorisnikId(Guid Id)
        {
            return context.porudzbina.FirstOrDefault(e => e.id_korisnik == Id);
        }


    }
}

using AutoMapper;
using Knjizara.Entitets;
using Knjizara.Entitets.Confirmation;
using Knjizara.Models.PorudzbinaKnjiga;

namespace Knjizara.Data
{
    public class PorudzbinaKnjigaRepository : IPorudzbinaKnjigaRepository
    {
        private readonly KnjizaraDBContext context;
        private readonly IMapper mapper;

        public PorudzbinaKnjigaRepository(KnjizaraDBContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public PorudzbinaKnjigaConfirmation AddPorudzbinaKnjiga(PorudzbinaKnjiga porudzbinaKnjiga)
        {
            var createdPorudzbinaKnjiga = context.Add(porudzbinaKnjiga);
            return mapper.Map<PorudzbinaKnjigaConfirmation>(createdPorudzbinaKnjiga.Entity);
        }

        public List<PorudzbinaKnjiga> GetPorudzbinaKnjiga()
        {
            List<PorudzbinaKnjiga> porudzbinaKnjiga = context.porudzbinaKnjiga.ToList();
            return porudzbinaKnjiga;
        }

        public PorudzbinaKnjiga GetPorudzbinaKnjigaId(Guid id_knjige, Guid id_porudzbina)
        {
            return context.porudzbinaKnjiga.FirstOrDefault(e => e.id_knjige == id_knjige && e.id_porudzbina == id_porudzbina);
            /* var porudzbinaKnjiga = await context.porudzbinaKnjiga.FindAsync(id_knjige, id_porudzbina);
            return mapper.Map<PorudzbinaKnjigaDto>(porudzbinaKnjiga);*/
        }

        public void RemovePorudzbinaKnjiga(Guid id_knjige, Guid id_porudzbina)
        {
            var porudzbinaKnjiga = GetPorudzbinaKnjigaId(id_knjige, id_porudzbina);
            context.Remove(porudzbinaKnjiga);
        }

        public bool SaveChanges()
        {
            return context.SaveChanges() > 0;
        }

        public List<PorudzbinaKnjiga> GetPorudzbinaKnjigaByPorudzbinaId(Guid id_porudzbina)
        {
            return context.porudzbinaKnjiga.Where(e => e.id_porudzbina == id_porudzbina).ToList();
        }

       
        public void RemovePorudzbineByPorudzbinaId(Guid id_porudzbina)
        {
            var porudzbineZaBrisanje = GetPorudzbinaKnjigaByPorudzbinaId(id_porudzbina);
            context.RemoveRange(porudzbineZaBrisanje);
        }
    }
}

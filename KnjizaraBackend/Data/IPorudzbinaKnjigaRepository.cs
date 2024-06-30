using Knjizara.Entitets;
using Knjizara.Entitets.Confirmation;
using Knjizara.Models.PorudzbinaKnjiga;

namespace Knjizara.Data
{
    public interface IPorudzbinaKnjigaRepository
    {
        List<PorudzbinaKnjiga> GetPorudzbinaKnjiga();
        public PorudzbinaKnjiga GetPorudzbinaKnjigaId(Guid id_knjige, Guid id_porudzbina);
        public void RemovePorudzbinaKnjiga(Guid id_knjige, Guid id_porudzbina);
        public PorudzbinaKnjigaConfirmation AddPorudzbinaKnjiga(PorudzbinaKnjiga porudzbinaKnjiga);
       // public PorudzbinaKnjiga UpdatePorudzbinaKnjiga(PorudzbinaKnjiga porudzbinaKnjiga);
        public bool SaveChanges();

        List<PorudzbinaKnjiga> GetPorudzbinaKnjigaByPorudzbinaId(Guid id_porudzbina);
        public void RemovePorudzbineByPorudzbinaId(Guid id_porudzbina);
    }
}

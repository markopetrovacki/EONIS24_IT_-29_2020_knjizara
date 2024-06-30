using Knjizara.Entitets;
using Knjizara.Entitets.Confirmation;
using Knjizara.Models.Porudzbina;

namespace Knjizara.Data
{
    public interface IPorudzbinaRepository
    {
        List<Porudzbina> GetPorudzbina();
        public Porudzbina GetPorudzbinaId(Guid Id);
        public void RemovePorudzbina(Guid Id);
        public PorudzbinaConfirmation AddPorudzbina(Porudzbina porudzbina);
        public Porudzbina UpdatePorudzbina(Porudzbina porudzbina);
        public bool SaveChanges();

        public List<Porudzbina> GetPorudzbinaByKorisnikId(Guid Id);
        public Porudzbina GetPorudzbinaByDostavaId(Guid Id);
       // public Porudzbina GetPorudzbinaByKnjigaId(Guid Id);
    }
}

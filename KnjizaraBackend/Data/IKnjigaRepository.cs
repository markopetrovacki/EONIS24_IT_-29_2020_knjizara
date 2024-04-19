using Knjizara.Entitets;
using Knjizara.Entitets.Confirmation;
using Knjizara.Models.Knjiga;

namespace Knjizara.Data
{
    public interface IKnjigaRepository
    {
        List<Knjiga> GetKnjiga();
        public Knjiga GetKnjigaId(Guid Id);
        public void RemoveKnjiga(Guid Id);
        public KnjigaConfirmation AddKnjiga(Knjiga knjiga);
        public Knjiga UpdateKnjiga(Knjiga knjiga);
        public bool SaveChanges();

        public Knjiga GetKnjigaByDobavljacId(Guid Id);

    }
}

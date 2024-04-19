using Knjizara.Entitets;
using Knjizara.Entitets.Confirmation;
using Knjizara.Models.Dobavljac;

namespace Knjizara.Data
{
    public interface IDobavljacRepository
    {
        List<Dobavljac> GetDobavljac();
        public Dobavljac GetDobavljacId(Guid Id);
        public void RemoveDobavljac(Guid Id);
        public DobavljacConfirmation AddDobavljac(Dobavljac dobavljac);
        public Dobavljac UpdateDobavljac(Dobavljac dobavljac);
        public bool SaveChanges();
    }
}

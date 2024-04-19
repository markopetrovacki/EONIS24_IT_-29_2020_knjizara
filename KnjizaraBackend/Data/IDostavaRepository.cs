using Knjizara.Entitets;
using Knjizara.Entitets.Confirmation;
using Knjizara.Models.Dostava;

namespace Knjizara.Data
{
    public interface IDostavaRepository
    {
        List<Dostava> GetDostava();
        public Dostava GetDostavaId(Guid Id);
        public void RemoveDostava(Guid Id);
        public DostavaConfirmation AddDostava(Dostava dostava);
        public Dostava UpdateDostava(Dostava dostava);
        public bool SaveChanges();
    }
}

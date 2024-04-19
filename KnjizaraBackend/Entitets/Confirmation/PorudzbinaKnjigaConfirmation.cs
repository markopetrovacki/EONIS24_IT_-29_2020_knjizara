namespace Knjizara.Entitets.Confirmation
{
    public class PorudzbinaKnjigaConfirmation
    {

        public Guid id_knjige { get; set; }

        public Guid id_porudzbina { get; set; }
        public int kolicina { get; set; }
    }
}

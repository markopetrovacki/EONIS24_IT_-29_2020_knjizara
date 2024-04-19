namespace Knjizara.Entitets.Confirmation
{
    public class PorudzbinaConfirmation
    {
        public Guid id_porudzbina { get; set; }
        public DateTime datum_kreiranja { get; set; }

        public int ukupna_cena { get; set; }
    }
}

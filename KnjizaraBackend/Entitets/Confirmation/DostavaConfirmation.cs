namespace Knjizara.Entitets.Confirmation
{
    public class DostavaConfirmation
    {
        public Guid id_dostava { get; set; }
        public string adresa_dostave { get; set; }

        public DateTime datum_dostave { get; set; }
    }
}

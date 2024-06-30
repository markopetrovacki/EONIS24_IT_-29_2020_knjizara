namespace Knjizara.Models.Dostava
{
    public class DostavaUpdateDto
    {
        public Guid id_dostava { get; set; }

        public string adresa_dostave { get; set; }

        public DateTime datum_dostave { get; set; }

        public string grad { get; set; }

        public string drzava { get; set; }

        public string postanski_broj { get; set; }

        public string broj_telefona { get; set; }

        public string ime { get; set; }
    }
}

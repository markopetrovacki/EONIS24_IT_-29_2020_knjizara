namespace Knjizara.Models.Dostava
{
    public class DostavaCreationDto
    {
        public string adresa_dostave { get; set; }

        public DateTime datum_dostave { get; set; }
        
        public string grad { get; set; }

        public string drzava { get; set; }

        public int postanski_broj { get; set; }

        public int broj_telefona { get; set; }

        public string ime { get; set; }
    }
}

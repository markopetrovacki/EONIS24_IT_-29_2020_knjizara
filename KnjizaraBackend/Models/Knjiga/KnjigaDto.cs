namespace Knjizara.Models.Knjiga
{
    public class KnjigaDto
    {
        public Guid id_knjige { get; set; }

        public string naziv_knjige { get; set; }

        public string opis { get; set; }

        public int stanje_na_lageru { get; set; }

        public int cena { get; set; }

        public string zanr { get; set; }

        public string ime_autora { get; set; }

        public string prezime_autora { get; set; }

        public string slika { get; set; }
        public Guid id_dobavljac { get; set; }
    }
}

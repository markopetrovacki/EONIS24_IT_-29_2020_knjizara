namespace Knjizara.Models.Korisnik
{
    public class KorisnikDto
    {
        public Guid id_korisnik { get; set; }
        public string username { get; set; }
        public string pasword { get; set; }

        public string ime_korisnika { get; set; }

        public string prezime_korisnika { get; set; }

        public string adresa_korisnika { get; set; }

        public string grad_korisnika { get; set; }

        public int kontakt_telefon { get; set; }

        public string status_korisnika { get; set; }

    }
}

namespace Knjizara.Entitets.Confirmation
{
    public class KorisnikConfirmation
    {
        public Guid id_korisnik { get; set; }
        public string ime_korisnika { get; set; }

        public string prezime_korisnika { get; set; }
        public string username { get; set; } = string.Empty;
        public string pasword { get; set; } = string.Empty;
    }
}

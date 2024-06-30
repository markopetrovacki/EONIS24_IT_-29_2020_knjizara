using Sieve.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Security.Permissions;

namespace Knjizara.Entitets
{
    public class Korisnik
    {

        [Key]
        [Required]
        public Guid id_korisnik {  get; set; }
        public string username { get; set; } //= string.Empty;  DA BUDE UNIK DA NE MOGU DVA KORISNIKA SA ISTIM IMENOM
        public string password { get; set; } //= string.Empty;

        [Sieve(CanFilter = true, CanSort = true)] public string ime_korisnika {  get; set; }

        [Sieve(CanFilter = true, CanSort = true)] public string prezime_korisnika { get; set; }

        [Sieve(CanFilter = true, CanSort = true)] public string adresa_korisnika { get; set; }

        [Sieve(CanFilter = true, CanSort = true)] public string grad_korisnika { get; set; }

        [Sieve(CanFilter = true, CanSort = true)] public string kontakt_telefon {  get; set; }

        [Sieve(CanFilter = true, CanSort = true)] public string status_korisnika { get; set; } 
    }
}

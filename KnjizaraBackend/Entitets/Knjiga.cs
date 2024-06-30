using Sieve.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Knjizara.Entitets
{
    public class Knjiga
    {
   
        [Key]
        [Required]
        public Guid id_knjige {  get; set; }

        [Sieve(CanFilter = true, CanSort = true)] public string naziv_knjige { get; set; }

        [Sieve(CanFilter = true, CanSort = true)] public string opis { get; set; }

        [Sieve(CanFilter = true, CanSort = true)] public int stanje_na_lageru {  get; set; }

        [Sieve(CanFilter = true, CanSort = true)] public int cena {  get; set; }

        [Sieve(CanFilter = true, CanSort = true)] public string zanr {  get; set; }

        [Sieve(CanFilter = true, CanSort = true)] public string ime_autora {  get; set; }

        [Sieve(CanFilter = true, CanSort = true)] public string prezime_autora { get; set; }

        public string slika { get; set; }
        public Guid id_dobavljac {  get; set; }
    }
}

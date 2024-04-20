using Sieve.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Knjizara.Entitets
{
    public class Dostava
    {

        [Key]
        [Required]
        public Guid id_dostava {  get; set; }

        [Sieve(CanFilter = true, CanSort = true)] public string adresa_dostave {  get; set; }

        [Sieve(CanFilter = true, CanSort = true)] public DateTime datum_dostave { get; set; }

        [Sieve(CanFilter = true, CanSort = true)] public string grad {  get; set; }

        [Sieve(CanFilter = true, CanSort = true)] public string drzava { get; set; }

        [Sieve(CanFilter = true, CanSort = true)] public int postanski_broj {  get; set; }

        [Sieve(CanFilter = true, CanSort = true)] public int broj_telefona {  get; set; }

        [Sieve(CanFilter = true, CanSort = true)] public string ime {  get; set; }
    }
}

using Sieve.Attributes;
using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Knjizara.Entitets
{
    public class Porudzbina
    {

        [Key]
        [Required]
        public Guid id_porudzbina {  get; set; }
        [Sieve(CanFilter = true, CanSort = true)] public DateTime datum_kreiranja {  get; set; }
        [Sieve(CanFilter = true, CanSort = true)] public int ukupna_cena {  get; set; }
        [Sieve(CanFilter = true, CanSort = true)] public string status_porudzbine {  get; set; }
        [Sieve(CanFilter = true, CanSort = true)] public string status_placanja { get; set; }
        [Sieve(CanFilter = true, CanSort = true)] public string dobavljac {  get; set; }
        [Sieve(CanFilter = true, CanSort = true)] public string broj_posiljke {  get; set; }
        [Sieve(CanFilter = true, CanSort = true)] public DateTime rok_isporuke { get; set; }
        public Guid id_korisnik { get; set; }
        public Guid id_dostava { get; set; }

       // public Dostava dostava { get; set; }
        //public Korisnik korisnik { get; set;}

    }
}

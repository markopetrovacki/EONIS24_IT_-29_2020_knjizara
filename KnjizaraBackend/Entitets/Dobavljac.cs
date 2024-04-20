using Sieve.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Knjizara.Entitets
{
    public class Dobavljac
    {

    [Key]
    [Required]
    public Guid id_dobavljaca {  get; set; }

    [Sieve(CanFilter =true, CanSort = true)] public string naziv_dobavljaca { get; set; }

    [Sieve(CanFilter = true, CanSort = true)] public int pib {  get; set; }

    [Sieve(CanFilter = true, CanSort = true)] public int telefon_dobavljaca { get; set; }

    [Sieve(CanFilter = true, CanSort = true)] public string adresa_dobavljaca { get; set; }


    }
}

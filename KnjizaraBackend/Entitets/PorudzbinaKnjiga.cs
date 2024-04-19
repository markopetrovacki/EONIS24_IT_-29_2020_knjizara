namespace Knjizara.Entitets
{
    public class PorudzbinaKnjiga
    {
        public Guid id_knjige {  get; set; }
	    public Guid id_porudzbina {  get; set; }
	    public int kolicina {  get; set; } 

       // public Knjiga knjiga { get; set; }
       // public Porudzbina porudzbina { get; set; }
    }
}

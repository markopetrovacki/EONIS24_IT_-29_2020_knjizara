namespace Knjizara.Models.PorudzbinaKnjiga
{
    public class PorudzbinaKnjigaDto
    {
        public Guid id_knjige { get; set; }

        public Guid id_porudzbina { get; set; }
        public int kolicina { get; set; }
    }
}

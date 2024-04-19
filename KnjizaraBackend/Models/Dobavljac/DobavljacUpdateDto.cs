namespace Knjizara.Models.Dobavljac
{
    public class DobavljacUpdateDto
    {
        public Guid id_dobavljaca { get; set; }

        public string naziv_dobavljaca { get; set; }

        public int pib { get; set; }

        public int telefon_dobavljaca { get; set; }

        public string adresa_dobavljaca { get; set; }

    }
}

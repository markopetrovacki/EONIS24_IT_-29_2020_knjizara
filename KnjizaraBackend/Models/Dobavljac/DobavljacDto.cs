namespace Knjizara.Models.Dobavljac
{
    public class DobavljacDto
    {
        public Guid id_dobavljaca { get; set; }
        public string naziv_dobavljaca { get; set; }

        public string pib { get; set; }

        public string telefon_dobavljaca { get; set; }

        public string adresa_dobavljaca { get; set; }


    }
}

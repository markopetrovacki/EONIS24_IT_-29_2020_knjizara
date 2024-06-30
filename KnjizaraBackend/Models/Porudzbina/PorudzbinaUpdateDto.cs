namespace Knjizara.Models.Porudzbina
{
    public class PorudzbinaUpdateDto
    {
        public Guid id_porudzbina { get; set; }

        public DateTime datum_kreiranja { get; set; }

        public int ukupna_cena { get; set; }

        public string status_porudzbine { get; set; }

        public string status_placanja { get; set; }

        public string dobavljac { get; set; }

        public string broj_posiljke { get; set; }

        public DateTime rok_isporuke { get; set; }

        public Guid id_korisnik { get; set; }

        public Guid id_dostava { get; set; }


    }
}

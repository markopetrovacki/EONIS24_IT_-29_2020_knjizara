using Microsoft.EntityFrameworkCore;

namespace Knjizara.Entitets
{
    public class KnjizaraDBContext : DbContext
    {
        private readonly IConfiguration configuration;

        public KnjizaraDBContext(DbContextOptions<KnjizaraDBContext> options, IConfiguration configuration) : base(options)
        {
            this.configuration = configuration;
        }

        public DbSet<Dobavljac> dobavljac { get; set; }
        public DbSet<Dostava> dostava { get; set; }
        public DbSet<Knjiga> knjiga { get; set; }
        public DbSet<Korisnik> korisnik {  get; set; }
        public DbSet<Porudzbina> porudzbina { get; set; }
        public DbSet<PorudzbinaKnjiga> porudzbinaKnjiga {  get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("KnjizaraDBConnection"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dobavljac>().ToTable("dobavljac", "knjizara");
            modelBuilder.Entity<Dostava>().ToTable("dostava", "knjizara");
            modelBuilder.Entity<Knjiga>().ToTable("knjiga", "knjizara");
            modelBuilder.Entity<Korisnik>().ToTable("korisnik", "knjizara");
            modelBuilder.Entity<Porudzbina>().ToTable("porudzbina", "knjizara");
            modelBuilder.Entity<PorudzbinaKnjiga>().ToTable("porudzbinaKnjiga", "knjizara");

            modelBuilder.Entity<PorudzbinaKnjiga>()
                .HasKey(pk => new {pk.id_knjige, pk.id_porudzbina});
           
            modelBuilder.Entity<PorudzbinaKnjiga>()
                .ToTable(tb => tb.HasTrigger("stanje_na_lageru"));
        }
    }
}

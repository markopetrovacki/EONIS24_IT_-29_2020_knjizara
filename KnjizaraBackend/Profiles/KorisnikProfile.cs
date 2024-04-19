using AutoMapper;
using Knjizara.Entitets;
using Knjizara.Entitets.Confirmation;
using Knjizara.Models.Korisnik;

namespace Knjizara.Profiles
{
    public class KorisnikProfile : Profile
    {
        public KorisnikProfile() 
        {
            CreateMap<Korisnik, KorisnikDto>()
               .ReverseMap();
            CreateMap<Korisnik, KorisnikConfirmation>()
               .ReverseMap();
            CreateMap<KorisnikConfirmation, KorisnikConfirmationDto>()
                .ReverseMap();
            CreateMap<Korisnik, KorisnikCreationDto>()
                .ReverseMap();
            CreateMap<Korisnik, KorisnikUpdateDto>()
                .ReverseMap();
        }
    }
}

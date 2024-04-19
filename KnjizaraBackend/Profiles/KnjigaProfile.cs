using AutoMapper;
using Knjizara.Entitets;
using Knjizara.Entitets.Confirmation;
using Knjizara.Models.Knjiga;

namespace Knjizara.Profiles
{
    public class KnjigaProfile :Profile
    {
        public KnjigaProfile() 
        {
            CreateMap<Knjiga, KnjigaDto>()
        .ReverseMap();
            CreateMap<Knjiga, KnjigaConfirmation>()
               .ReverseMap();
            CreateMap<KnjigaConfirmation, KnjigaConfirmationDto>()
                .ReverseMap();
            CreateMap<Knjiga, KnjigaCreationDto>()
                .ReverseMap();
            CreateMap<Knjiga, KnjigaUpdateDto>()
                .ReverseMap();
        }
    }
}

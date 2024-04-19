using AutoMapper;
using Knjizara.Entitets;
using Knjizara.Entitets.Confirmation;
using Knjizara.Models.PorudzbinaKnjiga;

namespace Knjizara.Profiles
{
    public class PorudzbinaKnjigaProfile : Profile
    {
        public PorudzbinaKnjigaProfile() 
        {
            CreateMap<PorudzbinaKnjiga, PorudzbinaKnjigaDto>()
               .ReverseMap();
            CreateMap<PorudzbinaKnjiga, PorudzbinaKnjigaConfirmation>()
               .ReverseMap();
            CreateMap<PorudzbinaKnjigaConfirmation, PorudzbinaKnjigaConfirmationDto>()
                .ReverseMap();
            CreateMap<PorudzbinaKnjiga, PorudzbinaKnjigaCreationDto>()
                .ReverseMap();
            CreateMap<PorudzbinaKnjiga, PorudzbinaKnjigaUpdateDto>()
                .ReverseMap();
        }
    }
}

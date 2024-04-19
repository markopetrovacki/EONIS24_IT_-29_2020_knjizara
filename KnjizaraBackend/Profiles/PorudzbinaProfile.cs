using AutoMapper;
using Knjizara.Entitets;
using Knjizara.Entitets.Confirmation;
using Knjizara.Models.Porudzbina;

namespace Knjizara.Profiles
{
    public class PorudzbinaProfile : Profile
    {
        public PorudzbinaProfile() 
        {
            CreateMap<Porudzbina, PorudzbinaDto>()
                .ReverseMap();
            CreateMap<Porudzbina, PorudzbinaConfirmation>()
               .ReverseMap();
            CreateMap<PorudzbinaConfirmation, PorudzbinaConfirmationDto>()
                .ReverseMap();
            CreateMap<Porudzbina, PorudzbinaCreationDto>()
                .ReverseMap();
            CreateMap<Porudzbina, PorudzbinaUpdateDto>()
                .ReverseMap();
        }
    }
}

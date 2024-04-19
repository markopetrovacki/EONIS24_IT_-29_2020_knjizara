using AutoMapper;
using Knjizara.Entitets;
using Knjizara.Entitets.Confirmation;
using Knjizara.Models.Dostava;

namespace Knjizara.Profiles
{
    public class DostavaProfile : Profile
    {
        public DostavaProfile() 
        {
            CreateMap<Dostava, DostavaDto>()
                .ReverseMap();
            CreateMap<Dostava, DostavaConfirmation>()
               .ReverseMap();
            CreateMap<DostavaConfirmation, DostavaConfirmationDto>()
                .ReverseMap();
            CreateMap<Dostava, DostavaCreationDto>()
                .ReverseMap();
            CreateMap<Dostava, DostavaUpdateDto>()
                .ReverseMap();

            

        }
    }
}

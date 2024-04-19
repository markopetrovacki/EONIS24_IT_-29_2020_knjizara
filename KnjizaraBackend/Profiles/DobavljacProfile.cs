using AutoMapper;
using Knjizara.Entitets;
using Knjizara.Entitets.Confirmation;
using Knjizara.Models.Dobavljac;

namespace Knjizara.Profiles
{
    public class DobavljacProfile : Profile
    {
        public DobavljacProfile() 
        {
            CreateMap<Dobavljac, DobavljacDto>()
                .ReverseMap();
            CreateMap<Dobavljac, DobavljacConfirmation>()
               .ReverseMap();
            CreateMap<DobavljacConfirmation, DobavljacConfirmationDto>()
                .ReverseMap();
            CreateMap<Dobavljac, DobavljacCreationDto>()
                .ReverseMap();
            CreateMap<Dobavljac, DobavljacUpdateDto>()
                .ReverseMap();

     

        }
    }
}

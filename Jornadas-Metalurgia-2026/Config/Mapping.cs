using AutoMapper;
using Jornadas_Metalurgia_2026.Models.User;
using Jornadas_Metalurgia_2026.Models.User.DTO;

namespace Jornadas_Metalurgia_2026.Config
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            //Defaults
            CreateMap<int?, int>().ConvertUsing((src, dest) => src ?? dest);
            CreateMap<bool?, bool>().ConvertUsing((src, dest) => src ?? dest);
            CreateMap<string?, string>().ConvertUsing((src, dest) => src ?? dest);



            CreateMap<User, UserWithoutPasswordDTO>().ForMember(

                dest => dest.Roles,
                opt => opt.MapFrom(e => e.Roles.Select(x => x.Name).ToList())
                );
        }
    }
}

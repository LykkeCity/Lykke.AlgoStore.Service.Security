using AutoMapper;
using Lykke.AlgoStore.Service.Security.Core.Domain;
using Lykke.AlgoStore.Service.Security.Models;

namespace Lykke.AlgoStore.Service.Security
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserRoleModel, UserRoleData>()
                .ForMember(dest => dest.Permissions, opt => opt.Ignore());

            CreateMap<UserRoleUpdateModel, UserRoleData>()
                .ForMember(dest => dest.Permissions, opt => opt.Ignore());
        }
    }
}

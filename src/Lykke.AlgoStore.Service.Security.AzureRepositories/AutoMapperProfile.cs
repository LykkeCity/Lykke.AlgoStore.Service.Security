using AutoMapper;
using Lykke.AlgoStore.Service.Security.AzureRepositories.Entities;
using Lykke.AlgoStore.Service.Security.Core.Domain;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.AlgoStore.Service.Security.AzureRepositories
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //To entities
            CreateMap<RolePermissionMatchData, RolePermissionMatchEntity>()
                .ForMember(dest => dest.PartitionKey, opt => opt.MapFrom(src => src.RoleId))
                .ForMember(dest => dest.RowKey, opt => opt.MapFrom(src => src.PermissionId))
                .ForMember(dest => dest.ETag, opt => opt.UseValue("*"));

            CreateMap<UserPermissionData, UserPermissionEntity>()
                .ForMember(dest => dest.PartitionKey, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.RowKey, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.ETag, opt => opt.UseValue("*"));

            CreateMap<UserRoleMatchData, UserRoleMatchEntity>()
                .ForMember(dest => dest.PartitionKey, opt => opt.MapFrom(src => src.ClientId))
                .ForMember(dest => dest.RowKey, opt => opt.MapFrom(src => src.RoleId))
                .ForMember(dest => dest.ETag, opt => opt.UseValue("*"));

            CreateMap<UserRoleData, UserRoleEntity>()
                .ForMember(dest => dest.PartitionKey, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.RowKey, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.ETag, opt => opt.UseValue("*"));

            ForAllMaps((map, cfg) =>
            {
                if (map.DestinationType.IsSubclassOf(typeof(TableEntity)))
                {
                    //cfg.ForMember("ETag", opt => opt.Ignore());
                    //cfg.ForMember("PartitionKey", opt => opt.Ignore());
                    //cfg.ForMember("RowKey", opt => opt.Ignore());
                    cfg.ForMember("Timestamp", opt => opt.Ignore());
                }
            });

            //From entities
            CreateMap<RolePermissionMatchEntity, RolePermissionMatchData>()
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.PartitionKey))
                .ForMember(dest => dest.PermissionId, opt => opt.MapFrom(src => src.RowKey));

            CreateMap<UserPermissionEntity, UserPermissionData>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PartitionKey))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.RowKey));

            CreateMap<UserRoleMatchEntity, UserRoleMatchData>()
                .ForMember(dest => dest.ClientId, opt => opt.MapFrom(src => src.PartitionKey))
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RowKey));

            CreateMap<UserRoleEntity, UserRoleData>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PartitionKey))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.RowKey))
                .ForMember(dest => dest.Permissions, opt => opt.Ignore());
        }
    }
}

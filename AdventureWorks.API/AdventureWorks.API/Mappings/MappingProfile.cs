using AutoMapper;
using AdventureWorks.DbModel.Models;
using AdventureWorks.API.Models;

namespace AdventureWorks.API.Mappings
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<ProductDbModel, ProductApiModel>()
                .ForMember(dest => dest.Rowguid, options =>
                {
                    options.MapFrom(src => src.Rowguid);
                    options.ConvertUsing(new GuidToStringMapConverter());
                });

            CreateMap<ProductApiModel, ProductDbModel>()
                .ForMember(dest => dest.Rowguid, options =>
                {
                    options.MapFrom(src => src.Rowguid);
                    options.ConvertUsing(new StringToGuidMapConverter());
                });
        }
    }
}

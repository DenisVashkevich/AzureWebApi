using AutoMapper;
using AdventureWorks.DbModel.Models;


namespace AdventureWorks.API.Models
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<ProductDbModel, ProductApiModel>();
            CreateMap<ProductApiModel, ProductDbModel>();
        }
    }
}

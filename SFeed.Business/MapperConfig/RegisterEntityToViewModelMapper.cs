using AutoMapper;
using SFeed.Core.Models;
using SFeed.SqlRepository;

namespace SFeed.Business.MapperConfig
{
    public class RegisterEntityToViewModelMapper
    {
        public static void Register(IMapperConfigurationExpression cfg)
        {
            //cfg.CreateMap<User, UserModel>();
            cfg.CreateMap<WallEntry, WallEntryModel>();
        }
    }
}

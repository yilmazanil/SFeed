using AutoMapper;
using SFeed.Data;
using SFeed.Model;

namespace SFeed.Business.MapperConfig
{
    public class RegisterEntityToViewModelMapper
    {
        public static void Register(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<User, UserViewModel>();
            cfg.CreateMap<SocialPost, SocialPostViewModel>();
        }
    }
}

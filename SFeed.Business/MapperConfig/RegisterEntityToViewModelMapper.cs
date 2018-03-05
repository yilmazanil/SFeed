using AutoMapper;
using SFeed.Core.Models.Newsfeed;
using SFeed.Core.Models.WallPost;
using SFeed.SqlRepository;

namespace SFeed.Business.MapperConfig
{
    public class RegisterEntityToViewModelMapper
    {
        public static void Register(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<WallPost, WallPostModel>();
            cfg.CreateMap<WallPostCreateRequest, WallPostModel>();
            cfg.CreateMap<NewsfeedAction, NewsfeedResponseItem>();
        }
    }
}

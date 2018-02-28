using SFeed.Business.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFeed.Model;
using SFeed.Data.Infrastructure;
using SFeed.Data;
using SFeed.Data.SqlRepositories;
using AutoMapper;

namespace SFeed.Business.Services
{
   

    public class UserWallService : IUserWallService
    {
        private readonly ISqlRepository<SocialPost> socialPostRepo;
        private readonly ISqlRepository<UserWall> userWallRepo;

        public UserWallService()
        {
            this.socialPostRepo = new SocialPostRepository();
            this.userWallRepo = new UserWallRepository();
        }

        public UserWallService(ISqlRepository<SocialPost> socialPostRepo,
             ISqlRepository<UserWall> userWallRepo)
        {
            this.socialPostRepo = socialPostRepo;
            this.userWallRepo = userWallRepo;
        }
        public long PublishToUserWall(int wallUserId, SocialPostModel entry)
        {
            var dbEntry = new SocialPost { Body = entry.Body, CreatedBy = entry.CreatedBy, IsDeleted = false, CreatedDate = DateTime.Now };
            var userWallEntry = new UserWall { UserId = wallUserId };
            socialPostRepo.Add(dbEntry);
            socialPostRepo.Commit();
            userWallEntry.SocialPostId = dbEntry.Id;
            userWallRepo.Add(userWallEntry);
            userWallRepo.Commit();
            return dbEntry.Id;
        }

        public IEnumerable<SocialPostModel> GetUserWall(int userId)
        {
            //TODO : Refactor after references
            var postRef = userWallRepo.GetMany(w => w.UserId == userId);
            var posts = socialPostRepo.GetMany(p => postRef.Any(u=>u.SocialPostId == p.Id));
            return Mapper.Map<IEnumerable<SocialPostModel>>(posts);
        }

        public void Delete(long postId)
        {
            userWallRepo.Delete(p => p.SocialPostId == postId);
            userWallRepo.Commit();
            socialPostRepo.Delete(p => p.Id == postId);
        }
    }
}

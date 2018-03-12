using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFeed.Business.MapperConfig;
using SFeed.Core.Models;
using SFeed.Core.Models.WallPost;
using System;
using System.Collections.Generic;

namespace SFeed.Tests
{
    public class ProviderTestBase
    {
        protected List<string> RandomUserNames = new List<string>
        { "Moira","Lasandra","Meg","Shery","Georgianne","Janean","Glenn","Colby","Eusebio","Joellen","My","Melodie","Joella","Lenna","Marguerita",
          "Jamel","Gilda","Marya","Tamela","Geneva","Florrie","Delphia","Walker","Arnulfo","Dacia","Arielle","Marhta","Alda","Pauline","Burl","Doug",
          "Boris","Alyse","Elijah","Adah","Maryetta","Darline","Breana","Brad","Juli","Yoshie","Kathline","Fanny","Oma","Ida","Shera","Willy","Lannie",
          "In","Karla" };
        protected List<string> RandomGroupNames = new List<string>
        { "antiutilitarianism","overindividualization","overintellectualization","dicyclopentadienyliron","bioelectrogenetically","triacetyloleandomycin",
            "trinitrophenylmethylnitramine","desoxyribonucleoprotein","deoxyribonucleoprotein","aerobacteriologically","antimaterialistically",
            "cyclotrimethylenetrinitramine","phenylethylmalonylurea","anatomicopathological","electrophysiologically","magnetothermoelectricity",
            "poliencephalomyelitis","polytetrafluoroethylene","chorioepitheliomata","disestablishmentarianism"  };

        protected string testPublicUser = "publicUser1";
        protected string testPublicGroup = "publicGroup1";
        protected string testPrivateGroup = "privateGroup1";

        [TestInitialize]
        public void InitializeCommon()
        {
            Mapper.Reset();
            Mapper.Initialize(cfg =>
            {
                RegisterEntityToViewModelMapper.Register(cfg);
            });
        }

        protected WallPostCreateRequest GetSampleWallCreateRequest(string postedBy, WallOwner targetWall)
        {
            var body = TestUtils.GenerateLoremIpsumText();

            return new WallPostCreateRequest
            {
                Body = body,
                PostedBy = postedBy,
                PostType = WallPostType.text,
                WallOwner = targetWall,
            };
        }

        protected string GetRandomUserName()
        {
            var index = new Random().Next(0, RandomUserNames.Count - 1);
            return RandomUserNames[index];
        }

        protected string GetRandomGroupName()
        {
            var index = new Random().Next(0, RandomGroupNames.Count - 1);
            return RandomGroupNames[index];
        }

        protected WallOwner GetRandomUserWallOwner(bool isPublic)
        {
            var name = GetRandomUserName();
            return new WallOwner { IsPublic = isPublic, Id = name, WallOwnerType = WallOwnerType.user };
        }

        protected WallOwner GetRandomGroupWallOwner(bool isPublic)
        {
            var name = GetRandomGroupName();
            return new WallOwner { IsPublic = isPublic, Id = name, WallOwnerType = WallOwnerType.group };
        }
    }
}

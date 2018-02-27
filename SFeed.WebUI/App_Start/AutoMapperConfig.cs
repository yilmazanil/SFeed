using AutoMapper;
using SFeed.Business.MapperConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SFeed.WebUI.App_Start
{
    public class AutoMapperConfig
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                RegisterEntityToViewModelMapper.Register(cfg);
            });
        }
    }
}
using SFeed.Core.Models.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace SFeed.SqlRepository.Implementation
{
    public class ApplicationConfigurationRepository
    {
        public List<ConfigurationParameter> FetchConfiguration()
        {
            using (var entities = new SocialFeedEntities())
            {
                return entities.ApplicationConfiguration.Select(p=> new ConfigurationParameter { Id = p.Id, Value = p.Value, Description = p.Description }).ToList();
            }
        }

        public ConfigurationParameter FetchParameter(string parameterId)
        {
            using (var entities = new SocialFeedEntities())
            {
                return entities.ApplicationConfiguration.Where(p=>p.Id == parameterId).Select(p => new ConfigurationParameter { Id = p.Id, Value = p.Value, Description = p.Description }).FirstOrDefault();
            }
        }
    }
}

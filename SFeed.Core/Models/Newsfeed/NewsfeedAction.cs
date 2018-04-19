using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFeed.Core.Models.Newsfeed
{
    public class NewsfeedAction
    {
        public string By { get; set; }

        public NewsfeedType Action { get; set; }
    }
}

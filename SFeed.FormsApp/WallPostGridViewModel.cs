using System;

namespace SFeed.FormsApp
{
    public class WallPostGridViewModel
    {
        
        public string Id { get; set; }

        public string Body { get; set; }

        public string PostedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}

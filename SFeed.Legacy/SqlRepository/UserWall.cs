//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SFeed.SqlRepository
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserWall
    {
        public string WallPostId { get; set; }
        public string UserId { get; set; }
    
        public virtual WallPost WallPost { get; set; }
    }
}

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
    
    public partial class GroupWall
    {
        public string WallPostId { get; set; }
        public string GroupId { get; set; }
    
        public virtual WallPost WallPost { get; set; }
    }
}

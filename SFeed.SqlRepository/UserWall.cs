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
        public string UserId { get; set; }
        public System.Guid WallEntryId { get; set; }
    
        public virtual WallEntry WallEntry { get; set; }
    }
}
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FlowRepository
{
    using System;
    using System.Collections.Generic;
    
    public partial class TagsToContent
    {
        public int ContentId { get; set; }
        public int TagId { get; set; }
        public Nullable<bool> bs { get; set; }
    
        public virtual Tag Tag { get; set; }
        public virtual Content Content { get; set; }
    }
}
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
    
    public partial class Error
    {
        public Error()
        {
            this.Errors1 = new HashSet<Error>();
        }
    
        public int Id { get; set; }
        public int FK_ErrorType { get; set; }
        public string ErrorMessage { get; set; }
        public string StackTrace { get; set; }
        public string UrlRoute { get; set; }
        public System.DateTime Timestamp { get; set; }
        public Nullable<int> FK_ParentErrorID { get; set; }
    
        public virtual ICollection<Error> Errors1 { get; set; }
        public virtual Error Error1 { get; set; }
        public virtual ErrorType ErrorType { get; set; }
    }
}

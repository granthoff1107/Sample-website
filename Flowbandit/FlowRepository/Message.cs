
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
    
public partial class Message
{

    public int Id { get; set; }

    public string Data { get; set; }

    public int ReceiverUserId { get; set; }

    public int SenderUserId { get; set; }

    public bool IsViewed { get; set; }

    public System.DateTime Timestamp { get; set; }



    public virtual User User { get; set; }

    public virtual User User1 { get; set; }

}

}
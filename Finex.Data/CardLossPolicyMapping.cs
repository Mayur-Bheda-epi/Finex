//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Finex.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class CardLossPolicyMapping
    {
        public int Id { get; set; }
        public Nullable<int> CardTypeId { get; set; }
        public Nullable<int> LossTypeId { get; set; }
        public Nullable<int> PolicyNumberId { get; set; }
        public Nullable<bool> IsSecured { get; set; }
    
        public virtual LossTypeMaster LossTypeMaster { get; set; }
        public virtual PolicyMaster PolicyMaster { get; set; }
    }
}
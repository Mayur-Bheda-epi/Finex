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
    
    public partial class DocumentUpload
    {
        public int Id { get; set; }
        public Nullable<int> CustId { get; set; }
        public Nullable<int> ClaimId { get; set; }
        public Nullable<int> DocumentTypeId { get; set; }
        public string DocumentPath { get; set; }
        public Nullable<System.DateTime> DateOfUpload { get; set; }
    
        public virtual Claim Claim { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual DocumentTypeMaster DocumentTypeMaster { get; set; }
    }
}

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
    
    public partial class DocumentTypeMaster
    {
        public DocumentTypeMaster()
        {
            this.DocumentUploads = new HashSet<DocumentUpload>();
        }
    
        public int DocumentTypeId { get; set; }
        public string DocumentTypeName { get; set; }
        public Nullable<int> CardTypeId { get; set; }
        public Nullable<int> LossTypeId { get; set; }
    
        public virtual CardTypeMaster CardTypeMaster { get; set; }
        public virtual ICollection<DocumentUpload> DocumentUploads { get; set; }
    }
}

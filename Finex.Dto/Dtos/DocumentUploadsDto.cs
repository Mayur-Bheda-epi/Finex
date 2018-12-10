using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Finex.Dto.Dtos
{
    public class DocumentUploadsDto
    {
        public int DocumentUploadsId { get; set; }

        public int CustomerId { get; set; }

        public int ClaimId { get; set; }

        public int DocumentTypeId { get; set; }

        public string DocumentTypeName { get; set; }

        public string DocumentPath { get; set; }

        public DateTime? DocumentUploadDate { get; set; }
    }
}

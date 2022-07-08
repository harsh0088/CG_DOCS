using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CgDocs.RequestModel
{
    public class DocumentRequest
    {
        public string DocumentName { get; set; }
        public string DocumentContentType { get; set; }
        public int? DocumentSize { get; set; }
        public int? DocumentCreatedBy { get; set; }
        public DateTime? DocumentCreatedAt { get; set; }
        public int? FolderId { get; set; }
        public bool DocumentIsDeleted { get; set; }
        public bool? DocumentIsFavourite { get; set; }
    }
}

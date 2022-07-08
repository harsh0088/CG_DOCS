using System;
using System.Collections.Generic;

namespace CgDocs.Models
{
    public partial class Documents
    {
        public int DocumentId { get; set; }
        public string DocumentName { get; set; }
        public string DocumentContentType { get; set; }
        public int? DocumentSize { get; set; }
        public int? DocumentCreatedBy { get; set; }
        public DateTime? DocumentCreatedAt { get; set; }
        public int? FolderId { get; set; }
        public bool DocumentIsDeleted { get; set; }
        public bool? DocumentIsFavourite { get; set; }

        public Users DocumentCreatedByNavigation { get; set; }
        public Folders Folder { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace CgDocs.Models
{
    public partial class Folders
    {
        public Folders()
        {
            Documents = new HashSet<Documents>();
        }

        public int FolderId { get; set; }
        public string FolderName { get; set; }
        public int? FolderCreatedBy { get; set; }
        public DateTime? FolderCreatedAt { get; set; }
        public bool? FolderIsDeleted { get; set; }
        public bool? FolderIsFavourite { get; set; }

        public Users FolderCreatedByNavigation { get; set; }
        public ICollection<Documents> Documents { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CgDocs.RequestModel
{
    public class FolderRequest
    {
        public string FolderName { get; set; }
        public int? FolderCreatedBy { get; set; }
        public DateTime? FolderCreatedAt { get; set; }
        public bool? FolderIsDeleted { get; set; }
        public bool? FolderIsFavourite { get; set; }
    }
}

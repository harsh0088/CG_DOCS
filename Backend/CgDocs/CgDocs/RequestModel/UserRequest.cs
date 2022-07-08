using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CgDocs.RequestModel
{
    public class UserRequest
    {
        public string Username { get; set; }
        public string UserPassword { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}

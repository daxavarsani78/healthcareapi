using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entity
{
    public class Tbl_Enquiry : BaseEntity
    {
        public string Name { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
        public string Message { get; set; } = String.Empty;
        public string PhoneNumber { get; set; } = String.Empty;
        public bool IsResolved { get; set; } = false;
    }
}

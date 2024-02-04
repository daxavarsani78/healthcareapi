using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.ViewModels.RequestModel
{
    public class EnquiryRequest
    {
        public int? Id { get; set; }
        public string Email { get; set; } = String.Empty;
        public string Message { get; set; } = String.Empty;
        public string PhoneNumber { get; set; } = String.Empty;
        public string Name { get; set; } = String.Empty;
        public DateTime CreatedDate { get; set; }
        public bool IsResolved { get; set; }
        public bool IsDeleted { get; set; }

    }

    public class EnquiryResult
    {
        public List<EnquiryRequest> Records { get; set; } = new List<EnquiryRequest>();
        public int TotalRecords { get; set; }
        public int DeleteRecords { get; set; }
        public int NotDeleteRecords { get; set; }

        public int NewRecords { get; set; }
        public int ResolvedRecords { get; set; }

    }
}

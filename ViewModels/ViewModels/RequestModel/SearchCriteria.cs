using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.ViewModels.RequestModel
{
    public class SearchCriteria
    {
        public string SearchText { get; set; } = String.Empty;
        public int PageNumber { get; set; } = 1;
        public string SortBy { get; set; } = "CreatedDate";
        public bool SortByeDscending { get; set; } = false;
        public int ResolvedOnly { get; set; } = -1;
        public bool DeletedOnly { get; set; } = false;
    }
}

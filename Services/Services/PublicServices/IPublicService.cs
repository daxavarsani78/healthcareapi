using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.ViewModels.RequestModel;

namespace Services.Services.PublicServices
{
    public interface IPublicService
    {
        Task<bool> AddEnquiry(EnquiryRequest request);
        Task<EnquiryResult> GetAllEnquiry(SearchCriteria request);
        Task<bool> DeleteEnquiry(int id);
        Task<bool> UpdateEnquiryStatus(int id, bool isResolve);
    }
}

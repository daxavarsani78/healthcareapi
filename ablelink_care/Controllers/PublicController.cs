using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
using Services.Services.PublicServices;
using Services.Services.UserIdentity;
using ViewModels.ViewModels.RequestModel;
using ViewModels.ViewModels.ResponseModel;

namespace AblelinkCare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicController : Controller
    {
        private readonly IPublicService _publicService;

        public PublicController(IPublicService publicService)
        {
            _publicService = publicService;
        }
        [HttpPost("enquiry")]
        public async Task<ActionResult> AddNewEnquiry(EnquiryRequest request)
        {
            var result = await _publicService.AddEnquiry(request);
            if (result != null && result)
            {
                string successMessage = request.Id == 0 || request.Id == null ? "Enquiry added successfully." : "Enquiry updated successfully.";
                return Ok(new ResponseBaseModel(200, successMessage));
            }
            else
                return StatusCode(500);
        }

        [Authorize]
        [HttpPost("get-enquiry")]
        public async Task<ActionResult> GetAllEnquiry([FromBody] SearchCriteria request)
        {
            var result = await _publicService.GetAllEnquiry(request);
            if (result != null)
                return Ok(new ResponseBaseModel(200, result));
            else
                return NotFound();
        }

        [Authorize]
        [HttpDelete("delete-enquiry/{id}")]
        public async Task<ActionResult> DeleteEnquiry(int id)
        {
            var result = await _publicService.DeleteEnquiry(id);
            if (result)
                return Ok(new ResponseBaseModel(200, "Enquiry deleted successfully."));
            else
                return StatusCode(500);
        }

        [Authorize]
        [HttpPut("mark-as-resolved-enquiry/{id}")]
        public async Task<ActionResult> DoMarkAsResolvedEnquiry(int id)
        {
            var result = await _publicService.UpdateEnquiryStatus(id,true);
            if (result)
                return Ok(new ResponseBaseModel(200, "Enquiry updated successfully."));
            else
                return StatusCode(500);
        }

        [Authorize]
        [HttpPut("mark-as-un-resolved-enquiry/{id}")]
        public async Task<ActionResult> DoMarkAsUnResolvedEnquiry(int id)
        {
            var result = await _publicService.UpdateEnquiryStatus(id, false);
            if (result)
                return Ok(new ResponseBaseModel(200, "Enquiry updated successfully."));
            else
                return StatusCode(500);
        }
    }
}

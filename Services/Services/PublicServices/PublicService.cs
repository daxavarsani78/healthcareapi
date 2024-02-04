using Data.Data;
using Data.Entity;
using MailKit.Search;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Common;
using ViewModels.ViewModels.RequestModel;

namespace Services.Services.PublicServices
{
    public class PublicService : IPublicService
    {
        private readonly AppDB _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PublicService(IHttpContextAccessor httpContextAccessor, AppDB dbContext)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<bool> AddEnquiry(EnquiryRequest request)
        {
            bool isSuccess = false;
            try
            {
                Tbl_Enquiry record = new Tbl_Enquiry();
                record.Name = request.Name;
                record.IsResolved = false;
                record.Email = request.Email;
                record.Message = request.Message;
                record.PhoneNumber = request.PhoneNumber;
                _dbContext.Tbl_Enquiry.Add(record);
                _dbContext.SaveChanges();
                isSuccess = true;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
               // if (isSuccess)
                   // CommonService.SendEmail("renishribadiya10@outlook.com", request.Email, "Your inquiry has been successfully received", getEmailHtml(request.Name), true, "renishribadiya10@outlook.com");
            }
        }
        public string getEmailHtml(string name)
        {
            return @$"<!DOCTYPE html> <html lang=""en""> <head> <meta charset=""UTF-8""> <meta name=""viewport"" content=""width=device-width, initial-scale=1.0""> <title>Enquiry Received</title> <style> body {{ font-family: Serif, sans-serif; background-color: #7fae7a; margin: 0; padding: 0; }} .container {{ max-width: 600px; margin: 0 auto; padding: 20px; background-color: #ffffff; border: 1px solid #e4e4e4; }} h2 {{ font-size: 24px; margin: 0; padding-bottom: 10px; }} p {{ font-size: 16px; margin: 0; padding-bottom: 20px; }} .footer {{ font-size: 12px; text-align: center; margin-top: 20px; color: #FBF8F7; }} </style> </head> <body> <div class=""container""> <h4>Hello {name}</h4> <p>Your enquiry has been received successfully. We will review your message and get back to you shortly.</p> <p>Thank you for considering our services. If you have any further questions or need immediate assistance, please don't hesitate to contact us.</p> <p>Best regards,<br> Ablelink Care </p> </div> <div class=""footer""> <p>This is an automated message. Please do not reply to this email.</p> <p>© 2023 Ablelink Care. All rights reserved.</p> </div> </body> </html>";
        }
        public async Task<EnquiryResult> GetAllEnquiry(SearchCriteria request)
        {
            try
            {
                bool resolveStatus = request.ResolvedOnly == 0 ? false : true;
                var records = from item in _dbContext.Tbl_Enquiry
                              where (string.IsNullOrEmpty(request.SearchText) ||
                                    item.Name.Contains(request.SearchText) ||
                                    item.Message.Contains(request.SearchText) ||
                                    item.PhoneNumber.Contains(request.SearchText) ||
                                    item.Email.Contains(request.SearchText)) && (request.ResolvedOnly == -1 || item.IsResolved == resolveStatus) && (item.IsDeleted == request.DeletedOnly)
                              select item;

                var query = records.Select(record => new EnquiryRequest
                {
                    Id = record.Id,
                    Name = record.Name,
                    Email = record.Email,
                    PhoneNumber = record.PhoneNumber,
                    Message = record.Message,
                    IsResolved = record.IsResolved,
                    CreatedDate = record.CreatedOn,
                    IsDeleted = record.IsDeleted,
                });

                if (!request.SortByeDscending)
                    switch (request.SortBy)
                    {
                        case "Name":
                            {
                                query = query.OrderBy(x => x.Name);
                                break;
                            }
                        case "Email":
                            {
                                query = query.OrderBy(x => x.Email);
                                break;
                            }
                        case "Mobile":
                            {
                                query = query.OrderBy(x => x.PhoneNumber);
                                break;
                            }
                        case "IsResolved":
                            {
                                query = query.OrderBy(x => x.IsResolved);
                                break;
                            }
                        case "Message":
                            {
                                query = query.OrderBy(x => x.Message);
                                break;
                            }
                        case "CreatedDate":
                            {
                                query = query.OrderBy(x => x.CreatedDate);
                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }
                else
                    switch (request.SortBy)
                    {

                        case "Name":
                            {
                                query = query.OrderByDescending(x => x.Name);
                                break;
                            }
                        case "Email":
                            {
                                query = query.OrderByDescending(x => x.Email);
                                break;
                            }
                        case "Mobile":
                            {
                                query = query.OrderByDescending(x => x.PhoneNumber);
                                break;
                            }
                        case "IsResolved":
                            {
                                query = query.OrderByDescending(x => x.IsResolved);
                                break;
                            }
                        case "Message":
                            {
                                query = query.OrderByDescending(x => x.Message);
                                break;
                            }
                        case "CreatedDate":
                            {
                                query = query.OrderByDescending(x => x.CreatedDate);
                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }
                EnquiryResult finalResult = new EnquiryResult();
                var recordCounts = from item in _dbContext.Tbl_Enquiry select item;
                finalResult.Records = await query.Skip((request.PageNumber - 1) * 10).Take(10).ToListAsync();
                finalResult.NotDeleteRecords = await recordCounts.Where(item => !item.IsDeleted).CountAsync();
                finalResult.DeleteRecords = await recordCounts.Where(item => item.IsDeleted).CountAsync();
                finalResult.TotalRecords = await recordCounts.CountAsync();
                finalResult.NewRecords = await recordCounts.Where(x => x.CreatedOn >= DateTime.Today.AddDays(-1) && !x.IsDeleted).CountAsync();
                finalResult.ResolvedRecords = await recordCounts.Where(x => x.IsResolved && !x.IsDeleted).CountAsync();
                return finalResult;
            }
            catch (Exception)
            {
                return new EnquiryResult();
            }
        }

        public async Task<bool> DeleteEnquiry(int id)
        {
            try
            {
                Tbl_Enquiry? record = await _dbContext.Tbl_Enquiry.FirstOrDefaultAsync(x => x.Id == id);
                if (record != null)
                {
                    record.IsDeleted = !record.IsDeleted;
                    _dbContext.Tbl_Enquiry.Update(record);
                    _dbContext.SaveChanges();
                }
                else
                    return false;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> UpdateEnquiryStatus(int id, bool isResolve)
        {
            try
            {
                Tbl_Enquiry? record = await _dbContext.Tbl_Enquiry.FirstOrDefaultAsync(x => x.Id == id);
                if (record != null)
                {
                    record.IsResolved = isResolve;
                    _dbContext.Tbl_Enquiry.Update(record);
                    _dbContext.SaveChanges();
                }
                else
                    return false;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

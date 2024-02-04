using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.ViewModels.ResponseModel
{
    public class ResponseBaseModel
    {
        public string? message { get; set; } = null;
        public object? response { get; set; } = null;
        public int status { get; set; }

        public ResponseBaseModel(int statusCode, object responseBody, string responseMessage)
        {
            message = responseMessage;
            response = responseBody;
            status = statusCode;
        }

        public ResponseBaseModel(int statusCode, string responseMessage)
        {
            message = responseMessage;
            status = statusCode;
        }


        public ResponseBaseModel(int statusCode, object responseBody)
        {
            response = responseBody;
            status = statusCode;
        }
    }
}

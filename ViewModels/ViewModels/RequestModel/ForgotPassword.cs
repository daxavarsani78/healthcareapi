using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.ViewModels.RequestModel
{
    public class ForgotPassword
    {
        public string password { get; set; }
        public string userName { get; set; }
        public string securityCode { get; set; }
    }
}

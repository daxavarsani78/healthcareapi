using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.ViewModels.RequestModel
{
    public class UserSignin
    {
        public string Email { get; set; }
        public int? Otp { get; set; }
    }
}

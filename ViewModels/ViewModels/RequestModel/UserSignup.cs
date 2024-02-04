using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.ViewModels.RequestModel
{
    public class UserSignup
    {
        public string FirstName { get; set; } = String.Empty;
        public string LastName { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;
        public string PhoneNumber { get; set; } = String.Empty;
        public string Secret { get; set; } = String.Empty;
    }
}

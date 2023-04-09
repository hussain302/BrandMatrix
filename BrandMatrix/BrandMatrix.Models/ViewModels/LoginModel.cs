using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrandMatrix.Models.ViewModels
{
    public class LoginModel
    {
        public string LoginId { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public bool IsRemember { get; set; }
    }
}

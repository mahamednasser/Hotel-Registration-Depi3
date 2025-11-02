using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estra7a.Models.ViewModels
{
    public class RegisterUserVM
    {
        public string Name { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmedPassword { get; set; }
        public string Email { get; set; }

        public string Address { get; set; }
    }
}

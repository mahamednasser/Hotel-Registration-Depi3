using Estra7a.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estra7a.Models.ViewModels
{
    public class ApplicationUserVM
    {
        public IEnumerable<ApplicationUser> UserList { get; set; }
    }
}

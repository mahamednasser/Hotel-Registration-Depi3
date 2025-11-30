using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estra7a.Services.ServiceContracts
{
    public interface ICartService
    {
        Task<Cart> GetOrCreateCartAsync(string userId);
    }

}

using SaipemE_PTW.Shared.Models.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaipemE_PTW.Services.Common
{
    public interface IMenuService
    {
        Task<List<MenuCard>> GetMenuAsync(System.Globalization.CultureInfo language);
    }
}

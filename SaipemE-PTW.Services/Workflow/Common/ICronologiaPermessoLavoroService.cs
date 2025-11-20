using SaipemE_PTW.Shared.Models.PWT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaipemE_PTW.Services.Workflow.Common
{
    public interface ICronologiaPermessoLavoroService
    {
        Task<IEnumerable<CronologiaPermessoLavoro>> GetCronologiaAsync(CancellationToken ct = default);
    }
}

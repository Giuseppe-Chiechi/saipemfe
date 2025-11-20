// Data:2025-11-10 - Service application per AttachmentTypes
// Sicurezza: incapsula EF Core, evita esporre DbContext a livello endpoint; input validato a monte
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SaipemE_PTW.Producer.Data;
using SaipemE_PTW.Shared.Models.PWT;

namespace SaipemE_PTW.Producer.Services
{
    // Data:2025-11-10 - Interfaccia service AttachmentTypes
    public interface IAttachmentTypeService
    {
        Task<IEnumerable<AttachmentTypeDto>> GetAsync(string lang, CancellationToken ct);
    }

    // Data:2025-11-10 - Implementazione service sicura con logging
    public sealed class AttachmentTypeService : IAttachmentTypeService
    {
        private readonly AppDbContext _db;
        private readonly ILogger<AttachmentTypeService> _logger;

        public AttachmentTypeService(AppDbContext db, ILogger<AttachmentTypeService> logger)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<AttachmentTypeDto>> GetAsync(string lang, CancellationToken ct)
        {
            try
            {
                // Data:2025-11-10 - Query con no-tracking per performance
                var entities = await _db.AttachmentTypes
                .Include(a => a.Localizations)
                .AsNoTracking()
                .ToListAsync(ct);

                return entities.Select(a => a.ToDto(lang));
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("[AttachmentTypeService] Operazione cancellata");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[AttachmentTypeService] Errore durante la lettura dei dati");
                throw; // Data:2025-11-10 - lascio gestire all'endpoint con Results.Problem
            }
        }
    }
}

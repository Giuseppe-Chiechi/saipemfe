using Microsoft.Extensions.Logging;
using SaipemE_PTW.Shared.Models.Auth;
using SaipemE_PTW.Shared.Models.PWT;


namespace SaipemE_PTW.Services.Workflow.Common
{
    public sealed class CronologiaPermessoLavoroService : ICronologiaPermessoLavoroService
    {
        private readonly ILogger<CronologiaPermessoLavoroService> _logger;
        private static readonly IReadOnlyList<CronologiaPermessoLavoro> _seed = CreateSeed();
        private static readonly TimeSpan SimulatedLatency = TimeSpan.FromMilliseconds(500); // 500ms per UX realistica

        public CronologiaPermessoLavoroService(ILogger<CronologiaPermessoLavoroService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger)); // Data: 2025-10-21 - DI null-check
        }

        public async Task<IEnumerable<CronologiaPermessoLavoro>> GetCronologiaAsync(CancellationToken ct = default)
        {
            try
            {
                await Task.Delay(SimulatedLatency, ct); // Data: 2025-10-21 - Simula round-trip microservizio
                _logger.LogInformation("[CronologiaPermessoLavoroService] Fetched list. Count={Count}", _seed.Count);
                // Data: 2025-10-21 - Restituisce copia immutabile per evitare side effects
                return _seed.ToList();
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("[CronologiaPermessoLavoroService] Request cancelled");
                return Array.Empty<CronologiaPermessoLavoro>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[CronologiaPermessoLavoroService] Error fetching list");
                return Array.Empty<CronologiaPermessoLavoro>();
            }
        }


        // Data: 2025-10-21 - Seed dati fittizi coerenti con PermessoLavoroModel
        private static IReadOnlyList<CronologiaPermessoLavoro> CreateSeed()
        {
            List<CronologiaPermessoLavoro> _ret;
            try
            {
                var now = DateTime.UtcNow.Date;
                _ret = new List<CronologiaPermessoLavoro>
                {
                    new CronologiaPermessoLavoro
                    {
                        Id = 1,
                        Azione = "Creato",
                        Nome = "Mario",
                        Cognome = "Rossi",
                        Data = now,
                        Ora = new TimeSpan(8, 0, 0),
                        Email = "MarioRossi@test.it",
                        Matricola = "A12345",
                        Ruolo = new UserRoleDto { Id = 2, Nome = "Autorita Esecutrice PA" }
                      },
                    new CronologiaPermessoLavoro
                        {
                            Id = 2,
                            Azione = "Salvato in bozza",
                            Nome = "Giuseppe",
                            Cognome = "Bianchi",
                            Data = now,
                            Ora = new TimeSpan(8, 0, 0),
                            Email = "GiuseppeBianchi@test.it",
                            Matricola = "A12346",
                            Ruolo = new UserRoleDto { Id = 2, Nome = "Autorita Esecutrice PA" }
                          },
                    new CronologiaPermessoLavoro
                    {
                        Id = 1,
                        Azione = "Creato",
                        Nome = "Mario",
                        Cognome = "Rossi",
                        Data = now,
                        Ora = new TimeSpan(8, 0, 0),
                        Email = "MarioRossi@test.it",
                        Matricola = "A12345",
                        Ruolo = new UserRoleDto { Id = 2, Nome = "Autorita Esecutrice PA" }
                      },
                    new CronologiaPermessoLavoro
                        {
                            Id = 2,
                            Azione = "Salvato in bozza",
                            Nome = "Giuseppe",
                            Cognome = "Bianchi",
                            Data = now,
                            Ora = new TimeSpan(8, 0, 0),
                            Email = "GiuseppeBianchi@test.it",
                            Matricola = "A12346",
                            Ruolo = new UserRoleDto { Id = 2, Nome = "Autorita Esecutrice PA" }
                          },
                     new CronologiaPermessoLavoro
                    {
                        Id = 1,
                        Azione = "Creato",
                        Nome = "Mario",
                        Cognome = "Rossi",
                        Data = now,
                        Ora = new TimeSpan(8, 0, 0),
                        Email = "MarioRossi@test.it",
                        Matricola = "A12345",
                        Ruolo = new UserRoleDto { Id = 2, Nome = "Autorita Esecutrice PA" }
                      },
                    new CronologiaPermessoLavoro
                        {
                            Id = 2,
                            Azione = "Salvato in bozza",
                            Nome = "Giuseppe",
                            Cognome = "Bianchi",
                            Data = now,
                            Ora = new TimeSpan(8, 0, 0),
                            Email = "GiuseppeBianchi@test.it",
                            Matricola = "A12346",
                            Ruolo = new UserRoleDto { Id = 2, Nome = "Autorita Esecutrice PA" }
                          },
                    new CronologiaPermessoLavoro
                    {
                        Id = 1,
                        Azione = "Creato",
                        Nome = "Mario",
                        Cognome = "Rossi",
                        Data = now,
                        Ora = new TimeSpan(8, 0, 0),
                        Email = "MarioRossi@test.it",
                        Matricola = "A12345",
                        Ruolo = new UserRoleDto { Id = 2, Nome = "Autorita Esecutrice PA" }
                      },
                    new CronologiaPermessoLavoro
                        {
                            Id = 2,
                            Azione = "Salvato in bozza",
                            Nome = "Giuseppe",
                            Cognome = "Bianchi",
                            Data = now,
                            Ora = new TimeSpan(8, 0, 0),
                            Email = "GiuseppeBianchi@test.it",
                            Matricola = "A12346",
                            Ruolo = new UserRoleDto { Id = 2, Nome = "Autorita Esecutrice PA" }
                          },
                     new CronologiaPermessoLavoro
                    {
                        Id = 1,
                        Azione = "Creato",
                        Nome = "Mario",
                        Cognome = "Rossi",
                        Data = now,
                        Ora = new TimeSpan(8, 0, 0),
                        Email = "MarioRossi@test.it",
                        Matricola = "A12345",
                        Ruolo = new UserRoleDto { Id = 2, Nome = "Autorita Esecutrice PA" }
                      },
                    new CronologiaPermessoLavoro
                        {
                            Id = 2,
                            Azione = "Salvato in bozza",
                            Nome = "Giuseppe",
                            Cognome = "Bianchi",
                            Data = now,
                            Ora = new TimeSpan(8, 0, 0),
                            Email = "GiuseppeBianchi@test.it",
                            Matricola = "A12346",
                            Ruolo = new UserRoleDto { Id = 2, Nome = "Autorita Esecutrice PA" }
                          },
                    new CronologiaPermessoLavoro
                    {
                        Id = 1,
                        Azione = "Creato",
                        Nome = "Mario",
                        Cognome = "Rossi",
                        Data = now,
                        Ora = new TimeSpan(8, 0, 0),
                        Email = "MarioRossi@test.it",
                        Matricola = "A12345",
                        Ruolo = new UserRoleDto { Id = 2, Nome = "Autorita Esecutrice PA" }
                      },
                    new CronologiaPermessoLavoro
                        {
                            Id = 2,
                            Azione = "Salvato in bozza",
                            Nome = "Giuseppe",
                            Cognome = "Bianchi",
                            Data = now,
                            Ora = new TimeSpan(8, 0, 0),
                            Email = "GiuseppeBianchi@test.it",
                            Matricola = "A12346",
                            Ruolo = new UserRoleDto { Id = 2, Nome = "Autorita Esecutrice PA" }
                          },
                     new CronologiaPermessoLavoro
                    {
                        Id = 1,
                        Azione = "Creato",
                        Nome = "Mario",
                        Cognome = "Rossi",
                        Data = now,
                        Ora = new TimeSpan(8, 0, 0),
                        Email = "MarioRossi@test.it",
                        Matricola = "A12345",
                        Ruolo = new UserRoleDto { Id = 2, Nome = "Autorita Esecutrice PA" }
                      },
                    new CronologiaPermessoLavoro
                        {
                            Id = 2,
                            Azione = "Salvato in bozza",
                            Nome = "Giuseppe",
                            Cognome = "Bianchi",
                            Data = now,
                            Ora = new TimeSpan(8, 0, 0),
                            Email = "GiuseppeBianchi@test.it",
                            Matricola = "A12346",
                            Ruolo = new UserRoleDto { Id = 2, Nome = "Autorita Esecutrice PA" }
                          },
                    new CronologiaPermessoLavoro
                    {
                        Id = 1,
                        Azione = "Creato",
                        Nome = "Mario",
                        Cognome = "Rossi",
                        Data = now,
                        Ora = new TimeSpan(8, 0, 0),
                        Email = "MarioRossi@test.it",
                        Matricola = "A12345",
                        Ruolo = new UserRoleDto { Id = 2, Nome = "Autorita Esecutrice PA" }
                      },
                    new CronologiaPermessoLavoro
                        {
                            Id = 2,
                            Azione = "Salvato in bozza",
                            Nome = "Giuseppe",
                            Cognome = "Bianchi",
                            Data = now,
                            Ora = new TimeSpan(8, 0, 0),
                            Email = "GiuseppeBianchi@test.it",
                            Matricola = "A12346",
                            Ruolo = new UserRoleDto { Id = 2, Nome = "Autorita Esecutrice PA" }
                          },
                     new CronologiaPermessoLavoro
                    {
                        Id = 1,
                        Azione = "Creato",
                        Nome = "Mario",
                        Cognome = "Rossi",
                        Data = now,
                        Ora = new TimeSpan(8, 0, 0),
                        Email = "MarioRossi@test.it",
                        Matricola = "A12345",
                        Ruolo = new UserRoleDto { Id = 2, Nome = "Autorita Esecutrice PA" }
                      },
                    new CronologiaPermessoLavoro
                        {
                            Id = 2,
                            Azione = "Salvato in bozza",
                            Nome = "Giuseppe",
                            Cognome = "Bianchi",
                            Data = now,
                            Ora = new TimeSpan(8, 0, 0),
                            Email = "GiuseppeBianchi@test.it",
                            Matricola = "A12346",
                            Ruolo = new UserRoleDto { Id = 2, Nome = "Autorita Esecutrice PA" }
                          },
                    new CronologiaPermessoLavoro
                    {
                        Id = 1,
                        Azione = "Creato",
                        Nome = "Mario",
                        Cognome = "Rossi",
                        Data = now,
                        Ora = new TimeSpan(8, 0, 0),
                        Email = "MarioRossi@test.it",
                        Matricola = "A12345",
                        Ruolo = new UserRoleDto { Id = 2, Nome = "Autorita Esecutrice PA" }
                      },
                    new CronologiaPermessoLavoro
                        {
                            Id = 2,
                            Azione = "Salvato in bozza",
                            Nome = "Giuseppe",
                            Cognome = "Bianchi",
                            Data = now,
                            Ora = new TimeSpan(8, 0, 0),
                            Email = "GiuseppeBianchi@test.it",
                            Matricola = "A12346",
                            Ruolo = new UserRoleDto { Id = 2, Nome = "Autorita Esecutrice PA" }
                          },
                     new CronologiaPermessoLavoro
                    {
                        Id = 1,
                        Azione = "Creato",
                        Nome = "Mario",
                        Cognome = "Rossi",
                        Data = now,
                        Ora = new TimeSpan(8, 0, 0),
                        Email = "MarioRossi@test.it",
                        Matricola = "A12345",
                        Ruolo = new UserRoleDto { Id = 2, Nome = "Autorita Esecutrice PA" }
                      },
                    new CronologiaPermessoLavoro
                        {
                            Id = 2,
                            Azione = "Salvato in bozza",
                            Nome = "Giuseppe",
                            Cognome = "Bianchi",
                            Data = now,
                            Ora = new TimeSpan(8, 0, 0),
                            Email = "GiuseppeBianchi@test.it",
                            Matricola = "A12346",
                            Ruolo = new UserRoleDto { Id = 2, Nome = "Autorita Esecutrice PA" }
                          },
                    new CronologiaPermessoLavoro
                    {
                        Id = 1,
                        Azione = "Creato",
                        Nome = "Mario",
                        Cognome = "Rossi",
                        Data = now,
                        Ora = new TimeSpan(8, 0, 0),
                        Email = "MarioRossi@test.it",
                        Matricola = "A12345",
                        Ruolo = new UserRoleDto { Id = 2, Nome = "Autorita Esecutrice PA" }
                      },
                    new CronologiaPermessoLavoro
                        {
                            Id = 2,
                            Azione = "Salvato in bozza",
                            Nome = "Giuseppe",
                            Cognome = "Bianchi",
                            Data = now,
                            Ora = new TimeSpan(8, 0, 0),
                            Email = "GiuseppeBianchi@test.it",
                            Matricola = "A12346",
                            Ruolo = new UserRoleDto { Id = 2, Nome = "Autorita Esecutrice PA" }
                          },
                     new CronologiaPermessoLavoro
                    {
                        Id = 1,
                        Azione = "Creato",
                        Nome = "Mario",
                        Cognome = "Rossi",
                        Data = now,
                        Ora = new TimeSpan(8, 0, 0),
                        Email = "MarioRossi@test.it",
                        Matricola = "A12345",
                        Ruolo = new UserRoleDto { Id = 2, Nome = "Autorita Esecutrice PA" }
                      },
                    new CronologiaPermessoLavoro
                        {
                            Id = 2,
                            Azione = "Salvato in bozza",
                            Nome = "Giuseppe",
                            Cognome = "Bianchi",
                            Data = now,
                            Ora = new TimeSpan(8, 0, 0),
                            Email = "GiuseppeBianchi@test.it",
                            Matricola = "A12346",
                            Ruolo = new UserRoleDto { Id = 2, Nome = "Autorita Esecutrice PA" }
                          },
                    new CronologiaPermessoLavoro
                    {
                        Id = 1,
                        Azione = "Creato",
                        Nome = "Mario",
                        Cognome = "Rossi",
                        Data = now,
                        Ora = new TimeSpan(8, 0, 0),
                        Email = "MarioRossi@test.it",
                        Matricola = "A12345",
                        Ruolo = new UserRoleDto { Id = 2, Nome = "Autorita Esecutrice PA" }
                      },
                    new CronologiaPermessoLavoro
                        {
                            Id = 2,
                            Azione = "Salvato in bozza",
                            Nome = "Giuseppe",
                            Cognome = "Bianchi",
                            Data = now,
                            Ora = new TimeSpan(8, 0, 0),
                            Email = "GiuseppeBianchi@test.it",
                            Matricola = "A12346",
                            Ruolo = new UserRoleDto { Id = 2, Nome = "Autorita Esecutrice PA" }
                          },
                     new CronologiaPermessoLavoro
                    {
                        Id = 1,
                        Azione = "Creato",
                        Nome = "Mario",
                        Cognome = "Rossi",
                        Data = now,
                        Ora = new TimeSpan(8, 0, 0),
                        Email = "MarioRossi@test.it",
                        Matricola = "A12345",
                        Ruolo = new UserRoleDto { Id = 2, Nome = "Autorita Esecutrice PA" }
                      },
                    new CronologiaPermessoLavoro
                        {
                            Id = 2,
                            Azione = "Salvato in bozza",
                            Nome = "Giuseppe",
                            Cognome = "Bianchi",
                            Data = now,
                            Ora = new TimeSpan(8, 0, 0),
                            Email = "GiuseppeBianchi@test.it",
                            Matricola = "A12346",
                            Ruolo = new UserRoleDto { Id = 2, Nome = "Autorita Esecutrice PA" }
                          },
                    new CronologiaPermessoLavoro
                    {
                        Id = 1,
                        Azione = "Creato",
                        Nome = "Mario",
                        Cognome = "Rossi",
                        Data = now,
                        Ora = new TimeSpan(8, 0, 0),
                        Email = "MarioRossi@test.it",
                        Matricola = "A12345",
                        Ruolo = new UserRoleDto { Id = 2, Nome = "Autorita Esecutrice PA" }
                      },
                    new CronologiaPermessoLavoro
                        {
                            Id = 2,
                            Azione = "Salvato in bozza",
                            Nome = "Giuseppe",
                            Cognome = "Bianchi",
                            Data = now,
                            Ora = new TimeSpan(8, 0, 0),
                            Email = "GiuseppeBianchi@test.it",
                            Matricola = "A12346",
                            Ruolo = new UserRoleDto { Id = 2, Nome = "Autorita Esecutrice PA" }
                          },
                };
                return _ret;
            }
            catch
            {
                return null;
            }




        }

        // Data: 2025-10-21 - Sanifica identificativo PDL (whitelist alfanumerica e simboli consentiti)
        private static string SanitizeId(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;
            var span = input.Trim().AsSpan();
            Span<char> buffer = stackalloc char[Math.Min(64, span.Length)]; // prevenzione overflow
            var idx = 0;
            foreach (var ch in span)
            {
                if (char.IsLetterOrDigit(ch) || ch == '-' || ch == '_')
                {
                    if (idx < buffer.Length) buffer[idx++] = ch;
                    else break;
                }
            }
            return new string(buffer[..idx]);
        }
    }
}

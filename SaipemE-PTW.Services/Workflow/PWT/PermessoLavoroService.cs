using Microsoft.Extensions.Logging;
using SaipemE_PTW.Shared.Models.PWT; // Data: 2025-10-21 - Logging tramite ILogger<T> per compatibilità tra progetti

namespace SaipemE_PTW.Services.Workflow.PWT
{
    // Data: 2025-10-21 - Implementazione mock di microservizio Permessi di Lavoro
    // Note sicurezza: sanifica input, evita eccezioni non gestite, supporta cancellazione, logga con ILogger
    public sealed class PermessoLavoroService : IPermessoLavoroService
    {
        private readonly ILogger<PermessoLavoroService> _logger;
        private static readonly IReadOnlyList<PermessoLavoroModel> _seed = CreateSeed();
        private static readonly TimeSpan SimulatedLatency = TimeSpan.FromMilliseconds(500); // 500ms per UX realistica

        public PermessoLavoroService(ILogger<PermessoLavoroService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger)); // Data: 2025-10-21 - DI null-check
        }

        public async Task<IEnumerable<PermessoLavoroModel>> GetPermessiAsync(CancellationToken ct = default)
        {
            try
            {
                await Task.Delay(SimulatedLatency, ct); // Data: 2025-10-21 - Simula round-trip microservizio
                _logger.LogInformation("[PermessoLavoroService] Fetched list. Count={Count}", _seed.Count);
                // Data: 2025-10-21 - Restituisce copia immutabile per evitare side effects
                return _seed.ToList();
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("[PermessoLavoroService] Request cancelled");
                return Array.Empty<PermessoLavoroModel>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[PermessoLavoroService] Error fetching list");
                return Array.Empty<PermessoLavoroModel>();
            }
        }

        public async Task<PermessoLavoroModel?> GetPermessoByIdAsync(string pdl, CancellationToken ct = default)
        {
            var id = SanitizeId(pdl);
            if (string.IsNullOrWhiteSpace(id))
            {
                _logger.LogWarning("[PermessoLavoroService] Invalid PDL id");
                return null;
            }

            try
            {
                await Task.Delay(SimulatedLatency, ct);
                var result = _seed.FirstOrDefault(x => string.Equals(x.pdl, id, StringComparison.OrdinalIgnoreCase));
                _logger.LogInformation("[PermessoLavoroService] Fetched item. Id={Id}, Found={Found}", id, result is not null);
                return result;
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("[PermessoLavoroService] Request cancelled by token. Id={Id}", id);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[PermessoLavoroService] Error fetching item. Id={Id}", id);
                return null;
            }
        }

        // Data: 2025-10-21 - Seed dati fittizi coerenti con PermessoLavoroModel
        private static IReadOnlyList<PermessoLavoroModel> CreateSeed()
        {
            List<PermessoLavoroModel> _ret = new();
            try
            {
                var now = DateTime.UtcNow.Date;
                _ret = new List<PermessoLavoroModel>
            {

new PermessoLavoroModel
{
#region intestazione
id = 8,
pdl = "PDL-2025-0008",
Certificato = "00000008",
Stato = new TipoStatoPermessoLavoroDto{ Id=8,Nome="In Approvazione OA" },
Sito = "Ravenna",
DataInizioLavori = now,
Parte = new TipoParteDto{Id=3, Nome="Parte 3"},
DataFineLavori = DateTime.Now.AddDays(30),
Tipo = new TipoPDLDto{Id=1,Nome ="Permesso di Lavoro Generico (Caldo)"},
#endregion

                    
ValiditaDataInizio = now.AddDays(1),
ValiditaOraInizio = new TimeSpan(9, 0, 0),
ValiditaDataFine = now.AddDays(2),
ValiditaOraFine = new TimeSpan(17, 30, 0),
ValiditaAreaLavoro = "Unit B - Area 7",
ValiditaApparecchiatura = "EX-220",
AllegatiAltroDescrizione = "Disegni aggiornati",
DescrizioneAttivita = "Sostituzione tupo idraulico",
TipoLavoroAltroDescrizione = "Utilizzo piattaforma elevabile",
AutoritaRichiedenteNome = "Anna",
AutoritaRichiedenteCognome = "Verdi",
AutoritaRichiedenteImpresa = "Impresa Gamma",
AutoritaRichiedenteData = now,
AutoritaRichiedenteOra = new TimeSpan(10, 0, 0),
AutoritaRichiedenteFirma = "AV-Hash",
AutoritaEsecutriceNome = "Luigi",
AutoritaEsecutriceCognome = "Bianchi",
AutoritaEsecutriceImpresa = "Impresa Delta",
AutoritaEsecutriceData = now.AddDays(1),
AutoritaEsecutriceOra = new TimeSpan(9, 0, 0),
AutoritaEsecutriceSubAppaltatore = true,
AutoritaEsecutriceFornitore = false,
AutoritaEsecutriceFirma = "PN-Hash",
AttrezzaturaAltro = "Linea vita",
AttrezzaturaAntiscintilla = "N/A",
TipoAttrezzature = new List<string> { "Autogru", "Carrelli elevatori" },
TipoMisurePrecauzione = new List<string> { "Rilevazione gas in continuo (LEL, O2, H2S, etc.)", "Uso di materiale anti scintilla" },
TipoAllegati = new List<string> { "Scheda POS","SWC", "Altro..." },//GETTA
AttachmentType = new List<AttachmentTypeDto>
{
    new AttachmentTypeDto
    {
        Id = 1,
        Code = "Layout"
    },
    new AttachmentTypeDto
    {
        Id = 4,
        Code = "SWC"
    },
},
TipoCertificati = new List<string> { "Spazio confinato", "N.A." },
TipoAttivita = new List<string> { "Meccanica" },
TipoLavoro = new List<string> { "SollevaMenti" },
TipoAutoritaRichiedente = new List<string> { "SubAppaltatore" },
TipoAutoritaEsecutrice = new List<string> { "Fornitore" },
TipoDPI = new List<string> { "Maschera con filtri", "Visiera", "Guanti" },
ProdottiChimiciUtilizzati = string.Empty,
AltroProdotti = string.Empty,
Prescrizioni = "Area transennata",
AltriDPI = "Scarpe antinfortunistiche",
ProtezioneCollettiva = "Reti di protezione",

#region Rinnovo
DataRinnovoLavoro = now,
OraInizioRinnovo = new TimeSpan(8, 0, 0),
OraFineRinnovo = new TimeSpan(8, 0, 0),
FirmaAutoritaEsecutriceInizioRinnovoLavori = "Nome cognome esempio",
FirmaAutoritaEsecutriceFineRinnovoLavori = "Nome cognome esempio",
FirmaAutoritaOperativaInizioRinnovoLavori = "Nome cognome esempio",
FirmaAutoritaOperativaFineRinnovoLavori = "Nome cognome esempio",
ListaRinnovoLavoro = new List<ListaRinnovoLavoro>
{
new ListaRinnovoLavoro {
Autorizzante = "Nome cognome esempio",
DataFine = now,
DataInizio = now,
Modulo = "Modulo esempio",
Note = "Note esempio"
}
},
#endregion,

#region Sospensione
//DataRinnovoLavoro = now,
//OraInizioRinnovo = new TimeSpan(8, 0, 0),
//OraFineRinnovo = new TimeSpan(8, 0, 0),
//FirmaAutoritaEsecutriceInizioRinnovoLavori = "Nome cognome esempio",
//FirmaAutoritaEsecutriceFineRinnovoLavori = "Nome cognome esempio",
//FirmaAutoritaOperativaInizioRinnovoLavori = "Nome cognome esempio",
//FirmaAutoritaOperativaFineRinnovoLavori = "Nome cognome esempio",
ListaSospensione = new List<ListaSospensione>
{
new ListaSospensione {
    DataInizioSospensione = now,
   Motivo = "Esempio motivo",
   AutorizzanteSospensione = "Paolo Rossi",
   DataRiavvio = now,
   AutorizzanteRiavvio = "Mario Bianchi",
    Note = "Note esempio"
}
},
#endregion,


#region gas test
gasTest = new List<GasTest>
{
new GasTest
{
    DataTest = now,
    OraTest = new TimeSpan(8, 0, 0),
    tipoGas = new List<TipoGasDto>
    {
        new TipoGasDto{ Id = 1, Nome = "%Oxygen", Value="11"},
        new TipoGasDto{ Id = 2, Nome = "%LEL", Value="11"},
        new TipoGasDto{ Id = 3, Nome = "H2S(ppm)", Value="11"},
        new TipoGasDto{ Id = 4, Nome = "CE(ppm)", Value="11"}
    },

},
    new GasTest
{
    DataTest = now,
    OraTest = new TimeSpan(8, 0, 0),
    tipoGas = new List<TipoGasDto>
    {
        new TipoGasDto{ Id = 1, Nome = "%Oxygen", Value="o1"},
        new TipoGasDto{ Id = 2, Nome = "%LEL", Value="l2"},
        new TipoGasDto{ Id = 3, Nome = "H2S(ppm)", Value="h3"},
        new TipoGasDto{ Id = 4, Nome = "CE(ppm)", Value="c4"}
    },

}
},
#endregion
_timeline = new()
{
new() { Title = "In Lavorazione RA", Date = "02/10/2025", Active = "", Bozza = "" },
new() { Title = "In Lavorazione PA", Date = "05/10/2025", Active = "", Bozza = "" },
new() { Title = "In Lavorazione IA", Date = "13/10/2025", Active = "", Bozza = "" },
new() { Title = "In Lavorazione PTWC", Date = "20/10/2025", Active = "", Bozza = "" },
new() { Title = "In Lavorazione CSE", Date = "24/10/2025", Active = "", Bozza = "" },
new() { Title = "In Lavorazione AGT", Date = "28/10/2025", Active = "", Bozza = "" },
new() { Title = "In Approvazione PA", Date = "03/11/2025", Active = "", Bozza = "" },
new() { Title = "In Approvazione OA", Date = "23/11/2025", Active = "active-step", Bozza = "Bozza" },

}
},

new PermessoLavoroModel
{
#region intestazione
id = 9,
pdl = "PDL-2025-0009",
Certificato = "00000009",
Stato = new TipoStatoPermessoLavoroDto{ Id=8,Nome="Autorizzato" },
Sito = "Ravenna",
DataInizioLavori = now,
Parte = new TipoParteDto{Id=3, Nome="Parte 3"},
DataFineLavori = DateTime.Now.AddDays(30),
Tipo = new TipoPDLDto{Id=9,Nome ="Permesso di Lavoro Generico (Caldo)"},
#endregion

                   
ValiditaDataInizio = now.AddDays(1),
ValiditaOraInizio = new TimeSpan(9, 0, 0),
ValiditaDataFine = now.AddDays(2),
ValiditaOraFine = new TimeSpan(17, 30, 0),
ValiditaAreaLavoro = "Unit B - Area 7",
ValiditaApparecchiatura = "EX-220",
AllegatiAltroDescrizione = "Disegni aggiornati",
DescrizioneAttivita = "Riparazione scale mobili",
TipoLavoroAltroDescrizione = "Utilizzo piattaforma elevabile",
AutoritaRichiedenteNome = "Anna",
AutoritaRichiedenteCognome = "Verdi",
AutoritaRichiedenteImpresa = "Impresa Gamma",
AutoritaRichiedenteData = now,
AutoritaRichiedenteOra = new TimeSpan(10, 0, 0),
AutoritaRichiedenteFirma = "AV-Hash",
AutoritaEsecutriceNome = "Luigi",
AutoritaEsecutriceCognome = "Bianchi",
AutoritaEsecutriceImpresa = "Impresa Delta",
AutoritaEsecutriceData = now.AddDays(1),
AutoritaEsecutriceOra = new TimeSpan(9, 0, 0),
AutoritaEsecutriceSubAppaltatore = true,
AutoritaEsecutriceFornitore = false,
AutoritaEsecutriceFirma = "PN-Hash",
AttrezzaturaAltro = "Linea vita",
AttrezzaturaAntiscintilla = "N/A",
TipoAttrezzature = new List<string> { "Autogru", "Carrelli elevatori" },
TipoMisurePrecauzione = new List<string> { "Rilevazione gas in continuo (LEL, O2, H2S, etc.)", "Uso di materiale anti scintilla" },
TipoAllegati = new List<string> { "Scheda POS", "Altro..." },
TipoCertificati = new List<string> { "Spazio confinato", "N.A." },
TipoAttivita = new List<string> { "Meccanica" },
TipoLavoro = new List<string> { "SollevaMenti" },
TipoAutoritaRichiedente = new List<string> { "SubAppaltatore" },
TipoAutoritaEsecutrice = new List<string> { "Fornitore" },
TipoDPI = new List<string> { "Maschera con filtri", "Visiera", "Guanti" },
ProdottiChimiciUtilizzati = string.Empty,
AltroProdotti = string.Empty,
Prescrizioni = "Area transennata",
AltriDPI = "Scarpe antinfortunistiche",
ProtezioneCollettiva = "Reti di protezione",

#region Rinnovo
DataRinnovoLavoro = now,
OraInizioRinnovo = new TimeSpan(8, 0, 0),
OraFineRinnovo = new TimeSpan(8, 0, 0),
FirmaAutoritaEsecutriceInizioRinnovoLavori = "Nome cognome esempio",
FirmaAutoritaEsecutriceFineRinnovoLavori = "Nome cognome esempio",
FirmaAutoritaOperativaInizioRinnovoLavori = "Nome cognome esempio",
FirmaAutoritaOperativaFineRinnovoLavori = "Nome cognome esempio",
ListaRinnovoLavoro = new List<ListaRinnovoLavoro>
{
new ListaRinnovoLavoro {
Autorizzante = "Nome cognome esempio",
DataFine = now,
DataInizio = now,
Modulo = "Modulo esempio",
Note = "Note esempio"
}
},
#endregion
_timeline = new()
{
    new() { Title = "In Lavorazione RA", Date = "02/10/2025", Active = "", Bozza = "" },
new() { Title = "In Lavorazione PA", Date = "05/10/2025", Active = "", Bozza = "" },
new() { Title = "In Lavorazione IA", Date = "13/10/2025", Active = "", Bozza = "" },
new() { Title = "In Lavorazione PTWC", Date = "20/10/2025", Active = "", Bozza = "" },
new() { Title = "In Lavorazione CSE", Date = "24/10/2025", Active = "", Bozza = "" },
new() { Title = "In Lavorazione AGT", Date = "28/10/2025", Active = "", Bozza = "" },
new() { Title = "In Approvazione PA", Date = "03/11/2025", Active = "", Bozza = "" },
new() { Title = "In Approvazione OA", Date = "23/11/2025", Active = "", Bozza = "" },
new() { Title = "Approvato", Date = "23/11/2025", Active = "active-step", Bozza = "" },
}
},
new PermessoLavoroModel
{
// Data: 2025-10-21 - Mock record 2
#region intestazione
id = 10,
pdl = "PDL-2025-0009",
Certificato = "00000009",
Stato = new TipoStatoPermessoLavoroDto{ Id=8,Nome="Scaduto" },
Sito = "Ravenna",
DataInizioLavori = now,
Parte = new TipoParteDto{Id=3, Nome="Parte 3"},
DataFineLavori = DateTime.Now.AddDays(30),
Tipo = new TipoPDLDto{Id=9,Nome ="Permesso di Lavoro Generico (Caldo)"},
#endregion

                  
ValiditaDataInizio = now.AddDays(1),
ValiditaOraInizio = new TimeSpan(9, 0, 0),
ValiditaDataFine = now.AddDays(2),
ValiditaOraFine = new TimeSpan(17, 30, 0),
ValiditaAreaLavoro = "Unit B - Area 7",
ValiditaApparecchiatura = "EX-220",
AllegatiAltroDescrizione = "Disegni aggiornati",
DescrizioneAttivita = "Sostituzione chiavi porta",
TipoLavoroAltroDescrizione = "Utilizzo piattaforma elevabile",
AutoritaRichiedenteNome = "Anna",
AutoritaRichiedenteCognome = "Verdi",
AutoritaRichiedenteImpresa = "Impresa Gamma",
AutoritaRichiedenteData = now,
AutoritaRichiedenteOra = new TimeSpan(10, 0, 0),
AutoritaRichiedenteFirma = "AV-Hash",
AutoritaEsecutriceNome = "Luigi",
AutoritaEsecutriceCognome = "Bianchi",
AutoritaEsecutriceImpresa = "Impresa Delta",
AutoritaEsecutriceData = now.AddDays(1),
AutoritaEsecutriceOra = new TimeSpan(9, 0, 0),
AutoritaEsecutriceSubAppaltatore = true,
AutoritaEsecutriceFornitore = false,
AutoritaEsecutriceFirma = "PN-Hash",
AttrezzaturaAltro = "Linea vita",
AttrezzaturaAntiscintilla = "N/A",
TipoAttrezzature = new List<string> { "Autogru", "Carrelli elevatori" },
TipoMisurePrecauzione = new List<string> { "Rilevazione gas in continuo (LEL, O2, H2S, etc.)", "Uso di materiale anti scintilla" },
TipoAllegati = new List<string> { "Scheda POS", "Altro..." },
TipoCertificati = new List<string> { "Spazio confinato", "N.A." },
TipoAttivita = new List<string> { "Meccanica" },
TipoLavoro = new List<string> { "SollevaMenti" },
TipoAutoritaRichiedente = new List<string> { "SubAppaltatore" },
TipoAutoritaEsecutrice = new List<string> { "Fornitore" },
TipoDPI = new List<string> { "Maschera con filtri", "Visiera", "Guanti" },
ProdottiChimiciUtilizzati = string.Empty,
AltroProdotti = string.Empty,
Prescrizioni = "Area transennata",
AltriDPI = "Scarpe antinfortunistiche",
ProtezioneCollettiva = "Reti di protezione",

#region Rinnovo
DataRinnovoLavoro = now,
OraInizioRinnovo = new TimeSpan(8, 0, 0),
OraFineRinnovo = new TimeSpan(8, 0, 0),
FirmaAutoritaEsecutriceInizioRinnovoLavori = "Nome cognome esempio",
FirmaAutoritaEsecutriceFineRinnovoLavori = "Nome cognome esempio",
FirmaAutoritaOperativaInizioRinnovoLavori = "Nome cognome esempio",
FirmaAutoritaOperativaFineRinnovoLavori = "Nome cognome esempio",
ListaRinnovoLavoro = new List<ListaRinnovoLavoro>
{
new ListaRinnovoLavoro {
Autorizzante = "Nome cognome esempio",
DataFine = now,
DataInizio = now,
Modulo = "Modulo esempio",
Note = "Note esempio"
}
},
#endregion
_timeline = new()
{
    new() { Title = "In Lavorazione RA", Date = "02/10/2025", Active = "", Bozza = "" },
new() { Title = "In Lavorazione PA", Date = "05/10/2025", Active = "", Bozza = "" },
new() { Title = "In Lavorazione IA", Date = "13/10/2025", Active = "", Bozza = "" },
new() { Title = "In Lavorazione PTWC", Date = "20/10/2025", Active = "", Bozza = "" },
new() { Title = "In Lavorazione CSE", Date = "24/10/2025", Active = "", Bozza = "" },
new() { Title = "In Lavorazione AGT", Date = "28/10/2025", Active = "", Bozza = "" },
new() { Title = "In Approvazione PA", Date = "03/11/2025", Active = "", Bozza = "" },
new() { Title = "In Approvazione OA", Date = "23/11/2025", Active = "", Bozza = "" },
new() { Title = "Scaduto", Date = "23/12/2025", Active = "active-step", Bozza = "" },
}
},

new PermessoLavoroModel
{
// Data: 2025-10-21 - Mock record 2
#region intestazione
id = 11,
pdl = "PDL-2025-0009",
Certificato = "00000009",
Stato = new TipoStatoPermessoLavoroDto{ Id=8,Nome="Sospeso" },
Sito = "Ravenna",
DataInizioLavori = now,
Parte = new TipoParteDto{Id=3, Nome="Parte 3"},
DataFineLavori = DateTime.Now.AddDays(30),
Tipo = new TipoPDLDto{Id=9,Nome ="Permesso di Lavoro Generico (Caldo)"},
#endregion

                   
ValiditaDataInizio = now.AddDays(1),
ValiditaOraInizio = new TimeSpan(9, 0, 0),
ValiditaDataFine = now.AddDays(2),
ValiditaOraFine = new TimeSpan(17, 30, 0),
ValiditaAreaLavoro = "Unit B - Area 7",
ValiditaApparecchiatura = "EX-220",
AllegatiAltroDescrizione = "Disegni aggiornati",
DescrizioneAttivita = "Sostituzione passerella",
TipoLavoroAltroDescrizione = "Utilizzo piattaforma elevabile",
AutoritaRichiedenteNome = "Anna",
AutoritaRichiedenteCognome = "Verdi",
AutoritaRichiedenteImpresa = "Impresa Gamma",
AutoritaRichiedenteData = now,
AutoritaRichiedenteOra = new TimeSpan(10, 0, 0),
AutoritaRichiedenteFirma = "AV-Hash",
AutoritaEsecutriceNome = "Luigi",
AutoritaEsecutriceCognome = "Bianchi",
AutoritaEsecutriceImpresa = "Impresa Delta",
AutoritaEsecutriceData = now.AddDays(1),
AutoritaEsecutriceOra = new TimeSpan(9, 0, 0),
AutoritaEsecutriceSubAppaltatore = true,
AutoritaEsecutriceFornitore = false,
AutoritaEsecutriceFirma = "PN-Hash",
AttrezzaturaAltro = "Linea vita",
AttrezzaturaAntiscintilla = "N/A",
TipoAttrezzature = new List<string> { "Autogru", "Carrelli elevatori" },
TipoMisurePrecauzione = new List<string> { "Rilevazione gas in continuo (LEL, O2, H2S, etc.)", "Uso di materiale anti scintilla" },
TipoAllegati = new List<string> { "Scheda POS", "Altro..." },
TipoCertificati = new List<string> { "Spazio confinato", "N.A." },
TipoAttivita = new List<string> { "Meccanica" },
TipoLavoro = new List<string> { "SollevaMenti" },
TipoAutoritaRichiedente = new List<string> { "SubAppaltatore" },
TipoAutoritaEsecutrice = new List<string> { "Fornitore" },
TipoDPI = new List<string> { "Maschera con filtri", "Visiera", "Guanti" },
ProdottiChimiciUtilizzati = string.Empty,
AltroProdotti = string.Empty,
Prescrizioni = "Area transennata",
AltriDPI = "Scarpe antinfortunistiche",
ProtezioneCollettiva = "Reti di protezione",

#region Rinnovo
DataRinnovoLavoro = now,
OraInizioRinnovo = new TimeSpan(8, 0, 0),
OraFineRinnovo = new TimeSpan(8, 0, 0),
FirmaAutoritaEsecutriceInizioRinnovoLavori = "Nome cognome esempio",
FirmaAutoritaEsecutriceFineRinnovoLavori = "Nome cognome esempio",
FirmaAutoritaOperativaInizioRinnovoLavori = "Nome cognome esempio",
FirmaAutoritaOperativaFineRinnovoLavori = "Nome cognome esempio",
ListaRinnovoLavoro = new List<ListaRinnovoLavoro>
{
new ListaRinnovoLavoro {
Autorizzante = "Nome cognome esempio",
DataFine = now,
DataInizio = now,
Modulo = "Modulo esempio",
Note = "Note esempio"
}
},
#endregion
_timeline = new()
{
    new() { Title = "In Lavorazione RA", Date = "02/10/2025", Active = "", Bozza = "" },
new() { Title = "In Lavorazione PA", Date = "05/10/2025", Active = "", Bozza = "" },
new() { Title = "In Lavorazione IA", Date = "13/10/2025", Active = "", Bozza = "" },
new() { Title = "In Lavorazione PTWC", Date = "20/10/2025", Active = "", Bozza = "" },
new() { Title = "In Lavorazione CSE", Date = "24/10/2025", Active = "", Bozza = "" },
new() { Title = "In Lavorazione AGT", Date = "28/10/2025", Active = "", Bozza = "" },
new() { Title = "In Approvazione PA", Date = "03/11/2025", Active = "", Bozza = "" },
new() { Title = "In Approvazione OA", Date = "23/11/2025", Active = "", Bozza = "" },
new() { Title = "Sospeso", Date = "23/12/2025", Active = "active-step", Bozza = "" },
}
},
new PermessoLavoroModel
{
// Data: 2025-10-21 - Mock record 2
#region intestazione
id = 12,
pdl = "PDL-2025-0009",
Certificato = "00000009",
Stato = new TipoStatoPermessoLavoroDto{ Id=8,Nome="Rinnovato" },
Sito = "Ravenna",
DataInizioLavori = now,
Parte = new TipoParteDto{Id=3, Nome="Parte 3"},
DataFineLavori = DateTime.Now.AddDays(30),
Tipo = new TipoPDLDto{Id=9,Nome ="Permesso di Lavoro Generico (Caldo)"},
#endregion

                    
ValiditaDataInizio = now.AddDays(1),
ValiditaOraInizio = new TimeSpan(9, 0, 0),
ValiditaDataFine = now.AddDays(2),
ValiditaOraFine = new TimeSpan(17, 30, 0),
ValiditaAreaLavoro = "Unit B - Area 7",
ValiditaApparecchiatura = "EX-220",
AllegatiAltroDescrizione = "Disegni aggiornati",
DescrizioneAttivita = "Sostituzione passerella",
TipoLavoroAltroDescrizione = "Utilizzo piattaforma elevabile",
AutoritaRichiedenteNome = "Anna",
AutoritaRichiedenteCognome = "Verdi",
AutoritaRichiedenteImpresa = "Impresa Gamma",
AutoritaRichiedenteData = now,
AutoritaRichiedenteOra = new TimeSpan(10, 0, 0),
AutoritaRichiedenteFirma = "AV-Hash",
AutoritaEsecutriceNome = "Luigi",
AutoritaEsecutriceCognome = "Bianchi",
AutoritaEsecutriceImpresa = "Impresa Delta",
AutoritaEsecutriceData = now.AddDays(1),
AutoritaEsecutriceOra = new TimeSpan(9, 0, 0),
AutoritaEsecutriceSubAppaltatore = true,
AutoritaEsecutriceFornitore = false,
AutoritaEsecutriceFirma = "PN-Hash",
AttrezzaturaAltro = "Linea vita",
AttrezzaturaAntiscintilla = "N/A",
TipoAttrezzature = new List<string> { "Autogru", "Carrelli elevatori" },
TipoMisurePrecauzione = new List<string> { "Rilevazione gas in continuo (LEL, O2, H2S, etc.)", "Uso di materiale anti scintilla" },
TipoAllegati = new List<string> { "Scheda POS", "Altro..." },
TipoCertificati = new List<string> { "Spazio confinato", "N.A." },
TipoAttivita = new List<string> { "Meccanica" },
TipoLavoro = new List<string> { "SollevaMenti" },
TipoAutoritaRichiedente = new List<string> { "SubAppaltatore" },
TipoAutoritaEsecutrice = new List<string> { "Fornitore" },
TipoDPI = new List<string> { "Maschera con filtri", "Visiera", "Guanti" },
ProdottiChimiciUtilizzati = string.Empty,
AltroProdotti = string.Empty,
Prescrizioni = "Area transennata",
AltriDPI = "Scarpe antinfortunistiche",
ProtezioneCollettiva = "Reti di protezione",

#region Rinnovo
DataRinnovoLavoro = now,
OraInizioRinnovo = new TimeSpan(8, 0, 0),
OraFineRinnovo = new TimeSpan(8, 0, 0),
FirmaAutoritaEsecutriceInizioRinnovoLavori = "Nome cognome esempio",
FirmaAutoritaEsecutriceFineRinnovoLavori = "Nome cognome esempio",
FirmaAutoritaOperativaInizioRinnovoLavori = "Nome cognome esempio",
FirmaAutoritaOperativaFineRinnovoLavori = "Nome cognome esempio",
ListaRinnovoLavoro = new List<ListaRinnovoLavoro>
{
new ListaRinnovoLavoro {
Autorizzante = "Nome cognome esempio",
DataFine = now,
DataInizio = now,
Modulo = "Modulo esempio",
Note = "Note esempio"
}
},
#endregion
_timeline = new()
{
    new() { Title = "In Lavorazione RA", Date = "02/10/2025", Active = "", Bozza = "" },
new() { Title = "In Lavorazione PA", Date = "05/10/2025", Active = "", Bozza = "" },
new() { Title = "In Lavorazione IA", Date = "13/10/2025", Active = "", Bozza = "" },
new() { Title = "In Lavorazione PTWC", Date = "20/10/2025", Active = "", Bozza = "" },
new() { Title = "In Lavorazione CSE", Date = "24/10/2025", Active = "", Bozza = "" },
new() { Title = "In Lavorazione AGT", Date = "28/10/2025", Active = "", Bozza = "" },
new() { Title = "In Approvazione PA", Date = "03/11/2025", Active = "", Bozza = "" },
new() { Title = "In Approvazione OA", Date = "23/11/2025", Active = "", Bozza = "" },
new() { Title = "Rinnovato", Date = "23/12/2025", Active = "active-step", Bozza = "" },
}
},
new PermessoLavoroModel
{
// Data: 2025-10-21 - Mock record 2
#region intestazione
id = 13,
pdl = "PDL-2025-0009",
Certificato = "00000009",
Stato = new TipoStatoPermessoLavoroDto{ Id=8,Nome="Riattivato" },
Sito = "Ravenna",
DataInizioLavori = now,
Parte = new TipoParteDto{Id=3, Nome="Parte 3"},
DataFineLavori = DateTime.Now.AddDays(30),
Tipo = new TipoPDLDto{Id=9,Nome ="Permesso di Lavoro Generico (Caldo)"},
#endregion


ValiditaDataInizio = now.AddDays(1),
ValiditaOraInizio = new TimeSpan(9, 0, 0),
ValiditaDataFine = now.AddDays(2),
ValiditaOraFine = new TimeSpan(17, 30, 0),
ValiditaAreaLavoro = "Unit B - Area 7",
ValiditaApparecchiatura = "EX-220",
AllegatiAltroDescrizione = "Disegni aggiornati",
DescrizioneAttivita = "Sostituzione passerella",
TipoLavoroAltroDescrizione = "Utilizzo piattaforma elevabile",
AutoritaRichiedenteNome = "Anna",
AutoritaRichiedenteCognome = "Verdi",
AutoritaRichiedenteImpresa = "Impresa Gamma",
AutoritaRichiedenteData = now,
AutoritaRichiedenteOra = new TimeSpan(10, 0, 0),
AutoritaRichiedenteFirma = "AV-Hash",
AutoritaEsecutriceNome = "Luigi",
AutoritaEsecutriceCognome = "Bianchi",
AutoritaEsecutriceImpresa = "Impresa Delta",
AutoritaEsecutriceData = now.AddDays(1),
AutoritaEsecutriceOra = new TimeSpan(9, 0, 0),
AutoritaEsecutriceSubAppaltatore = true,
AutoritaEsecutriceFornitore = false,
AutoritaEsecutriceFirma = "PN-Hash",
AttrezzaturaAltro = "Linea vita",
AttrezzaturaAntiscintilla = "N/A",
TipoAttrezzature = new List<string> { "Autogru", "Carrelli elevatori" },
TipoMisurePrecauzione = new List<string> { "Rilevazione gas in continuo (LEL, O2, H2S, etc.)", "Uso di materiale anti scintilla" },
TipoAllegati = new List<string> { "Scheda POS", "Altro..." },
TipoCertificati = new List<string> { "Spazio confinato", "N.A." },
TipoAttivita = new List<string> { "Meccanica" },
TipoLavoro = new List<string> { "SollevaMenti" },
TipoAutoritaRichiedente = new List<string> { "SubAppaltatore" },
TipoAutoritaEsecutrice = new List<string> { "Fornitore" },
TipoDPI = new List<string> { "Maschera con filtri", "Visiera", "Guanti" },
ProdottiChimiciUtilizzati = string.Empty,
AltroProdotti = string.Empty,
Prescrizioni = "Area transennata",
AltriDPI = "Scarpe antinfortunistiche",
ProtezioneCollettiva = "Reti di protezione",

#region Rinnovo
DataRinnovoLavoro = now,
OraInizioRinnovo = new TimeSpan(8, 0, 0),
OraFineRinnovo = new TimeSpan(8, 0, 0),
FirmaAutoritaEsecutriceInizioRinnovoLavori = "Nome cognome esempio",
FirmaAutoritaEsecutriceFineRinnovoLavori = "Nome cognome esempio",
FirmaAutoritaOperativaInizioRinnovoLavori = "Nome cognome esempio",
FirmaAutoritaOperativaFineRinnovoLavori = "Nome cognome esempio",
ListaRinnovoLavoro = new List<ListaRinnovoLavoro>
{
new ListaRinnovoLavoro {
Autorizzante = "Nome cognome esempio",
DataFine = now,
DataInizio = now,
Modulo = "Modulo esempio",
Note = "Note esempio"
}
},
#endregion
_timeline = new()
{
    new() { Title = "In Lavorazione RA", Date = "02/10/2025", Active = "", Bozza = "" },
new() { Title = "In Lavorazione PA", Date = "05/10/2025", Active = "", Bozza = "" },
new() { Title = "In Lavorazione IA", Date = "13/10/2025", Active = "", Bozza = "" },
new() { Title = "In Lavorazione PTWC", Date = "20/10/2025", Active = "", Bozza = "" },
new() { Title = "In Lavorazione CSE", Date = "24/10/2025", Active = "", Bozza = "" },
new() { Title = "In Lavorazione AGT", Date = "28/10/2025", Active = "", Bozza = "" },
new() { Title = "In Approvazione PA", Date = "03/11/2025", Active = "", Bozza = "" },
new() { Title = "In Approvazione OA", Date = "23/11/2025", Active = "", Bozza = "" },
new() { Title = "Riattivato", Date = "23/12/2025", Active = "active-step", Bozza = "" },
}
},
    new PermessoLavoroModel
{
// Data: 2025-10-21 - Mock record 2
#region intestazione
id = 14,
pdl = "PDL-2025-0009",
Certificato = "00000009",
Stato = new TipoStatoPermessoLavoroDto{ Id=8,Nome="Chiuso" },
Sito = "Ravenna",
DataInizioLavori = now,
Parte = new TipoParteDto{Id=3, Nome="Parte 3"},
DataFineLavori = DateTime.Now.AddDays(30),
Tipo = new TipoPDLDto{Id=9,Nome ="Permesso di Lavoro Generico (Caldo)"},
#endregion

                  
ValiditaDataInizio = now.AddDays(1),
ValiditaOraInizio = new TimeSpan(9, 0, 0),
ValiditaDataFine = now.AddDays(2),
ValiditaOraFine = new TimeSpan(17, 30, 0),
ValiditaAreaLavoro = "Unit B - Area 7",
ValiditaApparecchiatura = "EX-220",
AllegatiAltroDescrizione = "Disegni aggiornati",
DescrizioneAttivita = "Sostituzione passerella",
TipoLavoroAltroDescrizione = "Utilizzo piattaforma elevabile",
AutoritaRichiedenteNome = "Anna",
AutoritaRichiedenteCognome = "Verdi",
AutoritaRichiedenteImpresa = "Impresa Gamma",
AutoritaRichiedenteData = now,
AutoritaRichiedenteOra = new TimeSpan(10, 0, 0),
AutoritaRichiedenteFirma = "AV-Hash",
AutoritaEsecutriceNome = "Luigi",
AutoritaEsecutriceCognome = "Bianchi",
AutoritaEsecutriceImpresa = "Impresa Delta",
AutoritaEsecutriceData = now.AddDays(1),
AutoritaEsecutriceOra = new TimeSpan(9, 0, 0),
AutoritaEsecutriceSubAppaltatore = true,
AutoritaEsecutriceFornitore = false,
AutoritaEsecutriceFirma = "PN-Hash",
AttrezzaturaAltro = "Linea vita",
AttrezzaturaAntiscintilla = "N/A",
TipoAttrezzature = new List<string> { "Autogru", "Carrelli elevatori" },
TipoMisurePrecauzione = new List<string> { "Rilevazione gas in continuo (LEL, O2, H2S, etc.)", "Uso di materiale anti scintilla" },
TipoAllegati = new List<string> { "Scheda POS", "Altro..." },
TipoCertificati = new List<string> { "Spazio confinato", "N.A." },
TipoAttivita = new List<string> { "Meccanica" },
TipoLavoro = new List<string> { "SollevaMenti" },
TipoAutoritaRichiedente = new List<string> { "SubAppaltatore" },
TipoAutoritaEsecutrice = new List<string> { "Fornitore" },
TipoDPI = new List<string> { "Maschera con filtri", "Visiera", "Guanti" },
ProdottiChimiciUtilizzati = string.Empty,
AltroProdotti = string.Empty,
Prescrizioni = "Area transennata",
AltriDPI = "Scarpe antinfortunistiche",
ProtezioneCollettiva = "Reti di protezione",

#region Rinnovo
DataRinnovoLavoro = now,
OraInizioRinnovo = new TimeSpan(8, 0, 0),
OraFineRinnovo = new TimeSpan(8, 0, 0),
FirmaAutoritaEsecutriceInizioRinnovoLavori = "Nome cognome esempio",
FirmaAutoritaEsecutriceFineRinnovoLavori = "Nome cognome esempio",
FirmaAutoritaOperativaInizioRinnovoLavori = "Nome cognome esempio",
FirmaAutoritaOperativaFineRinnovoLavori = "Nome cognome esempio",
ListaRinnovoLavoro = new List<ListaRinnovoLavoro>
{
new ListaRinnovoLavoro {
Autorizzante = "Nome cognome esempio",
DataFine = now,
DataInizio = now,
Modulo = "Modulo esempio",
Note = "Note esempio"
}
},
#endregion
_timeline = new()
{
    new() { Title = "In Lavorazione RA", Date = "02/10/2025", Active = "", Bozza = "" },
new() { Title = "In Lavorazione PA", Date = "05/10/2025", Active = "", Bozza = "" },
new() { Title = "In Lavorazione IA", Date = "13/10/2025", Active = "", Bozza = "" },
new() { Title = "In Lavorazione PTWC", Date = "20/10/2025", Active = "", Bozza = "" },
new() { Title = "In Lavorazione CSE", Date = "24/10/2025", Active = "", Bozza = "" },
new() { Title = "In Lavorazione AGT", Date = "28/10/2025", Active = "", Bozza = "" },
new() { Title = "In Approvazione PA", Date = "03/11/2025", Active = "", Bozza = "" },
new() { Title = "In Approvazione OA", Date = "23/11/2025", Active = "", Bozza = "" },
new() { Title = "Approvato", Date = "23/12/2025", Active = "", Bozza = "" },
new() { Title = "Chiuso", Date = "30/12/2025", Active = "active-step", Bozza = "" },
}
},
    new PermessoLavoroModel
{
#region intestazione
id = 15,
pdl = "PDL-2025-0009",
Certificato = "00000009",
Stato = new TipoStatoPermessoLavoroDto{ Id=8,Nome="Rigettato PA" },
Sito = "Ravenna",
DataInizioLavori = now,
Parte = new TipoParteDto{Id=3, Nome="Parte 3"},
DataFineLavori = DateTime.Now.AddDays(30),
Tipo = new TipoPDLDto{Id=9,Nome ="Permesso di Lavoro Generico (Caldo)"},
#endregion

                  
ValiditaDataInizio = now.AddDays(1),
ValiditaOraInizio = new TimeSpan(9, 0, 0),
ValiditaDataFine = now.AddDays(2),
ValiditaOraFine = new TimeSpan(17, 30, 0),
ValiditaAreaLavoro = "Unit B - Area 7",
ValiditaApparecchiatura = "EX-220",
AllegatiAltroDescrizione = "Disegni aggiornati",
DescrizioneAttivita = "Sostituzione passerella",
TipoLavoroAltroDescrizione = "Utilizzo piattaforma elevabile",
AutoritaRichiedenteNome = "Anna",
AutoritaRichiedenteCognome = "Verdi",
AutoritaRichiedenteImpresa = "Impresa Gamma",
AutoritaRichiedenteData = now,
AutoritaRichiedenteOra = new TimeSpan(10, 0, 0),
AutoritaRichiedenteFirma = "AV-Hash",
AutoritaEsecutriceNome = "Luigi",
AutoritaEsecutriceCognome = "Bianchi",
AutoritaEsecutriceImpresa = "Impresa Delta",
AutoritaEsecutriceData = now.AddDays(1),
AutoritaEsecutriceOra = new TimeSpan(9, 0, 0),
AutoritaEsecutriceSubAppaltatore = true,
AutoritaEsecutriceFornitore = false,
AutoritaEsecutriceFirma = "PN-Hash",
AttrezzaturaAltro = "Linea vita",
AttrezzaturaAntiscintilla = "N/A",
TipoAttrezzature = new List<string> { "Autogru", "Carrelli elevatori" },
TipoMisurePrecauzione = new List<string> { "Rilevazione gas in continuo (LEL, O2, H2S, etc.)", "Uso di materiale anti scintilla" },
TipoAllegati = new List<string> { "Scheda POS", "Altro..." },
TipoCertificati = new List<string> { "Spazio confinato", "N.A." },
TipoAttivita = new List<string> { "Meccanica" },
TipoLavoro = new List<string> { "SollevaMenti" },
TipoAutoritaRichiedente = new List<string> { "SubAppaltatore" },
TipoAutoritaEsecutrice = new List<string> { "Fornitore" },
TipoDPI = new List<string> { "Maschera con filtri", "Visiera", "Guanti" },
ProdottiChimiciUtilizzati = string.Empty,
AltroProdotti = string.Empty,
Prescrizioni = "Area transennata",
AltriDPI = "Scarpe antinfortunistiche",
ProtezioneCollettiva = "Reti di protezione",

#region Rinnovo
DataRinnovoLavoro = now,
OraInizioRinnovo = new TimeSpan(8, 0, 0),
OraFineRinnovo = new TimeSpan(8, 0, 0),
FirmaAutoritaEsecutriceInizioRinnovoLavori = "Nome cognome esempio",
FirmaAutoritaEsecutriceFineRinnovoLavori = "Nome cognome esempio",
FirmaAutoritaOperativaInizioRinnovoLavori = "Nome cognome esempio",
FirmaAutoritaOperativaFineRinnovoLavori = "Nome cognome esempio",
ListaRinnovoLavoro = new List<ListaRinnovoLavoro>
{
new ListaRinnovoLavoro {
Autorizzante = "Nome cognome esempio",
DataFine = now,
DataInizio = now,
Modulo = "Modulo esempio",
Note = "Note esempio"
}
},
#endregion
_timeline = new()
{
    new() { Title = "In Lavorazione RA", Date = "02/10/2025", Active = "", Bozza = "" },
new() { Title = "Rigettato PA", Date = "05/10/2025", Active = "active-step", Bozza = "" },

}
},
            new PermessoLavoroModel
{
// Data: 2025-10-21 - Mock record 2
#region intestazione
id = 16,
pdl = "PDL-2025-0009",
Certificato = "00000009",
Stato = new TipoStatoPermessoLavoroDto{ Id=8,Nome="Rigettato PTWC" },
Sito = "Ravenna",
DataInizioLavori = now,
Parte = new TipoParteDto{Id=3, Nome="Parte 3"},
DataFineLavori = DateTime.Now.AddDays(30),
Tipo = new TipoPDLDto{Id=9,Nome ="Permesso di Lavoro Generico (Caldo)"},
#endregion


                 
ValiditaDataInizio = now.AddDays(1),
ValiditaOraInizio = new TimeSpan(9, 0, 0),
ValiditaDataFine = now.AddDays(2),
ValiditaOraFine = new TimeSpan(17, 30, 0),
ValiditaAreaLavoro = "Unit B - Area 7",
ValiditaApparecchiatura = "EX-220",
AllegatiAltroDescrizione = "Disegni aggiornati",
DescrizioneAttivita = "Sostituzione passerella",
TipoLavoroAltroDescrizione = "Utilizzo piattaforma elevabile",
AutoritaRichiedenteNome = "Anna",
AutoritaRichiedenteCognome = "Verdi",
AutoritaRichiedenteImpresa = "Impresa Gamma",
AutoritaRichiedenteData = now,
AutoritaRichiedenteOra = new TimeSpan(10, 0, 0),
AutoritaRichiedenteFirma = "AV-Hash",
AutoritaEsecutriceNome = "Luigi",
AutoritaEsecutriceCognome = "Bianchi",
AutoritaEsecutriceImpresa = "Impresa Delta",
AutoritaEsecutriceData = now.AddDays(1),
AutoritaEsecutriceOra = new TimeSpan(9, 0, 0),
AutoritaEsecutriceSubAppaltatore = true,
AutoritaEsecutriceFornitore = false,
AutoritaEsecutriceFirma = "PN-Hash",
AttrezzaturaAltro = "Linea vita",
AttrezzaturaAntiscintilla = "N/A",
TipoAttrezzature = new List<string> { "Autogru", "Carrelli elevatori" },
TipoMisurePrecauzione = new List<string> { "Rilevazione gas in continuo (LEL, O2, H2S, etc.)", "Uso di materiale anti scintilla" },
TipoAllegati = new List<string> { "Scheda POS", "Altro..." },
TipoCertificati = new List<string> { "Spazio confinato", "N.A." },
TipoAttivita = new List<string> { "Meccanica" },
TipoLavoro = new List<string> { "SollevaMenti" },
TipoAutoritaRichiedente = new List<string> { "SubAppaltatore" },
TipoAutoritaEsecutrice = new List<string> { "Fornitore" },
TipoDPI = new List<string> { "Maschera con filtri", "Visiera", "Guanti" },
ProdottiChimiciUtilizzati = string.Empty,
AltroProdotti = string.Empty,
Prescrizioni = "Area transennata",
AltriDPI = "Scarpe antinfortunistiche",
ProtezioneCollettiva = "Reti di protezione",

#region Rinnovo
DataRinnovoLavoro = now,
OraInizioRinnovo = new TimeSpan(8, 0, 0),
OraFineRinnovo = new TimeSpan(8, 0, 0),
FirmaAutoritaEsecutriceInizioRinnovoLavori = "Nome cognome esempio",
FirmaAutoritaEsecutriceFineRinnovoLavori = "Nome cognome esempio",
FirmaAutoritaOperativaInizioRinnovoLavori = "Nome cognome esempio",
FirmaAutoritaOperativaFineRinnovoLavori = "Nome cognome esempio",
ListaRinnovoLavoro = new List<ListaRinnovoLavoro>
{
new ListaRinnovoLavoro {
Autorizzante = "Nome cognome esempio",
DataFine = now,
DataInizio = now,
Modulo = "Modulo esempio",
Note = "Note esempio"
}
},
#endregion
_timeline = new()
{
    new() { Title = "In Lavorazione RA", Date = "02/10/2025", Active = "", Bozza = "" },
new() { Title = "In Lavorazione PA", Date = "05/10/2025", Active = "", Bozza = "" },
new() { Title = "In Lavorazione IA", Date = "13/10/2025", Active = "", Bozza = "" },
new() { Title = "Rigettato PTWC", Date = "20/10/2025", Active = "active-step", Bozza = "" },

}
},

            new PermessoLavoroModel
{

// Data: 2025-10-21 - Mock record 2
#region intestazione
id = 17,
pdl = "PDL-2025-0009",
Certificato = "00000009",
Stato = new TipoStatoPermessoLavoroDto{ Id=8,Nome="Rigettato CSE" },
Sito = "Ravenna",
DataInizioLavori = now,
Parte = new TipoParteDto{Id=3, Nome="Parte 3"},
DataFineLavori = DateTime.Now.AddDays(30),
Tipo = new TipoPDLDto{Id=9,Nome ="Permesso di Lavoro Generico (Caldo)"},
#endregion


                   
ValiditaDataInizio = now.AddDays(1),
ValiditaOraInizio = new TimeSpan(9, 0, 0),
ValiditaDataFine = now.AddDays(2),
ValiditaOraFine = new TimeSpan(17, 30, 0),
ValiditaAreaLavoro = "Unit B - Area 7",
ValiditaApparecchiatura = "EX-220",
AllegatiAltroDescrizione = "Disegni aggiornati",
DescrizioneAttivita = "Sostituzione passerella",
TipoLavoroAltroDescrizione = "Utilizzo piattaforma elevabile",
AutoritaRichiedenteNome = "Anna",
AutoritaRichiedenteCognome = "Verdi",
AutoritaRichiedenteImpresa = "Impresa Gamma",
AutoritaRichiedenteData = now,
AutoritaRichiedenteOra = new TimeSpan(10, 0, 0),
AutoritaRichiedenteFirma = "AV-Hash",
AutoritaEsecutriceNome = "Luigi",
AutoritaEsecutriceCognome = "Bianchi",
AutoritaEsecutriceImpresa = "Impresa Delta",
AutoritaEsecutriceData = now.AddDays(1),
AutoritaEsecutriceOra = new TimeSpan(9, 0, 0),
AutoritaEsecutriceSubAppaltatore = true,
AutoritaEsecutriceFornitore = false,
AutoritaEsecutriceFirma = "PN-Hash",
AttrezzaturaAltro = "Linea vita",
AttrezzaturaAntiscintilla = "N/A",
TipoAttrezzature = new List<string> { "Autogru", "Carrelli elevatori" },
TipoMisurePrecauzione = new List<string> { "Rilevazione gas in continuo (LEL, O2, H2S, etc.)", "Uso di materiale anti scintilla" },
TipoAllegati = new List<string> { "Scheda POS", "Altro..." },
TipoCertificati = new List<string> { "Spazio confinato", "N.A." },
TipoAttivita = new List<string> { "Meccanica" },
TipoLavoro = new List<string> { "SollevaMenti" },
TipoAutoritaRichiedente = new List<string> { "SubAppaltatore" },
TipoAutoritaEsecutrice = new List<string> { "Fornitore" },
TipoDPI = new List<string> { "Maschera con filtri", "Visiera", "Guanti" },
ProdottiChimiciUtilizzati = string.Empty,
AltroProdotti = string.Empty,
Prescrizioni = "Area transennata",
AltriDPI = "Scarpe antinfortunistiche",
ProtezioneCollettiva = "Reti di protezione",

#region Rinnovo
DataRinnovoLavoro = now,
OraInizioRinnovo = new TimeSpan(8, 0, 0),
OraFineRinnovo = new TimeSpan(8, 0, 0),
FirmaAutoritaEsecutriceInizioRinnovoLavori = "Nome cognome esempio",
FirmaAutoritaEsecutriceFineRinnovoLavori = "Nome cognome esempio",
FirmaAutoritaOperativaInizioRinnovoLavori = "Nome cognome esempio",
FirmaAutoritaOperativaFineRinnovoLavori = "Nome cognome esempio",
ListaRinnovoLavoro = new List<ListaRinnovoLavoro>
{
new ListaRinnovoLavoro {
Autorizzante = "Nome cognome esempio",
DataFine = now,
DataInizio = now,
Modulo = "Modulo esempio",
Note = "Note esempio"
}
},
#endregion
_timeline = new()
{
    new() { Title = "In Lavorazione RA", Date = "02/10/2025", Active = "", Bozza = "" },
new() { Title = "In Lavorazione PA", Date = "05/10/2025", Active = "", Bozza = "" },
new() { Title = "In Lavorazione IA", Date = "13/10/2025", Active = "", Bozza = "" },
new() { Title = "In Lavorazione PTWC", Date = "20/10/2025", Active = "", Bozza = "" },
new() { Title = "In Lavorazione CSE", Date = "24/10/2025", Active = "active-step", Bozza = "" }
}
},

                new PermessoLavoroModel
{
// Data: 2025-10-21 - Mock record 2
#region intestazione
id = 18,
pdl = "PDL-2025-0009",
Certificato = "00000009",
Stato = new TipoStatoPermessoLavoroDto{ Id=8,Nome="Rigettato OA" },
Sito = "Ravenna",
DataInizioLavori = now,
Parte = new TipoParteDto{Id=3, Nome="Parte 3"},
DataFineLavori = DateTime.Now.AddDays(30),
Tipo = new TipoPDLDto{Id=9,Nome ="Permesso di Lavoro Generico (Caldo)"},
#endregion

              
ValiditaDataInizio = now.AddDays(1),
ValiditaOraInizio = new TimeSpan(9, 0, 0),
ValiditaDataFine = now.AddDays(2),
ValiditaOraFine = new TimeSpan(17, 30, 0),
ValiditaAreaLavoro = "Unit B - Area 7",
ValiditaApparecchiatura = "EX-220",
AllegatiAltroDescrizione = "Disegni aggiornati",
DescrizioneAttivita = "Sostituzione passerella",
TipoLavoroAltroDescrizione = "Utilizzo piattaforma elevabile",
AutoritaRichiedenteNome = "Anna",
AutoritaRichiedenteCognome = "Verdi",
AutoritaRichiedenteImpresa = "Impresa Gamma",
AutoritaRichiedenteData = now,
AutoritaRichiedenteOra = new TimeSpan(10, 0, 0),
AutoritaRichiedenteFirma = "AV-Hash",
AutoritaEsecutriceNome = "Luigi",
AutoritaEsecutriceCognome = "Bianchi",
AutoritaEsecutriceImpresa = "Impresa Delta",
AutoritaEsecutriceData = now.AddDays(1),
AutoritaEsecutriceOra = new TimeSpan(9, 0, 0),
AutoritaEsecutriceSubAppaltatore = true,
AutoritaEsecutriceFornitore = false,
AutoritaEsecutriceFirma = "PN-Hash",
AttrezzaturaAltro = "Linea vita",
AttrezzaturaAntiscintilla = "N/A",
TipoAttrezzature = new List<string> { "Autogru", "Carrelli elevatori" },
TipoMisurePrecauzione = new List<string> { "Rilevazione gas in continuo (LEL, O2, H2S, etc.)", "Uso di materiale anti scintilla" },
TipoAllegati = new List<string> { "Scheda POS", "Altro..." },
TipoCertificati = new List<string> { "Spazio confinato", "N.A." },
TipoAttivita = new List<string> { "Meccanica" },
TipoLavoro = new List<string> { "SollevaMenti" },
TipoAutoritaRichiedente = new List<string> { "SubAppaltatore" },
TipoAutoritaEsecutrice = new List<string> { "Fornitore" },
TipoDPI = new List<string> { "Maschera con filtri", "Visiera", "Guanti" },
ProdottiChimiciUtilizzati = string.Empty,
AltroProdotti = string.Empty,
Prescrizioni = "Area transennata",
AltriDPI = "Scarpe antinfortunistiche",
ProtezioneCollettiva = "Reti di protezione",

#region Rinnovo
DataRinnovoLavoro = now,
OraInizioRinnovo = new TimeSpan(8, 0, 0),
OraFineRinnovo = new TimeSpan(8, 0, 0),
FirmaAutoritaEsecutriceInizioRinnovoLavori = "Nome cognome esempio",
FirmaAutoritaEsecutriceFineRinnovoLavori = "Nome cognome esempio",
FirmaAutoritaOperativaInizioRinnovoLavori = "Nome cognome esempio",
FirmaAutoritaOperativaFineRinnovoLavori = "Nome cognome esempio",
ListaRinnovoLavoro = new List<ListaRinnovoLavoro>
{
new ListaRinnovoLavoro {
Autorizzante = "Nome cognome esempio",
DataFine = now,
DataInizio = now,
Modulo = "Modulo esempio",
Note = "Note esempio"
}
},
#endregion
_timeline = new()
{
    new() { Title = "In Lavorazione RA", Date = "02/10/2025", Active = "", Bozza = "" },
new() { Title = "In Lavorazione PA", Date = "05/10/2025", Active = "", Bozza = "" },
new() { Title = "In Lavorazione IA", Date = "13/10/2025", Active = "", Bozza = "" },
new() { Title = "In Lavorazione PTWC", Date = "20/10/2025", Active = "", Bozza = "" },
new() { Title = "In Lavorazione CSE", Date = "24/10/2025", Active = "", Bozza = "" },
new() { Title = "In Lavorazione AGT", Date = "28/10/2025", Active = "", Bozza = "" },
new() { Title = "In Approvazione PA", Date = "03/11/2025", Active = "", Bozza = "" },
new() { Title = "Rigettato OA", Date = "23/11/2025", Active = "active-step", Bozza = "" },

}
},
            };

                return _ret;
            }
            catch
            {
                return _ret;
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

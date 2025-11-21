using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Reflection.PortableExecutable;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SaipemE_PTW.Shared.Models.PWT
{
    public class PermessoLavoroModel
    {
        public List<TimelineItem>? _timeline { get; set; }

        #region intestazione
        public int? id { get; set; }
        public string? pdl { get; set; }
        public string? Certificato { get; set; }
        public TipoPDLDto? Tipo { get; set; }
        //public PWTtypeDto? Tipo { get; set; }
        
        public TipoStatoPermessoLavoroDto? Stato { get; set; } //indica lo stato
        public TipoParteDto? Parte { get; set; } //indica la parte in cui si trova il documento
        
        public DateTime? DataInizioLavori { get; set; }
        public DateTime? DataFineLavori { get; set; }
        #endregion


        #region Parte 1

        [Required]
        public string? Sito { get; set; }

        [Required(ErrorMessage = "La data di validità inizio è obbligatoria")]
        [DataType(DataType.Date)]
        public DateTime? ValiditaDataInizio { get; set; }

        
        public TimeSpan? ValiditaOraInizio { get; set; }
        public DateTime? ValiditaDataFine { get; set; }
        public TimeSpan? ValiditaOraFine { get; set; }
        public string? ValiditaAreaLavoro { get; set; }
        public string? ValiditaApparecchiatura { get; set; }
        
        public string? DescrizioneAttivita { get; set; }
        public string? TipoLavoroAltroDescrizione { get; set; }

        public int GasIsolamento { get; set; } = 0;

        public string? AutoritaRichiedenteNome { get; set; }
        public string? AutoritaRichiedenteCognome { get; set; }
        public string? AutoritaRichiedenteImpresa { get; set; }
        public DateTime? AutoritaRichiedenteData { get; set; }
        public TimeSpan? AutoritaRichiedenteOra { get; set; }
        public string? AutoritaRichiedenteFirma { get; set; }

        public string? AutoritaEsecutriceNome { get; set; }
        public string? AutoritaEsecutriceCognome { get; set; }
        public string? AutoritaEsecutriceImpresa { get; set; }
        public string? AutoritaEsecutriceContratto { get; set; }
        public DateTime? AutoritaEsecutriceData { get; set; }
        public TimeSpan? AutoritaEsecutriceOra { get; set; }
        public bool AutoritaEsecutriceSubAppaltatore { get; set; }
        public bool AutoritaEsecutriceFornitore { get; set; }
        public string? AutoritaEsecutriceFirma { get; set; }

        public string? AttrezzaturaAltro { get; set; }
        public string? AttrezzaturaAntiscintilla { get; set; }

        public List<string>? TipoAttrezzature { get; set; }
        public List<string>? TipoMisurePrecauzione { get; set; }
       
        
        public List<string>? TipoAttivita { get; set; }
        public List<string>? TipoLavoro { get; set; }
        public List<string>? TipoAutoritaRichiedente { get; set; }
        public List<string>? TipoAutoritaEsecutrice { get; set; }
        public List<string>? TipoDPI { get; set; }

        //GAS TEST
        public TipoFrequenza? GasTestFrequenza { get; set; }


        public string? ProdottiChimiciUtilizzati { get; set; }
        public string? AltroProdotti { get; set; }
        public string? Prescrizioni { get; set; }
        public string? AltriDPI { get; set; }
        public string? ProtezioneCollettiva { get; set; }


        //SOLO PER RADIOGRAFIE
        //Esperto Qualificato(EQ)
        public string? EspertoQualificatoNome { get; set; }
        public string? EspertoQualificatoCognome { get; set; }

        //Misure di Sicurezza
        public int DisponibileSitoNumeroEmergenza { get; set; } = 0;
        public int DisponibilePianoEmergenzaInizioLavori { get; set; } = 0;
        public int NecessarieBarrierePiombo { get; set; } = 0;
        public int InstallateTutteSegnalazioniSicurezza { get; set; } = 0;
        public int InstallateLuciLampeggiantiAttornoArea { get; set; } = 0;
        public int DeveEssereUsatoCollimatore { get; set; } = 0;
        public string? AltriRequisiti { get; set; }

        //Caratteristiche della Strumentazione Radiante
        public int TavolaDecadimentoAllegata { get; set; } = 0;
        public string? PersoneCoinvolteAttivitaRadiografica { get; set; }
        public string? AltriRequisitiSicurezza { get; set; }
        #endregion

        #region Parte 2
        public int IsolamentoFontiEnergetiche { get; set; } = 0;
        public string? CertificatoNumero { get; set; }
        public DateTime? DataCertificato { get; set; }
        public string? RischiPrescrizioni { get; set; }
        public string? AutoritaEmittenteNome { get; set; }
        public string? AutoritaEmittenteCognome { get; set; }
        public string? AutoritaEmittenteFirma { get; set; }
        public string? AutoritaEmittenteAssuntore { get; set; }
        public string? AutoritaEmittenteHSE { get; set; }
        public DateTime? AutoritaEmittenteFirmaData { get; set; }
        public TimeSpan? AutoritaEmittenteFirmaOra { get; set; }

        public List<string>? CertificatiIsolamentoFontiEnergetiche { get; set; }
        #endregion

        #region Parte CSE

        public string? CSENome { get; set; }
        public string? CSECognome { get; set; }
        public string? CSEFirma { get; set; }
        public DateTime? CSEFirmaData { get; set; }
        public TimeSpan? CSEFirmaOra { get; set; }


        #endregion


        #region Parte 3
       
        public int GasIsolamentoCertificato { get; set; } = 0;
        public string? Frequenza { get; set; }
        public string? PersonaAutorizzataTestGasNome { get; set; }
        public string? PersonaAutorizzataTestGasImpresa { get; set; }
        public string? PersonaAutorizzataTestGasFirma { get; set; }

        public string? PrecauzioniNome { get; set; }
        public DateTime? PrecauzioniDataInizio { get; set; }
        public TimeSpan? PrecauzioniOraInizio { get; set; }
        public DateTime? PrecauzioniDataFine { get; set; }
        public TimeSpan? PrecauzioniOraFine { get; set; }

        //public string? Assuntore { get; set; }
        //public string? HSE { get; set; }

        public string? AutoritaOperativaFirmaInizio { get; set; }
        public string? AutoritaOperativaFirmaFine { get; set; }
        public string? AutoritaEsecutriceFirmaInizio { get; set; }
        public string? AutoritaEsecutriceFirmaFine { get; set; }
        #endregion

        #region Rinnovo
        public string? FirmaAutoritaEsecutriceInizioRinnovoLavori { get; set; }
        public string? FirmaAutoritaEsecutriceFineRinnovoLavori { get; set; }
        public string? FirmaAutoritaOperativaInizioRinnovoLavori { get; set; }
        public string? FirmaAutoritaOperativaFineRinnovoLavori { get; set; }
        public string? NoteRinnovoLavori { get; set; }
        public DateTime? DataRinnovoLavoro { get; set; }
        public TimeSpan? OraInizioRinnovo { get; set; }
        public TimeSpan? OraFineRinnovo { get; set; }
        public List<ListaRinnovoLavoro>? ListaRinnovoLavoro { get; set; }

        #endregion

        #region Sospensione
       
        
        public string? AutorizzanteSospensione { get; set; }
        public string? MotivoSospensione { get; set; }
        public DateTime? DataInizioSospensione { get; set; }
        public TimeSpan? OraInizioSospensione { get; set; }
        public List<ListaSospensione>? ListaSospensione { get; set; }

        #endregion

        #region Riattivazioni

        public int RipristinareCondizioniSicurezzaSalute { get; set; }
        public DateTime? DataRiattivazione { get; set; }
        public TimeSpan? OraRiattivazione { get; set; }
        public List<ListaRiattivazione>? ListaRiattivazione { get; set; }
        public string? AutorizzanteRiattivazione { get; set; }
        public string? NoteRiattivazione { get; set; }
        #endregion

        #region Chiusura



        public int ConfermaPreliminareChiusura { get; set; } = 0;
        public DateTime? DataFineLavoriGiornaliera { get; set; }
        public string? NoteChiusura { get; set; }

        public int LavoroCompletatoAreaRipulita { get; set; }
        public int AreaVerificataLavoroAccettato { get; set; } = 0;

        #endregion

        #region Allegati
        public List<string>? TipoAllegati { get; set; }
        public List<AttachmentTypeDto>? AttachmentType { get; set; }
        public List<Allegati>? Allegati { get; set; } = new();
        public string? AllegatiAltroDescrizione { get; set; }
        #endregion

        #region Certificati
        public List<string>? TipoCertificati { get; set; }
        public string? CertificatiAltroDescrizione { get; set; }
        public List<string>? ListaCertificatiPresenti { get; set; } = new();
        #endregion

        #region Gas Test
        public List<GasTest>? gasTest { get; set; }
        #endregion
    }

   

    //public class PWTmodel
    //{
    //    public List<TimelineItem>? _timeline { get; set; }

    //    public Header header { get; set; } = new Header();
    //    public Part1 part1 { get; set; } = new Part1();
    //    public Part2 part2 { get; set; } = new Part2();
    //    public Part3 part3 { get; set; } = new Part3();
    //    public CSE cse { get; set; } = new CSE();
    //    public Renewal renewal { get; set; } = new Renewal();//Rinnovo
    //    public Suspension suspension { get; set; } = new Suspension();//Sospensione
    //    public Reactivation reactivation { get; set; } = new Reactivation();//Riattivazione
    //    public Closure closure { get; set; } = new Closure();//Chiusura
    //    public Attached attached { get; set; } = new Attached();//Allegati
    //    public Certified certified { get; set; } = new Certified();//Certificato
    //    public GasTest gastest { get; set; } = new GasTest();//GasTest
    //}

    //public class Header
    //{
    //    public int? Id { get; set; }
    //    public string? Pdl { get; set; }
    //    public string? Certified { get; set; }
    //    public PWTtypeDto? Tipo { get; set; }
    //    public WorkPermitStatusTypeDto? Stato { get; set; }
    //    public TipoParteDto? Parte { get; set; }

    //    public DateTime? DataInizioLavori { get; set; }
    //    public DateTime? DataFineLavori { get; set; }
    //}

    //public class Part1
    //{

    //}

    //public class Part2
    //{

    //}

    //public class Part3
    //{

    //}

    //public class CSE
    //{

    //}

    //public class Renewal
    //{

    //}

    //public class Suspension
    //{

    //}

    //public class Reactivation
    //{

    //}

    //public class Closure
    //{

    //}

    //public class Attached
    //{

    //}

    //public class Certified
    //{

    //}



}

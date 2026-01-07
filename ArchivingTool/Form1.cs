using ArchivingTool.Service.Arms.Services.Common;

using Newtonsoft.Json.Linq;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Net;
using System.Text.RegularExpressions;

namespace ArchivingTool
{
    public partial class Form1 : Form
    {
        // LawTrak
        private Service.Arms.Services.LawTrak.CitationImportService _lawTrakCitationImportService;
        private Service.Arms.Services.LawTrak.CaseImportService _lawTrakCaseImportService;
        private Service.Arms.Services.LawTrak.WarrantImportService _lawTrakWarrantImportService;
        private Service.Arms.Services.LawTrak.AffidavitImportService _lawTrakAffidavitImportService;
        private Service.Arms.Services.LawTrak.EvidenceImportService _lawTrakEvidenceImportService;
        private Service.Arms.Services.LawTrak.AccountingImportService _lawTrakAccountingImportService;
        private Service.Arms.Services.LawTrak.JuvenileImportService _lawTrakJuvenileImportService;
        private Service.Arms.Services.LawTrak.BookingImportService _lawTrakBookingImportService;
        private Service.Arms.Services.LawTrak.SummonImportService _lawTrakSummonImportService;
        private Service.Arms.Services.LawTrak.SubpoenaImportService _lawTrakSubpoenaJsonImportService;
        private Service.Arms.Services.LawTrak.PersonnelImportService _lawTrakPersonnelJsonImportService;
        private Service.Arms.Services.LawTrak.ScReceiptImportService _lawTrakScReceiptsJsonImportService;
        private Service.Arms.Services.LawTrak.JuryImportService _lawTrakJuryImportService;
        private Service.Arms.Services.LawTrak.PropertyCheckImportService _lawTrakpropertyCheckImportService;

        // Badge
        private Service.Arms.Services.Badge.CitationImportService _badgeCitationImportService;
        private Service.Arms.Services.Badge.CaseImportService _badgeCaseImportService;
        private Service.Arms.Services.Badge.CallImportService _badgeCallImportService;
        private Service.Arms.Services.Badge.WarrantImportService _badgeWarrantImportService;
        private Service.Arms.Services.Badge.ArrestImportService _badgeArrestImportService;
        private Service.Arms.Services.Badge.AlarmImportService _badgeAlarmImportService;
        private Service.Arms.Services.Badge.BusinessImportService _badgeBusinessImportService;
        private Service.Arms.Services.Badge.FieldInterviewImportService _badgeFieldInterviewImportService;
        private Service.Arms.Services.Badge.MasterPersonImportService _badgeMasterPersonImportService;
        private Service.Arms.Services.Badge.MasterVehicleImportService _badgeMasterVehicleImportService;
        private Service.Arms.Services.Badge.PropertyImportService _badgePropertyImportService;
        private Service.Arms.Services.Badge.TrafficStopImportService _badgeTrafficStopImportService;


        // Insynch

        // Common
        private ExportService _exportService;

        // General 
        private string sqlConnectionString;
        private string apiToken;
        private readonly ApiTokenService apiTokenService;
        string server = "", db = "", user = "", pass = "";
        bool isSqlAuth = false;
        Label statusLabel = null;

        public Form1()
        {
            InitializeComponent();

            sqlConnectionString = string.Empty;
            apiToken = string.Empty;
            apiTokenService = new ApiTokenService();
            btnGenerateToken.Click += (s, e) => GenerateToken();
            // LawTrak
            _lawTrakCitationImportService = new Service.Arms.Services.LawTrak.CitationImportService();
            _lawTrakCaseImportService = new Service.Arms.Services.LawTrak.CaseImportService();
            _lawTrakWarrantImportService = new Service.Arms.Services.LawTrak.WarrantImportService();
            _lawTrakAffidavitImportService = new Service.Arms.Services.LawTrak.AffidavitImportService();
            _lawTrakEvidenceImportService = new Service.Arms.Services.LawTrak.EvidenceImportService();
            _lawTrakAccountingImportService = new Service.Arms.Services.LawTrak.AccountingImportService();
            _lawTrakJuvenileImportService = new Service.Arms.Services.LawTrak.JuvenileImportService();
            _lawTrakBookingImportService = new Service.Arms.Services.LawTrak.BookingImportService();
            _lawTrakSummonImportService = new Service.Arms.Services.LawTrak.SummonImportService();
            _lawTrakSubpoenaJsonImportService = new Service.Arms.Services.LawTrak.SubpoenaImportService();
            _lawTrakPersonnelJsonImportService = new Service.Arms.Services.LawTrak.PersonnelImportService();
            _lawTrakScReceiptsJsonImportService = new Service.Arms.Services.LawTrak.ScReceiptImportService();
            _lawTrakJuryImportService = new Service.Arms.Services.LawTrak.JuryImportService();
            _lawTrakpropertyCheckImportService = new Service.Arms.Services.LawTrak.PropertyCheckImportService();

            // InSynch
            /*
            _lawTrakCitationExportService = new Service.Arms.Services.LawTrak.CitationExportService();
            _lawTrakCitationImportService = new Service.Arms.Services.LawTrak.CitationImportService();
            _lawTrakCaseExportService = new Service.Arms.Services.LawTrak.CaseExportService();
            _lawTrakCaseImportService = new Service.Arms.Services.LawTrak.CaseImportService();
            _lawTrakWarrantExportService = new Service.Arms.Services.LawTrak.WarrantExportService();
            _lawTrakWarrantImportService = new Service.Arms.Services.LawTrak.WarrantImportService();
            */
            // Badge
            _badgeCitationImportService = new Service.Arms.Services.Badge.CitationImportService();
            _badgeCaseImportService = new Service.Arms.Services.Badge.CaseImportService();
            _badgeCallImportService = new Service.Arms.Services.Badge.CallImportService();
            _badgeWarrantImportService = new Service.Arms.Services.Badge.WarrantImportService();
            _badgeArrestImportService = new Service.Arms.Services.Badge.ArrestImportService();
            _badgeAlarmImportService = new Service.Arms.Services.Badge.AlarmImportService();
            _badgeBusinessImportService = new Service.Arms.Services.Badge.BusinessImportService();
            _badgeFieldInterviewImportService = new Service.Arms.Services.Badge.FieldInterviewImportService();
            _badgeMasterPersonImportService = new Service.Arms.Services.Badge.MasterPersonImportService();
            _badgeMasterVehicleImportService = new Service.Arms.Services.Badge.MasterVehicleImportService();
            _badgePropertyImportService = new Service.Arms.Services.Badge.PropertyImportService();
            _badgeTrafficStopImportService = new Service.Arms.Services.Badge.TrafficStopImportService();

            // Common
            _exportService = new Service.Arms.Services.Common.ExportService();

        }

        private void ConnectSql(string rms)
        {
            if (rms == "LawTrak")
            {
                server = txtSqlServer_LawTrak.Text;
                db = txtSqlDb_LawTrak.Text;
                user = txtSqlUser_LawTrak.Text;
                pass = txtSqlPass_LawTrak.Text;
                isSqlAuth = cbSqlAuth_LawTrak.SelectedItem?.ToString() == "SQL Server";
                statusLabel = lblSqlStatus_LawTrak;
            }
            else if (rms == "InSynch")
            {
                server = txtSqlServer_InSynch.Text;
                db = txtSqlDb_InSynch.Text;
                user = txtSqlUser_InSynch.Text;
                pass = txtSqlPass_InSynch.Text;
                statusLabel = lblSqlStatus_InSynch;
            }
            else if (rms == "Badge")
            {
                server = txtSqlServer_Badge.Text;
                db = txtSqlDb_Badge.Text;
                user = txtSqlUser_Badge.Text;
                pass = txtSqlPass_Badge.Text;
                isSqlAuth = cbSqlAuth_Badge.SelectedItem?.ToString() == "SQL Server";
                statusLabel = lblSqlStatus_Badge;
            }
            if (isSqlAuth) // SQL Server authentication
            {
                if (string.IsNullOrWhiteSpace(server) || string.IsNullOrWhiteSpace(db) ||
                    string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(pass))
                {
                    MessageBox.Show("Please enter all connection details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                sqlConnectionString = $"Server={server};Database={db};User Id={user};Password={pass};TrustServerCertificate=True;";
            }
            else // Windows Authentication
            {
                if (string.IsNullOrWhiteSpace(server) || string.IsNullOrWhiteSpace(db))
                {
                    MessageBox.Show("Please enter server and database name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                sqlConnectionString = $"Server={server};Database={db};Integrated Security=True;TrustServerCertificate=True;";
            }

            if (CheckDatabaseConnection())
            {
                statusLabel.Text = "Database connected.";
                statusLabel.ForeColor = Color.DarkGreen;
            }
            else
            {
                statusLabel.Text = "Failed Connection";
                statusLabel.ForeColor = Color.DarkRed;
                MessageBox.Show("Connection refused, it clearly doesn’t like you... Please check connection details.", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GenerateToken()
        {
            if (!string.IsNullOrEmpty(txtAgencyKey.Text) && !string.IsNullOrEmpty(txtAgencyAPIKey.Text))
            {
                lblApiStatus.Text = "Token Created";
                lblApiStatus.ForeColor = Color.DarkGreen;
            }
            else
            {
                lblApiStatus.Text = "Invalid Credentials";
                lblApiStatus.ForeColor = Color.DarkRed;

            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cbSqlAuth_LawTrak.SelectedIndex = 0;
            cbSqlAuth_Badge.SelectedIndex = 0;
        }

        #region LawTrak Citation Process

        private async Task ProcessLawTrakCitationsExportImport()
        {
            var citationTypes = new[]
            {
        "sp_Arrest_Tickets_Mapped",
        "sp_Traffic_Tickets_Mapped",
        "sp_Warning_Ticket_Mapped",
        "sp_Warning_Ticket_local_Mapped"
    };

            var agencyKey = Guid.Parse(txtAgencyKey.Text);
            var folderPath = txtLTFolderPath.Text;
            const int pageSize = 100;

            foreach (var storedProcedureName in citationTypes)
            {
                WriteLog("Executing: " + storedProcedureName, "LawTrak", "Citations", agencyKey);
                int pageNumber = 1;
                bool hasMore = true;
                Guid? historyId = null;

                while (hasMore)
                {
                    // Calling export service with paging params
                    var citations = await Task.Run(() =>
                        _exportService.ExportData(
                            sqlConnectionString,
                            storedProcedureName,
                            agencyKey,
                            pageNumber,
                            pageSize
                        )
                    );

                    if (citations != null && citations.Count > 0)
                    {
                        historyId = await ImportLawTrakCitationData(citations, historyId, (pageNumber - 1) * pageSize, folderPath, agencyKey);

                        // If fewer rows returned than page size, last page
                        hasMore = citations.Count == pageSize;
                    }
                    else
                    {
                        hasMore = false;
                    }

                    pageNumber++;
                }
            }
        }
        /*
        private async Task<Guid?> ImportLawTrakCitationData(List<Dictionary<string, object>> citations, Guid? historyId, int index, string folderPath, Guid agencyKey)
        {
            WriteLog("-------------------", "LawTrak", "Citations", agencyKey);
            WriteLog($"Batch {index}", "LawTrak", "Citations", agencyKey);
            WriteLog("-------------------", "LawTrak", "Citations", agencyKey);

            foreach (var citation in citations)
            {
                string baseIDCID = "Unknown";
                string iDCID = "Unknown";

                try
                {
                    var citationJson = JObject.FromObject(citation);
                    baseIDCID = citationJson["Document"]?["CitationsData"]?["IDC ID"]?.ToString() ?? "Unknown";
                    iDCID = baseIDCID;

                    int retryCounter = 1;
                    bool success = false;
                    HttpStatusCode? statusCode = HttpStatusCode.OK;
                    string message = "";

                    do
                    {
                        citationJson["Document"]["CitationsData"]["IDC ID"] = iDCID;

                        var result = await _lawTrakCitationImportService
                            .ImportCitationsAsync(apiToken, citationJson, iDCID, historyId, folderPath, agencyKey);

                        success = result.Success;
                        statusCode = result.StatusCode;
                        message = result.Message;

                        if (statusCode == HttpStatusCode.Unauthorized)
                        {
                            await CreateNewToken();

                            result = await _lawTrakCitationImportService
                            .ImportCitationsAsync(apiToken, citationJson, iDCID, historyId, folderPath, agencyKey);

                            success = result.Success;
                            statusCode = result.StatusCode;
                            message = result.Message;
                        }

                        if (!success && statusCode == HttpStatusCode.Conflict)
                        {
                            iDCID = $"{baseIDCID}_{retryCounter++}";
                        }

                    }
                    while (!success && statusCode == HttpStatusCode.Conflict);

                    if (success)
                    {
                        Console.WriteLine($"Citation {index}: Successfully imported Citation IDC ID {iDCID}");
                    }
                    else
                    {
                        // All retries failed
                        var logMessage = $"Insertion FAILED for Citation IDC ID {iDCID}: {statusCode} - {message}";
                        WriteLog(logMessage, "LawTrak", "Citations", agencyKey);
                    }
                }
                catch (Exception ex)
                {
                    var logMessage = $"Citation {index} (Citation IDC ID {iDCID}) exception: {ex}";
                    WriteLog(logMessage, "LawTrak", "Citations", agencyKey);

                }

                index++;
            }

            return historyId;
        }
        */
        private async Task<Guid?> ImportLawTrakCitationData(List<Dictionary<string, object>> citations, Guid? historyId, int index, string folderPath, Guid agencyKey)
        {
            WriteLog("-------------------", "LawTrak", "Citations", agencyKey);
            WriteLog($"Batch {index}", "LawTrak", "Citations", agencyKey);
            WriteLog("-------------------", "LawTrak", "Citations", agencyKey);

            foreach (var citation in citations)
            {
                string baseIDCID = "Unknown";
                string iDCID = "Unknown";

                try
                {
                    var citationJson = JObject.FromObject(citation);

                    baseIDCID = citationJson["Document"]?["CitationsData"]?["IDC ID"]?.ToString() ?? "Unknown";
                    iDCID = baseIDCID;

                    int maxRetries = 5;
                    int attempt = 0;
                    bool success = false;
                    HttpStatusCode? statusCode = null;
                    string message = "";

                    while (!success && attempt < maxRetries)
                    {
                        attempt++;

                        try
                        {
                            EnsureKeyExists(citationJson, "Document");
                            EnsureKeyExists(citationJson, "Document.CitationsData");
                            EnsureKeyExists(citationJson, "Document.CitationsData['IDC ID']");
                            citationJson["Document"]["CitationsData"]["IDC ID"] = iDCID;
                        }
                        catch (Exception ex)
                        {
                            WriteLog($"Citation {index}: Missing required key → {ex.Message}", "LawTrak", "Citations", agencyKey);
                            break; // this is not retryable
                        }

                        var result = await _lawTrakCitationImportService
                            .ImportCitationsAsync(apiToken, citationJson, iDCID, historyId, folderPath, agencyKey);

                        success = result.Success;
                        statusCode = result.StatusCode;
                        message = result.Message;

                        if (success)
                            break;

                        if (statusCode == HttpStatusCode.Unauthorized)
                        {
                            WriteLog($"Citation {index} & (Citation IDC ID {iDCID}): Unauthorized, refreshing token (attempt {attempt})",
                                     "LawTrak", "Citations", agencyKey);

                            await CreateNewToken();
                            continue; // retry SAME case, SAME attachments
                        }

                        if (statusCode == HttpStatusCode.Conflict)
                        {
                            iDCID = $"{baseIDCID}_{attempt}";
                            continue;
                        }

                        // Any other failure → backoff and retry
                        await Task.Delay(3000);
                    }

                }
                catch (Exception ex)
                {
                    var logMessage = $"Citation {index} (Citation IDC ID {iDCID}) exception: {ex}";
                    WriteLog(logMessage, "LawTrak", "Citations", agencyKey);
                }

                index++;
            }

            return historyId;
        }


        #endregion

        #region LawTrak Warrents Process

        private async Task ProcessLawTrakWarrantsExportImport()
        {
            var warrantsTypes = new[]
            {
        "sp_Arrest_Warrants_Mapped",
        "sp_Search_Warrants_Mapped",
        "sp_Bench_Warrants_Mapped"
    };

            var agencyKey = Guid.Parse(txtAgencyKey.Text);
            var folderPath = txtLTFolderPath.Text;
            const int pageSize = 100;

            foreach (var storedProcedureName in warrantsTypes)
            {
                WriteLog("Executing: " + storedProcedureName, "LawTrak", "Warrants", agencyKey);
                int pageNumber = 1;
                bool hasMore = true;
                Guid? historyId = null;

                while (hasMore)
                {
                    // Calling export service with paging params
                    var warrants = await Task.Run(() =>
                        _exportService.ExportData(
                            sqlConnectionString,
                            storedProcedureName,
                            agencyKey,
                            pageNumber,
                            pageSize
                        )
                    );

                    if (warrants != null && warrants.Count > 0)
                    {
                        historyId = await ImportLawTrakWarrantData(warrants, historyId, (pageNumber - 1) * pageSize, folderPath, agencyKey);

                        // If we got fewer rows than requested, it’s the last page
                        hasMore = warrants.Count == pageSize;
                    }
                    else
                    {
                        hasMore = false;
                    }

                    pageNumber++;
                }
            }
        }
        /*
        private async Task<Guid?> ImportLawTrakWarrantData(List<Dictionary<string, object>> warrants, Guid? historyId, int index, string folderPath, Guid agencyKey)
        {
            WriteLog("-------------------", "LawTrak", "Warrants", agencyKey);
            WriteLog($"Batch {index}", "LawTrak", "Warrants", agencyKey);
            WriteLog("-------------------", "LawTrak", "Warrants", agencyKey);

            foreach (var warrant in warrants)
            {
                string baseIDCID = "Unknown";
                string iDCID = "Unknown";

                try
                {
                    var warrantJson = JObject.FromObject(warrant);
                    baseIDCID = warrantJson["Document"]?["WarrantsData"]?["IDC ID"]?.ToString() ?? "Unknown";
                    iDCID = baseIDCID;

                    int retryCounter = 1;
                    bool success = false;
                    HttpStatusCode? statusCode = HttpStatusCode.OK;
                    string message = "";

                    do
                    {
                        warrantJson["Document"]["WarrantsData"]["IDC ID"] = iDCID;

                        var result = await _lawTrakWarrantImportService
                            .ImportWarrantsAsync(apiToken, warrantJson, iDCID, historyId, folderPath, agencyKey);

                        success = result.Success;
                        statusCode = result.StatusCode;
                        message = result.Message;

                        if (statusCode == HttpStatusCode.Unauthorized)
                        {
                            await CreateNewToken();

                            result = await _lawTrakWarrantImportService
                            .ImportWarrantsAsync(apiToken, warrantJson, iDCID, historyId, folderPath, agencyKey);

                            success = result.Success;
                            statusCode = result.StatusCode;
                            message = result.Message;
                        }

                        if (!success && statusCode == HttpStatusCode.Conflict)
                        {
                            iDCID = $"{baseIDCID}_{retryCounter++}";
                        }

                    }
                    while (!success && statusCode == HttpStatusCode.Conflict);

                    if (success)
                    {
                        Console.WriteLine($"Warrant {index}: Successfully imported Warrant IDC ID {iDCID}");
                    }
                    else
                    {
                        // All retries failed
                        var logMessage = $"Insertion FAILED for Warrant IDC ID {iDCID}: {statusCode} - {message}";
                        WriteLog(logMessage, "LawTrak", "Warrants", agencyKey);
                    }
                }
                catch (Exception ex)
                {
                    var logMessage = $"Warrant {index} (Warrant IDC ID {iDCID}) exception: {ex}";
                    WriteLog(logMessage, "LawTrak", "Warrants", agencyKey);
                }

                index++;
            }

            return historyId;
        }*/

        private async Task<Guid?> ImportLawTrakWarrantData(List<Dictionary<string, object>> warrants, Guid? historyId, int index, string folderPath, Guid agencyKey)
        {
            WriteLog("-------------------", "LawTrak", "Warrants", agencyKey);
            WriteLog($"Batch {index}", "LawTrak", "Warrants", agencyKey);
            WriteLog("-------------------", "LawTrak", "Warrants", agencyKey);

            foreach (var warrant in warrants)
            {
                string baseIDCID = "Unknown";
                string iDCID = "Unknown";

                try
                {

                    var warrantJson = JObject.FromObject(warrant);
                    baseIDCID = warrantJson["Document"]?["WarrantsData"]?["IDC ID"]?.ToString() ?? "Unknown";
                    iDCID = baseIDCID;


                    int maxRetries = 5;
                    int attempt = 0;
                    bool success = false;
                    HttpStatusCode? statusCode = null;
                    string message = "";

                    while (!success && attempt < maxRetries)
                    {
                        attempt++;

                        try
                        {
                            EnsureKeyExists(warrantJson, "Document");
                            EnsureKeyExists(warrantJson, "Document.WarrantsData");
                            EnsureKeyExists(warrantJson, "Document.WarrantsData['IDC ID']");
                            warrantJson["Document"]["WarrantsData"]["IDC ID"] = iDCID;
                        }
                        catch (Exception ex)
                        {
                            WriteLog($"Warrant {index}: Missing required key → {ex.Message}", "LawTrak", "Warrants", agencyKey);
                            break; // this is not retryable
                        }

                        var result = await _lawTrakWarrantImportService
                            .ImportWarrantsAsync(apiToken, warrantJson, iDCID, historyId, folderPath, agencyKey);

                        success = result.Success;
                        statusCode = result.StatusCode;
                        message = result.Message;

                        if (success)
                            break;

                        if (statusCode == HttpStatusCode.Unauthorized)
                        {
                            WriteLog($"Warrant {index} & (Warrant IDC ID {iDCID}): Unauthorized, refreshing token (attempt {attempt})",
                                     "LawTrak", "Warrants", agencyKey);

                            await CreateNewToken();
                            continue; // retry SAME case, SAME attachments
                        }

                        if (statusCode == HttpStatusCode.Conflict)
                        {
                            iDCID = $"{baseIDCID}_{attempt}";
                            continue;
                        }

                        // Any other failure → backoff and retry
                        await Task.Delay(3000);
                    }

                }
                catch (Exception ex)
                {
                    var logMessage = $"Warrant {index} (Warrant IDC ID {iDCID}) exception: {ex}";
                    WriteLog(logMessage, "LawTrak", "Warrants", agencyKey);
                }

                index++;
            }

            return historyId;
        }

        #endregion

        #region LawTrak Case Process

        private async Task ProcessLawTrakCasesExportImport()
        {
            var CaseTypes = new[]
            {
                "sp_GetCasesAsJson"
            };

            var agencyKey = Guid.Parse(txtAgencyKey.Text);
            var folderPath = txtLTFolderPath.Text;
            const int pageSize = 50;

            foreach (var storedProcedureName in CaseTypes)
            {
                WriteLog("Executing: " + storedProcedureName, "LawTrak", "Cases", agencyKey);
                int pageNumber = 1;
                bool hasMore = true;
                Guid? historyId = null;

                while (hasMore)
                {
                    // Calling export service with paging params
                    var cases = await Task.Run(() =>
                        _exportService.ExportData(
                            sqlConnectionString,
                            storedProcedureName,
                            agencyKey,
                            pageNumber,
                            pageSize
                        )
                    );

                    if (cases != null && cases.Count > 0)
                    {
                        historyId = await ImportLawTrakCaseData(cases, historyId, (pageNumber - 1) * pageSize, folderPath, agencyKey);

                        // If we got fewer rows than requested, it’s the last page
                        hasMore = cases.Count == pageSize;
                    }
                    else
                    {
                        hasMore = false;
                    }

                    pageNumber++;
                }
            }
        }

        /*private async Task<Guid?> ImportLawTrakCaseData(List<Dictionary<string, object>> cases, Guid? historyId, int index, string folderPath, Guid agencyKey)
        {
            WriteLog("-------------------", "LawTrak", "Cases", agencyKey);
            WriteLog($"Batch {index}", "LawTrak", "Cases", agencyKey);
            WriteLog("-------------------", "LawTrak", "Cases", agencyKey);

            foreach (var singleCase in cases)
            {
                string baseIDCID = "Unknown";
                string iDCID = "Unknown";

                try
                {
                    var caseJson = JObject.FromObject(singleCase);
                    baseIDCID = caseJson["Document"]?["CaseData"]?["IDC ID"]?.ToString() ?? "Unknown";
                    iDCID = baseIDCID;

                    int maxRetries = 5;
                    int attempt = 0;
                    int retryCounter = 1;
                    bool success = false;
                    HttpStatusCode? statusCode = HttpStatusCode.OK;
                    string message = "";

                    do
                    {
                        try
                        {
                            EnsureKeyExists(caseJson, "Document");
                            EnsureKeyExists(caseJson, "Document.CaseData");
                            EnsureKeyExists(caseJson, "Document.CaseData['IDC ID']");

                            caseJson["Document"]["CaseData"]["IDC ID"] = iDCID;
                        }
                        catch (Exception ex)
                        {
                            WriteLog($"Case {index}: Missing required key → {ex.Message}", "LawTrak", "Cases", agencyKey);
                            continue; // skip this case and move on
                        }


                        var result = await _lawTrakCaseImportService
                            .ImportCasesAsync(apiToken, caseJson, iDCID, historyId, folderPath, agencyKey);

                        success = result.Success;
                        statusCode = result.StatusCode;
                        message = result.Message;

                        if (statusCode == HttpStatusCode.Unauthorized)
                        {
                            await CreateNewToken();

                            result = await _lawTrakCaseImportService
                                .ImportCasesAsync(apiToken, caseJson, iDCID, historyId, folderPath, agencyKey);

                            success = result.Success;
                            statusCode = result.StatusCode;
                            message = result.Message;
                        }

                        if (!success && statusCode == HttpStatusCode.Conflict)
                        {
                            iDCID = $"{baseIDCID}_{retryCounter++}";
                        }

                    }
                    while (!success && statusCode == HttpStatusCode.Conflict);

                    if (success)
                    {
                        Console.WriteLine($"Case {index}: Successfully imported Case IDC ID {iDCID}");
                    }
                    else
                    {
                        // All retries failed
                        var logMessage = $"Insertion FAILED for Case IDC ID {iDCID}: {statusCode} - {message}";
                        WriteLog(logMessage, "LawTrak", "Cases", agencyKey);

                    }
                }
                catch (Exception ex)
                {
                    var logMessage = $"Case {index} (Case IDC ID {iDCID}) exception: {ex}";
                    WriteLog(logMessage, "LawTrak", "Cases", agencyKey);
                }

                index++;
            }

            return historyId;
        }*/

        private async Task<Guid?> ImportLawTrakCaseData(List<Dictionary<string, object>> cases, Guid? historyId, int index, string folderPath, Guid agencyKey)
        {
            WriteLog("-------------------", "LawTrak", "Cases", agencyKey);
            WriteLog($"Batch {index}", "LawTrak", "Cases", agencyKey);
            WriteLog("-------------------", "LawTrak", "Cases", agencyKey);

            foreach (var singleCase in cases)
            {
                string baseIDCID = "Unknown";
                string iDCID = "Unknown";

                try
                {
                    var caseJson = JObject.FromObject(singleCase);
                    baseIDCID = caseJson["Document"]?["CaseData"]?["IDC ID"]?.ToString() ?? "Unknown";
                    iDCID = baseIDCID;

                    int maxRetries = 5;
                    int attempt = 0;
                    bool success = false;
                    HttpStatusCode? statusCode = null;
                    string message = "";

                    while (!success && attempt < maxRetries)
                    {
                        attempt++;

                        try
                        {
                            EnsureKeyExists(caseJson, "Document");
                            EnsureKeyExists(caseJson, "Document.CaseData");
                            EnsureKeyExists(caseJson, "Document.CaseData['IDC ID']");
                            caseJson["Document"]["CaseData"]["IDC ID"] = iDCID;
                        }
                        catch (Exception ex)
                        {
                            WriteLog($"Case {index}: Missing required key → {ex.Message}", "LawTrak", "Cases", agencyKey);
                            break; // this is not retryable
                        }

                        var result = await _lawTrakCaseImportService
                            .ImportCasesAsync(apiToken, caseJson, iDCID, historyId, folderPath, agencyKey);

                        success = result.Success;
                        statusCode = result.StatusCode;
                        message = result.Message;

                        if (success)
                            break;

                        if (statusCode == HttpStatusCode.Unauthorized)
                        {
                            WriteLog($"Case {index} & (Case IDC ID {iDCID}): Unauthorized, refreshing token (attempt {attempt})",
                                     "LawTrak", "Cases", agencyKey);

                            await CreateNewToken();
                            continue; // retry SAME case, SAME attachments
                        }

                        if (statusCode == HttpStatusCode.Conflict)
                        {
                            iDCID = $"{baseIDCID}_{attempt}";
                            continue;
                        }

                        // Any other failure → backoff and retry
                        await Task.Delay(3000);
                    }

                }
                catch (Exception ex)
                {
                    var logMessage = $"Case {index} (Case IDC ID {iDCID}) exception: {ex}";
                    WriteLog(logMessage, "LawTrak", "Cases", agencyKey);
                }

                index++;
            }

            return historyId;
        }

        #endregion

        #region LawTrak Affidavit Process

        private async Task ProcessLawTrakAffidavitsExportImport()
        {
            var affidavitTypes = new[]
            {
        "sp_Affidavit_CriminalWarrant_Mapped",
        "sp_Affidavit_PersonalS_Mapped",
        "sp_Affidavit_ProbableCause_Mapped"
    };

            var agencyKey = Guid.Parse(txtAgencyKey.Text);
            var folderPath = txtLTFolderPath.Text;
            const int pageSize = 100;

            foreach (var storedProcedureName in affidavitTypes)
            {
                WriteLog("Executing: " + storedProcedureName, "LawTrak", "Affidavits", agencyKey);
                int pageNumber = 1;
                bool hasMore = true;
                Guid? historyId = null;

                while (hasMore)
                {
                    // Calling export service with paging params
                    var affidavits = await Task.Run(() =>
                        _exportService.ExportData(
                            sqlConnectionString,
                            storedProcedureName,
                            agencyKey,
                            pageNumber,
                            pageSize
                        )
                    );

                    if (affidavits != null && affidavits.Count > 0)
                    {
                        historyId = await ImportLawTrakAffidavitData(affidavits, historyId, (pageNumber - 1) * pageSize, folderPath, agencyKey);

                        // If fewer rows returned than page size, last page
                        hasMore = affidavits.Count == pageSize;
                    }
                    else
                    {
                        hasMore = false;
                    }

                    pageNumber++;
                }
            }
        }
        /*
        private async Task<Guid?> ImportLawTrakAffidavitData(List<Dictionary<string, object>> affidavits, Guid? historyId, int index, string folderPath, Guid agencyKey)
        {
            WriteLog("-------------------", "LawTrak", "Affidavits", agencyKey);
            WriteLog($"Batch {index}", "LawTrak", "Affidavits", agencyKey);
            WriteLog("-------------------", "LawTrak", "Affidavits", agencyKey);

            foreach (var affidavit in affidavits)
            {
                string baseIDCID = "Unknown";
                string iDCID = "Unknown";

                try
                {
                    var affidavitJson = JObject.FromObject(affidavit);
                    baseIDCID = affidavitJson["Document"]?["AffidavitsData"]?["IDC ID"]?.ToString() ?? "Unknown";
                    iDCID = baseIDCID;

                    int retryCounter = 1;
                    bool success = false;
                    HttpStatusCode? statusCode = HttpStatusCode.OK;
                    string message = "";

                    do
                    {
                        affidavitJson["Document"]["AffidavitsData"]["IDC ID"] = iDCID;

                        var result = await _lawTrakAffidavitImportService
                            .ImportAffidavitsAsync(apiToken, affidavitJson, iDCID, historyId, folderPath, agencyKey);

                        success = result.Success;
                        statusCode = result.StatusCode;
                        message = result.Message;

                        if (statusCode == HttpStatusCode.Unauthorized)
                        {
                            await CreateNewToken();

                            result = await _lawTrakAffidavitImportService
                            .ImportAffidavitsAsync(apiToken, affidavitJson, iDCID, historyId, folderPath, agencyKey);

                            success = result.Success;
                            statusCode = result.StatusCode;
                            message = result.Message;
                        }

                        if (!success && statusCode == HttpStatusCode.Conflict)
                        {
                            iDCID = $"{baseIDCID}_{retryCounter++}";
                        }

                    }
                    while (!success && statusCode == HttpStatusCode.Conflict);

                    if (success)
                    {
                        Console.WriteLine($"Affidavit {index}: Successfully imported Affidavit IDC ID {iDCID}");
                    }
                    else
                    {
                        // All retries failed
                        var logMessage = $"Insertion FAILED for Affidavit IDC ID {iDCID}: {statusCode} - {message}";
                        WriteLog(logMessage, "LawTrak", "Affidavits", agencyKey);
                    }
                }
                catch (Exception ex)
                {
                    var logMessage = $"Affidavit {index} (Affidavit IDC ID {iDCID}) exception: {ex}";
                    WriteLog(logMessage, "LawTrak", "Affidavits", agencyKey);
                }

                index++;
            }

            return historyId;
        }*/

        private async Task<Guid?> ImportLawTrakAffidavitData(List<Dictionary<string, object>> affidavits, Guid? historyId, int index, string folderPath, Guid agencyKey)
        {
            WriteLog("-------------------", "LawTrak", "Affidavits", agencyKey);
            WriteLog($"Batch {index}", "LawTrak", "Affidavits", agencyKey);
            WriteLog("-------------------", "LawTrak", "Affidavits", agencyKey);

            foreach (var affidavit in affidavits)
            {
                string baseIDCID = "Unknown";
                string iDCID = "Unknown";

                try
                {
                    var affidavitJson = JObject.FromObject(affidavit);
                    baseIDCID = affidavitJson["Document"]?["AffidavitsData"]?["IDC ID"]?.ToString() ?? "Unknown";
                    iDCID = baseIDCID;

                    int maxRetries = 5;
                    int attempt = 0;
                    bool success = false;
                    HttpStatusCode? statusCode = null;
                    string message = "";

                    while (!success && attempt < maxRetries)
                    {
                        attempt++;

                        try
                        {
                            EnsureKeyExists(affidavitJson, "Document");
                            EnsureKeyExists(affidavitJson, "Document.AffidavitsData");
                            EnsureKeyExists(affidavitJson, "Document.AffidavitsData['IDC ID']");
                            affidavitJson["Document"]["AffidavitsData"]["IDC ID"] = iDCID;
                        }
                        catch (Exception ex)
                        {
                            WriteLog($"Affidavit {index}: Missing required key → {ex.Message}", "LawTrak", "Affidavits", agencyKey);
                            break; // this is not retryable
                        }

                        var result = await _lawTrakAffidavitImportService
                            .ImportAffidavitsAsync(apiToken, affidavitJson, iDCID, historyId, folderPath, agencyKey);

                        success = result.Success;
                        statusCode = result.StatusCode;
                        message = result.Message;

                        if (success)
                            break;

                        if (statusCode == HttpStatusCode.Unauthorized)
                        {
                            WriteLog($"Affidavit {index} & (Affidavit IDC ID {iDCID}): Unauthorized, refreshing token (attempt {attempt})",
                                     "LawTrak", "Affidavits", agencyKey);

                            await CreateNewToken();
                            continue;
                        }

                        if (statusCode == HttpStatusCode.Conflict)
                        {
                            iDCID = $"{baseIDCID}_{attempt}";
                            continue;
                        }

                        // Any other failure → backoff and retry
                        await Task.Delay(3000);
                    }

                }
                catch (Exception ex)
                {
                    var logMessage = $"Affidavit {index} (Affidavit IDC ID {iDCID}) exception: {ex}";
                    WriteLog(logMessage, "LawTrak", "Affidavit", agencyKey);
                }

                index++;
            }

            return historyId;
        }

        #endregion

        #region LawTrak Evidence Process

        private async Task ProcessLawTrakEvidencesExportImport()
        {
            var evidenceTypes = new[]
            {
        "sp_Evidence_Main_Mapped",
        "sp_Evidence_Log_Mapped",
        "sp_LostandFounds_Mapped"
    };

            var agencyKey = Guid.Parse(txtAgencyKey.Text);
            var folderPath = txtLTFolderPath.Text;
            const int pageSize = 100;

            foreach (var storedProcedureName in evidenceTypes)
            {
                WriteLog("Executing: " + storedProcedureName, "LawTrak", "Evidences", agencyKey);
                int pageNumber = 1;
                bool hasMore = true;
                Guid? historyId = null;

                while (hasMore)
                {
                    // Calling export service with paging params
                    var evidences = await Task.Run(() =>
                        _exportService.ExportData(
                            sqlConnectionString,
                            storedProcedureName,
                            agencyKey,
                            pageNumber,
                            pageSize
                        )
                    );

                    if (evidences != null && evidences.Count > 0)
                    {
                        historyId = await ImportLawTrakEvidenceData(evidences, historyId, (pageNumber - 1) * pageSize, folderPath, agencyKey);

                        // If fewer rows returned than page size, last page
                        hasMore = evidences.Count == pageSize;
                    }
                    else
                    {
                        hasMore = false;
                    }

                    pageNumber++;
                }
            }
        }
        /*
        private async Task<Guid?> ImportLawTrakEvidenceData(List<Dictionary<string, object>> evidences, Guid? historyId, int index, string folderPath, Guid agencyKey)
        {
            WriteLog("-------------------", "LawTrak", "Evidences", agencyKey);
            WriteLog($"Batch {index}", "LawTrak", "Evidences", agencyKey);
            WriteLog("-------------------", "LawTrak", "Evidences", agencyKey);
            foreach (var evidence in evidences)
            {
                string baseIDCID = "Unknown";
                string iDCID = "Unknown";

                try
                {
                    var evidenceJson = JObject.FromObject(evidence);
                    baseIDCID = evidenceJson["Document"]?["EvidenceData"]?["IDC ID"]?.ToString() ?? "Unknown";
                    iDCID = baseIDCID;

                    int retryCounter = 1;
                    bool success = false;
                    HttpStatusCode? statusCode = HttpStatusCode.OK;
                    string message = "";

                    do
                    {
                        evidenceJson["Document"]["EvidenceData"]["IDC ID"] = iDCID;

                        var result = await _lawTrakEvidenceImportService
                            .ImportEvidencesAsync(apiToken, evidenceJson, iDCID, historyId, folderPath, agencyKey);

                        success = result.Success;
                        statusCode = result.StatusCode;
                        message = result.Message;

                        if (statusCode == HttpStatusCode.Unauthorized)
                        {
                            await CreateNewToken();

                            result = await _lawTrakEvidenceImportService
                            .ImportEvidencesAsync(apiToken, evidenceJson, iDCID, historyId, folderPath, agencyKey);

                            success = result.Success;
                            statusCode = result.StatusCode;
                            message = result.Message;
                        }

                        if (!success && statusCode == HttpStatusCode.Conflict)
                        {
                            iDCID = $"{baseIDCID}_{retryCounter++}";
                        }

                    }
                    while (!success && statusCode == HttpStatusCode.Conflict);

                    if (success)
                    {
                        Console.WriteLine($"Evidence {index}: Successfully imported Evidence IDC ID {iDCID}");
                    }
                    else
                    {
                        // All retries failed
                        var logMessage = $"Insertion FAILED for Evidence IDC ID {iDCID}: {statusCode} - {message}";
                        WriteLog(logMessage, "LawTrak", "Evidences", agencyKey);
                    }
                }
                catch (Exception ex)
                {
                    var logMessage = $"Evidence {index} (Evidence IDC ID {iDCID}) exception: {ex}";
                    WriteLog(logMessage, "LawTrak", "Evidences", agencyKey);
                }

                index++;
            }

            return historyId;
        }*/

        private async Task<Guid?> ImportLawTrakEvidenceData(List<Dictionary<string, object>> evidences, Guid? historyId, int index, string folderPath, Guid agencyKey)
        {
            WriteLog("-------------------", "LawTrak", "Evidences", agencyKey);
            WriteLog($"Batch {index}", "LawTrak", "Evidences", agencyKey);
            WriteLog("-------------------", "LawTrak", "Evidences", agencyKey);
            foreach (var evidence in evidences)
            {
                string baseIDCID = "Unknown";
                string iDCID = "Unknown";


                try
                {
                    var evidenceJson = JObject.FromObject(evidence);
                    baseIDCID = evidenceJson["Document"]?["EvidenceData"]?["IDC ID"]?.ToString() ?? "Unknown";
                    iDCID = baseIDCID;

                    int maxRetries = 5;
                    int attempt = 0;
                    bool success = false;
                    HttpStatusCode? statusCode = null;
                    string message = "";

                    while (!success && attempt < maxRetries)
                    {
                        attempt++;

                        try
                        {
                            EnsureKeyExists(evidenceJson, "Document");
                            EnsureKeyExists(evidenceJson, "Document.EvidenceData");
                            EnsureKeyExists(evidenceJson, "Document.EvidenceData['IDC ID']");
                            evidenceJson["Document"]["EvidenceData"]["IDC ID"] = iDCID;
                        }
                        catch (Exception ex)
                        {
                            WriteLog($"Evidence {index}: Missing required key → {ex.Message}", "LawTrak", "Evidences", agencyKey);
                            break; // this is not retryable
                        }

                        var result = await _lawTrakEvidenceImportService
                            .ImportEvidencesAsync(apiToken, evidenceJson, iDCID, historyId, folderPath, agencyKey);

                        success = result.Success;
                        statusCode = result.StatusCode;
                        message = result.Message;

                        if (success)
                            break;

                        if (statusCode == HttpStatusCode.Unauthorized)
                        {
                            WriteLog($"Evidence {index} & (Evidence IDC ID {iDCID}): Unauthorized, refreshing token (attempt {attempt})",
                                     "LawTrak", "Evidences", agencyKey);

                            await CreateNewToken();
                            continue;
                        }

                        if (statusCode == HttpStatusCode.Conflict)
                        {
                            iDCID = $"{baseIDCID}_{attempt}";
                            continue;
                        }

                        // Any other failure → backoff and retry
                        await Task.Delay(3000);
                    }

                }
                catch (Exception ex)
                {
                    var logMessage = $"Evidence {index} (Evidence IDC ID {iDCID}) exception: {ex}";
                    WriteLog(logMessage, "LawTrak", "Evidences", agencyKey);
                }

                index++;
            }

            return historyId;
        }

        #endregion

        #region LawTrak Accounting Process

        private async Task ProcessLawTrakAccountingExportImport()
        {
            var accountingTypes = new[]
            {
        "sp_Accounting_Monthly_Reports_Mapped"
    };

            var agencyKey = Guid.Parse(txtAgencyKey.Text);
            var folderPath = txtLTFolderPath.Text;
            const int pageSize = 100;

            foreach (var storedProcedureName in accountingTypes)
            {
                WriteLog("Executing: " + storedProcedureName, "LawTrak", "Accounting Monthly Reports", agencyKey);
                int pageNumber = 1;
                bool hasMore = true;
                Guid? historyId = null;

                while (hasMore)
                {
                    // Calling export service with paging params
                    var monthlyReports = await Task.Run(() =>
                        _exportService.ExportData(
                            sqlConnectionString,
                            storedProcedureName,
                            agencyKey,
                            pageNumber,
                            pageSize
                        )
                    );

                    if (monthlyReports != null && monthlyReports.Count > 0)
                    {
                        historyId = await ImportLawTrakAccountingData(monthlyReports, historyId, (pageNumber - 1) * pageSize, folderPath, agencyKey);

                        // If fewer rows returned than page size, last page
                        hasMore = monthlyReports.Count == pageSize;
                    }
                    else
                    {
                        hasMore = false;
                    }

                    pageNumber++;
                }
            }
        }
        /*
        private async Task<Guid?> ImportLawTrakAccountingData(List<Dictionary<string, object>> monthlyReports, Guid? historyId, int index, string folderPath, Guid agencyKey)
        {
            WriteLog("-------------------", "LawTrak", "Accounting Monthly Reports", agencyKey);
            WriteLog($"Batch {index}", "LawTrak", "Accounting Monthly Reports", agencyKey);
            WriteLog("-------------------", "LawTrak", "Accounting Monthly Reports", agencyKey);
            foreach (var monthlyReport in monthlyReports)
            {
                string baseIDCID = "Unknown";
                string iDCID = "Unknown";

                try
                {
                    var accountingJson = JObject.FromObject(monthlyReport);
                    baseIDCID = accountingJson["Document"]?["AccountingMonthlyData"]?["IDC ID"]?.ToString() ?? "Unknown";
                    iDCID = baseIDCID;

                    int retryCounter = 1;
                    bool success = false;
                    HttpStatusCode? statusCode = HttpStatusCode.OK;
                    string message = "";

                    do
                    {
                        accountingJson["Document"]["AccountingMonthlyData"]["IDC ID"] = iDCID;

                        var result = await _lawTrakAccountingImportService
                            .ImportAccountingAsync(apiToken, accountingJson, iDCID, historyId, folderPath, agencyKey);

                        success = result.Success;
                        statusCode = result.StatusCode;
                        message = result.Message;

                        if (statusCode == HttpStatusCode.Unauthorized)
                        {
                            await CreateNewToken();

                            result = await _lawTrakAccountingImportService
                            .ImportAccountingAsync(apiToken, accountingJson, iDCID, historyId, folderPath, agencyKey);

                            success = result.Success;
                            statusCode = result.StatusCode;
                            message = result.Message;
                        }

                        if (!success && statusCode == HttpStatusCode.Conflict)
                        {
                            iDCID = $"{baseIDCID}_{retryCounter++}";
                        }

                    }
                    while (!success && statusCode == HttpStatusCode.Conflict);

                    if (success)
                    {
                        Console.WriteLine($"Accounting Monthly Reports {index}: Successfully imported Accounting Monthly Reports IDC ID {iDCID}");
                    }
                    else
                    {
                        // All retries failed
                        var logMessage = $"Insertion FAILED for Accounting Monthly Reports IDC ID {iDCID}: {statusCode} - {message}";
                        WriteLog(logMessage, "LawTrak", "Accounting Monthly Reports", agencyKey);
                    }
                }
                catch (Exception ex)
                {
                    var logMessage = $"Accounting Monthly Reports {index} (Accounting Monthly Reports IDC ID {iDCID}) exception: {ex}";
                    WriteLog(logMessage, "LawTrak", "Accounting Monthly Reports", agencyKey);
                }

                index++;
            }

            return historyId;
        }*/

        private async Task<Guid?> ImportLawTrakAccountingData(List<Dictionary<string, object>> monthlyReports, Guid? historyId, int index, string folderPath, Guid agencyKey)
        {
            WriteLog("-------------------", "LawTrak", "Accounting Monthly Reports", agencyKey);
            WriteLog($"Batch {index}", "LawTrak", "Accounting Monthly Reports", agencyKey);
            WriteLog("-------------------", "LawTrak", "Accounting Monthly Reports", agencyKey);
            foreach (var monthlyReport in monthlyReports)
            {
                string baseIDCID = "Unknown";
                string iDCID = "Unknown";

                try
                {
                    var accountingJson = JObject.FromObject(monthlyReport);
                    baseIDCID = accountingJson["Document"]?["AccountingMonthlyData"]?["IDC ID"]?.ToString() ?? "Unknown";
                    iDCID = baseIDCID;

                    int maxRetries = 5;
                    int attempt = 0;
                    bool success = false;
                    HttpStatusCode? statusCode = null;
                    string message = "";

                    while (!success && attempt < maxRetries)
                    {
                        attempt++;

                        try
                        {
                            EnsureKeyExists(accountingJson, "Document");
                            EnsureKeyExists(accountingJson, "Document.AccountingMonthlyData");
                            EnsureKeyExists(accountingJson, "Document.AccountingMonthlyData['IDC ID']");
                            accountingJson["Document"]["AccountingMonthlyData"]["IDC ID"] = iDCID;
                        }
                        catch (Exception ex)
                        {
                            WriteLog($"Accounting Monthly Reports {index}: Missing required key → {ex.Message}", "LawTrak", "Accounting Monthly Reports", agencyKey);
                            break; // this is not retryable
                        }

                        var result = await _lawTrakAccountingImportService
                            .ImportAccountingAsync(apiToken, accountingJson, iDCID, historyId, folderPath, agencyKey);

                        success = result.Success;
                        statusCode = result.StatusCode;
                        message = result.Message;

                        if (success)
                            break;

                        if (statusCode == HttpStatusCode.Unauthorized)
                        {
                            WriteLog($"Accounting Monthly Reports {index} & (Accounting Monthly Reports IDC ID {iDCID}): Unauthorized, refreshing token (attempt {attempt})",
                                     "LawTrak", "Accounting Monthly Reports", agencyKey);

                            await CreateNewToken();
                            continue;
                        }

                        if (statusCode == HttpStatusCode.Conflict)
                        {
                            iDCID = $"{baseIDCID}_{attempt}";
                            continue;
                        }

                        // Any other failure → backoff and retry
                        await Task.Delay(3000);
                    }

                }
                catch (Exception ex)
                {
                    var logMessage = $"Accounting Monthly Report {index} (Accounting Monthly Reports IDC ID {iDCID}) exception: {ex}";
                    WriteLog(logMessage, "LawTrak", "Accounting Monthly Reports", agencyKey);
                }

                index++;
            }

            return historyId;
        }

        #endregion

        #region LawTrak Juvenile Process

        private async Task ProcessLawTrakJuvenileExportImport()
        {
            var juvenileTypes = new[]
            {
        "sp_JuvenilePetitions_Mapped",
        "sp_JuvenileCompalints_Mapped"
    };

            var agencyKey = Guid.Parse(txtAgencyKey.Text);
            var folderPath = txtLTFolderPath.Text;
            const int pageSize = 100;

            foreach (var storedProcedureName in juvenileTypes)
            {
                WriteLog("Executing: " + storedProcedureName, "LawTrak", "Juvenile", agencyKey);
                int pageNumber = 1;
                bool hasMore = true;
                Guid? historyId = null;

                while (hasMore)
                {
                    // Calling export service with paging params
                    var juveniles = await Task.Run(() =>
                        _exportService.ExportData(
                            sqlConnectionString,
                            storedProcedureName,
                            agencyKey,
                            pageNumber,
                            pageSize
                        )
                    );

                    if (juveniles != null && juveniles.Count > 0)
                    {
                        historyId = await ImportLawTrakJuvenileData(juveniles, historyId, (pageNumber - 1) * pageSize, folderPath, agencyKey);

                        // If fewer rows returned than page size, last page
                        hasMore = juveniles.Count == pageSize;
                    }
                    else
                    {
                        hasMore = false;
                    }

                    pageNumber++;
                }
            }
        }
        /*
        private async Task<Guid?> ImportLawTrakJuvenileData(List<Dictionary<string, object>> juveniles, Guid? historyId, int index, string folderPath, Guid agencyKey)
        {
            WriteLog("-------------------", "LawTrak", "Juvenile", agencyKey);
            WriteLog($"Batch {index}", "LawTrak", "Juvenile", agencyKey);
            WriteLog("-------------------", "LawTrak", "Juvenile", agencyKey);
            foreach (var juvenile in juveniles)
            {
                string baseIDCID = "Unknown";
                string iDCID = "Unknown";

                try
                {
                    var juvenileJson = JObject.FromObject(juvenile);
                    baseIDCID = juvenileJson["Document"]?["JuvenilesData"]?["IDC ID"]?.ToString() ?? "Unknown";
                    iDCID = baseIDCID;

                    int retryCounter = 1;
                    bool success = false;
                    HttpStatusCode? statusCode = HttpStatusCode.OK;
                    string message = "";

                    do
                    {
                        juvenileJson["Document"]["JuvenilesData"]["IDC ID"] = iDCID;

                        var result = await _lawTrakJuvenileImportService
                            .ImportJuvenileAsync(apiToken, juvenileJson, iDCID, historyId, folderPath, agencyKey);

                        success = result.Success;
                        statusCode = result.StatusCode;
                        message = result.Message;

                        if (statusCode == HttpStatusCode.Unauthorized)
                        {
                            await CreateNewToken();

                            result = await _lawTrakJuvenileImportService
                            .ImportJuvenileAsync(apiToken, juvenileJson, iDCID, historyId, folderPath, agencyKey);

                            success = result.Success;
                            statusCode = result.StatusCode;
                            message = result.Message;
                        }

                        if (!success && statusCode == HttpStatusCode.Conflict)
                        {
                            iDCID = $"{baseIDCID}_{retryCounter++}";
                        }

                    }
                    while (!success && statusCode == HttpStatusCode.Conflict);

                    if (success)
                    {
                        Console.WriteLine($"Juvenile {index}: Successfully imported Juvenile IDC ID {iDCID}");
                    }
                    else
                    {
                        // All retries failed
                        var logMessage = $"Insertion FAILED for Juvenile Reports IDC ID {iDCID}: {statusCode} - {message}";
                        WriteLog(logMessage, "LawTrak", "Juvenile", agencyKey);
                    }
                }
                catch (Exception ex)
                {
                    var logMessage = $"Juvenile Reports {index} (Juvenile IDC ID {iDCID}) exception: {ex}";
                    WriteLog(logMessage, "LawTrak", "Juvenile", agencyKey);
                }

                index++;
            }

            return historyId;
        }*/

        private async Task<Guid?> ImportLawTrakJuvenileData(List<Dictionary<string, object>> juveniles, Guid? historyId, int index, string folderPath, Guid agencyKey)
        {
            WriteLog("-------------------", "LawTrak", "Juvenile", agencyKey);
            WriteLog($"Batch {index}", "LawTrak", "Juvenile", agencyKey);
            WriteLog("-------------------", "LawTrak", "Juvenile", agencyKey);
            foreach (var juvenile in juveniles)
            {
                string baseIDCID = "Unknown";
                string iDCID = "Unknown";

                try
                {
                    var juvenileJson = JObject.FromObject(juvenile);
                    baseIDCID = juvenileJson["Document"]?["JuvenilesData"]?["IDC ID"]?.ToString() ?? "Unknown";
                    iDCID = baseIDCID;

                    int maxRetries = 5;
                    int attempt = 0;
                    bool success = false;
                    HttpStatusCode? statusCode = null;
                    string message = "";

                    while (!success && attempt < maxRetries)
                    {
                        attempt++;

                        try
                        {
                            EnsureKeyExists(juvenileJson, "Document");
                            EnsureKeyExists(juvenileJson, "Document.JuvenilesData");
                            EnsureKeyExists(juvenileJson, "Document.JuvenilesData['IDC ID']");
                            juvenileJson["Document"]["JuvenilesData"]["IDC ID"] = iDCID;
                        }
                        catch (Exception ex)
                        {
                            WriteLog($"Juvenile {index}: Missing required key → {ex.Message}", "LawTrak", "Juvenile", agencyKey);
                            break; // this is not retryable
                        }

                        var result = await _lawTrakJuvenileImportService
                            .ImportJuvenileAsync(apiToken, juvenileJson, iDCID, historyId, folderPath, agencyKey);

                        success = result.Success;
                        statusCode = result.StatusCode;
                        message = result.Message;

                        if (success)
                            break;

                        if (statusCode == HttpStatusCode.Unauthorized)
                        {
                            WriteLog($"Juvenile {index} & (Juvenile IDC ID {iDCID}): Unauthorized, refreshing token (attempt {attempt})",
                                     "LawTrak", "Juvenile", agencyKey);

                            await CreateNewToken();
                            continue;
                        }

                        if (statusCode == HttpStatusCode.Conflict)
                        {
                            iDCID = $"{baseIDCID}_{attempt}";
                            continue;
                        }

                        // Any other failure → backoff and retry
                        await Task.Delay(3000);
                    }

                }
                catch (Exception ex)
                {
                    var logMessage = $"Juvenile {index} (Juvenile IDC ID {iDCID}) exception: {ex}";
                    WriteLog(logMessage, "LawTrak", "Juvenile", agencyKey);
                }

                index++;
            }

            return historyId;
        }

        #endregion

        #region LawTrak Booking Process

        private async Task ProcessLawTrakBookingExportImport()
        {
            var bookingTypes = new[]
            {
        "sp_Bookings_Mapped"
            };

            var agencyKey = Guid.Parse(txtAgencyKey.Text);
            var folderPath = txtLTFolderPath.Text;
            const int pageSize = 100;

            foreach (var storedProcedureName in bookingTypes)
            {
                WriteLog("Executing: " + storedProcedureName, "LawTrak", "Bookings", agencyKey);

                int pageNumber = 1;
                bool hasMore = true;
                Guid? historyId = null;

                while (hasMore)
                {
                    // Calling export service with paging params
                    var bookings = await Task.Run(() =>
                        _exportService.ExportData(
                            sqlConnectionString,
                            storedProcedureName,
                            agencyKey,
                            pageNumber,
                            pageSize
                        )
                    );

                    if (bookings != null && bookings.Count > 0)
                    {
                        historyId = await ImportLawTrakBookingData(bookings, historyId, (pageNumber - 1) * pageSize, folderPath, agencyKey);

                        // If fewer rows returned than page size, last page
                        hasMore = bookings.Count == pageSize;
                    }
                    else
                    {
                        hasMore = false;
                    }

                    pageNumber++;
                }
            }
        }
        /*
        private async Task<Guid?> ImportLawTrakBookingData(List<Dictionary<string, object>> bookings, Guid? historyId, int index, string folderPath, Guid agencyKey)
        {
            WriteLog("-------------------", "LawTrak", "Bookings", agencyKey);
            WriteLog($"Batch {index}", "LawTrak", "Bookings", agencyKey);
            WriteLog("-------------------", "LawTrak", "Bookings", agencyKey);
            foreach (var booking in bookings)
            {
                string baseIDCID = "Unknown";
                string iDCID = "Unknown";

                try
                {
                    var bookingJson = JObject.FromObject(booking);
                    baseIDCID = bookingJson["Document"]?["BookingsData"]?["IDC ID"]?.ToString() ?? "Unknown";
                    iDCID = baseIDCID;

                    int retryCounter = 1;
                    bool success = false;
                    HttpStatusCode? statusCode = HttpStatusCode.OK;
                    string message = "";

                    do
                    {
                        bookingJson["Document"]["BookingsData"]["IDC ID"] = iDCID;

                        var result = await _lawTrakBookingImportService
                            .ImportBookingAsync(apiToken, bookingJson, iDCID, historyId, folderPath, agencyKey);

                        success = result.Success;
                        statusCode = result.StatusCode;
                        message = result.Message;

                        if (statusCode == HttpStatusCode.Unauthorized)
                        {
                            await CreateNewToken();

                            result = await _lawTrakBookingImportService
                            .ImportBookingAsync(apiToken, bookingJson, iDCID, historyId, folderPath, agencyKey);

                            success = result.Success;
                            statusCode = result.StatusCode;
                            message = result.Message;
                        }

                        if (!success && statusCode == HttpStatusCode.Conflict)
                        {
                            iDCID = $"{baseIDCID}_{retryCounter++}";
                        }

                    }
                    while (!success && statusCode == HttpStatusCode.Conflict);

                    if (success)
                    {
                        Console.WriteLine($"Booking {index}: Successfully imported Booking IDC ID {iDCID}");
                    }
                    else
                    {
                        // All retries failed
                        var logMessage = $"Insertion FAILED for Booking IDC ID {iDCID}: {statusCode} - {message}";
                        WriteLog(logMessage, "LawTrak", "Bookings", agencyKey);
                    }
                }
                catch (Exception ex)
                {
                    var logMessage = $"Booking {index} (Booking IDC ID {iDCID}) exception: {ex}";
                    WriteLog(logMessage, "LawTrak", "Bookings", agencyKey);
                }

                index++;
            }

            return historyId;
        }*/

        private async Task<Guid?> ImportLawTrakBookingData(List<Dictionary<string, object>> bookings, Guid? historyId, int index, string folderPath, Guid agencyKey)
        {
            WriteLog("-------------------", "LawTrak", "Bookings", agencyKey);
            WriteLog($"Batch {index}", "LawTrak", "Bookings", agencyKey);
            WriteLog("-------------------", "LawTrak", "Bookings", agencyKey);
            foreach (var booking in bookings)
            {
                string baseIDCID = "Unknown";
                string iDCID = "Unknown";

                try
                {
                    var bookingJson = JObject.FromObject(booking);
                    baseIDCID = bookingJson["Document"]?["BookingsData"]?["IDC ID"]?.ToString() ?? "Unknown";
                    iDCID = baseIDCID;

                    int maxRetries = 5;
                    int attempt = 0;
                    bool success = false;
                    HttpStatusCode? statusCode = null;
                    string message = "";

                    while (!success && attempt < maxRetries)
                    {
                        attempt++;

                        try
                        {
                            EnsureKeyExists(bookingJson, "Document");
                            EnsureKeyExists(bookingJson, "Document.BookingsData");
                            EnsureKeyExists(bookingJson, "Document.BookingsData['IDC ID']");
                            bookingJson["Document"]["BookingsData"]["IDC ID"] = iDCID;
                        }
                        catch (Exception ex)
                        {
                            WriteLog($"Booking {index}: Missing required key → {ex.Message}", "LawTrak", "Bookings", agencyKey);
                            break; // this is not retryable
                        }

                        var result = await _lawTrakBookingImportService
                            .ImportBookingAsync(apiToken, bookingJson, iDCID, historyId, folderPath, agencyKey);

                        success = result.Success;
                        statusCode = result.StatusCode;
                        message = result.Message;

                        if (success)
                            break;

                        if (statusCode == HttpStatusCode.Unauthorized)
                        {
                            WriteLog($"Booking {index} & (Booking IDC ID {iDCID}): Unauthorized, refreshing token (attempt {attempt})",
                                     "LawTrak", "Bookings", agencyKey);

                            await CreateNewToken();
                            continue;
                        }

                        if (statusCode == HttpStatusCode.Conflict)
                        {
                            iDCID = $"{baseIDCID}_{attempt}";
                            continue;
                        }

                        // Any other failure → backoff and retry
                        await Task.Delay(3000);
                    }

                }
                catch (Exception ex)
                {
                    var logMessage = $"Booking {index} (Booking IDC ID {iDCID}) exception: {ex}";
                    WriteLog(logMessage, "LawTrak", "Bookings", agencyKey);
                }

                index++;
            }

            return historyId;
        }

        #endregion

        #region LawTrak Summon Process

        private async Task ProcessLawTrakSummonExportImport()
        {
            var summonTypes = new[]
            {
        "sp_Summons_Mapped"
    };

            var agencyKey = Guid.Parse(txtAgencyKey.Text);
            var folderPath = txtLTFolderPath.Text;
            const int pageSize = 100;

            foreach (var storedProcedureName in summonTypes)
            {
                WriteLog("Executing: " + storedProcedureName, "LawTrak", "Summons", agencyKey);

                int pageNumber = 1;
                bool hasMore = true;
                Guid? historyId = null;

                while (hasMore)
                {
                    // Calling export service with paging params
                    var summons = await Task.Run(() =>
                        _exportService.ExportData(
                            sqlConnectionString,
                            storedProcedureName,
                            agencyKey,
                            pageNumber,
                            pageSize
                        )
                    );

                    if (summons != null && summons.Count > 0)
                    {
                        historyId = await ImportLawTrakSummonData(summons, historyId, (pageNumber - 1) * pageSize, folderPath, agencyKey);

                        // If fewer rows returned than page size, last page
                        hasMore = summons.Count == pageSize;
                    }
                    else
                    {
                        hasMore = false;
                    }

                    pageNumber++;
                }
            }
        }

        /*      
                private async Task<Guid?> ImportLawTrakSummonData(List<Dictionary<string, object>> Summons, Guid? historyId, int index, string folderPath, Guid agencyKey)
                {
                    WriteLog("-------------------", "LawTrak", "Summons", agencyKey);
                    WriteLog($"Batch {index}", "LawTrak", "Summons", agencyKey);
                    WriteLog("-------------------", "LawTrak", "Summons", agencyKey);
                    foreach (var summon in Summons)
                    {
                        string baseIDCID = "Unknown";
                        string iDCID = "Unknown";

                        try
                        {
                            var summonJson = JObject.FromObject(summon);
                            baseIDCID = summonJson["Document"]?["SummonsData"]?["IDC ID"]?.ToString() ?? "Unknown";
                            iDCID = baseIDCID;

                            int retryCounter = 1;
                            bool success = false;
                            HttpStatusCode? statusCode = HttpStatusCode.OK;
                            string message = "";

                            do
                            {
                                summonJson["Document"]["SummonsData"]["IDC ID"] = iDCID;

                                var result = await _lawTrakSummonImportService
                                    .ImportSummonAsync(apiToken, summonJson, iDCID, historyId, folderPath, agencyKey);

                                success = result.Success;
                                statusCode = result.StatusCode;
                                message = result.Message;

                                if (statusCode == HttpStatusCode.Unauthorized)
                                {
                                    await CreateNewToken();

                                    result = await _lawTrakSummonImportService
                                    .ImportSummonAsync(apiToken, summonJson, iDCID, historyId, folderPath, agencyKey);

                                    success = result.Success;
                                    statusCode = result.StatusCode;
                                    message = result.Message;
                                }

                                if (!success && statusCode == HttpStatusCode.Conflict)
                                {
                                    iDCID = $"{baseIDCID}_{retryCounter++}";
                                }

                            }
                            while (!success && statusCode == HttpStatusCode.Conflict);

                            if (success)
                            {
                                Console.WriteLine($"Summon {index}: Successfully imported Summon IDC ID {iDCID}");
                            }
                            else
                            {
                                // All retries failed
                                var logMessage = $"Insertion FAILED for Summon IDC ID {iDCID}: {statusCode} - {message}";
                                WriteLog(logMessage, "LawTrak", "Summons", agencyKey);
                            }
                        }
                        catch (Exception ex)
                        {
                            var logMessage = $"Summon {index} (Summon IDC ID {iDCID}) exception: {ex}";
                            WriteLog(logMessage, "LawTrak", "Summons", agencyKey);
                        }

                        index++;
                    }

                    return historyId;
                }*/

        private async Task<Guid?> ImportLawTrakSummonData(List<Dictionary<string, object>> Summons, Guid? historyId, int index, string folderPath, Guid agencyKey)
        {
            WriteLog("-------------------", "LawTrak", "Summons", agencyKey);
            WriteLog($"Batch {index}", "LawTrak", "Summons", agencyKey);
            WriteLog("-------------------", "LawTrak", "Summons", agencyKey);
            foreach (var summon in Summons)
            {
                string baseIDCID = "Unknown";
                string iDCID = "Unknown";

                try
                {
                    var summonJson = JObject.FromObject(summon);
                    baseIDCID = summonJson["Document"]?["SummonsData"]?["IDC ID"]?.ToString() ?? "Unknown";
                    iDCID = baseIDCID;

                    int maxRetries = 5;
                    int attempt = 0;
                    bool success = false;
                    HttpStatusCode? statusCode = null;
                    string message = "";

                    while (!success && attempt < maxRetries)
                    {
                        attempt++;

                        try
                        {
                            EnsureKeyExists(summonJson, "Document");
                            EnsureKeyExists(summonJson, "Document.SummonsData");
                            EnsureKeyExists(summonJson, "Document.SummonsData['IDC ID']");
                            summonJson["Document"]["SummonsData"]["IDC ID"] = iDCID;
                        }
                        catch (Exception ex)
                        {
                            WriteLog($"Booking {index}: Missing required key → {ex.Message}", "LawTrak", "Summons", agencyKey);
                            break; // this is not retryable
                        }

                        var result = await _lawTrakSummonImportService
                            .ImportSummonAsync(apiToken, summonJson, iDCID, historyId, folderPath, agencyKey);

                        success = result.Success;
                        statusCode = result.StatusCode;
                        message = result.Message;

                        if (success)
                            break;

                        if (statusCode == HttpStatusCode.Unauthorized)
                        {
                            WriteLog($"Summon {index} & (Summon IDC ID {iDCID}): Unauthorized, refreshing token (attempt {attempt})",
                                     "LawTrak", "Summons", agencyKey);

                            await CreateNewToken();
                            continue;
                        }

                        if (statusCode == HttpStatusCode.Conflict)
                        {
                            iDCID = $"{baseIDCID}_{attempt}";
                            continue;
                        }

                        // Any other failure → backoff and retry
                        await Task.Delay(3000);
                    }

                }
                catch (Exception ex)
                {
                    var logMessage = $"Summon {index} (Summon IDC ID {iDCID}) exception: {ex}";
                    WriteLog(logMessage, "LawTrak", "Summons", agencyKey);
                }

                index++;
            }

            return historyId;
        }

        #endregion

        #region LawTrak Subpoena Process

        private async Task ProcessLawTrakSubpoenaExportImport()
        {
            var subpoenaTypes = new[]
            {
        "sp_Subpoenas_Mapped"
    };

            var agencyKey = Guid.Parse(txtAgencyKey.Text);
            var folderPath = txtLTFolderPath.Text;
            const int pageSize = 100;

            foreach (var storedProcedureName in subpoenaTypes)
            {
                WriteLog("Executing: " + storedProcedureName, "LawTrak", "Subpoena", agencyKey);

                int pageNumber = 1;
                bool hasMore = true;
                Guid? historyId = null;

                while (hasMore)
                {
                    // Calling export service with paging params
                    var subpoenas = await Task.Run(() =>
                        _exportService.ExportData(
                            sqlConnectionString,
                            storedProcedureName,
                            agencyKey,
                            pageNumber,
                            pageSize
                        )
                    );

                    if (subpoenas != null && subpoenas.Count > 0)
                    {
                        historyId = await ImportLawTrakSubpoenaData(subpoenas, historyId, (pageNumber - 1) * pageSize, folderPath, agencyKey);

                        // If fewer rows returned than page size, last page
                        hasMore = subpoenas.Count == pageSize;
                    }
                    else
                    {
                        hasMore = false;
                    }

                    pageNumber++;
                }
            }
        }

        private async Task<Guid?> ImportLawTrakSubpoenaData(List<Dictionary<string, object>> Subpoena, Guid? historyId, int index, string folderPath, Guid agencyKey)
        {
            WriteLog("-------------------", "LawTrak", "Subpoena", agencyKey);
            WriteLog($"Batch {index}", "LawTrak", "Subpoena", agencyKey);
            WriteLog("-------------------", "LawTrak", "Subpoena", agencyKey);
            foreach (var subpoena in Subpoena)
            {
                string baseIDCID = "Unknown";
                string iDCID = "Unknown";

                try
                {
                    var subpoenaJson = JObject.FromObject(subpoena);
                    baseIDCID = subpoenaJson["Document"]?["SubpoenasData"]?["IDC ID"]?.ToString() ?? "Unknown";
                    iDCID = baseIDCID;

                    int retryCounter = 1;
                    bool success = false;
                    HttpStatusCode? statusCode = HttpStatusCode.OK;
                    string message = "";

                    do
                    {
                        subpoenaJson["Document"]["SubpoenasData"]["IDC ID"] = iDCID;

                        var result = await _lawTrakSubpoenaJsonImportService
                            .ImportSubpoenaAsync(apiToken, subpoenaJson, iDCID, historyId, folderPath, agencyKey);

                        success = result.Success;
                        statusCode = result.StatusCode;
                        message = result.Message;

                        if (statusCode == HttpStatusCode.Unauthorized)
                        {
                            await CreateNewToken();

                            result = await _lawTrakSubpoenaJsonImportService
                            .ImportSubpoenaAsync(apiToken, subpoenaJson, iDCID, historyId, folderPath, agencyKey);

                            success = result.Success;
                            statusCode = result.StatusCode;
                            message = result.Message;
                        }

                        if (!success && statusCode == HttpStatusCode.Conflict)
                        {
                            iDCID = $"{baseIDCID}_{retryCounter++}";
                        }

                    }
                    while (!success && statusCode == HttpStatusCode.Conflict);

                    if (success)
                    {
                        Console.WriteLine($"Subpoena {index}: Successfully imported Subpoena IDC ID {iDCID}");
                    }
                    else
                    {
                        // All retries failed
                        var logMessage = $"Insertion FAILED for Subpoena IDC ID {iDCID}: {statusCode} - {message}";
                        WriteLog(logMessage, "LawTrak", "Subpoena", agencyKey);
                    }
                }
                catch (Exception ex)
                {
                    var logMessage = $"Subpoena {index} (Subpoena IDC ID {iDCID}) exception: {ex}";
                    WriteLog(logMessage, "LawTrak", "Subpoena", agencyKey);
                }

                index++;
            }

            return historyId;
        }

        #endregion

        #region LawTrak Personnel Process

        private async Task ProcessLawTrakPersonnelExportImport()
        {
            var subpoenaTypes = new[]
            {
                "sp_Master_Employee_Mapped",
                "sp_Inventory_Maintenance_Mapped"
    };

            var agencyKey = Guid.Parse(txtAgencyKey.Text);
            var folderPath = txtLTFolderPath.Text;
            const int pageSize = 100;

            foreach (var storedProcedureName in subpoenaTypes)
            {
                WriteLog("Executing: " + storedProcedureName, "LawTrak", "Personnel", agencyKey);

                int pageNumber = 1;
                bool hasMore = true;
                Guid? historyId = null;

                while (hasMore)
                {
                    // Calling export service with paging params
                    var personnels = await Task.Run(() =>
                        _exportService.ExportData(
                            sqlConnectionString,
                            storedProcedureName,
                            agencyKey,
                            pageNumber,
                            pageSize
                        )
                    );

                    if (personnels != null && personnels.Count > 0)
                    {
                        historyId = await ImportLawTrakPersonnelData(personnels, historyId, (pageNumber - 1) * pageSize, folderPath, agencyKey);

                        // If fewer rows returned than page size, last page
                        hasMore = personnels.Count == pageSize;
                    }
                    else
                    {
                        hasMore = false;
                    }

                    pageNumber++;
                }
            }
        }
        /*
        private async Task<Guid?> ImportLawTrakPersonnelData(List<Dictionary<string, object>> Personnel, Guid? historyId, int index, string folderPath, Guid agencyKey)
        {
            WriteLog("-------------------", "LawTrak", "Personnel", agencyKey);
            WriteLog($"Batch {index}", "LawTrak", "Personnel", agencyKey);
            WriteLog("-------------------", "LawTrak", "Personnel", agencyKey);
            foreach (var personnel in Personnel)
            {
                string baseIDCID = "Unknown";
                string iDCID = "Unknown";

                try
                {
                    var personnelJson = JObject.FromObject(personnel);
                    baseIDCID = personnelJson["Document"]?["PersonnelData"]?["IDC ID"]?.ToString() ?? "Unknown";
                    iDCID = baseIDCID;

                    int retryCounter = 1;
                    bool success = false;
                    HttpStatusCode? statusCode = HttpStatusCode.OK;
                    string message = "";

                    do
                    {
                        personnelJson["Document"]["PersonnelData"]["IDC ID"] = iDCID;

                        var result = await _lawTrakPersonnelJsonImportService
                            .ImportPersonnelAsync(apiToken, personnelJson, iDCID, historyId, folderPath, agencyKey);

                        success = result.Success;
                        statusCode = result.StatusCode;
                        message = result.Message;

                        if (statusCode == HttpStatusCode.Unauthorized)
                        {
                            await CreateNewToken();

                            result = await _lawTrakPersonnelJsonImportService
                            .ImportPersonnelAsync(apiToken, personnelJson, iDCID, historyId, folderPath, agencyKey);

                            success = result.Success;
                            statusCode = result.StatusCode;
                            message = result.Message;
                        }

                        if (!success && statusCode == HttpStatusCode.Conflict)
                        {
                            iDCID = $"{baseIDCID}_{retryCounter++}";
                        }

                    }
                    while (!success && statusCode == HttpStatusCode.Conflict);

                    if (success)
                    {
                        Console.WriteLine($"Personnel {index}: Successfully imported Personnel IDC ID {iDCID}");
                    }
                    else
                    {
                        // All retries failed
                        var logMessage = $"Insertion FAILED for Personnel IDC ID {iDCID}: {statusCode} - {message}";
                        WriteLog(logMessage, "LawTrak", "Personnel", agencyKey);
                    }
                }
                catch (Exception ex)
                {
                    var logMessage = $"Personnel {index} (Personnel IDC ID {iDCID}) exception: {ex}";
                    WriteLog(logMessage, "LawTrak", "Personnel", agencyKey);
                }

                index++;
            }

            return historyId;
        }*/

        private async Task<Guid?> ImportLawTrakPersonnelData(List<Dictionary<string, object>> Personnel, Guid? historyId, int index, string folderPath, Guid agencyKey)
        {
            WriteLog("-------------------", "LawTrak", "Personnel", agencyKey);
            WriteLog($"Batch {index}", "LawTrak", "Personnel", agencyKey);
            WriteLog("-------------------", "LawTrak", "Personnel", agencyKey);
            foreach (var personnel in Personnel)
            {
                string baseIDCID = "Unknown";
                string iDCID = "Unknown";

                try
                {
                    var personnelJson = JObject.FromObject(personnel);
                    baseIDCID = personnelJson["Document"]?["PersonnelData"]?["IDC ID"]?.ToString() ?? "Unknown";
                    iDCID = baseIDCID;

                    int maxRetries = 5;
                    int attempt = 0;
                    bool success = false;
                    HttpStatusCode? statusCode = null;
                    string message = "";

                    while (!success && attempt < maxRetries)
                    {
                        attempt++;

                        try
                        {
                            EnsureKeyExists(personnelJson, "Document");
                            EnsureKeyExists(personnelJson, "Document.PersonnelData");
                            EnsureKeyExists(personnelJson, "Document.PersonnelData['IDC ID']");
                            personnelJson["Document"]["PersonnelData"]["IDC ID"] = iDCID;
                        }
                        catch (Exception ex)
                        {
                            WriteLog($"Personnel {index}: Missing required key → {ex.Message}", "LawTrak", "Personnel", agencyKey);
                            break; // this is not retryable
                        }

                        var result = await _lawTrakPersonnelJsonImportService
                            .ImportPersonnelAsync(apiToken, personnelJson, iDCID, historyId, folderPath, agencyKey);

                        success = result.Success;
                        statusCode = result.StatusCode;
                        message = result.Message;

                        if (success)
                            break;

                        if (statusCode == HttpStatusCode.Unauthorized)
                        {
                            WriteLog($"Personnel {index} & (Personnel IDC ID {iDCID}): Unauthorized, refreshing token (attempt {attempt})",
                                     "LawTrak", "Personnel", agencyKey);

                            await CreateNewToken();
                            continue;
                        }

                        if (statusCode == HttpStatusCode.Conflict)
                        {
                            iDCID = $"{baseIDCID}_{attempt}";
                            continue;
                        }

                        // Any other failure → backoff and retry
                        await Task.Delay(3000);
                    }

                }
                catch (Exception ex)
                {
                    var logMessage = $"Personnel {index} (Personnel IDC ID {iDCID}) exception: {ex}";
                    WriteLog(logMessage, "LawTrak", "Personnel", agencyKey);
                }

                index++;
            }

            return historyId;
        }

        #endregion

        #region LawTrak ScReceipts Process

        private async Task ProcessLawTrakScReceiptsExportImport()
        {
            var scReceiptsTypes = new[]
            {
                "sp_SC_Receipts_Mapped"
    };

            var agencyKey = Guid.Parse(txtAgencyKey.Text);
            var folderPath = txtLTFolderPath.Text;
            const int pageSize = 100;

            foreach (var storedProcedureName in scReceiptsTypes)
            {
                WriteLog("Executing: " + storedProcedureName, "LawTrak", "ScReceipts", agencyKey);

                int pageNumber = 1;
                bool hasMore = true;
                Guid? historyId = null;

                while (hasMore)
                {
                    // Calling export service with paging params
                    var scReceipts = await Task.Run(() =>
                        _exportService.ExportData(
                            sqlConnectionString,
                            storedProcedureName,
                            agencyKey,
                            pageNumber,
                            pageSize
                        )
                    );

                    if (scReceipts != null && scReceipts.Count > 0)
                    {
                        historyId = await ImportLawTrakScReceiptData(scReceipts, historyId, (pageNumber - 1) * pageSize, folderPath, agencyKey);

                        // If fewer rows returned than page size, last page
                        hasMore = scReceipts.Count == pageSize;
                    }
                    else
                    {
                        hasMore = false;
                    }

                    pageNumber++;
                }
            }
        }
        /*
        private async Task<Guid?> ImportLawTrakScReceiptData(List<Dictionary<string, object>> ScReceipts, Guid? historyId, int index, string folderPath, Guid agencyKey)
        {
            WriteLog("-------------------", "LawTrak", "ScReceipts", agencyKey);
            WriteLog($"Batch {index}", "LawTrak", "ScReceipts", agencyKey);
            WriteLog("-------------------", "LawTrak", "ScReceipts", agencyKey);
            foreach (var scReceipt in ScReceipts)
            {
                string baseIDCID = "Unknown";
                string iDCID = "Unknown";

                try
                {
                    var scReceiptJson = JObject.FromObject(scReceipt);
                    baseIDCID = scReceiptJson["Document"]?["ReceiptsData"]?["IDC ID"]?.ToString() ?? "Unknown";
                    iDCID = baseIDCID;

                    int retryCounter = 1;
                    bool success = false;
                    HttpStatusCode? statusCode = HttpStatusCode.OK;
                    string message = "";

                    do
                    {
                        scReceiptJson["Document"]["ReceiptsData"]["IDC ID"] = iDCID;

                        var result = await _lawTrakScReceiptsJsonImportService
                            .ImportScReceiptAsync(apiToken, scReceiptJson, iDCID, historyId, folderPath, agencyKey);

                        success = result.Success;
                        statusCode = result.StatusCode;
                        message = result.Message;

                        if (statusCode == HttpStatusCode.Unauthorized)
                        {
                            await CreateNewToken();

                            result = await _lawTrakScReceiptsJsonImportService
                            .ImportScReceiptAsync(apiToken, scReceiptJson, iDCID, historyId, folderPath, agencyKey);

                            success = result.Success;
                            statusCode = result.StatusCode;
                            message = result.Message;
                        }

                        if (!success && statusCode == HttpStatusCode.Conflict)
                        {
                            iDCID = $"{baseIDCID}_{retryCounter++}";
                        }

                    }
                    while (!success && statusCode == HttpStatusCode.Conflict);

                    if (success)
                    {
                        Console.WriteLine($"ScReceipt {index}: Successfully imported ScReceipt IDC ID {iDCID}");
                    }
                    else
                    {
                        // All retries failed
                        var logMessage = $"Insertion FAILED for ScReceipt IDC ID {iDCID}: {statusCode} - {message}";
                        WriteLog(logMessage, "LawTrak", "ScReceipts", agencyKey);
                    }
                }
                catch (Exception ex)
                {
                    var logMessage = $"ScReceipt {index} (ScReceipt IDC ID {iDCID}) exception: {ex}";
                    WriteLog(logMessage, "LawTrak", "ScReceipts", agencyKey);
                }

                index++;
            }

            return historyId;
        }*/

        private async Task<Guid?> ImportLawTrakScReceiptData(List<Dictionary<string, object>> ScReceipts, Guid? historyId, int index, string folderPath, Guid agencyKey)
        {
            WriteLog("-------------------", "LawTrak", "ScReceipts", agencyKey);
            WriteLog($"Batch {index}", "LawTrak", "ScReceipts", agencyKey);
            WriteLog("-------------------", "LawTrak", "ScReceipts", agencyKey);
            foreach (var scReceipt in ScReceipts)
            {
                string baseIDCID = "Unknown";
                string iDCID = "Unknown";

                try
                {
                    var scReceiptJson = JObject.FromObject(scReceipt);
                    baseIDCID = scReceiptJson["Document"]?["ReceiptsData"]?["IDC ID"]?.ToString() ?? "Unknown";
                    iDCID = baseIDCID;

                    int maxRetries = 5;
                    int attempt = 0;
                    bool success = false;
                    HttpStatusCode? statusCode = null;
                    string message = "";

                    while (!success && attempt < maxRetries)
                    {
                        attempt++;

                        try
                        {
                            EnsureKeyExists(scReceiptJson, "Document");
                            EnsureKeyExists(scReceiptJson, "Document.ReceiptsData");
                            EnsureKeyExists(scReceiptJson, "Document.ReceiptsData['IDC ID']");
                            scReceiptJson["Document"]["ReceiptsData"]["IDC ID"] = iDCID;
                        }
                        catch (Exception ex)
                        {
                            WriteLog($"ScReceipt {index}: Missing required key → {ex.Message}", "LawTrak", "ScReceipts", agencyKey);
                            break; // this is not retryable
                        }

                        var result = await _lawTrakScReceiptsJsonImportService
                            .ImportScReceiptAsync(apiToken, scReceiptJson, iDCID, historyId, folderPath, agencyKey);

                        success = result.Success;
                        statusCode = result.StatusCode;
                        message = result.Message;

                        if (success)
                            break;

                        if (statusCode == HttpStatusCode.Unauthorized)
                        {
                            WriteLog($"ScReceipt {index} & (ScReceipt IDC ID {iDCID}): Unauthorized, refreshing token (attempt {attempt})",
                                     "LawTrak", "ScReceipts", agencyKey);

                            await CreateNewToken();
                            continue;
                        }

                        if (statusCode == HttpStatusCode.Conflict)
                        {
                            iDCID = $"{baseIDCID}_{attempt}";
                            continue;
                        }

                        // Any other failure → backoff and retry
                        await Task.Delay(3000);
                    }

                }
                catch (Exception ex)
                {
                    var logMessage = $"ScReceipt {index} (ScReceipt IDC ID {iDCID}) exception: {ex}";
                    WriteLog(logMessage, "LawTrak", "ScReceipts", agencyKey);
                }

                index++;
            }

            return historyId;
        }

        #endregion

        #region LawTrak Jury Process

        private async Task ProcessLawTrakJurysExportImport()
        {
            var JuryTypes = new[]
            {
                "sp_Jury_Master_Mapped"
    };

            var agencyKey = Guid.Parse(txtAgencyKey.Text);
            var folderPath = txtLTFolderPath.Text;
            const int pageSize = 100;

            foreach (var storedProcedureName in JuryTypes)
            {
                WriteLog("Executing: " + storedProcedureName, "LawTrak", "Jury", agencyKey);

                int pageNumber = 1;
                bool hasMore = true;
                Guid? historyId = null;

                while (hasMore)
                {
                    // Calling export service with paging params
                    var jury = await Task.Run(() =>
                        _exportService.ExportData(
                            sqlConnectionString,
                            storedProcedureName,
                            agencyKey,
                            pageNumber,
                            pageSize
                        )
                    );

                    if (jury != null && jury.Count > 0)
                    {
                        historyId = await ImportLawTrakJuryData(jury, historyId, (pageNumber - 1) * pageSize, folderPath, agencyKey);

                        // If fewer rows returned than page size, last page
                        hasMore = jury.Count == pageSize;
                    }
                    else
                    {
                        hasMore = false;
                    }

                    pageNumber++;
                }
            }
        }

        /*
        private async Task<Guid?> ImportLawTrakJuryData(List<Dictionary<string, object>> Jurys, Guid? historyId, int index, string folderPath, Guid agencyKey)
        {
            WriteLog("-------------------", "LawTrak", "Jury", agencyKey);
            WriteLog($"Batch {index}", "LawTrak", "Jury", agencyKey);
            WriteLog("-------------------", "LawTrak", "Jury", agencyKey);
            foreach (var jury in Jurys)
            {
                string baseIDCID = "Unknown";
                string iDCID = "Unknown";

                try
                {
                    var juryJson = JObject.FromObject(jury);
                    baseIDCID = juryJson["Document"]?["JurysData"]?["IDC ID"]?.ToString() ?? "Unknown";
                    iDCID = baseIDCID;

                    int retryCounter = 1;
                    bool success = false;
                    HttpStatusCode? statusCode = HttpStatusCode.OK;
                    string message = "";

                    do
                    {
                        juryJson["Document"]["JurysData"]["IDC ID"] = iDCID;

                        var result = await _lawTrakJuryImportService
                            .ImportJuryAsync(apiToken, juryJson, iDCID, historyId, folderPath, agencyKey);

                        success = result.Success;
                        statusCode = result.StatusCode;
                        message = result.Message;

                        if (statusCode == HttpStatusCode.Unauthorized)
                        {
                            await CreateNewToken();

                            result = await _lawTrakJuryImportService
                            .ImportJuryAsync(apiToken, juryJson, iDCID, historyId, folderPath, agencyKey);

                            success = result.Success;
                            statusCode = result.StatusCode;
                            message = result.Message;
                        }

                        if (!success && statusCode == HttpStatusCode.Conflict)
                        {
                            iDCID = $"{baseIDCID}_{retryCounter++}";
                        }

                    }
                    while (!success && statusCode == HttpStatusCode.Conflict);

                    if (success)
                    {
                        Console.WriteLine($"Jury {index}: Successfully imported Jury IDC ID {iDCID}");
                    }
                    else
                    {
                        // All retries failed
                        var logMessage = $"Insertion FAILED for Jury IDC ID {iDCID}: {statusCode} - {message}";
                        WriteLog(logMessage, "LawTrak", "Jury", agencyKey);
                    }
                }
                catch (Exception ex)
                {
                    var logMessage = $"Jury {index} (Jury IDC ID {iDCID}) exception: {ex}";
                    WriteLog(logMessage, "LawTrak", "Jury", agencyKey);
                }

                index++;
            }

            return historyId;
        }*/

        private async Task<Guid?> ImportLawTrakJuryData(List<Dictionary<string, object>> Jurys, Guid? historyId, int index, string folderPath, Guid agencyKey)
        {
            WriteLog("-------------------", "LawTrak", "Jury", agencyKey);
            WriteLog($"Batch {index}", "LawTrak", "Jury", agencyKey);
            WriteLog("-------------------", "LawTrak", "Jury", agencyKey);
            foreach (var jury in Jurys)
            {
                string baseIDCID = "Unknown";
                string iDCID = "Unknown";

                try
                {
                    var juryJson = JObject.FromObject(jury);
                    baseIDCID = juryJson["Document"]?["JurysData"]?["IDC ID"]?.ToString() ?? "Unknown";
                    iDCID = baseIDCID;

                    int maxRetries = 5;
                    int attempt = 0;
                    bool success = false;
                    HttpStatusCode? statusCode = null;
                    string message = "";

                    while (!success && attempt < maxRetries)
                    {
                        attempt++;

                        try
                        {
                            EnsureKeyExists(juryJson, "Document");
                            EnsureKeyExists(juryJson, "Document.JurysData");
                            EnsureKeyExists(juryJson, "Document.JurysData['IDC ID']");
                            juryJson["Document"]["JurysData"]["IDC ID"] = iDCID;
                        }
                        catch (Exception ex)
                        {
                            WriteLog($"Jury {index}: Missing required key → {ex.Message}", "LawTrak", "Jury", agencyKey);
                            break; // this is not retryable
                        }

                        var result = await _lawTrakJuryImportService
                            .ImportJuryAsync(apiToken, juryJson, iDCID, historyId, folderPath, agencyKey);

                        success = result.Success;
                        statusCode = result.StatusCode;
                        message = result.Message;

                        if (success)
                            break;

                        if (statusCode == HttpStatusCode.Unauthorized)
                        {
                            WriteLog($"Jury {index} & (Jury IDC ID {iDCID}): Unauthorized, refreshing token (attempt {attempt})",
                                     "LawTrak", "Jury", agencyKey);

                            await CreateNewToken();
                            continue;
                        }

                        if (statusCode == HttpStatusCode.Conflict)
                        {
                            iDCID = $"{baseIDCID}_{attempt}";
                            continue;
                        }

                        // Any other failure → backoff and retry
                        await Task.Delay(3000);
                    }

                }
                catch (Exception ex)
                {
                    var logMessage = $"Jury {index} (Jury IDC ID {iDCID}) exception: {ex}";
                    WriteLog(logMessage, "LawTrak", "Jury", agencyKey);
                }

                index++;
            }

            return historyId;
        }

        #endregion

        #region LawTrak PropertyChecks Process

        private async Task ProcessLawTrakPropertyChecksExportImport()
        {
            var PropertyCheckTypes = new[]
            {
                "sp_Property_Check_Mapped"
    };

            var agencyKey = Guid.Parse(txtAgencyKey.Text);
            var folderPath = txtLTFolderPath.Text;
            const int pageSize = 100;

            foreach (var storedProcedureName in PropertyCheckTypes)
            {
                WriteLog("Executing: " + storedProcedureName, "LawTrak", "PropertyChecks", agencyKey);

                int pageNumber = 1;
                bool hasMore = true;
                Guid? historyId = null;

                while (hasMore)
                {
                    // Calling export service with paging params
                    var propertyChecks = await Task.Run(() =>
                        _exportService.ExportData(
                            sqlConnectionString,
                            storedProcedureName,
                            agencyKey,
                            pageNumber,
                            pageSize
                        )
                    );

                    if (propertyChecks != null && propertyChecks.Count > 0)
                    {
                        historyId = await ImportLawTrakPropertyCheckData(propertyChecks, historyId, (pageNumber - 1) * pageSize, folderPath, agencyKey);

                        // If fewer rows returned than page size, last page
                        hasMore = propertyChecks.Count == pageSize;
                    }
                    else
                    {
                        hasMore = false;
                    }

                    pageNumber++;
                }
            }
        }

        /*private async Task<Guid?> ImportLawTrakPropertyCheckData(List<Dictionary<string, object>> PropertyChecks, Guid? historyId, int index, string folderPath, Guid agencyKey)
        {
            WriteLog("-------------------", "LawTrak", "PropertyChecks", agencyKey);
            WriteLog($"Batch {index}", "LawTrak", "PropertyChecks", agencyKey);
            WriteLog("-------------------", "LawTrak", "PropertyChecks", agencyKey);
            foreach (var propertyCheck in PropertyChecks)
            {
                string baseIDCID = "Unknown";
                string iDCID = "Unknown";

                try
                {
                    var propertyCheckJson = JObject.FromObject(propertyCheck);
                    baseIDCID = propertyCheckJson["Document"]?["PropertyChecksData"]?["IDC ID"]?.ToString() ?? "Unknown";
                    iDCID = baseIDCID;

                    int retryCounter = 1;
                    bool success = false;
                    HttpStatusCode? statusCode = HttpStatusCode.OK;
                    string message = "";

                    do
                    {
                        propertyCheckJson["Document"]["PropertyChecksData"]["IDC ID"] = iDCID;

                        var result = await _lawTrakpropertyCheckImportService
                            .ImportPropertyCheckAsync(apiToken, propertyCheckJson, iDCID, historyId, folderPath, agencyKey);

                        success = result.Success;
                        statusCode = result.StatusCode;
                        message = result.Message;

                        if (statusCode == HttpStatusCode.Unauthorized)
                        {
                            await CreateNewToken();

                            result = await _lawTrakpropertyCheckImportService
                            .ImportPropertyCheckAsync(apiToken, propertyCheckJson, iDCID, historyId, folderPath, agencyKey);

                            success = result.Success;
                            statusCode = result.StatusCode;
                            message = result.Message;
                        }

                        if (!success && statusCode == HttpStatusCode.Conflict)
                        {
                            iDCID = $"{baseIDCID}_{retryCounter++}";
                        }

                    }
                    while (!success && statusCode == HttpStatusCode.Conflict);

                    if (success)
                    {
                        Console.WriteLine($"PropertyCheck {index}: Successfully imported PropertyCheck IDC ID {iDCID}");
                    }
                    else
                    {
                        // All retries failed
                        var logMessage = $"Insertion FAILED for PropertyCheck IDC ID {iDCID}: {statusCode} - {message}";
                        WriteLog(logMessage, "LawTrak", "PropertyChecks", agencyKey);
                    }
                }
                catch (Exception ex)
                {
                    var logMessage = $"PropertyCheck {index} (PropertyCheck IDC ID {iDCID}) exception: {ex}";
                    WriteLog(logMessage, "LawTrak", "PropertyChecks", agencyKey);
                }

                index++;
            }

            return historyId;
        }*/

        private async Task<Guid?> ImportLawTrakPropertyCheckData(List<Dictionary<string, object>> PropertyChecks, Guid? historyId, int index, string folderPath, Guid agencyKey)
        {
            WriteLog("-------------------", "LawTrak", "PropertyChecks", agencyKey);
            WriteLog($"Batch {index}", "LawTrak", "PropertyChecks", agencyKey);
            WriteLog("-------------------", "LawTrak", "PropertyChecks", agencyKey);
            foreach (var propertyCheck in PropertyChecks)
            {
                string baseIDCID = "Unknown";
                string iDCID = "Unknown";

                try
                {
                    var propertyCheckJson = JObject.FromObject(propertyCheck);
                    baseIDCID = propertyCheckJson["Document"]?["PropertyChecksData"]?["IDC ID"]?.ToString() ?? "Unknown";
                    iDCID = baseIDCID;

                    int maxRetries = 5;
                    int attempt = 0;
                    bool success = false;
                    HttpStatusCode? statusCode = null;
                    string message = "";

                    while (!success && attempt < maxRetries)
                    {
                        attempt++;

                        try
                        {
                            EnsureKeyExists(propertyCheckJson, "Document");
                            EnsureKeyExists(propertyCheckJson, "Document.PropertyChecksData");
                            EnsureKeyExists(propertyCheckJson, "Document.PropertyChecksData['IDC ID']");
                            propertyCheckJson["Document"]["PropertyChecksData"]["IDC ID"] = iDCID;
                        }
                        catch (Exception ex)
                        {
                            WriteLog($"PropertyCheck {index}: Missing required key → {ex.Message}", "LawTrak", "Jury", agencyKey);
                            break; // this is not retryable
                        }

                        var result = await _lawTrakpropertyCheckImportService
                            .ImportPropertyCheckAsync(apiToken, propertyCheckJson, iDCID, historyId, folderPath, agencyKey);

                        success = result.Success;
                        statusCode = result.StatusCode;
                        message = result.Message;

                        if (success)
                            break;

                        if (statusCode == HttpStatusCode.Unauthorized)
                        {
                            WriteLog($"PropertyCheck {index} & (PropertyCheck IDC ID {iDCID}): Unauthorized, refreshing token (attempt {attempt})",
                                     "LawTrak", "PropertyChecks", agencyKey);

                            await CreateNewToken();
                            continue;
                        }

                        if (statusCode == HttpStatusCode.Conflict)
                        {
                            iDCID = $"{baseIDCID}_{attempt}";
                            continue;
                        }

                        // Any other failure → backoff and retry
                        await Task.Delay(3000);
                    }

                }
                catch (Exception ex)
                {
                    var logMessage = $"PropertyCheck {index} (PropertyCheck IDC ID {iDCID}) exception: {ex}";
                    WriteLog(logMessage, "LawTrak", "PropertyChecks", agencyKey);
                }

                index++;
            }

            return historyId;
        }

        #endregion

        #region Badge Citation Process

        private async Task ProcessBadgeCitationsExportImport()
        {
            var citationTypes = new[]
            {
                "sp_Citation_Mapped"
            };

            var agencyKey = Guid.Parse(txtAgencyKey.Text);
            var folderPath = txtBadgeFolderPath.Text;
            const int pageSize = 1000;

            foreach (var storedProcedureName in citationTypes)
            {
                int pageNumber = 1;
                bool hasMore = true;
                Guid? historyId = null;

                while (hasMore)
                {
                    // Calling export service with paging params
                    var citations = await Task.Run(() =>
                        _exportService.ExportData(
                            sqlConnectionString,
                            storedProcedureName,
                            agencyKey,
                            pageNumber,
                            pageSize
                        )
                    );

                    if (citations != null && citations.Count > 0)
                    {
                        historyId = await ImportBadgeCitationData(citations, historyId, (pageNumber - 1) * pageSize, folderPath, agencyKey);

                        // If fewer rows returned than page size, last page
                        hasMore = citations.Count == pageSize;
                    }
                    else
                    {
                        hasMore = false;
                    }

                    pageNumber++;
                }
            }
        }
        /*private async Task<Guid?> ImportBadgeCitationData(List<Dictionary<string, object>> citations, Guid? historyId, int index, string folderPath, Guid agencyKey)
        {
            WriteLog("-------------------", "Badge", "Citations", agencyKey);
            WriteLog($"Batch {index}", "Badge", "Citations", agencyKey);
            WriteLog("-------------------", "Badge", "Citations", agencyKey);

            foreach (var citation in citations)
            {
                string baseIDCID = "Unknown";
                string iDCID = "Unknown";

                try
                {
                    var citationJson = JObject.FromObject(citation);
                    baseIDCID = citationJson["Document"]?["CitationData"]?["Citation Number"]?.ToString() ?? "Unknown";
                    iDCID = baseIDCID;

                    int retryCounter = 1;
                    bool success = false;
                    HttpStatusCode? statusCode = HttpStatusCode.OK;
                    string message = "";

                    do
                    {
                        citationJson["Document"]["CitationData"]["Citation Number"] = iDCID;


                        var result = await _badgeCitationImportService
                            .ImportCitationsAsync(apiToken, citationJson, iDCID, historyId, folderPath, agencyKey);



                        success = result.Success;
                        statusCode = result.StatusCode;
                        message = result.Message;

                        if (statusCode == HttpStatusCode.Unauthorized)
                        {
                            await CreateNewToken();

                            result = await _badgeCitationImportService
                            .ImportCitationsAsync(apiToken, citationJson, iDCID, historyId, folderPath, agencyKey);

                            success = result.Success;
                            statusCode = result.StatusCode;
                            message = result.Message;
                        }

                        if (!success && statusCode == HttpStatusCode.Conflict)
                        {
                            iDCID = $"{baseIDCID}_{retryCounter++}";
                        }

                    }
                    while (!success && statusCode == HttpStatusCode.Conflict);

                    if (success)
                    {
                        Console.WriteLine($"Citation {index}: Successfully imported Citation Number {iDCID}");
                    }
                    else
                    {
                        // All retries failed
                        var logMessage = $"Insertion FAILED for Citation Number {iDCID}: {statusCode} - {message}";
                        WriteLog(logMessage, "Badge", "Citations", agencyKey);
                    }
                }
                catch (Exception ex)
                {
                    var logMessage = $"Citation {index} (Citation Number {iDCID}) exception: {ex}";
                    WriteLog(logMessage, "Badge", "Citations", agencyKey);

                }

                index++;
            }

            return historyId;
        }*/

        private async Task<Guid?> ImportBadgeCitationData(List<Dictionary<string, object>> citations, Guid? historyId, int index, string folderPath, Guid agencyKey)
        {
            WriteLog("-------------------", "Badge", "Citations", agencyKey);
            WriteLog($"Batch {index}", "Badge", "Citations", agencyKey);
            WriteLog("-------------------", "Badge", "Citations", agencyKey);

            foreach (var citation in citations)
            {
                string baseIDCID = "Unknown";
                string iDCID = "Unknown";

                try
                {
                    var citationJson = JObject.FromObject(citation);
                    baseIDCID = citationJson["Document"]?["CitationData"]?["Citation Number"]?.ToString() ?? "Unknown";
                    iDCID = baseIDCID;

                    int maxRetries = 5;
                    int attempt = 0;
                    bool success = false;
                    HttpStatusCode? statusCode = null;
                    string message = "";

                    while (!success && attempt < maxRetries)
                    {
                        attempt++;

                        try
                        {
                            EnsureKeyExists(citationJson, "Document");
                            EnsureKeyExists(citationJson, "Document.CitationData");
                            EnsureKeyExists(citationJson, "Document.CitationData['Citation Number']");
                            citationJson["Document"]["CitationData"]["Citation Number"] = iDCID;
                        }
                        catch (Exception ex)
                        {
                            WriteLog($"Citation {index}: Missing required key → {ex.Message}", "Badge", "Citations", agencyKey);
                            break; // this is not retryable
                        }

                        var result = await _badgeCitationImportService
                            .ImportCitationsAsync(apiToken, citationJson, iDCID, historyId, folderPath, agencyKey);

                        success = result.Success;
                        statusCode = result.StatusCode;
                        message = result.Message;

                        if (success)
                            break;

                        if (statusCode == HttpStatusCode.Unauthorized)
                        {
                            WriteLog($"Citation {index} & (Citation IDC ID {iDCID}): Unauthorized, refreshing token (attempt {attempt})",
                                     "Badge", "Citations", agencyKey);

                            await CreateNewToken();
                            continue;
                        }

                        if (statusCode == HttpStatusCode.Conflict)
                        {
                            iDCID = $"{baseIDCID}_{attempt}";
                            continue;
                        }

                        // Any other failure → backoff and retry
                        await Task.Delay(3000);
                    }

                }
                catch (Exception ex)
                {
                    var logMessage = $"Citation {index} (Citation IDC ID {iDCID}) exception: {ex}";
                    WriteLog(logMessage, "Badge", "Citations", agencyKey);
                }

                index++;
            }

            return historyId;
        }

        #endregion

        #region Badge Warrents Process

        private async Task ProcessBadgeWarrantsExportImport()
        {
            var warrantsTypes = new[]
            {
        "sp_Warrant_Mapped"
    };

            var agencyKey = Guid.Parse(txtAgencyKey.Text);
            var folderPath = txtBadgeFolderPath.Text;
            const int pageSize = 1000;

            foreach (var storedProcedureName in warrantsTypes)
            {
                int pageNumber = 1;
                bool hasMore = true;
                Guid? historyId = null;

                while (hasMore)
                {
                    // Calling export service with paging params
                    var warrants = await Task.Run(() =>
                        _exportService.ExportData(
                            sqlConnectionString,
                            storedProcedureName,
                            agencyKey,
                            pageNumber,
                            pageSize
                        )
                    );

                    if (warrants != null && warrants.Count > 0)
                    {
                        historyId = await ImportBadgeWarrantData(warrants, historyId, (pageNumber - 1) * pageSize, folderPath, agencyKey);

                        // If we got fewer rows than requested, it’s the last page
                        hasMore = warrants.Count == pageSize;
                    }
                    else
                    {
                        hasMore = false;
                    }

                    pageNumber++;
                }
            }
        }
        /*
        private async Task<Guid?> ImportBadgeWarrantData(List<Dictionary<string, object>> warrants, Guid? historyId, int index, string folderPath, Guid agencyKey)
        {
            WriteLog("-------------------", "Badge", "Warrants", agencyKey);
            WriteLog($"Batch {index}", "Badge", "Warrants", agencyKey);
            WriteLog("-------------------", "Badge", "Warrants", agencyKey);

            foreach (var singleWarrant in warrants)
            {
                string warrantNumber = "Unknown";
                try
                {
                    // Safely get Warrant Number
                    if (singleWarrant.ContainsKey("Warrant Number"))
                    {
                        warrantNumber = singleWarrant["Warrant Number"]?.ToString() ?? "Unknown";
                    }

                    var (success, statusCode, message) = await _badgeWarrantImportService
                        .ImportWarrantsAsync(apiToken, singleWarrant, historyId, folderPath, agencyKey);

                    // Retry if unauthorized
                    if (statusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        await CreateNewToken();

                        if (!string.IsNullOrWhiteSpace(apiToken))
                        {
                            var retryResult = await _badgeWarrantImportService
                                .ImportWarrantsAsync(apiToken, singleWarrant, historyId, folderPath, agencyKey);

                            success = retryResult.Success;
                            statusCode = retryResult.StatusCode;
                            message = retryResult.Message;
                        }
                    }

                    if (success)
                    {
                        Console.WriteLine($"Warrant {index}: Successfully imported Warrant Number {warrantNumber}");
                    }
                    else
                    {
                        var logMessage = $"Insertion failed for Warrant Number {warrantNumber}: {statusCode} - {message}";
                        WriteLog(logMessage, "Badge", "Warrants", agencyKey);
                    }
                }
                catch (Exception ex)
                {
                    var logMessage = $"Warrant {index} (Warrant Number {warrantNumber}) exception: {ex}";
                    WriteLog(logMessage, "Badge", "Warrants", agencyKey);
                }

                index++;
            }

            return historyId;
        }*/

        private async Task<Guid?> ImportBadgeWarrantData(List<Dictionary<string, object>> warrants, Guid? historyId, int index, string folderPath, Guid agencyKey)
        {
            WriteLog("-------------------", "Badge", "Warrants", agencyKey);
            WriteLog($"Batch {index}", "Badge", "Warrants", agencyKey);
            WriteLog("-------------------", "Badge", "Warrants", agencyKey);

            foreach (var warrant in warrants)
            {
                string baseIDCID = "Unknown";
                string iDCID = "Unknown";

                try
                {
                    var warrantJson = JObject.FromObject(warrant);
                    baseIDCID = warrantJson["Document"]?["WarrantData"]?["Warrant Number"]?.ToString() ?? "Unknown";
                    iDCID = baseIDCID;

                    int maxRetries = 5;
                    int attempt = 0;
                    bool success = false;
                    HttpStatusCode? statusCode = null;
                    string message = "";

                    while (!success && attempt < maxRetries)
                    {
                        attempt++;

                        try
                        {
                            EnsureKeyExists(warrantJson, "Document");
                            EnsureKeyExists(warrantJson, "Document.WarrantData");
                            EnsureKeyExists(warrantJson, "Document.WarrantData['Warrant Number']");
                            warrantJson["Document"]["WarrantData"]["Warrant Number"] = iDCID;
                        }
                        catch (Exception ex)
                        {
                            WriteLog($"Warrant {index}: Missing required key → {ex.Message}", "Badge", "Warrants", agencyKey);
                            break; // this is not retryable
                        }

                        var result = await _badgeWarrantImportService
                            .ImportWarrantAsync(apiToken, warrantJson, iDCID, historyId, folderPath, agencyKey);

                        success = result.Success;
                        statusCode = result.StatusCode;
                        message = result.Message;

                        if (success)
                            break;

                        if (statusCode == HttpStatusCode.Unauthorized)
                        {
                            WriteLog($"Warrant {index} & (Warrant IDC ID {iDCID}): Unauthorized, refreshing token (attempt {attempt})",
                                     "Badge", "Warrants", agencyKey);

                            await CreateNewToken();
                            continue;
                        }

                        if (statusCode == HttpStatusCode.Conflict)
                        {
                            iDCID = $"{baseIDCID}_{attempt}";
                            continue;
                        }

                        // Any other failure → backoff and retry
                        await Task.Delay(3000);
                    }

                }
                catch (Exception ex)
                {
                    var logMessage = $"Warrant {index} (Warrant IDC ID {iDCID}) exception: {ex}";
                    WriteLog(logMessage, "Badge", "Warrants", agencyKey);
                }

                index++;
            }

            return historyId;
        }

        #endregion

        #region Badge Case Process

        private async Task ProcessBadgeCasesExportImport()
        {
            var CaseTypes = new[]
            {
                "sp_GetCaseData_JSON"
            };

            var agencyKey = Guid.Parse(txtAgencyKey.Text);
            var folderPath = txtBadgeFolderPath.Text;
            const int pageSize = 1000;

            foreach (var storedProcedureName in CaseTypes)
            {
                int pageNumber = 1;
                bool hasMore = true;
                Guid? historyId = null;

                while (hasMore)
                {
                    // Calling export service with paging params
                    var cases = await Task.Run(() =>
                        _exportService.ExportData(
                            sqlConnectionString,
                            storedProcedureName,
                            agencyKey,
                            pageNumber,
                            pageSize
                        )
                    );

                    if (cases != null && cases.Count > 0)
                    {
                        historyId = await ImportBadgeCaseData(cases, historyId, (pageNumber - 1) * pageSize, folderPath, agencyKey);

                        // If we got fewer rows than requested, it’s the last page
                        hasMore = cases.Count == pageSize;
                    }
                    else
                    {
                        hasMore = false;
                    }

                    pageNumber++;
                }
            }
        }
        /*
        private async Task<Guid?> ImportBadgeCaseData(List<Dictionary<string, object>> cases, Guid? historyId, int index, string folderPath, Guid agencyKey)
        {
            WriteLog("-------------------", "Badge", "Cases", agencyKey);
            WriteLog($"Batch {index}", "Badge", "Cases", agencyKey);
            WriteLog("-------------------", "Badge", "Cases", agencyKey);
            foreach (var singleCase in cases)
            {
                string caseNumber = "Unknown";
                try
                {
                    if (singleCase.ContainsKey("CaseData") && singleCase["CaseData"] is Dictionary<string, object> caseDataDict)
                    {
                        caseNumber = caseDataDict.ContainsKey("Case Number") ? caseDataDict["Case Number"]?.ToString() : "Unknown";
                    }

                    var (success, statusCode, message) = await _badgeCaseImportService
                        .ImportCasesAsync(apiToken, singleCase, historyId, folderPath, agencyKey);

                    // Retry if unauthorized
                    if (statusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        await CreateNewToken();

                        if (!string.IsNullOrWhiteSpace(apiToken))
                        {
                            var retryResult = await _badgeCaseImportService
                                .ImportCasesAsync(apiToken, singleCase, historyId, folderPath, agencyKey);
                            success = retryResult.Success;
                            statusCode = retryResult.StatusCode;
                            message = retryResult.Message;
                        }
                    }

                    if (success)
                    {
                        Console.WriteLine($"Case {index}: Successfully imported Case Number {caseNumber}");
                    }
                    else
                    {
                        var logMessage = $"Insertion failed for Case Number {caseNumber}: {statusCode} - {message}";
                        WriteLog(logMessage, "Badge", "Cases", agencyKey);
                    }
                }
                catch (Exception ex)
                {
                    var logMessage = $"Case {index} (Case Number {caseNumber}) exception: {ex}";
                    WriteLog(logMessage, "Badge", "Cases", agencyKey);
                }

                index++;
            }

            return historyId;
        }
        */
        private async Task<Guid?> ImportBadgeCaseData(List<Dictionary<string, object>> cases, Guid? historyId, int index, string folderPath, Guid agencyKey)
        {
            WriteLog("-------------------", "Badge", "Cases", agencyKey);
            WriteLog($"Batch {index}", "Badge", "Cases", agencyKey);
            WriteLog("-------------------", "Badge", "Cases", agencyKey);

            foreach (var singleCase in cases)
            {
                string baseIDCID = "Unknown";
                string iDCID = "Unknown";

                try
                {
                    var caseJson = JObject.FromObject(singleCase);
                    baseIDCID = caseJson["Document"]?["CaseData"]?["Case Number"]?.ToString() ?? "Unknown";
                    iDCID = baseIDCID;

                    int maxRetries = 5;
                    int attempt = 0;
                    bool success = false;
                    HttpStatusCode? statusCode = null;
                    string message = "";

                    while (!success && attempt < maxRetries)
                    {
                        attempt++;

                        try
                        {
                            EnsureKeyExists(caseJson, "Document");
                            EnsureKeyExists(caseJson, "Document.CaseData");
                            EnsureKeyExists(caseJson, "Document.CaseData['Case Number']");
                            caseJson["Document"]["CaseData"]["Case Number"] = iDCID;
                        }
                        catch (Exception ex)
                        {
                            WriteLog($"Call {index}: Missing required key → {ex.Message}", "Badge", "Cases", agencyKey);
                            break; // this is not retryable
                        }

                        var result = await _badgeCaseImportService
                            .ImportCaseAsync(apiToken, caseJson, iDCID, historyId, folderPath, agencyKey);

                        success = result.Success;
                        statusCode = result.StatusCode;
                        message = result.Message;

                        if (success)
                            break;

                        if (statusCode == HttpStatusCode.Unauthorized)
                        {
                            WriteLog($"Case {index} & (Case Number {iDCID}): Unauthorized, refreshing token (attempt {attempt})",
                                     "Badge", "Cases", agencyKey);

                            await CreateNewToken();
                            continue;
                        }

                        if (statusCode == HttpStatusCode.Conflict)
                        {
                            iDCID = $"{baseIDCID}_{attempt}";
                            continue;
                        }

                        // Any other failure → backoff and retry
                        await Task.Delay(3000);
                    }

                }
                catch (Exception ex)
                {
                    var logMessage = $"Case {index} (Case Number {iDCID}) exception: {ex}";
                    WriteLog(logMessage, "Badge", "Cases", agencyKey);
                }

                index++;
            }

            return historyId;
        }

        #endregion

        #region Badge Call Process

        private async Task ProcessBadgeCallsExportImport()
        {
            var CallTypes = new[]
            {
                "sp_Call_Mapped"
            };

            var agencyKey = Guid.Parse(txtAgencyKey.Text);
            var folderPath = txtBadgeFolderPath.Text;
            const int pageSize = 1000;

            foreach (var storedProcedureName in CallTypes)
            {
                int pageNumber = 1;
                bool hasMore = true;
                Guid? historyId = null;

                while (hasMore)
                {
                    // Calling export service with paging params
                    var calls = await Task.Run(() =>
                        _exportService.ExportData(
                            sqlConnectionString,
                            storedProcedureName,
                            agencyKey,
                            pageNumber,
                            pageSize
                        )
                    );

                    if (calls != null && calls.Count > 0)
                    {
                        historyId = await ImportBadgeCallData(calls, historyId, (pageNumber - 1) * pageSize, folderPath, agencyKey);

                        // If we got fewer rows than requested, it’s the last page
                        hasMore = calls.Count == pageSize;
                    }
                    else
                    {
                        hasMore = false;
                    }

                    pageNumber++;
                }
            }
        }
        /*
        private async Task<Guid?> ImportBadgeCallData(List<Dictionary<string, object>> calls, Guid? historyId, int index, string folderPath, Guid agencyKey)
        {
            WriteLog("-------------------", "Badge", "Calls", agencyKey);
            WriteLog($"Batch {index}", "Badge", "Calls", agencyKey);
            WriteLog("-------------------", "Badge", "Calls", agencyKey);
            foreach (var singleCall in calls)
            {
                string cfsNumber = "Unknown";
                try
                {
                    if (singleCall.ContainsKey("CallData") && singleCall["CallData"] is Dictionary<string, object> callDataDict)
                    {
                        cfsNumber = callDataDict.ContainsKey("CFS Number") ? callDataDict["CFS Number"]?.ToString() : "Unknown";
                    }

                    var (success, statusCode, message) = await _badgeCallImportService
                        .ImportCallsAsync(apiToken, singleCall, historyId, folderPath, agencyKey);

                    // Retry if unauthorized
                    if (statusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        await CreateNewToken();

                        if (!string.IsNullOrWhiteSpace(apiToken))
                        {
                            var retryResult = await _badgeCallImportService
                                .ImportCallsAsync(apiToken, singleCall, historyId, folderPath, agencyKey);
                            success = retryResult.Success;
                            statusCode = retryResult.StatusCode;
                            message = retryResult.Message;
                        }
                    }

                    if (success)
                    {
                        Console.WriteLine($"Call {index}: Successfully imported Call/CFS Number {cfsNumber}");
                    }
                    else
                    {
                        var logMessage = $"Insertion failed for Call/CFS Number {cfsNumber}: {statusCode} - {message}";
                        WriteLog(logMessage, "Badge", "Calls", agencyKey);
                    }
                }
                catch (Exception ex)
                {
                    var logMessage = $"Call {index} (Call/CFS Number {cfsNumber}) exception: {ex}";
                    WriteLog(logMessage, "Badge", "Calls", agencyKey);

                }

                index++;
            }

            return historyId;
        }*/

        private async Task<Guid?> ImportBadgeCallData(List<Dictionary<string, object>> calls, Guid? historyId, int index, string folderPath, Guid agencyKey)
        {
            WriteLog("-------------------", "Badge", "Calls", agencyKey);
            WriteLog($"Batch {index}", "Badge", "Calls", agencyKey);
            WriteLog("-------------------", "Badge", "Calls", agencyKey);

            foreach (var call in calls)
            {
                string baseIDCID = "Unknown";
                string iDCID = "Unknown";

                try
                {
                    var callJson = JObject.FromObject(call);
                    baseIDCID = callJson["Document"]?["CallData"]?["CFS Number"]?.ToString() ?? "Unknown";
                    iDCID = baseIDCID;

                    int maxRetries = 5;
                    int attempt = 0;
                    bool success = false;
                    HttpStatusCode? statusCode = null;
                    string message = "";

                    while (!success && attempt < maxRetries)
                    {
                        attempt++;

                        try
                        {
                            EnsureKeyExists(callJson, "Document");
                            EnsureKeyExists(callJson, "Document.CallData");
                            EnsureKeyExists(callJson, "Document.CallData['CFS Number']");
                            callJson["Document"]["CallData"]["CFS Number"] = iDCID;
                        }
                        catch (Exception ex)
                        {
                            WriteLog($"Call {index}: Missing required key → {ex.Message}", "Badge", "Calls", agencyKey);
                            break; // this is not retryable
                        }

                        var result = await _badgeCallImportService
                            .ImportCallAsync(apiToken, callJson, iDCID, historyId, folderPath, agencyKey);

                        success = result.Success;
                        statusCode = result.StatusCode;
                        message = result.Message;

                        if (success)
                            break;

                        if (statusCode == HttpStatusCode.Unauthorized)
                        {
                            WriteLog($"Call {index} & (Call CFS Number {iDCID}): Unauthorized, refreshing token (attempt {attempt})",
                                     "Badge", "Calls", agencyKey);

                            await CreateNewToken();
                            continue;
                        }

                        if (statusCode == HttpStatusCode.Conflict)
                        {
                            iDCID = $"{baseIDCID}_{attempt}";
                            continue;
                        }

                        // Any other failure → backoff and retry
                        await Task.Delay(3000);
                    }

                }
                catch (Exception ex)
                {
                    var logMessage = $"Call {index} (Call CFS Number {iDCID}) exception: {ex}";
                    WriteLog(logMessage, "Badge", "Calls", agencyKey);
                }

                index++;
            }

            return historyId;
        }

        #endregion

        #region Badge Arrest Process

        private async Task ProcessBadgeArrestsExportImport()
        {
            var spTypes = new[]
            {
                "sp_Arrest_Mapped"
            };

            var agencyKey = Guid.Parse(txtAgencyKey.Text);
            var folderPath = txtBadgeFolderPath.Text;
            const int pageSize = 1000;

            foreach (var storedProcedureName in spTypes)
            {
                int pageNumber = 1;
                bool hasMore = true;
                Guid? historyId = null;

                while (hasMore)
                {
                    // Calling export service with paging params
                    var arrests = await Task.Run(() =>
                        _exportService.ExportData(
                            sqlConnectionString,
                            storedProcedureName,
                            agencyKey,
                            pageNumber,
                            pageSize
                        )
                    );

                    if (arrests != null && arrests.Count > 0)
                    {
                        historyId = await ImportBadgeArrestData(arrests, historyId, (pageNumber - 1) * pageSize, folderPath, agencyKey);

                        // If fewer rows returned than page size, last page
                        hasMore = arrests.Count == pageSize;
                    }
                    else
                    {
                        hasMore = false;
                    }

                    pageNumber++;
                }
            }
        }
       
        private async Task<Guid?> ImportBadgeArrestData(List<Dictionary<string, object>> arrests, Guid? historyId, int index, string folderPath, Guid agencyKey)
        {
            WriteLog("-------------------", "Badge", "Arrests", agencyKey);
            WriteLog($"Batch {index}", "Badge", "Arrests", agencyKey);
            WriteLog("-------------------", "Badge", "Arrests", agencyKey);

            foreach (var arrest in arrests)
            {
                string baseIDCID = "Unknown";
                string iDCID = "Unknown";

                try
                {
                    var arrestJson = JObject.FromObject(arrest);
                    baseIDCID = arrestJson["Document"]?["ArrestData"]?["Arrest Number"]?.ToString() ?? "Unknown";
                    iDCID = baseIDCID;

                    int maxRetries = 5;
                    int attempt = 0;
                    bool success = false;
                    HttpStatusCode? statusCode = null;
                    string message = "";

                    while (!success && attempt < maxRetries)
                    {
                        attempt++;

                        try
                        {
                            EnsureKeyExists(arrestJson, "Document");
                            EnsureKeyExists(arrestJson, "Document.ArrestData");
                            EnsureKeyExists(arrestJson, "Document.ArrestData['Arrest Number']");
                            arrestJson["Document"]["ArrestData"]["Arrest Number"] = iDCID;
                        }
                        catch (Exception ex)
                        {
                            WriteLog($"Arrest {index}: Missing required key → {ex.Message}", "Badge", "Arrests", agencyKey);
                            break; // this is not retryable
                        }

                        var result = await _badgeArrestImportService
                            .ImportArrestAsync(apiToken, arrestJson, iDCID, historyId, folderPath, agencyKey);

                        success = result.Success;
                        statusCode = result.StatusCode;
                        message = result.Message;

                        if (success)
                            break;

                        if (statusCode == HttpStatusCode.Unauthorized)
                        {
                            WriteLog($"Arrest {index} & (Arrest Number {iDCID}): Unauthorized, refreshing token (attempt {attempt})",
                                     "Badge", "Arrests", agencyKey);

                            await CreateNewToken();
                            continue;
                        }

                        if (statusCode == HttpStatusCode.Conflict)
                        {
                            iDCID = $"{baseIDCID}_{attempt}";
                            continue;
                        }

                        // Any other failure → backoff and retry
                        await Task.Delay(3000);
                    }

                }
                catch (Exception ex)
                {
                    var logMessage = $"Arrest {index} (Arrest Number {iDCID}) exception: {ex}";
                    WriteLog(logMessage, "Badge", "Arrests", agencyKey);
                }

                index++;
            }

            return historyId;
        }

        #endregion

        #region Badge Alarm Process

        private async Task ProcessBadgeAlarmsExportImport()
        {
            var spTypes = new[]
            {
                "sp_Alarm_Mapped"
            };

            var agencyKey = Guid.Parse(txtAgencyKey.Text);
            var folderPath = txtBadgeFolderPath.Text;
            const int pageSize = 1000;

            foreach (var storedProcedureName in spTypes)
            {
                int pageNumber = 1;
                bool hasMore = true;
                Guid? historyId = null;

                while (hasMore)
                {
                    // Calling export service with paging params
                    var alarms = await Task.Run(() =>
                        _exportService.ExportData(
                            sqlConnectionString,
                            storedProcedureName,
                            agencyKey,
                            pageNumber,
                            pageSize
                        )
                    );

                    if (alarms != null && alarms.Count > 0)
                    {
                        historyId = await ImportBadgeAlarmData(alarms, historyId, (pageNumber - 1) * pageSize, folderPath, agencyKey);

                        // If fewer rows returned than page size, last page
                        hasMore = alarms.Count == pageSize;
                    }
                    else
                    {
                        hasMore = false;
                    }

                    pageNumber++;
                }
            }
        }

        private async Task<Guid?> ImportBadgeAlarmData(List<Dictionary<string, object>> alarms, Guid? historyId, int index, string folderPath, Guid agencyKey)
        {
            WriteLog("-------------------", "Badge", "Alarms", agencyKey);
            WriteLog($"Batch {index}", "Badge", "Alarms", agencyKey);
            WriteLog("-------------------", "Badge", "Alarms", agencyKey);

            foreach (var alarm in alarms)
            {
                string baseIDCID = "Unknown";
                string iDCID = "Unknown";

                try
                {
                    var alarmJson = JObject.FromObject(alarm);
                    baseIDCID = alarmJson["Document"]?["AlarmPermitData"]?["Permit Number"]?.ToString() ?? "Unknown";
                    iDCID = baseIDCID;

                    int maxRetries = 5;
                    int attempt = 0;
                    bool success = false;
                    HttpStatusCode? statusCode = null;
                    string message = "";

                    while (!success && attempt < maxRetries)
                    {
                        attempt++;

                        try
                        {
                            EnsureKeyExists(alarmJson, "Document");
                            EnsureKeyExists(alarmJson, "Document.AlarmPermitData");
                            EnsureKeyExists(alarmJson, "Document.AlarmPermitData['Permit Number']");
                            alarmJson["Document"]["AlarmPermitData"]["Permit Number"] = iDCID;
                        }
                        catch (Exception ex)
                        {
                            WriteLog($"Alarm {index}: Missing required key → {ex.Message}", "Badge", "Alarms", agencyKey);
                            break; // this is not retryable
                        }

                        var result = await _badgeAlarmImportService
                            .ImportAlarmAsync(apiToken, alarmJson, iDCID, historyId, folderPath, agencyKey);

                        success = result.Success;
                        statusCode = result.StatusCode;
                        message = result.Message;

                        if (success)
                            break;

                        if (statusCode == HttpStatusCode.Unauthorized)
                        {
                            WriteLog($"Alarm {index} & (Alarm Permit Number {iDCID}): Unauthorized, refreshing token (attempt {attempt})",
                                     "Badge", "Alarms", agencyKey);

                            await CreateNewToken();
                            continue;
                        }

                        if (statusCode == HttpStatusCode.Conflict)
                        {
                            iDCID = $"{baseIDCID}_{attempt}";
                            continue;
                        }

                        // Any other failure → backoff and retry
                        await Task.Delay(3000);
                    }

                }
                catch (Exception ex)
                {
                    var logMessage = $"Alarm {index} (Alarm Permit Number {iDCID}) exception: {ex}";
                    WriteLog(logMessage, "Badge", "Alarms", agencyKey);
                }

                index++;
            }

            return historyId;
        }

        #endregion

        #region Badge Business Process

        private async Task ProcessBadgeBusinessExportImport()
        {
            var spTypes = new[]
            {
                "sp_Business_Mapped"
            };

            var agencyKey = Guid.Parse(txtAgencyKey.Text);
            var folderPath = txtBadgeFolderPath.Text;
            const int pageSize = 1000;

            foreach (var storedProcedureName in spTypes)
            {
                int pageNumber = 1;
                bool hasMore = true;
                Guid? historyId = null;

                while (hasMore)
                {
                    // Calling export service with paging params
                    var businesses = await Task.Run(() =>
                        _exportService.ExportData(
                            sqlConnectionString,
                            storedProcedureName,
                            agencyKey,
                            pageNumber,
                            pageSize
                        )
                    );

                    if (businesses != null && businesses.Count > 0)
                    {
                        historyId = await ImportBadgeBusinessData(businesses, historyId, (pageNumber - 1) * pageSize, folderPath, agencyKey);

                        // If fewer rows returned than page size, last page
                        hasMore = businesses.Count == pageSize;
                    }
                    else
                    {
                        hasMore = false;
                    }

                    pageNumber++;
                }
            }
        }

        private async Task<Guid?> ImportBadgeBusinessData(List<Dictionary<string, object>> businesses, Guid? historyId, int index, string folderPath, Guid agencyKey)
        {
            WriteLog("-------------------", "Badge", "Businesses", agencyKey);
            WriteLog($"Batch {index}", "Badge", "Businesses", agencyKey);
            WriteLog("-------------------", "Badge", "Businesses", agencyKey);

            foreach (var business in businesses)
            {
                string baseIDCID = "Unknown";
                string iDCID = "Unknown";

                try
                {
                    var businessJson = JObject.FromObject(business);
                    baseIDCID = businessJson["Document"]?["BusinessData"]?["Name"]?.ToString() ?? "Unknown";
                    iDCID = baseIDCID;

                    int maxRetries = 5;
                    int attempt = 0;
                    bool success = false;
                    HttpStatusCode? statusCode = null;
                    string message = "";

                    while (!success && attempt < maxRetries)
                    {
                        attempt++;

                        try
                        {
                            EnsureKeyExists(businessJson, "Document");
                            EnsureKeyExists(businessJson, "Document.BusinessData");
                            EnsureKeyExists(businessJson, "Document.BusinessData['Name']");
                            businessJson["Document"]["BusinessData"]["Name"] = iDCID;
                        }
                        catch (Exception ex)
                        {
                            WriteLog($"Business {index}: Missing required key → {ex.Message}", "Badge", "Businessess", agencyKey);
                            break; // this is not retryable
                        }

                        var result = await _badgeBusinessImportService
                            .ImportBusinessAsync(apiToken, businessJson, iDCID, historyId, folderPath, agencyKey);

                        success = result.Success;
                        statusCode = result.StatusCode;
                        message = result.Message;

                        if (success)
                            break;

                        if (statusCode == HttpStatusCode.Unauthorized)
                        {
                            WriteLog($"Business {index} & (Business Name {iDCID}): Unauthorized, refreshing token (attempt {attempt})",
                                     "Badge", "Businesses", agencyKey);

                            await CreateNewToken();
                            continue;
                        }

                        if (statusCode == HttpStatusCode.Conflict)
                        {
                            iDCID = $"{baseIDCID}_{attempt}";
                            continue;
                        }

                        // Any other failure → backoff and retry
                        await Task.Delay(3000);
                    }

                }
                catch (Exception ex)
                {
                    var logMessage = $"Business {index} (Business Name {iDCID}) exception: {ex}";
                    WriteLog(logMessage, "Badge", "Businesses", agencyKey);
                }

                index++;
            }

            return historyId;
        }

        #endregion

        #region Badge Field Interview Process

        private async Task ProcessBadgeFieldInterviewExportImport()
        {
            var spTypes = new[]
            {
                "sp_FieldInterview_Mapped"
            };

            var agencyKey = Guid.Parse(txtAgencyKey.Text);
            var folderPath = txtBadgeFolderPath.Text;
            const int pageSize = 1000;

            foreach (var storedProcedureName in spTypes)
            {
                int pageNumber = 1;
                bool hasMore = true;
                Guid? historyId = null;

                while (hasMore)
                {
                    // Calling export service with paging params
                    var fieldInterviews = await Task.Run(() =>
                        _exportService.ExportData(
                            sqlConnectionString,
                            storedProcedureName,
                            agencyKey,
                            pageNumber,
                            pageSize
                        )
                    );

                    if (fieldInterviews != null && fieldInterviews.Count > 0)
                    {
                        historyId = await ImportBadgeFieldInterviewData(fieldInterviews, historyId, (pageNumber - 1) * pageSize, folderPath, agencyKey);

                        // If fewer rows returned than page size, last page
                        hasMore = fieldInterviews.Count == pageSize;
                    }
                    else
                    {
                        hasMore = false;
                    }

                    pageNumber++;
                }
            }
        }

        private async Task<Guid?> ImportBadgeFieldInterviewData(List<Dictionary<string, object>> fieldInterviews, Guid? historyId, int index, string folderPath, Guid agencyKey)
        {
            WriteLog("-------------------", "Badge", "FieldInterviews", agencyKey);
            WriteLog($"Batch {index}", "Badge", "FieldInterviews", agencyKey);
            WriteLog("-------------------", "Badge", "FieldInterviews", agencyKey);

            foreach (var fieldInterview in fieldInterviews)
            {
                string baseIDCID = "Unknown";
                string iDCID = "Unknown";

                try
                {
                    var fieldInterviewJson = JObject.FromObject(fieldInterview);
                    baseIDCID = fieldInterviewJson["Document"]?["FieldInterviewData"]?["Citation Number"]?.ToString() ?? "Unknown";
                    iDCID = baseIDCID;

                    int maxRetries = 5;
                    int attempt = 0;
                    bool success = false;
                    HttpStatusCode? statusCode = null;
                    string message = "";

                    while (!success && attempt < maxRetries)
                    {
                        attempt++;

                        try
                        {
                            EnsureKeyExists(fieldInterviewJson, "Document");
                            EnsureKeyExists(fieldInterviewJson, "Document.FieldInterviewData");
                            EnsureKeyExists(fieldInterviewJson, "Document.FieldInterviewData['Field Interview Number']");
                            fieldInterviewJson["Document"]["FieldInterviewData"]["Field Interview Number"] = iDCID;
                        }
                        catch (Exception ex)
                        {
                            WriteLog($"Field Interview  {index}: Missing required key → {ex.Message}", "Badge", "FieldInterviews", agencyKey);
                            break; // this is not retryable
                        }

                        var result = await _badgeFieldInterviewImportService
                            .ImportFieldInterviewAsync(apiToken, fieldInterviewJson, iDCID, historyId, folderPath, agencyKey);

                        success = result.Success;
                        statusCode = result.StatusCode;
                        message = result.Message;

                        if (success)
                            break;

                        if (statusCode == HttpStatusCode.Unauthorized)
                        {
                            WriteLog($"Field Interview {index} & (Field Interview Number {iDCID}): Unauthorized, refreshing token (attempt {attempt})",
                                     "Badge", "FieldInterviews", agencyKey);

                            await CreateNewToken();
                            continue;
                        }

                        if (statusCode == HttpStatusCode.Conflict)
                        {
                            iDCID = $"{baseIDCID}_{attempt}";
                            continue;
                        }

                        // Any other failure → backoff and retry
                        await Task.Delay(3000);
                    }

                }
                catch (Exception ex)
                {
                    var logMessage = $"FieldInterview {index} (Field Interview Number {iDCID}) exception: {ex}";
                    WriteLog(logMessage, "Badge", "FieldInterviews", agencyKey);
                }

                index++;
            }

            return historyId;
        }

        #endregion

        #region Badge Master Person Process

        private async Task ProcessBadgeMasterPersonExportImport()
        {
            var spTypes = new[]
            {
                "sp_MasterName_Mapped"
            };

            var agencyKey = Guid.Parse(txtAgencyKey.Text);
            var folderPath = txtBadgeFolderPath.Text;
            const int pageSize = 1000;

            foreach (var storedProcedureName in spTypes)
            {
                int pageNumber = 1;
                bool hasMore = true;
                Guid? historyId = null;

                while (hasMore)
                {
                    // Calling export service with paging params
                    var persons = await Task.Run(() =>
                        _exportService.ExportData(
                            sqlConnectionString,
                            storedProcedureName,
                            agencyKey,
                            pageNumber,
                            pageSize
                        )
                    );

                    if (persons != null && persons.Count > 0)
                    {
                        historyId = await ImportBadgeMasterPersonData(persons, historyId, (pageNumber - 1) * pageSize, folderPath, agencyKey);

                        // If fewer rows returned than page size, last page
                        hasMore = persons.Count == pageSize;
                    }
                    else
                    {
                        hasMore = false;
                    }

                    pageNumber++;
                }
            }
        }

        private async Task<Guid?> ImportBadgeMasterPersonData(List<Dictionary<string, object>> persons, Guid? historyId, int index, string folderPath, Guid agencyKey)
        {
            WriteLog("-------------------", "Badge", "MasterPersons", agencyKey);
            WriteLog($"Batch {index}", "Badge", "MasterPersons", agencyKey);
            WriteLog("-------------------", "Badge", "MasterPersons", agencyKey);

            foreach (var person in persons)
            {
                string baseIDCID = "Unknown";
                string iDCID = "Unknown";

                try
                {
                    var personJson = JObject.FromObject(person);
                    baseIDCID = personJson["Document"]?["MasterNameData"]?["Citation Number"]?.ToString() ?? "Unknown";
                    iDCID = baseIDCID;

                    int maxRetries = 5;
                    int attempt = 0;
                    bool success = false;
                    HttpStatusCode? statusCode = null;
                    string message = "";

                    while (!success && attempt < maxRetries)
                    {
                        attempt++;

                        try
                        {
                            EnsureKeyExists(personJson, "Document");
                            EnsureKeyExists(personJson, "Document.MasterNameData");
                            EnsureKeyExists(personJson, "Document.MasterNameData['Name']");
                            personJson["Document"]["MasterNameData"]["Name"] = iDCID;
                        }
                        catch (Exception ex)
                        {
                            WriteLog($"Master Person {index}: Missing required key → {ex.Message}", "Badge", "MasterPersons", agencyKey);
                            break; // this is not retryable
                        }

                        var result = await _badgeMasterPersonImportService
                            .ImportMasterPersonAsync(apiToken, personJson, iDCID, historyId, folderPath, agencyKey);

                        success = result.Success;
                        statusCode = result.StatusCode;
                        message = result.Message;

                        if (success)
                            break;

                        if (statusCode == HttpStatusCode.Unauthorized)
                        {
                            WriteLog($"Master Person {index} & (Person Name {iDCID}): Unauthorized, refreshing token (attempt {attempt})",
                                     "Badge", "MasterPersons", agencyKey);

                            await CreateNewToken();
                            continue;
                        }

                        if (statusCode == HttpStatusCode.Conflict)
                        {
                            iDCID = $"{baseIDCID}_{attempt}";
                            continue;
                        }

                        // Any other failure → backoff and retry
                        await Task.Delay(3000);
                    }

                }
                catch (Exception ex)
                {
                    var logMessage = $"Master Person {index} (Person Name {iDCID}) exception: {ex}";
                    WriteLog(logMessage, "Badge", "MasterPersons", agencyKey);
                }

                index++;
            }

            return historyId;
        }

        #endregion

        #region Badge Master Vehicle Process

        private async Task ProcessBadgeMasterVehicleExportImport()
        {
            var spTypes = new[]
            {
                "sp_MasterVehicle_Mapped"
            };

            var agencyKey = Guid.Parse(txtAgencyKey.Text);
            var folderPath = txtBadgeFolderPath.Text;
            const int pageSize = 1000;

            foreach (var storedProcedureName in spTypes)
            {
                int pageNumber = 1;
                bool hasMore = true;
                Guid? historyId = null;

                while (hasMore)
                {
                    // Calling export service with paging params
                    var vehicles = await Task.Run(() =>
                        _exportService.ExportData(
                            sqlConnectionString,
                            storedProcedureName,
                            agencyKey,
                            pageNumber,
                            pageSize
                        )
                    );

                    if (vehicles != null && vehicles.Count > 0)
                    {
                        historyId = await ImportBadgeMasterVehicleData(vehicles, historyId, (pageNumber - 1) * pageSize, folderPath, agencyKey);

                        // If fewer rows returned than page size, last page
                        hasMore = vehicles.Count == pageSize;
                    }
                    else
                    {
                        hasMore = false;
                    }

                    pageNumber++;
                }
            }
        }

        private async Task<Guid?> ImportBadgeMasterVehicleData(List<Dictionary<string, object>> vehicles, Guid? historyId, int index, string folderPath, Guid agencyKey)
        {
            WriteLog("-------------------", "Badge", "MasterVehicles", agencyKey);
            WriteLog($"Batch {index}", "Badge", "MasterVehicles", agencyKey);
            WriteLog("-------------------", "Badge", "MasterVehicles", agencyKey);

            foreach (var vehicle in vehicles)
            {
                string baseIDCID = "Unknown";
                string iDCID = "Unknown";

                try
                {
                    var vehicleJson = JObject.FromObject(vehicle);
                    baseIDCID = vehicleJson["Document"]?["MasterVehicleData"]?["License Plate"]?.ToString() ?? "Unknown";
                    iDCID = baseIDCID;

                    int maxRetries = 5;
                    int attempt = 0;
                    bool success = false;
                    HttpStatusCode? statusCode = null;
                    string message = "";

                    while (!success && attempt < maxRetries)
                    {
                        attempt++;

                        try
                        {
                            EnsureKeyExists(vehicleJson, "Document");
                            EnsureKeyExists(vehicleJson, "Document.MasterVehicleData");
                            EnsureKeyExists(vehicleJson, "Document.MasterVehicleData['License Plate']");
                            vehicleJson["Document"]["MasterVehicleData"]["License Plate"] = iDCID;
                        }
                        catch (Exception ex)
                        {
                            WriteLog($"Vehicle {index}: Missing required key → {ex.Message}", "Badge", "MasterVehicles", agencyKey);
                            break; // this is not retryable
                        }

                        var result = await _badgeMasterVehicleImportService
                            .ImportMasterVehicleAsync(apiToken, vehicleJson, iDCID, historyId, folderPath, agencyKey);

                        success = result.Success;
                        statusCode = result.StatusCode;
                        message = result.Message;

                        if (success)
                            break;

                        if (statusCode == HttpStatusCode.Unauthorized)
                        {
                            WriteLog($"Vehicle {index} & (License Plate {iDCID}): Unauthorized, refreshing token (attempt {attempt})",
                                     "Badge", "MasterVehicles", agencyKey);

                            await CreateNewToken();
                            continue;
                        }

                        if (statusCode == HttpStatusCode.Conflict)
                        {
                            iDCID = $"{baseIDCID}_{attempt}";
                            continue;
                        }

                        // Any other failure → backoff and retry
                        await Task.Delay(3000);
                    }

                }
                catch (Exception ex)
                {
                    var logMessage = $"Vehicle {index} (License Plate {iDCID}) exception: {ex}";
                    WriteLog(logMessage, "Badge", "MasterVehicles", agencyKey);
                }

                index++;
            }

            return historyId;
        }

        #endregion

        #region Badge Property Process

        private async Task ProcessBadgePropertyExportImport()
        {
            var spTypes = new[]
            {
                "sp_Property_Mapped"
            };

            var agencyKey = Guid.Parse(txtAgencyKey.Text);
            var folderPath = txtBadgeFolderPath.Text;
            const int pageSize = 1000;

            foreach (var storedProcedureName in spTypes)
            {
                int pageNumber = 1;
                bool hasMore = true;
                Guid? historyId = null;

                while (hasMore)
                {
                    // Calling export service with paging params
                    var properties = await Task.Run(() =>
                        _exportService.ExportData(
                            sqlConnectionString,
                            storedProcedureName,
                            agencyKey,
                            pageNumber,
                            pageSize
                        )
                    );

                    if (properties != null && properties.Count > 0)
                    {
                        historyId = await ImportBadgePropertyData(properties, historyId, (pageNumber - 1) * pageSize, folderPath, agencyKey);

                        // If fewer rows returned than page size, last page
                        hasMore = properties.Count == pageSize;
                    }
                    else
                    {
                        hasMore = false;
                    }

                    pageNumber++;
                }
            }
        }

        private async Task<Guid?> ImportBadgePropertyData(List<Dictionary<string, object>> properties, Guid? historyId, int index, string folderPath, Guid agencyKey)
        {
            WriteLog("-------------------", "Badge", "Properties", agencyKey);
            WriteLog($"Batch {index}", "Badge", "Properties", agencyKey);
            WriteLog("-------------------", "Badge", "Properties", agencyKey);

            foreach (var property in properties)
            {
                string baseIDCID = "Unknown";
                string iDCID = "Unknown";

                try
                {
                    var propertyJson = JObject.FromObject(property);
                    baseIDCID = propertyJson["Document"]?["PropertyData"]?["Citation Number"]?.ToString() ?? "Unknown";
                    iDCID = baseIDCID;

                    int maxRetries = 5;
                    int attempt = 0;
                    bool success = false;
                    HttpStatusCode? statusCode = null;
                    string message = "";

                    while (!success && attempt < maxRetries)
                    {
                        attempt++;

                        try
                        {
                            EnsureKeyExists(propertyJson, "Document");
                            EnsureKeyExists(propertyJson, "Document.PropertyData");
                            EnsureKeyExists(propertyJson, "Document.PropertyData['Property Number']");
                            propertyJson["Document"]["PropertyData"]["Property Number"] = iDCID;
                        }
                        catch (Exception ex)
                        {
                            WriteLog($"Property {index}: Missing required key → {ex.Message}", "Badge", "Properties", agencyKey);
                            break; // this is not retryable
                        }

                        var result = await _badgePropertyImportService
                            .ImportPropertyAsync(apiToken, propertyJson, iDCID, historyId, folderPath, agencyKey);

                        success = result.Success;
                        statusCode = result.StatusCode;
                        message = result.Message;

                        if (success)
                            break;

                        if (statusCode == HttpStatusCode.Unauthorized)
                        {
                            WriteLog($"Property {index} & (Property Number {iDCID}): Unauthorized, refreshing token (attempt {attempt})",
                                     "Badge", "Properties", agencyKey);

                            await CreateNewToken();
                            continue;
                        }

                        if (statusCode == HttpStatusCode.Conflict)
                        {
                            iDCID = $"{baseIDCID}_{attempt}";
                            continue;
                        }

                        // Any other failure → backoff and retry
                        await Task.Delay(3000);
                    }

                }
                catch (Exception ex)
                {
                    var logMessage = $"Property {index} (Property Number {iDCID}) exception: {ex}";
                    WriteLog(logMessage, "Badge", "Properties", agencyKey);
                }

                index++;
            }

            return historyId;
        }

        #endregion

        #region Badge Traffic Stop Process

        private async Task ProcessBadgeTrafficStopExportImport()
        {
            var spTypes = new[]
            {
                "sp_Citation_Mapped"
            };

            var agencyKey = Guid.Parse(txtAgencyKey.Text);
            var folderPath = txtBadgeFolderPath.Text;
            const int pageSize = 1000;

            foreach (var storedProcedureName in spTypes)
            {
                int pageNumber = 1;
                bool hasMore = true;
                Guid? historyId = null;

                while (hasMore)
                {
                    // Calling export service with paging params
                    var trafficStops = await Task.Run(() =>
                        _exportService.ExportData(
                            sqlConnectionString,
                            storedProcedureName,
                            agencyKey,
                            pageNumber,
                            pageSize
                        )
                    );

                    if (trafficStops != null && trafficStops.Count > 0)
                    {
                        historyId = await ImportBadgeTrafficStopData(trafficStops, historyId, (pageNumber - 1) * pageSize, folderPath, agencyKey);

                        // If fewer rows returned than page size, last page
                        hasMore = trafficStops.Count == pageSize;
                    }
                    else
                    {
                        hasMore = false;
                    }

                    pageNumber++;
                }
            }
        }

        private async Task<Guid?> ImportBadgeTrafficStopData(List<Dictionary<string, object>> trafficStops, Guid? historyId, int index, string folderPath, Guid agencyKey)
        {
            WriteLog("-------------------", "Badge", "TrafficStops", agencyKey);
            WriteLog($"Batch {index}", "Badge", "TrafficStops", agencyKey);
            WriteLog("-------------------", "Badge", "TrafficStops", agencyKey);

            foreach (var trafficStop in trafficStops)
            {
                string baseIDCID = "Unknown";
                string iDCID = "Unknown";

                try
                {
                    var trafficStopJson = JObject.FromObject(trafficStop);
                    baseIDCID = trafficStopJson["Document"]?["TrafficStopData"]?["Stop Number"]?.ToString() ?? "Unknown";
                    iDCID = baseIDCID;

                    int maxRetries = 5;
                    int attempt = 0;
                    bool success = false;
                    HttpStatusCode? statusCode = null;
                    string message = "";

                    while (!success && attempt < maxRetries)
                    {
                        attempt++;

                        try
                        {
                            EnsureKeyExists(trafficStopJson, "Document");
                            EnsureKeyExists(trafficStopJson, "Document.TrafficStopData");
                            EnsureKeyExists(trafficStopJson, "Document.TrafficStopData['Stop Number']");
                            trafficStopJson["Document"]["TrafficStopData"]["Stop Number"] = iDCID;
                        }
                        catch (Exception ex)
                        {
                            WriteLog($"Traffic Stop {index}: Missing required key → {ex.Message}", "Badge", "TrafficStops", agencyKey);
                            break; // this is not retryable
                        }

                        var result = await _badgeTrafficStopImportService
                            .ImportTrafficStopAsync(apiToken, trafficStopJson, iDCID, historyId, folderPath, agencyKey);

                        success = result.Success;
                        statusCode = result.StatusCode;
                        message = result.Message;

                        if (success)
                            break;

                        if (statusCode == HttpStatusCode.Unauthorized)
                        {
                            WriteLog($"Traffic Stop {index} & (Stop Number {iDCID}): Unauthorized, refreshing token (attempt {attempt})",
                                     "Badge", "TrafficStops", agencyKey);

                            await CreateNewToken();
                            continue;
                        }

                        if (statusCode == HttpStatusCode.Conflict)
                        {
                            iDCID = $"{baseIDCID}_{attempt}";
                            continue;
                        }

                        // Any other failure → backoff and retry
                        await Task.Delay(3000);
                    }

                }
                catch (Exception ex)
                {
                    var logMessage = $"Traffic Stop {index} (Stop Number {iDCID}) exception: {ex}";
                    WriteLog(logMessage, "Badge", "TrafficStops", agencyKey);
                }

                index++;
            }

            return historyId;
        }

        #endregion

        private static string GetFilePath(string folderName)
        {
            var basePath = Directory.GetCurrentDirectory();
            var filePath = Path.Combine(basePath, folderName);

            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);

            return filePath;
        }

        private static void WriteToLogFile(string error)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\ErrorLog_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
            if (!File.Exists(filepath))
            {
                using StreamWriter sw = File.CreateText(filepath);
                sw.WriteLine(error);
            }
            else
            {
                using StreamWriter sw = File.AppendText(filepath);
                sw.WriteLine(error);
            }
        }

        private static void WriteToLogFile(string rms, string module, Guid agencyKey, string message)
        {
            try
            {
                string exePath = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
                string current = Path.GetDirectoryName(exePath);

                string appFolder;

                // Detect if running from bin folder
                if (current.Contains(Path.Combine("bin", "")))
                {
                    appFolder = Directory.GetParent(current)?.Parent?.Parent?.FullName;
                }
                else
                {
                    appFolder = current;
                }

                // Log root
                string root = Path.Combine(appFolder, "Logs");

                string logDir = Path.Combine(root, rms, agencyKey.ToString(), module);
                Directory.CreateDirectory(logDir);

                string filePath = Path.Combine(logDir, $"ActivityLog_{DateTime.Now:yyyy_MM_dd}.txt");

                using (StreamWriter sw = new StreamWriter(filePath, true))
                {
                    sw.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to write log: {ex.Message}");
            }
        }

        private static void WriteLog(string message, string rms = null, string module = null, Guid? agencyKey = null, string logType = "INFO")
        {
            try
            {

                string appFolder = GetAppRoot();

                string logsRoot = Path.Combine(appFolder, "Logs");

                string logFolder;

                // If module logging params are provided → module log folder
                if (rms != null && module != null && agencyKey.HasValue)
                {
                    logFolder = Path.Combine(logsRoot, rms, agencyKey.ToString(), module);
                }
                else
                {
                    // Default: /Logs/General
                    logFolder = Path.Combine(logsRoot, "General");
                }

                Directory.CreateDirectory(logFolder);

                string filePath = Path.Combine(logFolder, $"{logType}_Log_{DateTime.Now:yyyy_MM_dd}.txt");

                using (var sw = new StreamWriter(filePath, true))
                {
                    sw.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{logType}]  {message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Log error: " + ex.Message);
            }
        }



        private static (string, bool) GetFileName(string folderPath, int index)
        {
            string fileName = "";
            bool isLastFile = false;

            var lastFile = new DirectoryInfo(folderPath)
                .GetFiles("*.csv")
                .OrderByDescending(f => f.CreationTime)
                .FirstOrDefault();

            if (lastFile == null || index == 0)
            {
                fileName = GetFileName();
            }
            else
            {
                fileName = lastFile.Name;
                isLastFile = true;
            }

            var path = Path.Combine(folderPath, fileName);

            if (!File.Exists(path))
            {
                File.Create(path).Dispose();
            }

            return (fileName, isLastFile);
        }

        private static string GetFileName()
        {
            return System.Guid.NewGuid() + "_" + DateTime.UtcNow.ToString("yyyy_MM_dd_HH_mm_ss") + ".csv";
        }

        private static int ExtractNumber(string input)
        {
            return int.TryParse(Regex.Match(input ?? "", @"\d+").Value, out int result) ? result : 0;
        }

        private async Task CreateNewToken()
        {
            var responseBody = await apiTokenService.GetToken(txtAgencyKey.Text, txtAgencyAPIKey.Text);

            lblApiStatus.Text = responseBody.Message;

            if (responseBody.Success && responseBody.Data != null)
            {
                apiToken = responseBody.Data.Token;
                lblApiStatus.Text = "API connection successful.";
            }
            else
                apiToken = string.Empty;
        }

        private bool CheckDatabaseConnection()
        {
            using var con = new SqlConnection(sqlConnectionString);
            try
            {
                con.Open();
                return true;
            }
            catch (SqlException ex)
            {
                WriteLog(ex.ToString(), logType: "ERROR");
                return false;
            }
        }

        private async void BtnGenerateToken_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAgencyKey.Text) || string.IsNullOrWhiteSpace(txtAgencyAPIKey.Text))
            {
                MessageBox.Show("Please enter Agency Key and API Key.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            await CreateNewToken();
        }

        private void BtnSqlConnect_LawTrak_Click(object sender, EventArgs e)
        {
            ConnectSql("LawTrak");
        }

        private void BtnSqlConnect_InSynch_Click(object sender, EventArgs e)
        {
            ConnectSql("InSynch");
        }

        private void BtnSqlConnect_Badge_Click(object sender, EventArgs e)
        {
            ConnectSql("Badge");
        }

        private async Task ApplyStoredProceduresForRmsAsync(string rmsName)
        {
            string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SqlScripts", rmsName);

            if (!Directory.Exists(folderPath))
                return;

            foreach (var sqlFile in Directory.GetFiles(folderPath, "*.sql"))
            {
                string script = await File.ReadAllTextAsync(sqlFile);

                using var conn = new SqlConnection(sqlConnectionString);
                await conn.OpenAsync();

                using var cmd = new SqlCommand(script, conn)
                {
                    CommandTimeout = 120
                };

                await cmd.ExecuteNonQueryAsync();
            }
            WriteLog("Stored procedures are successfully created in the database", logType: "Info");
        }

        public static string GetAppRoot()
        {
            string exePath = Process.GetCurrentProcess().MainModule.FileName;
            string current = Path.GetDirectoryName(exePath);

            // If running inside bin/, walk up
            if (current.Contains(Path.Combine("bin", "")))
                return Directory.GetParent(current)?.Parent?.Parent?.FullName;

            return current;
        }

        private void EnsureKeyExists(JObject obj, string path)
        {
            var token = obj.SelectToken(path);
            if (token == null)
                throw new Exception($"Missing key: {path}");
        }
        private async void BtnLawTrak_Migration_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sqlConnectionString) || string.IsNullOrWhiteSpace(apiToken))
                {
                    MessageBox.Show("Please check SQL database and API connection first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                var agencyKey = Guid.Parse(txtAgencyKey.Text);
                progressBarLawTrak.Visible = true;
                progressBarLawTrak.Style = ProgressBarStyle.Marquee;

                await ApplyStoredProceduresForRmsAsync("LawTrak");
                // Prepare a queue of tasks (as Func<Task>)
                var tasksQueue = new List<(string Module, Func<Task> Action)>();

                if (cbCitations_LawTrak.Checked)
                    tasksQueue.Add(("Citations", ProcessLawTrakCitationsExportImport));
                if (cbWarrants_LawTrak.Checked)
                    tasksQueue.Add(("Warrants", ProcessLawTrakWarrantsExportImport));
                if (cbCases_LawTrak.Checked)
                    tasksQueue.Add(("Cases", ProcessLawTrakCasesExportImport));
                if (cbAffidavits_LawTrak.Checked)
                    tasksQueue.Add(("Affidavits", ProcessLawTrakAffidavitsExportImport));
                if (cbEvidences_LawTrak.Checked)
                    tasksQueue.Add(("Evidences", ProcessLawTrakEvidencesExportImport));
                if (cbAccounting_LawTrak.Checked)
                    tasksQueue.Add(("Accounting", ProcessLawTrakAccountingExportImport));
                if (cbJuvenile_LawTrak.Checked)
                    tasksQueue.Add(("Juvenile", ProcessLawTrakJuvenileExportImport));
                if (cbBookings_LawTrak.Checked)
                    tasksQueue.Add(("Bookings", ProcessLawTrakBookingExportImport));
                if (cbSummons_LawTrak.Checked)
                    tasksQueue.Add(("Summons", ProcessLawTrakSummonExportImport));
                if (cbSubpoena_LawTrak.Checked)
                    tasksQueue.Add(("Subpoena", ProcessLawTrakSubpoenaExportImport));
                if (cbPersonnel_LawTrak.Checked)
                    tasksQueue.Add(("Personnell", ProcessLawTrakPersonnelExportImport));
                if (cbSCReceipts_LawTrak.Checked)
                    tasksQueue.Add(("ScReceipts", ProcessLawTrakScReceiptsExportImport));
                if (cbJury_LawTrak.Checked)
                    tasksQueue.Add(("Jury", ProcessLawTrakJurysExportImport));
                if (cbPropertyCheck_LawTrak.Checked)
                    tasksQueue.Add(("PropertyChecks", ProcessLawTrakPropertyChecksExportImport));

                // Limit max number of parallel tasks
                int maxParallel = 3; // We can adjust this number based on system capabilities
                var semaphore = new SemaphoreSlim(maxParallel);

                var runningTasks = tasksQueue.Select(async task =>
                {
                    await semaphore.WaitAsync(); // Wait for available slot
                    try
                    {
                        await task.Action();
                    }
                    catch (Exception ex)
                    {
                        // Module-specific error log
                        WriteLog(ex.ToString(), "LawTrak", task.Module, agencyKey, "ERROR");
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                });

                await Task.WhenAll(runningTasks); // Wait for all tasks to complete

                MessageBox.Show("Selected import process completed, Please check log files.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString(), logType: "ERROR");
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            finally
            {
                progressBarLawTrak.Visible = false;
            }

        }

        private async void BtnInSynch_Migration_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sqlConnectionString) || string.IsNullOrWhiteSpace(apiToken))
                {
                    MessageBox.Show("Please check SQL database and API connection first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                var agencyKey = Guid.Parse(txtAgencyKey.Text);
                progressBarInSynch.Visible = true;
                progressBarInSynch.Style = ProgressBarStyle.Marquee;

                await ApplyStoredProceduresForRmsAsync("InSynch");
                // Prepare a queue of tasks (as Func<Task>)
                var tasksQueue = new List<(string Module, Func<Task> Action)>();

                // Add tasks based on selected checkboxes


                // Limit max number of parallel tasks
                int maxParallel = 3; // We can adjust this number based on system capabilities
                var semaphore = new SemaphoreSlim(maxParallel);

                var runningTasks = tasksQueue.Select(async task =>
                {
                    await semaphore.WaitAsync(); // Wait for available slot
                    try
                    {
                        await task.Action();
                    }
                    catch (Exception ex)
                    {
                        // Module-specific error log
                        WriteLog(ex.ToString(), "InSynch", task.Module, agencyKey, "ERROR");
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                });

                await Task.WhenAll(runningTasks); // Wait for all tasks to complete

                MessageBox.Show("Selected import process completed, Please check log files.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString(), logType: "ERROR");
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            finally
            {
                progressBarInSynch.Visible = false;
            }

        }

        private async void BtnBadge_Migration_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sqlConnectionString) || string.IsNullOrWhiteSpace(apiToken))
                {
                    MessageBox.Show("Please check SQL database and API connection first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                var agencyKey = Guid.Parse(txtAgencyKey.Text);
                progressBarBadge.Visible = true;
                progressBarBadge.Style = ProgressBarStyle.Marquee;

                await ApplyStoredProceduresForRmsAsync("Badge");
                // Prepare a queue of tasks (as Func<Task>)
                var tasksQueue = new List<(string Module, Func<Task> Action)>();

                if (cbCitations_Badge.Checked)
                    tasksQueue.Add(("Citations", ProcessBadgeCitationsExportImport));
                if (cbWarrants_Badge.Checked)
                    tasksQueue.Add(("Warrants", ProcessBadgeWarrantsExportImport));
                if (cbCases_Badge.Checked)
                    tasksQueue.Add(("Cases", ProcessBadgeCasesExportImport));
                if (cbCalls_Badge.Checked)
                    tasksQueue.Add(("Calls", ProcessBadgeCallsExportImport));
                if (cbAlarm_Badge.Checked)
                    tasksQueue.Add(("Alarms", ProcessBadgeAlarmsExportImport));
                if (cbArrest_Badge.Checked)
                    tasksQueue.Add(("Arrests", ProcessBadgeArrestsExportImport));
                if (cbMasterPerson_Badge.Checked)
                    tasksQueue.Add(("MasterPersons", ProcessBadgeMasterPersonExportImport)); 
                if (cbMasterVehicle_Badge.Checked)
                    tasksQueue.Add(("MasterVehicles", ProcessBadgeMasterVehicleExportImport));
                if (cbProperty_Badge.Checked)
                    tasksQueue.Add(("Properties", ProcessBadgePropertyExportImport));
                if (cbBusiness_Badge.Checked)
                    tasksQueue.Add(("Businesses", ProcessBadgeBusinessExportImport));
                if (cbFieldInterview_Badge.Checked)
                    tasksQueue.Add(("FieldInterviews", ProcessBadgeFieldInterviewExportImport));
                if (cbTrafficStop_Badge.Checked)
                    tasksQueue.Add(("TrafficStops", ProcessBadgeTrafficStopExportImport));
                // Limit max number of parallel tasks
                int maxParallel = 3; // We can adjust this number based on system capabilities
                var semaphore = new SemaphoreSlim(maxParallel);

                var runningTasks = tasksQueue.Select(async task =>
                {
                    await semaphore.WaitAsync(); // Wait for available slot
                    try
                    {
                        await task.Action();
                    }
                    catch (Exception ex)
                    {
                        // Module-specific error log
                        WriteLog(ex.ToString(), "Badge", task.Module, agencyKey, "ERROR");
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                });

                await Task.WhenAll(runningTasks); // Wait for all tasks to complete

                MessageBox.Show("Selected import process completed, Please check log files.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString(), logType: "ERROR");
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            finally
            {
                progressBarBadge.Visible = false;
            }

        }


        // Reset LawTrak UI fields
        private void BtnLawTrak_Reset_Click(object sender, EventArgs e)
        {
            server = "";
            db = "";
            user = "";
            pass = "";
            isSqlAuth = false;
            statusLabel = null;

            // (LawTrak-specific ones)
            txtSqlServer_LawTrak.Text = "";
            txtSqlDb_LawTrak.Text = "";
            txtSqlUser_LawTrak.Text = "";
            txtSqlPass_LawTrak.Text = "";
            cbSqlAuth_LawTrak.SelectedIndex = 0;
            lblSqlStatus_LawTrak.Text = "";
        }

        // Reset InSynch UI fields 
        private void BtnInSynch_Reset_Click(object sender, EventArgs e)
        {
            server = "";
            db = "";
            user = "";
            pass = "";
            statusLabel = null;

            // (InSynch - specific ones)
            txtSqlServer_InSynch.Text = "";
            txtSqlDb_InSynch.Text = "";
            txtSqlUser_InSynch.Text = "";
            txtSqlPass_InSynch.Text = "";
            lblSqlStatus_InSynch.Text = "";
        }

        // Reset Badge UI fields
        private void BtnBadge_Reset_Click(object sender, EventArgs e)
        {
            server = "";
            db = "";
            user = "";
            pass = "";
            isSqlAuth = false;
            statusLabel = null;

            // (Badge-specific ones)
            txtSqlServer_Badge.Text = "";
            txtSqlDb_Badge.Text = "";
            txtSqlUser_Badge.Text = "";
            txtSqlPass_Badge.Text = "";
            cbSqlAuth_Badge.SelectedIndex = 0;
            lblSqlStatus_Badge.Text = "";
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Select a folder";
                folderDialog.ShowNewFolderButton = true;

                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    if (sender == btnLawTrakBrowse)
                    {
                        txtLTFolderPath.Text = folderDialog.SelectedPath;
                    }
                    else if (sender == btnBadgeBrowse)
                    {
                        txtBadgeFolderPath.Text = folderDialog.SelectedPath;
                    }
                }
            }
        }
    }
}

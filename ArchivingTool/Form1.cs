using ArchivingTool.Model.Arms;
using ArchivingTool.Service.Arms.Models;
using ArchivingTool.Service.Arms.Services.Common;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Identity.Client;
using Microsoft.VisualBasic.ApplicationServices;
using MongoDB.Driver.Core.Servers;
using Newtonsoft.Json.Linq;
using System;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.Net.Http;
using System.Security.Policy;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement; 

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


        // Badge
        private Service.Arms.Services.Badge.CitationImportService _badgeCitationImportService;
        private Service.Arms.Services.Badge.CaseImportService _badgeCaseImportService;
        private Service.Arms.Services.Badge.CallImportService _badgeCallImportService;
        private Service.Arms.Services.Badge.WarrantImportService _badgeWarrantImportService;

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
                statusLabel.Text = "Successful Connection";
                statusLabel.ForeColor = Color.DarkGreen;
            }
            else
            {
                statusLabel.Text = "Failed Connection";
                statusLabel.ForeColor = Color.DarkRed;
                MessageBox.Show("Failed to connect to database. Please check connection details.", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        "sp_Warning_Ticket_Mapped"
    };

            var agencyKey = Guid.Parse(txtAgencyKey.Text);
            var folderPath = txtLTFolderPath.Text;
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
        private async Task<Guid?> ImportLawTrakCitationData(List<Dictionary<string, object>> citations, Guid? historyId, int index, string folderPath, Guid agencyKey)
        {
            WriteToLogFile("LawTrak", "Citations", agencyKey, "-------------------");
            WriteToLogFile("LawTrak", "Citations", agencyKey, "Citations Logs");
            WriteToLogFile("LawTrak", "Citations", agencyKey, $"Batch {index}");
            WriteToLogFile("LawTrak", "Citations", agencyKey, "-------------------");
            foreach (var citation in citations)
            {
                string citationNumber = "Unknown";
                try
                {
                    var citationJson = JObject.FromObject(citation);

                        citationNumber = citationJson["Document"]?["CitationsData"]?["Citation Number"]?.ToString() ?? "Unknown";

                    var (success, statusCode, message) = await _lawTrakCitationImportService
                        .ImportCitationsAsync(apiToken, citation, historyId, folderPath, agencyKey);

                    // Retry if unauthorized
                    if (statusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        await CreateNewToken();
                        if (!string.IsNullOrWhiteSpace(apiToken))
                        {
                            var retryResult = await _lawTrakCitationImportService
                                .ImportCitationsAsync(apiToken, citation, historyId, folderPath, agencyKey);

                            success = retryResult.Success;
                            statusCode = retryResult.StatusCode;
                            message = retryResult.Message;
                        }
                    }

                    if (success)
                    {
                        Console.WriteLine($"Batch {index}: Successfully imported citation {citationNumber}.");
                    }
                    else
                    {
                        var logMessage = $"Insertion failed for citation {citationNumber}: {statusCode} - {message}";
                        WriteToLogFile("LawTrak", "Citations", agencyKey, logMessage);
                    }
                }
                catch (Exception ex)
                {
                    var logMessage = $"Citation {index} (Citation Number {citationNumber}) exception: {ex}";
                    WriteToLogFile("LawTrak", "Citations", agencyKey, logMessage);
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

        private async Task<Guid?> ImportLawTrakWarrantData(
    List<Dictionary<string, object>> warrants,
    Guid? historyId,
    int index,
    string folderPath,
    Guid agencyKey)
        {
            WriteToLogFile("LawTrak", "Warrants", agencyKey, "-------------------");
            WriteToLogFile("LawTrak", "Warrants", agencyKey, "Warrants Logs");
            WriteToLogFile("LawTrak", "Warrants", agencyKey, $"Batch {index}");
            WriteToLogFile("LawTrak", "Warrants", agencyKey, "-------------------");
            foreach (var singleWarrant in warrants)
            {
                string warrantNumber = "Unknown";
                try
                {
                    var warrantJson = JObject.FromObject(singleWarrant);

                    warrantNumber = warrantJson["Document"]?["WarrantsData"]?["Warrant Number"]?.ToString() ?? "Unknown";

                    var (success, statusCode, message) = await _lawTrakWarrantImportService
                        .ImportWarrantsAsync(apiToken, singleWarrant, historyId, folderPath, agencyKey);

                    // Retry if unauthorized
                    if (statusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        await CreateNewToken();

                        if (!string.IsNullOrWhiteSpace(apiToken))
                        {
                            var retryResult = await _lawTrakWarrantImportService
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
                        WriteToLogFile("LawTrak", "Warrants", agencyKey, logMessage);
                    }
                }
                catch (Exception ex)
                {
                    var logMessage = $"Warrant {index} (Warrant Number {warrantNumber}) exception: {ex}";
                    WriteToLogFile("LawTrak", "Warrants", agencyKey, logMessage);
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
            const int pageSize = 100;

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


        private async Task<Guid?> ImportLawTrakCaseData(
            List<Dictionary<string, object>> cases,
            Guid? historyId,
            int index,
            string folderPath,
            Guid agencyKey)
        {
            WriteToLogFile("LawTrak", "Cases", agencyKey, "-------------------");
            WriteToLogFile("LawTrak", "Cases", agencyKey, "Citations Logs");
            WriteToLogFile("LawTrak", "Cases", agencyKey, $"Batch {index}");
            WriteToLogFile("LawTrak", "Cases", agencyKey, "-------------------");
            foreach (var singleCase in cases)
            {
                string caseNumber = "Unknown";
                try
                {
                    // Safely get the nested Case Number
                    if (singleCase.ContainsKey("CaseData") && singleCase["CaseData"] is Dictionary<string, object> caseDataDict)
                    {
                        caseNumber = caseDataDict.ContainsKey("Case Number") ? caseDataDict["Case Number"]?.ToString() : "Unknown";
                    }

                    var (success, statusCode, message) = await _lawTrakCaseImportService
                        .ImportCasesAsync(apiToken, singleCase, historyId, folderPath, agencyKey);

                    // Retry if unauthorized
                    if (statusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        await CreateNewToken();

                        if (!string.IsNullOrWhiteSpace(apiToken))
                        {
                            var retryResult = await _lawTrakCaseImportService
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
                        WriteToLogFile("LawTrak", "Cases", agencyKey, logMessage);
                    }
                }
                catch (Exception ex)
                {
                    var logMessage = $"Case {index} (Case Number {caseNumber}) exception: {ex}";
                    WriteToLogFile("LawTrak", "Cases", agencyKey, logMessage);
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
        private async Task<Guid?> ImportLawTrakAffidavitData(List<Dictionary<string, object>> affidavits, Guid? historyId, int index, string folderPath, Guid agencyKey)
        {
            WriteToLogFile("LawTrak", "Affidavits", agencyKey, "-------------------");
            WriteToLogFile("LawTrak", "Affidavits", agencyKey, "Affidavits Logs");
            WriteToLogFile("LawTrak", "Affidavits", agencyKey, $"Batch {index}");
            WriteToLogFile("LawTrak", "Affidavits", agencyKey, "-------------------");
            foreach (var affidavit in affidavits)
            {
                string affidavitNumber = "Unknown";
                try
                {

                    var affidavitJson = JObject.FromObject(affidavit);

                    affidavitNumber = affidavitJson["Document"]?["AffidavitsData"]?["Case Number"]?.ToString() ?? "Unknown";


                    var (success, statusCode, message) = await _lawTrakAffidavitImportService
                        .ImportAffidavitsAsync(apiToken, affidavit, historyId, folderPath, agencyKey);

                    // Retry if unauthorized
                    if (statusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        await CreateNewToken();
                        if (!string.IsNullOrWhiteSpace(apiToken))
                        {
                            var retryResult = await _lawTrakAffidavitImportService
                                .ImportAffidavitsAsync(apiToken, affidavit, historyId, folderPath, agencyKey);

                            success = retryResult.Success;
                            statusCode = retryResult.StatusCode;
                            message = retryResult.Message;
                        }
                    }

                    if (success)
                    {
                        Console.WriteLine($"Batch {index}: Successfully imported affidavit {affidavitNumber}.");
                    }
                    else
                    {
                        var logMessage = $"Insertion failed for affidavit {affidavitNumber}: {statusCode} - {message}";
                        WriteToLogFile("LawTrak", "Affidavits", agencyKey, logMessage);
                    }
                }
                catch (Exception ex)
                {
                    var logMessage = $"Affidavit {index} (Affidavit Number {affidavitNumber}) exception: {ex}";
                    WriteToLogFile("LawTrak", "Affidavits", agencyKey, logMessage);
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
        private async Task<Guid?> ImportLawTrakEvidenceData(List<Dictionary<string, object>> evidences, Guid? historyId, int index, string folderPath, Guid agencyKey)
        {
            WriteToLogFile("LawTrak", "Evidences", agencyKey, "-------------------");
            WriteToLogFile("LawTrak", "Evidences", agencyKey, "Evidences Logs");
            WriteToLogFile("LawTrak", "Evidences", agencyKey, $"Batch {index}");
            WriteToLogFile("LawTrak", "Evidences", agencyKey, "-------------------");
            foreach (var evidence in evidences)
            {
                string evidenceNumber = "Unknown";
                try
                {

                    var evidenceJson = JObject.FromObject(evidence);

                    string evidenceType = evidenceJson["Document"]?["EvidenceData"]?["Evidence Type"]?.ToString() ?? "";

                    // deciding which field to use
                    if (evidenceType == "Main Entry")
                    {
                        evidenceNumber = evidenceJson["Document"]?["EvidenceData"]?["Incident"]?.ToString() ?? "Unknown";
                    }
                    else if (evidenceType == "Lost And Found")
                    {
                        evidenceNumber = evidenceJson["Document"]?["EvidenceData"]?["Control"]?.ToString() ?? "Unknown";
                    }
                    else
                    {
                        evidenceNumber = evidenceJson["Document"]?["EvidenceData"]?["Case Number"]?.ToString() ?? "Unknown";
                    }


                    var (success, statusCode, message) = await _lawTrakEvidenceImportService
                        .ImportEvidencesAsync(apiToken, evidence, historyId, folderPath, agencyKey);

                    // Retry if unauthorized
                    if (statusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        await CreateNewToken();
                        if (!string.IsNullOrWhiteSpace(apiToken))
                        {
                            var retryResult = await _lawTrakEvidenceImportService
                                .ImportEvidencesAsync(apiToken, evidence, historyId, folderPath, agencyKey);

                            success = retryResult.Success;
                            statusCode = retryResult.StatusCode;
                            message = retryResult.Message;
                        }
                    }

                    if (success)
                    {
                        Console.WriteLine($"Batch {index}: Successfully imported evidence {evidenceNumber}.");
                    }
                    else
                    {
                        var logMessage = $"Insertion failed for evidence {evidenceNumber}: {statusCode} - {message}";
                        WriteToLogFile("LawTrak", "Evidences", agencyKey, logMessage);
                    }
                }
                catch (Exception ex)
                {
                    var logMessage = $"Evidence {index} (Evidence Number {evidenceNumber}) exception: {ex}";
                    WriteToLogFile("LawTrak", "Evidences", agencyKey, logMessage);
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
        private async Task<Guid?> ImportLawTrakAccountingData(List<Dictionary<string, object>> monthlyReports, Guid? historyId, int index, string folderPath, Guid agencyKey)
        {
            WriteToLogFile("LawTrak", "Accounting Monthly Reports", agencyKey, "-------------------");
            WriteToLogFile("LawTrak", "Accounting Monthly Reports", agencyKey, "Accounting Monthly Reports Logs");
            WriteToLogFile("LawTrak", "Accounting Monthly Reports", agencyKey, $"Batch {index}");
            WriteToLogFile("LawTrak", "Accounting Monthly Reports", agencyKey, "-------------------");
            foreach (var monthlyReport in monthlyReports)
            {
                string accountingNumber = "Unknown";
                try
                {

                    var accountingJson = JObject.FromObject(monthlyReport);

                    accountingNumber = accountingJson["Document"]?["AccountingMonthlyData"]?["Accounting Date"]?.ToString() ?? "Unknown";


                    var (success, statusCode, message) = await _lawTrakAccountingImportService
                        .ImportAccountingAsync(apiToken, monthlyReport, historyId, folderPath, agencyKey);

                    // Retry if unauthorized
                    if (statusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        await CreateNewToken();
                        if (!string.IsNullOrWhiteSpace(apiToken))
                        {
                            var retryResult = await _lawTrakAccountingImportService
                                .ImportAccountingAsync(apiToken, monthlyReport, historyId, folderPath, agencyKey);

                            success = retryResult.Success;
                            statusCode = retryResult.StatusCode;
                            message = retryResult.Message;
                        }
                    }

                    if (success)
                    {
                        Console.WriteLine($"Batch {index}: Successfully imported Accounting Monthly Reports {accountingNumber}.");
                    }
                    else
                    {
                        var logMessage = $"Insertion failed for Accounting Monthly Reports {accountingNumber}: {statusCode} - {message}";
                        WriteToLogFile("LawTrak", "Accounting Monthly Reports", agencyKey, logMessage);
                    }
                }
                catch (Exception ex)
                {
                    var logMessage = $"Accounting Monthly Reports {index} (Accounting Number {accountingNumber}) exception: {ex}";
                    WriteToLogFile("LawTrak", "Accounting Monthly Reports", agencyKey, logMessage);
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
                        historyId = await ImportLawTrakJuvenileData(monthlyReports, historyId, (pageNumber - 1) * pageSize, folderPath, agencyKey);

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
        private async Task<Guid?> ImportLawTrakJuvenileData(List<Dictionary<string, object>> juveniles, Guid? historyId, int index, string folderPath, Guid agencyKey)
        {
            WriteToLogFile("LawTrak", "Juvenile", agencyKey, "-------------------");
            WriteToLogFile("LawTrak", "Juvenile", agencyKey, "Juvenile Logs");
            WriteToLogFile("LawTrak", "Juvenile", agencyKey, $"Batch {index}");
            WriteToLogFile("LawTrak", "Juvenile", agencyKey, "-------------------");
            foreach (var juvenile in juveniles)
            {
                string juvenileNumber = "Unknown";
                try
                {
                    var juvenileJson = JObject.FromObject(juvenile);

                    juvenileNumber = juvenileJson["Document"]?["JuvenilesData"]?["Unique Number"]?.ToString() ?? "Unknown";


                    var (success, statusCode, message) = await _lawTrakJuvenileImportService
                        .ImportJuvenileAsync(apiToken, juvenile, historyId, folderPath, agencyKey);

                    // Retry if unauthorized
                    if (statusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        await CreateNewToken();
                        if (!string.IsNullOrWhiteSpace(apiToken))
                        {
                            var retryResult = await _lawTrakJuvenileImportService
                                .ImportJuvenileAsync(apiToken, juvenile, historyId, folderPath, agencyKey);

                            success = retryResult.Success;
                            statusCode = retryResult.StatusCode;
                            message = retryResult.Message;
                        }
                    }

                    if (success)
                    {
                        Console.WriteLine($"Batch {index}: Successfully imported juvenile {juvenileNumber}.");
                    }
                    else
                    {
                        var logMessage = $"Insertion failed for juvenile Record {juvenileNumber}: {statusCode} - {message}";
                        WriteToLogFile("LawTrak", "Juvenile", agencyKey, logMessage);
                    }
                }
                catch (Exception ex)
                {
                    var logMessage = $"Juvenile Reports {index} (Juvenile Number {juvenileNumber}) exception: {ex}";
                    WriteToLogFile("LawTrak", "Juvenile", agencyKey, logMessage);
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
                        historyId = await ImportLawTrakBookingData(monthlyReports, historyId, (pageNumber - 1) * pageSize, folderPath, agencyKey);

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
        private async Task<Guid?> ImportLawTrakBookingData(List<Dictionary<string, object>> bookings, Guid? historyId, int index, string folderPath, Guid agencyKey)
        {
            WriteToLogFile("LawTrak", "Bookings", agencyKey, "-------------------");
            WriteToLogFile("LawTrak", "Bookings", agencyKey, "Bookings Logs");
            WriteToLogFile("LawTrak", "Bookings", agencyKey, $"Batch {index}");
            WriteToLogFile("LawTrak", "Bookings", agencyKey, "-------------------");
            foreach (var booking in bookings)
            {
                string bookingNumber = "Unknown";
                try
                {

                    var bookingJson = JObject.FromObject(booking);

                    bookingNumber = bookingJson["Document"]?["BookingsData"]?["Booking Number"]?.ToString() ?? "Unknown";


                    var (success, statusCode, message) = await _lawTrakBookingImportService
                        .ImportBookingAsync(apiToken, booking, historyId, folderPath, agencyKey);

                    // Retry if unauthorized
                    if (statusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        await CreateNewToken();
                        if (!string.IsNullOrWhiteSpace(apiToken))
                        {
                            var retryResult = await _lawTrakBookingImportService
                                .ImportBookingAsync(apiToken, booking, historyId, folderPath, agencyKey);

                            success = retryResult.Success;
                            statusCode = retryResult.StatusCode;
                            message = retryResult.Message;
                        }
                    }

                    if (success)
                    {
                        Console.WriteLine($"Batch {index}: Successfully imported Bookings {bookingNumber}.");
                    }
                    else
                    {
                        var logMessage = $"Insertion failed for Bookings {bookingNumber}: {statusCode} - {message}";
                        WriteToLogFile("LawTrak", "Bookings", agencyKey, logMessage);
                    }
                }
                catch (Exception ex)
                {
                    var logMessage = $"Bookings {index} (Booking Number {bookingNumber}) exception: {ex}";
                    WriteToLogFile("LawTrak", "Bookings", agencyKey, logMessage);
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
                "sp_GetCitations"
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
        private async Task<Guid?> ImportBadgeCitationData(List<Dictionary<string, object>> citations, Guid? historyId, int index, string folderPath, Guid agencyKey)
        {
            WriteToLogFile("Badge", "Citations", agencyKey, "-------------------");
            WriteToLogFile("Badge", "Citations", agencyKey, "Citations Logs");
            WriteToLogFile("Badge", "Citations", agencyKey, $"Batch {index}");
            WriteToLogFile("Badge", "Citations", agencyKey, "-------------------");
            foreach (var citation in citations)
            {
                string citationNumber = "Unknown";
                try
                {

                    if (citation.ContainsKey("Citation Number"))
                        citationNumber = citation["Citation Number"]?.ToString() ?? "Unknown";

                    var (success, statusCode, message) = await _badgeCitationImportService
                        .ImportCitationsAsync(apiToken, citation, historyId, folderPath, agencyKey);

                    // Retry if unauthorized
                    if (statusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        await CreateNewToken();
                        if (!string.IsNullOrWhiteSpace(apiToken))
                        {
                            var retryResult = await _badgeCitationImportService
                                .ImportCitationsAsync(apiToken, citation, historyId, folderPath, agencyKey);

                            success = retryResult.Success;
                            statusCode = retryResult.StatusCode;
                            message = retryResult.Message;
                        }
                    }

                    if (success)
                    {
                        Console.WriteLine($"Batch {index}: Successfully imported citation {citationNumber}.");
                    }
                    else
                    {
                        var logMessage = $"Insertion failed for citation {citationNumber}: {statusCode} - {message}";
                        WriteToLogFile("Badge", "Citations", agencyKey, logMessage);
                    }
                }
                catch (Exception ex)
                {
                    var logMessage = $"Citation {index} (Citation Number {citationNumber}) exception: {ex}";
                    WriteToLogFile("Badge", "Citations", agencyKey, logMessage);
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
        "sp_GetWarrants"
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

        private async Task<Guid?> ImportBadgeWarrantData(
    List<Dictionary<string, object>> warrants,
    Guid? historyId,
    int index,
    string folderPath,
    Guid agencyKey)
        {
            WriteToLogFile("Badge", "Warrants", agencyKey, "-------------------");
            WriteToLogFile("Badge", "Warrants", agencyKey, "Warrants Logs");
            WriteToLogFile("Badge", "Warrants", agencyKey, $"Batch {index}");
            WriteToLogFile("Badge", "Warrants", agencyKey, "-------------------");

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
                        WriteToLogFile("Badge", "Warrants", agencyKey, logMessage);
                    }
                }
                catch (Exception ex)
                {
                    var logMessage = $"Warrant {index} (Warrant Number {warrantNumber}) exception: {ex}";
                    WriteToLogFile("Badge", "Warrants", agencyKey, logMessage);
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

        private async Task<Guid?> ImportBadgeCaseData(
            List<Dictionary<string, object>> cases,
            Guid? historyId,
            int index,
            string folderPath,
            Guid agencyKey)
        {
            WriteToLogFile("Badge", "Cases", agencyKey, "-------------------");
            WriteToLogFile("Badge", "Cases", agencyKey, "Cases Logs");
            WriteToLogFile("Badge", "Cases", agencyKey, $"Batch {index}");
            WriteToLogFile("Badge", "Cases", agencyKey, "-------------------");
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
                        WriteToLogFile("Badge", "Cases", agencyKey, logMessage);
                    }
                }
                catch (Exception ex)
                {
                    var logMessage = $"Case {index} (Case Number {caseNumber}) exception: {ex}";
                    WriteToLogFile("Badge", "Cases", agencyKey, logMessage);
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
                "sp_GetCallData_JSON"
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

        private async Task<Guid?> ImportBadgeCallData(
            List<Dictionary<string, object>> calls,
            Guid? historyId,
            int index,
            string folderPath,
            Guid agencyKey)
        {
            WriteToLogFile("Badge", "Calls", agencyKey, "-------------------");
            WriteToLogFile("Badge", "Calls", agencyKey, "Calls Logs");
            WriteToLogFile("Badge", "Calls", agencyKey, $"Batch {index}");
            WriteToLogFile("Badge", "Calls", agencyKey, "-------------------");

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
                        WriteToLogFile("Badge", "Calls", agencyKey, logMessage);
                    }
                }
                catch (Exception ex)
                {
                    var logMessage = $"Call {index} (Call/CFS Number {cfsNumber}) exception: {ex}";
                    WriteToLogFile("Badge", "Calls", agencyKey, logMessage);
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
                // Base folder: Logs/<RMS>/<Module>
                string baseDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
            string logDir = Path.Combine(baseDir, rms, agencyKey.ToString(), module); 
            if (!Directory.Exists(logDir))
            {
                Directory.CreateDirectory(logDir);
            }

            string fileName = $"ActivityLog_{DateTime.Now:yyyy_MM_dd}.txt";
            string filePath = Path.Combine(logDir, fileName);


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
                WriteToLogFile(ex.ToString());
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

        private void TabLawTrak_Click(object sender, EventArgs e)
        {

        }

        private void LawTrak_SQL_Connection_Enter(object sender, EventArgs e)
        {

        }

        private void BtnSqlConnect_LawTrak_Click(object sender, EventArgs e)
        {
            ConnectSql("LawTrak");
        }

        private void gbLawTrak_Modules_Enter(object sender, EventArgs e)
        {

        }

        private void LblSqlStatus_InSynch_Click(object sender, EventArgs e)
        {

        }

        private void InSynch_SQL_Connection_Enter(object sender, EventArgs e)
        {

        }

        private void BtnSqlConnect_InSynch_Click(object sender, EventArgs e)
        {
            ConnectSql("InSynch");
        }
        private void gbInSynch_Modules_Enter(object sender, EventArgs e)
        {

        }
        private void Badge_SQL_Connection_Enter(object sender, EventArgs e)
        {

        }

        private void BtnSqlConnect_Badge_Click(object sender, EventArgs e)
        {
            ConnectSql("Badge");
        }
        private void gbBadge_Modules_Enter(object sender, EventArgs e)
        {

        }
        private async void BtnLawTrak_Migration_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sqlConnectionString) || string.IsNullOrWhiteSpace(apiToken))
                {
                    MessageBox.Show("Please check sql database and api connection first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                progressBarLawTrak.Visible = true;
                progressBarLawTrak.Style = ProgressBarStyle.Marquee;

                if (cbCitations_LawTrak.Checked)
                {
                    await ProcessLawTrakCitationsExportImport();
                }

                if (cbWarrants_LawTrak.Checked)
                {
                    await ProcessLawTrakWarrantsExportImport();
                }

                if (cbCases_LawTrak.Checked)
                {
                    await ProcessLawTrakCasesExportImport();
                }

                if (cbAffidavits_LawTrak.Checked)
                {
                    await ProcessLawTrakAffidavitsExportImport();
                }

                if (cbEvidences_LawTrak.Checked)
                {
                    await ProcessLawTrakEvidencesExportImport();
                }

                if (cbAccounting_LawTrak.Checked)
                {
                    await ProcessLawTrakAccountingExportImport();
                }

                if (cbJuvenile_LawTrak.Checked)
                {
                    await ProcessLawTrakJuvenileExportImport();
                }

                if (cbBookings_LawTrak.Checked)
                {
                    await ProcessLawTrakBookingExportImport();
                }

                MessageBox.Show("Selected import process completed, Please check log files.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                WriteToLogFile(ex.ToString());
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            progressBarLawTrak.Visible = false;

        }

        private async void BtnInSynch_Migration_Click(object sender, EventArgs e)
        {

        }

        private async void BtnBadge_Migration_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sqlConnectionString) || string.IsNullOrWhiteSpace(apiToken))
                {
                    MessageBox.Show("Please check sql database and api connection first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                progressBarBadge.Visible = true;
                progressBarBadge.Style = ProgressBarStyle.Marquee;
                if (cbCitations_Badge.Checked)
                {
                    await ProcessBadgeCitationsExportImport();
                }

                if (cbWarrants_Badge.Checked)
                {
                    await ProcessBadgeWarrantsExportImport();
                }

                if (cbCases_Badge.Checked)
                {
                    await ProcessBadgeCasesExportImport();
                }

                if (cbCalls_Badge.Checked)
                {
                    await ProcessBadgeCallsExportImport();
                }

                MessageBox.Show("Selected import process completed, Please check log files.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                WriteToLogFile(ex.ToString());
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            progressBarBadge.Visible = false;
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

        private void cbCases_LawTrak_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void grpApiAuth_Enter(object sender, EventArgs e)
        {

        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void progressBar2_Click(object sender, EventArgs e)
        {

        }

        private void progressBar3_Click(object sender, EventArgs e)
        {

        }

        private void lblApiStatus_Click_1(object sender, EventArgs e)
        {

        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Select a folder";
                folderDialog.ShowNewFolderButton = true;

                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    txtLTFolderPath.Text = folderDialog.SelectedPath; // display selected folder
                }
            }
        }

        private void txtLTFolderPath_TextChanged(object sender, EventArgs e)
        {

        }

        private void gbLawTrak_Folder_Enter(object sender, EventArgs e)
        {

        }
        private void txtBadgeFolderPath_TextChanged(object sender, EventArgs e)
        {

        }
        private void gbBadge_Folder_Enter(object sender, EventArgs e)
        {

        }

        private void Badge_Modules_Enter(object sender, EventArgs e)
        {

        }

        private void txtSqlServer_InSynch_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

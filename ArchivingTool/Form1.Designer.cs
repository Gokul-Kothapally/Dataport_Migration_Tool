using MongoDB.Driver;
using System.Reflection;
using System.Windows.Forms;

namespace ArchivingTool
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            tabControl = new TabControl();
            tabLawTrak = new TabPage();
            gbLawTrak_Folder = new GroupBox();
            btnLawTrakBrowse = new Button();
            txtLTFolderPath = new TextBox();
            gbLawTrak_Modules = new GroupBox();
            progressBarLawTrak = new ProgressBar();
            btnLawTrak_Migration = new Button();
            cbCases_LawTrak = new CheckBox();
            cbCitations_LawTrak = new CheckBox();
            cbWarrants_LawTrak = new CheckBox();
            cbBookings_LawTrak = new CheckBox();
            cbJuvenile_LawTrak = new CheckBox();
            cbAccounting_LawTrak = new CheckBox();
            cbEvidences_LawTrak = new CheckBox();
            cbAffidavits_LawTrak = new CheckBox();
            gbLawTrak_SQL_Conn = new GroupBox();
            btnLawTrak_Reset = new Button();
            txtSqlServer_LawTrak = new TextBox();
            txtSqlDb_LawTrak = new TextBox();
            txtSqlUser_LawTrak = new TextBox();
            txtSqlPass_LawTrak = new TextBox();
            cbSqlAuth_LawTrak = new ComboBox();
            btnSqlConnect_LawTrak = new Button();
            lblSqlStatus_LawTrak = new Label();
            tabInSynch = new TabPage();
            gbInSynch_Modules = new GroupBox();
            progressBarInSynch = new ProgressBar();
            btnInSynch_Migration = new Button();
            cbCitations_InSynch = new CheckBox();
            cbCases_InSynch = new CheckBox();
            gbInSynch_SQL_Conn = new GroupBox();
            btnInSynch_Reset = new Button();
            txtSqlServer_InSynch = new TextBox();
            txtSqlDb_InSynch = new TextBox();
            txtSqlUser_InSynch = new TextBox();
            txtSqlPass_InSynch = new TextBox();
            btnSqlConnect_InSynch = new Button();
            lblSqlStatus_InSynch = new Label();
            tabBadge = new TabPage();
            gbBadge_Folder = new GroupBox();
            btnBadgeBrowse = new Button();
            txtBadgeFolderPath = new TextBox();
            gbBadge_SQL_Conn = new GroupBox();
            btnBadge_Reset = new Button();
            txtSqlServer_Badge = new TextBox();
            txtSqlDb_Badge = new TextBox();
            txtSqlUser_Badge = new TextBox();
            txtSqlPass_Badge = new TextBox();
            cbSqlAuth_Badge = new ComboBox();
            btnSqlConnect_Badge = new Button();
            lblSqlStatus_Badge = new Label();
            gbBadge_Modules = new GroupBox();
            progressBarBadge = new ProgressBar();
            cbCalls_Badge = new CheckBox();
            cbCases_Badge = new CheckBox();
            cbCitations_Badge = new CheckBox();
            cbWarrants_Badge = new CheckBox();
            btnBadge_Migration = new Button();
            txtAgencyKey = new TextBox();
            txtAgencyAPIKey = new TextBox();
            btnGenerateToken = new Button();
            lblApiStatus = new Label();
            grpApiAuth = new GroupBox();
            folderBrowserDialog1 = new FolderBrowserDialog();    
            tabControl.SuspendLayout();
            tabLawTrak.SuspendLayout();
            gbLawTrak_Folder.SuspendLayout();
            gbLawTrak_Modules.SuspendLayout();
            gbLawTrak_SQL_Conn.SuspendLayout();
            tabInSynch.SuspendLayout();
            gbInSynch_Modules.SuspendLayout();
            gbInSynch_SQL_Conn.SuspendLayout();
            tabBadge.SuspendLayout();
            gbBadge_Folder.SuspendLayout();
            gbBadge_SQL_Conn.SuspendLayout();
            gbBadge_Modules.SuspendLayout();
            grpApiAuth.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl
            // 
            tabControl.Controls.Add(tabLawTrak);
            tabControl.Controls.Add(tabInSynch);
            tabControl.Controls.Add(tabBadge);
            tabControl.Location = new Point(13, 232);
            tabControl.Margin = new Padding(4);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(950, 830);
            tabControl.TabIndex = 0;
            // 
            // tabLawTrak
            // 
            tabLawTrak.Controls.Add(gbLawTrak_Folder);
            tabLawTrak.Controls.Add(gbLawTrak_Modules);
            tabLawTrak.Controls.Add(gbLawTrak_SQL_Conn);
            tabLawTrak.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            tabLawTrak.Location = new Point(4, 34);
            tabLawTrak.Margin = new Padding(4);
            tabLawTrak.Name = "tabLawTrak";
            tabLawTrak.Size = new Size(942, 792);
            tabLawTrak.TabIndex = 0;
            tabLawTrak.Text = "LawTrak";
            tabLawTrak.Click += TabLawTrak_Click;
            // 
            // gbLawTrak_Folder
            // 
            gbLawTrak_Folder.Controls.Add(btnLawTrakBrowse);
            gbLawTrak_Folder.Controls.Add(txtLTFolderPath);
            gbLawTrak_Folder.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            gbLawTrak_Folder.Location = new Point(8, 18);
            gbLawTrak_Folder.Name = "gbLawTrak_Folder";
            gbLawTrak_Folder.Size = new Size(920, 182);
            gbLawTrak_Folder.TabIndex = 16;
            gbLawTrak_Folder.TabStop = false;
            gbLawTrak_Folder.Text = "Browse Attachments Folder";
            gbLawTrak_Folder.Enter += gbLawTrak_Folder_Enter;
            // 
            // btnLawTrakBrowse
            // 
            btnLawTrakBrowse.Location = new Point(747, 62);
            btnLawTrakBrowse.Name = "btnLawTrakBrowse";
            btnLawTrakBrowse.Size = new Size(146, 34);
            btnLawTrakBrowse.TabIndex = 14;
            btnLawTrakBrowse.Text = "Browse…";
            btnLawTrakBrowse.UseVisualStyleBackColor = true;
            btnLawTrakBrowse.Click += btnBrowse_Click;
            // 
            // txtLTFolderPath
            // 
            txtLTFolderPath.Location = new Point(25, 62);
            txtLTFolderPath.Name = "txtLTFolderPath";
            txtLTFolderPath.Size = new Size(677, 34);
            txtLTFolderPath.TabIndex = 15;
            txtLTFolderPath.TextChanged += txtLTFolderPath_TextChanged;
            // 
            // gbLawTrak_Modules
            // 
            gbLawTrak_Modules.Controls.Add(cbBookings_LawTrak);
            gbLawTrak_Modules.Controls.Add(cbJuvenile_LawTrak);
            gbLawTrak_Modules.Controls.Add(cbAccounting_LawTrak);
            gbLawTrak_Modules.Controls.Add(cbEvidences_LawTrak);
            gbLawTrak_Modules.Controls.Add(cbAffidavits_LawTrak);
            gbLawTrak_Modules.Controls.Add(progressBarLawTrak);
            gbLawTrak_Modules.Controls.Add(cbCases_LawTrak);
            gbLawTrak_Modules.Controls.Add(btnLawTrak_Migration);
            gbLawTrak_Modules.Controls.Add(cbCitations_LawTrak);
            gbLawTrak_Modules.Controls.Add(cbWarrants_LawTrak);
            gbLawTrak_Modules.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            gbLawTrak_Modules.Location = new Point(8, 483);
            gbLawTrak_Modules.Name = "gbLawTrak_Modules";
            gbLawTrak_Modules.Size = new Size(920, 223);
            gbLawTrak_Modules.TabIndex = 13;
            gbLawTrak_Modules.TabStop = false;
            gbLawTrak_Modules.Text = "Modules";
            gbLawTrak_Modules.Enter += gbLawTrak_Modules_Enter;
            // 
            // progressBarLawTrak
            // 
            progressBarLawTrak.Location = new Point(532, 139);
            progressBarLawTrak.Margin = new Padding(4, 5, 4, 5);
            progressBarLawTrak.Name = "progressBarLawTrak";
            progressBarLawTrak.Size = new Size(353, 48);
            progressBarLawTrak.TabIndex = 22;
            progressBarLawTrak.Visible = false;
            progressBarLawTrak.Click += progressBar1_Click;
            // 
            // btnLawTrak_Migration
            // 
            btnLawTrak_Migration.Font = new Font("Segoe UI", 10F);
            btnLawTrak_Migration.Location = new Point(650, 90);
            btnLawTrak_Migration.Name = "btnLawTrak_Migration";
            btnLawTrak_Migration.Size = new Size(125, 41);
            btnLawTrak_Migration.TabIndex = 13;
            btnLawTrak_Migration.Text = "Migrate";
            btnLawTrak_Migration.UseVisualStyleBackColor = true;
            btnLawTrak_Migration.Click += BtnLawTrak_Migration_Click;
            // 
            // cbCitations_LawTrak
            // 
            cbCitations_LawTrak.Font = new Font("Segoe UI", 10F);
            cbCitations_LawTrak.Location = new Point(17, 37);
            cbCitations_LawTrak.Margin = new Padding(4);
            cbCitations_LawTrak.Name = "cbCitations_LawTrak";
            cbCitations_LawTrak.Size = new Size(122, 56);
            cbCitations_LawTrak.TabIndex = 11;
            cbCitations_LawTrak.Text = "Citations";
            // 
            // cbWarrants_LawTrak
            // 
            cbWarrants_LawTrak.Font = new Font("Segoe UI", 10F);
            cbWarrants_LawTrak.Location = new Point(147, 48);
            cbWarrants_LawTrak.Margin = new Padding(4);
            cbWarrants_LawTrak.Name = "cbWarrants_LawTrak";
            cbWarrants_LawTrak.Size = new Size(116, 35);
            cbWarrants_LawTrak.TabIndex = 12;
            cbWarrants_LawTrak.Text = "Warrants";
            // 
            // cbCases_LawTrak
            // 
            cbCases_LawTrak.Font = new Font("Segoe UI", 10F);
            cbCases_LawTrak.Location = new Point(284, 37);
            cbCases_LawTrak.Margin = new Padding(4);
            cbCases_LawTrak.Name = "cbCases_LawTrak";
            cbCases_LawTrak.Size = new Size(100, 56);
            cbCases_LawTrak.TabIndex = 21;
            cbCases_LawTrak.Text = "Cases";
            cbCases_LawTrak.CheckedChanged += cbCases_LawTrak_CheckedChanged;
            // 
            // cbJuvenile_LawTrak
            // 
            cbJuvenile_LawTrak.Font = new Font("Segoe UI", 10F);
            cbJuvenile_LawTrak.Location = new Point(17, 139);
            cbJuvenile_LawTrak.Margin = new Padding(4);
            cbJuvenile_LawTrak.Name = "cbJuvenile_LawTrak";
            cbJuvenile_LawTrak.Size = new Size(148, 56);
            cbJuvenile_LawTrak.TabIndex = 26;
            cbJuvenile_LawTrak.Text = "Juveniles";
            // 
            // cbAccounting_LawTrak
            // 
            cbAccounting_LawTrak.Font = new Font("Segoe UI", 10F);
            cbAccounting_LawTrak.Location = new Point(284, 90);
            cbAccounting_LawTrak.Margin = new Padding(4);
            cbAccounting_LawTrak.Name = "cbAccounting_LawTrak";
            cbAccounting_LawTrak.Size = new Size(148, 56);
            cbAccounting_LawTrak.TabIndex = 25;
            cbAccounting_LawTrak.Text = "Accounting";
            // 
            // cbEvidences_LawTrak
            // 
            cbEvidences_LawTrak.Font = new Font("Segoe UI", 10F);
            cbEvidences_LawTrak.Location = new Point(147, 91);
            cbEvidences_LawTrak.Margin = new Padding(4);
            cbEvidences_LawTrak.Name = "cbEvidences_LawTrak";
            cbEvidences_LawTrak.Size = new Size(129, 56);
            cbEvidences_LawTrak.TabIndex = 24;
            cbEvidences_LawTrak.Text = "Evidences";
            // 
            // cbAffidavits_LawTrak
            // 
            cbAffidavits_LawTrak.Font = new Font("Segoe UI", 10F);
            cbAffidavits_LawTrak.Location = new Point(16, 90);
            cbAffidavits_LawTrak.Margin = new Padding(4);
            cbAffidavits_LawTrak.Name = "cbAffidavits_LawTrak";
            cbAffidavits_LawTrak.Size = new Size(122, 56);
            cbAffidavits_LawTrak.TabIndex = 23;
            cbAffidavits_LawTrak.Text = "Affidavits";
            // 
            // cbBookings_LawTrak
            // 
            cbBookings_LawTrak.Font = new Font("Segoe UI", 10F);
            cbBookings_LawTrak.Location = new Point(147, 139);
            cbBookings_LawTrak.Margin = new Padding(4);
            cbBookings_LawTrak.Name = "cbBookings_LawTrak";
            cbBookings_LawTrak.Size = new Size(129, 56);
            cbBookings_LawTrak.TabIndex = 27;
            cbBookings_LawTrak.Text = "Bookings";
            // 
            // gbLawTrak_SQL_Conn
            // 
            gbLawTrak_SQL_Conn.Controls.Add(btnLawTrak_Reset);
            gbLawTrak_SQL_Conn.Controls.Add(txtSqlServer_LawTrak);
            gbLawTrak_SQL_Conn.Controls.Add(txtSqlDb_LawTrak);
            gbLawTrak_SQL_Conn.Controls.Add(txtSqlUser_LawTrak);
            gbLawTrak_SQL_Conn.Controls.Add(txtSqlPass_LawTrak);
            gbLawTrak_SQL_Conn.Controls.Add(cbSqlAuth_LawTrak);
            gbLawTrak_SQL_Conn.Controls.Add(btnSqlConnect_LawTrak);
            gbLawTrak_SQL_Conn.Controls.Add(lblSqlStatus_LawTrak);
            gbLawTrak_SQL_Conn.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            gbLawTrak_SQL_Conn.Location = new Point(8, 216);
            gbLawTrak_SQL_Conn.Name = "gbLawTrak_SQL_Conn";
            gbLawTrak_SQL_Conn.Size = new Size(919, 261);
            gbLawTrak_SQL_Conn.TabIndex = 12;
            gbLawTrak_SQL_Conn.TabStop = false;
            gbLawTrak_SQL_Conn.Text = "SQL Connection";
            gbLawTrak_SQL_Conn.Enter += LawTrak_SQL_Connection_Enter;
            // 
            // btnLawTrak_Reset
            // 
            btnLawTrak_Reset.Font = new Font("Segoe UI", 10F);
            btnLawTrak_Reset.Location = new Point(636, 121);
            btnLawTrak_Reset.Margin = new Padding(4);
            btnLawTrak_Reset.Name = "btnLawTrak_Reset";
            btnLawTrak_Reset.Size = new Size(125, 38);
            btnLawTrak_Reset.TabIndex = 21;
            btnLawTrak_Reset.Text = "Reset";
            btnLawTrak_Reset.Click += BtnLawTrak_Reset_Click;
            // 
            // txtSqlServer_LawTrak
            // 
            txtSqlServer_LawTrak.Font = new Font("Segoe UI", 10F);
            txtSqlServer_LawTrak.Location = new Point(16, 42);
            txtSqlServer_LawTrak.Margin = new Padding(4);
            txtSqlServer_LawTrak.Name = "txtSqlServer_LawTrak";
            txtSqlServer_LawTrak.PlaceholderText = "SQL Server";
            txtSqlServer_LawTrak.Size = new Size(249, 34);
            txtSqlServer_LawTrak.TabIndex = 14;
            // 
            // txtSqlDb_LawTrak
            // 
            txtSqlDb_LawTrak.Font = new Font("Segoe UI", 10F);
            txtSqlDb_LawTrak.Location = new Point(319, 42);
            txtSqlDb_LawTrak.Margin = new Padding(4);
            txtSqlDb_LawTrak.Name = "txtSqlDb_LawTrak";
            txtSqlDb_LawTrak.PlaceholderText = "Database";
            txtSqlDb_LawTrak.Size = new Size(249, 34);
            txtSqlDb_LawTrak.TabIndex = 15;
            // 
            // txtSqlUser_LawTrak
            // 
            txtSqlUser_LawTrak.Font = new Font("Segoe UI", 10F);
            txtSqlUser_LawTrak.Location = new Point(16, 125);
            txtSqlUser_LawTrak.Margin = new Padding(4);
            txtSqlUser_LawTrak.Name = "txtSqlUser_LawTrak";
            txtSqlUser_LawTrak.PlaceholderText = "Username";
            txtSqlUser_LawTrak.Size = new Size(249, 34);
            txtSqlUser_LawTrak.TabIndex = 16;
            // 
            // txtSqlPass_LawTrak
            // 
            txtSqlPass_LawTrak.Font = new Font("Segoe UI", 10F);
            txtSqlPass_LawTrak.Location = new Point(319, 125);
            txtSqlPass_LawTrak.Margin = new Padding(4);
            txtSqlPass_LawTrak.Name = "txtSqlPass_LawTrak";
            txtSqlPass_LawTrak.PlaceholderText = "Password";
            txtSqlPass_LawTrak.Size = new Size(249, 34);
            txtSqlPass_LawTrak.TabIndex = 17;
            txtSqlPass_LawTrak.UseSystemPasswordChar = true;
            // 
            // cbSqlAuth_LawTrak
            // 
            cbSqlAuth_LawTrak.Font = new Font("Segoe UI", 10F);
            cbSqlAuth_LawTrak.Items.AddRange(new object[] { "Windows", "SQL Server" });
            cbSqlAuth_LawTrak.Location = new Point(636, 42);
            cbSqlAuth_LawTrak.Margin = new Padding(4);
            cbSqlAuth_LawTrak.Name = "cbSqlAuth_LawTrak";
            cbSqlAuth_LawTrak.Size = new Size(249, 36);
            cbSqlAuth_LawTrak.TabIndex = 18;
            // 
            // btnSqlConnect_LawTrak
            // 
            btnSqlConnect_LawTrak.Font = new Font("Segoe UI", 10F);
            btnSqlConnect_LawTrak.Location = new Point(138, 190);
            btnSqlConnect_LawTrak.Margin = new Padding(4);
            btnSqlConnect_LawTrak.Name = "btnSqlConnect_LawTrak";
            btnSqlConnect_LawTrak.Size = new Size(125, 38);
            btnSqlConnect_LawTrak.TabIndex = 19;
            btnSqlConnect_LawTrak.Text = "Connect";
            btnSqlConnect_LawTrak.Click += BtnSqlConnect_LawTrak_Click;
            // 
            // lblSqlStatus_LawTrak
            // 
            lblSqlStatus_LawTrak.Font = new Font("Segoe UI", 10F);
            lblSqlStatus_LawTrak.Location = new Point(318, 195);
            lblSqlStatus_LawTrak.Margin = new Padding(4, 0, 4, 0);
            lblSqlStatus_LawTrak.Name = "lblSqlStatus_LawTrak";
            lblSqlStatus_LawTrak.Size = new Size(250, 25);
            lblSqlStatus_LawTrak.TabIndex = 20;
            // 
            // tabInSynch
            // 
            tabInSynch.Controls.Add(gbInSynch_Modules);
            tabInSynch.Controls.Add(gbInSynch_SQL_Conn);
            tabInSynch.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            tabInSynch.Location = new Point(4, 34);
            tabInSynch.Margin = new Padding(4);
            tabInSynch.Name = "tabInSynch";
            tabInSynch.Size = new Size(942, 792);
            tabInSynch.TabIndex = 1;
            tabInSynch.Text = "In-Synch";
            // 
            // gbInSynch_Modules
            // 
            gbInSynch_Modules.Controls.Add(progressBarInSynch);
            gbInSynch_Modules.Controls.Add(btnInSynch_Migration);
            gbInSynch_Modules.Controls.Add(cbCitations_InSynch);
            gbInSynch_Modules.Controls.Add(cbCases_InSynch);
            gbInSynch_Modules.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            gbInSynch_Modules.Location = new Point(8, 282);
            gbInSynch_Modules.Name = "gbInSynch_Modules";
            gbInSynch_Modules.Size = new Size(918, 243);
            gbInSynch_Modules.TabIndex = 10;
            gbInSynch_Modules.TabStop = false;
            gbInSynch_Modules.Text = "Modules";
            gbInSynch_Modules.Enter += gbInSynch_Modules_Enter;
            // 
            // progressBarInSynch
            // 
            progressBarInSynch.Location = new Point(511, 147);
            progressBarInSynch.Margin = new Padding(4, 5, 4, 5);
            progressBarInSynch.Name = "progressBarInSynch";
            progressBarInSynch.Size = new Size(353, 48);
            progressBarInSynch.TabIndex = 23;
            progressBarInSynch.Visible = false;
            progressBarInSynch.Click += progressBar2_Click;
            // 
            // btnInSynch_Migration
            // 
            btnInSynch_Migration.Font = new Font("Segoe UI", 10F);
            btnInSynch_Migration.Location = new Point(722, 96);
            btnInSynch_Migration.Name = "btnInSynch_Migration";
            btnInSynch_Migration.Size = new Size(142, 43);
            btnInSynch_Migration.TabIndex = 11;
            btnInSynch_Migration.Text = "Migrate";
            btnInSynch_Migration.UseVisualStyleBackColor = true;
            btnInSynch_Migration.Click += BtnInSynch_Migration_Click;
            // 
            // cbCitations_InSynch
            // 
            cbCitations_InSynch.Font = new Font("Segoe UI", 10F);
            cbCitations_InSynch.Location = new Point(23, 45);
            cbCitations_InSynch.Margin = new Padding(4);
            cbCitations_InSynch.Name = "cbCitations_InSynch";
            cbCitations_InSynch.Size = new Size(122, 48);
            cbCitations_InSynch.TabIndex = 9;
            cbCitations_InSynch.Text = "Citations";
            // 
            // cbCases_InSynch
            // 
            cbCases_InSynch.Font = new Font("Segoe UI", 10F);
            cbCases_InSynch.Location = new Point(185, 45);
            cbCases_InSynch.Margin = new Padding(4);
            cbCases_InSynch.Name = "cbCases_InSynch";
            cbCases_InSynch.Size = new Size(87, 48);
            cbCases_InSynch.TabIndex = 10;
            cbCases_InSynch.Text = "Cases";
            // 
            // gbInSynch_SQL_Conn
            // 
            gbInSynch_SQL_Conn.Controls.Add(btnInSynch_Reset);
            gbInSynch_SQL_Conn.Controls.Add(txtSqlServer_InSynch);
            gbInSynch_SQL_Conn.Controls.Add(txtSqlDb_InSynch);
            gbInSynch_SQL_Conn.Controls.Add(txtSqlUser_InSynch);
            gbInSynch_SQL_Conn.Controls.Add(txtSqlPass_InSynch);
            gbInSynch_SQL_Conn.Controls.Add(btnSqlConnect_InSynch);
            gbInSynch_SQL_Conn.Controls.Add(lblSqlStatus_InSynch);
            gbInSynch_SQL_Conn.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            gbInSynch_SQL_Conn.Location = new Point(8, 22);
            gbInSynch_SQL_Conn.Name = "gbInSynch_SQL_Conn";
            gbInSynch_SQL_Conn.Size = new Size(919, 254);
            gbInSynch_SQL_Conn.TabIndex = 9;
            gbInSynch_SQL_Conn.TabStop = false;
            gbInSynch_SQL_Conn.Text = "SQL Connection";
            gbInSynch_SQL_Conn.Enter += InSynch_SQL_Connection_Enter;
            // 
            // btnInSynch_Reset
            // 
            btnInSynch_Reset.Font = new Font("Segoe UI", 10F);
            btnInSynch_Reset.Location = new Point(615, 134);
            btnInSynch_Reset.Margin = new Padding(4);
            btnInSynch_Reset.Name = "btnInSynch_Reset";
            btnInSynch_Reset.Size = new Size(125, 38);
            btnInSynch_Reset.TabIndex = 22;
            btnInSynch_Reset.Text = "Reset";
            btnInSynch_Reset.Click += BtnInSynch_Reset_Click;
            // 
            // txtSqlServer_InSynch
            // 
            txtSqlServer_InSynch.Font = new Font("Segoe UI", 10F);
            txtSqlServer_InSynch.Location = new Point(23, 65);
            txtSqlServer_InSynch.Margin = new Padding(4);
            txtSqlServer_InSynch.Name = "txtSqlServer_InSynch";
            txtSqlServer_InSynch.PlaceholderText = "SQL Server";
            txtSqlServer_InSynch.Size = new Size(249, 34);
            txtSqlServer_InSynch.TabIndex = 7;
            txtSqlServer_InSynch.TextChanged += txtSqlServer_InSynch_TextChanged;
            // 
            // txtSqlDb_InSynch
            // 
            txtSqlDb_InSynch.Font = new Font("Segoe UI", 10F);
            txtSqlDb_InSynch.Location = new Point(314, 65);
            txtSqlDb_InSynch.Margin = new Padding(4);
            txtSqlDb_InSynch.Name = "txtSqlDb_InSynch";
            txtSqlDb_InSynch.PlaceholderText = "Database";
            txtSqlDb_InSynch.Size = new Size(249, 34);
            txtSqlDb_InSynch.TabIndex = 8;
            // 
            // txtSqlUser_InSynch
            // 
            txtSqlUser_InSynch.Font = new Font("Segoe UI", 10F);
            txtSqlUser_InSynch.Location = new Point(23, 136);
            txtSqlUser_InSynch.Margin = new Padding(4);
            txtSqlUser_InSynch.Name = "txtSqlUser_InSynch";
            txtSqlUser_InSynch.PlaceholderText = "Username";
            txtSqlUser_InSynch.Size = new Size(249, 34);
            txtSqlUser_InSynch.TabIndex = 9;
            // 
            // txtSqlPass_InSynch
            // 
            txtSqlPass_InSynch.Font = new Font("Segoe UI", 10F);
            txtSqlPass_InSynch.Location = new Point(314, 136);
            txtSqlPass_InSynch.Margin = new Padding(4);
            txtSqlPass_InSynch.Name = "txtSqlPass_InSynch";
            txtSqlPass_InSynch.PlaceholderText = "Password";
            txtSqlPass_InSynch.Size = new Size(249, 34);
            txtSqlPass_InSynch.TabIndex = 10;
            txtSqlPass_InSynch.UseSystemPasswordChar = true;
            // 
            // btnSqlConnect_InSynch
            // 
            btnSqlConnect_InSynch.Font = new Font("Segoe UI", 10F);
            btnSqlConnect_InSynch.Location = new Point(147, 190);
            btnSqlConnect_InSynch.Margin = new Padding(4);
            btnSqlConnect_InSynch.Name = "btnSqlConnect_InSynch";
            btnSqlConnect_InSynch.Size = new Size(125, 38);
            btnSqlConnect_InSynch.TabIndex = 12;
            btnSqlConnect_InSynch.Text = "Connect";
            btnSqlConnect_InSynch.Click += BtnSqlConnect_InSynch_Click;
            // 
            // lblSqlStatus_InSynch
            // 
            lblSqlStatus_InSynch.Font = new Font("Segoe UI", 10F);
            lblSqlStatus_InSynch.Location = new Point(314, 195);
            lblSqlStatus_InSynch.Margin = new Padding(4, 0, 4, 0);
            lblSqlStatus_InSynch.Name = "lblSqlStatus_InSynch";
            lblSqlStatus_InSynch.Size = new Size(250, 25);
            lblSqlStatus_InSynch.TabIndex = 13;
            lblSqlStatus_InSynch.Click += LblSqlStatus_InSynch_Click;
            // 
            // tabBadge
            // 
            tabBadge.Controls.Add(gbBadge_Folder);
            tabBadge.Controls.Add(gbBadge_SQL_Conn);
            tabBadge.Controls.Add(gbBadge_Modules);
            tabBadge.Location = new Point(4, 34);
            tabBadge.Name = "tabBadge";
            tabBadge.Padding = new Padding(3);
            tabBadge.Size = new Size(942, 792);
            tabBadge.TabIndex = 2;
            tabBadge.Text = "Badge";
            tabBadge.UseVisualStyleBackColor = true;
            // 
            // gbBadge_Folder
            // 
            gbBadge_Folder.Controls.Add(btnBadgeBrowse);
            gbBadge_Folder.Controls.Add(txtBadgeFolderPath);
            gbBadge_Folder.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            gbBadge_Folder.Location = new Point(11, 19);
            gbBadge_Folder.Name = "gbBadge_Folder";
            gbBadge_Folder.Size = new Size(920, 153);
            gbBadge_Folder.TabIndex = 19;
            gbBadge_Folder.TabStop = false;
            gbBadge_Folder.Text = "Browse Attachments Folder";
            gbBadge_Folder.Enter += gbBadge_Folder_Enter;
            // 
            // btnBadgeBrowse
            // 
            btnBadgeBrowse.Location = new Point(742, 62);
            btnBadgeBrowse.Name = "btnBadgeBrowse";
            btnBadgeBrowse.Size = new Size(143, 34);
            btnBadgeBrowse.TabIndex = 14;
            btnBadgeBrowse.Text = "Browse…";
            btnBadgeBrowse.UseVisualStyleBackColor = true;
            // 
            // txtBadgeFolderPath
            // 
            txtBadgeFolderPath.Location = new Point(25, 62);
            txtBadgeFolderPath.Name = "txtBadgeFolderPath";
            txtBadgeFolderPath.Size = new Size(677, 31);
            txtBadgeFolderPath.TabIndex = 15;
            txtBadgeFolderPath.TextChanged += txtBadgeFolderPath_TextChanged;
            // 
            // gbBadge_SQL_Conn
            // 
            gbBadge_SQL_Conn.Controls.Add(btnBadge_Reset);
            gbBadge_SQL_Conn.Controls.Add(txtSqlServer_Badge);
            gbBadge_SQL_Conn.Controls.Add(txtSqlDb_Badge);
            gbBadge_SQL_Conn.Controls.Add(txtSqlUser_Badge);
            gbBadge_SQL_Conn.Controls.Add(txtSqlPass_Badge);
            gbBadge_SQL_Conn.Controls.Add(cbSqlAuth_Badge);
            gbBadge_SQL_Conn.Controls.Add(btnSqlConnect_Badge);
            gbBadge_SQL_Conn.Controls.Add(lblSqlStatus_Badge);
            gbBadge_SQL_Conn.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            gbBadge_SQL_Conn.Location = new Point(12, 178);
            gbBadge_SQL_Conn.Name = "gbBadge_SQL_Conn";
            gbBadge_SQL_Conn.Size = new Size(919, 261);
            gbBadge_SQL_Conn.TabIndex = 17;
            gbBadge_SQL_Conn.TabStop = false;
            gbBadge_SQL_Conn.Text = "SQL Connection";
            gbBadge_SQL_Conn.Enter += Badge_SQL_Connection_Enter;
            // 
            // btnBadge_Reset
            // 
            btnBadge_Reset.Font = new Font("Segoe UI", 10F);
            btnBadge_Reset.Location = new Point(636, 121);
            btnBadge_Reset.Margin = new Padding(4);
            btnBadge_Reset.Name = "btnBadge_Reset";
            btnBadge_Reset.Size = new Size(125, 38);
            btnBadge_Reset.TabIndex = 21;
            btnBadge_Reset.Text = "Reset";
            btnBadge_Reset.Click += BtnBadge_Reset_Click;
            // 
            // txtSqlServer_Badge
            // 
            txtSqlServer_Badge.Font = new Font("Segoe UI", 10F);
            txtSqlServer_Badge.Location = new Point(16, 42);
            txtSqlServer_Badge.Margin = new Padding(4);
            txtSqlServer_Badge.Name = "txtSqlServer_Badge";
            txtSqlServer_Badge.PlaceholderText = "SQL Server";
            txtSqlServer_Badge.Size = new Size(249, 34);
            txtSqlServer_Badge.TabIndex = 14;
            // 
            // txtSqlDb_Badge
            // 
            txtSqlDb_Badge.Font = new Font("Segoe UI", 10F);
            txtSqlDb_Badge.Location = new Point(319, 42);
            txtSqlDb_Badge.Margin = new Padding(4);
            txtSqlDb_Badge.Name = "txtSqlDb_Badge";
            txtSqlDb_Badge.PlaceholderText = "Database";
            txtSqlDb_Badge.Size = new Size(249, 34);
            txtSqlDb_Badge.TabIndex = 15;
            // 
            // txtSqlUser_Badge
            // 
            txtSqlUser_Badge.Font = new Font("Segoe UI", 10F);
            txtSqlUser_Badge.Location = new Point(16, 125);
            txtSqlUser_Badge.Margin = new Padding(4);
            txtSqlUser_Badge.Name = "txtSqlUser_Badge";
            txtSqlUser_Badge.PlaceholderText = "Username";
            txtSqlUser_Badge.Size = new Size(249, 34);
            txtSqlUser_Badge.TabIndex = 16;
            // 
            // txtSqlPass_Badge
            // 
            txtSqlPass_Badge.Font = new Font("Segoe UI", 10F);
            txtSqlPass_Badge.Location = new Point(319, 125);
            txtSqlPass_Badge.Margin = new Padding(4);
            txtSqlPass_Badge.Name = "txtSqlPass_Badge";
            txtSqlPass_Badge.PlaceholderText = "Password";
            txtSqlPass_Badge.Size = new Size(249, 34);
            txtSqlPass_Badge.TabIndex = 17;
            txtSqlPass_Badge.UseSystemPasswordChar = true;
            // 
            // cbSqlAuth_Badge
            // 
            cbSqlAuth_Badge.Font = new Font("Segoe UI", 10F);
            cbSqlAuth_Badge.Items.AddRange(new object[] { "Windows", "SQL Server" });
            cbSqlAuth_Badge.Location = new Point(636, 42);
            cbSqlAuth_Badge.Margin = new Padding(4);
            cbSqlAuth_Badge.Name = "cbSqlAuth_Badge";
            cbSqlAuth_Badge.Size = new Size(249, 36);
            cbSqlAuth_Badge.TabIndex = 18;
            // 
            // btnSqlConnect_Badge
            // 
            btnSqlConnect_Badge.Font = new Font("Segoe UI", 10F);
            btnSqlConnect_Badge.Location = new Point(138, 190);
            btnSqlConnect_Badge.Margin = new Padding(4);
            btnSqlConnect_Badge.Name = "btnSqlConnect_Badge";
            btnSqlConnect_Badge.Size = new Size(125, 38);
            btnSqlConnect_Badge.TabIndex = 19;
            btnSqlConnect_Badge.Text = "Connect";
            btnSqlConnect_Badge.Click += BtnSqlConnect_Badge_Click;
            // 
            // lblSqlStatus_Badge
            // 
            lblSqlStatus_Badge.Font = new Font("Segoe UI", 10F);
            lblSqlStatus_Badge.Location = new Point(318, 195);
            lblSqlStatus_Badge.Margin = new Padding(4, 0, 4, 0);
            lblSqlStatus_Badge.Name = "lblSqlStatus_Badge";
            lblSqlStatus_Badge.Size = new Size(250, 25);
            lblSqlStatus_Badge.TabIndex = 20;
            // 
            // gbBadge_Modules
            // 
            gbBadge_Modules.Controls.Add(progressBarBadge);
            gbBadge_Modules.Controls.Add(cbCalls_Badge);
            gbBadge_Modules.Controls.Add(cbCases_Badge);
            gbBadge_Modules.Controls.Add(cbCitations_Badge);
            gbBadge_Modules.Controls.Add(cbWarrants_Badge);
            gbBadge_Modules.Controls.Add(btnBadge_Migration);
            gbBadge_Modules.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            gbBadge_Modules.Location = new Point(11, 445);
            gbBadge_Modules.Name = "gbBadge_Modules";
            gbBadge_Modules.Size = new Size(920, 223);
            gbBadge_Modules.TabIndex = 18;
            gbBadge_Modules.TabStop = false;
            gbBadge_Modules.Text = "Modules";
            gbBadge_Modules.Enter += Badge_Modules_Enter;
            // 
            // progressBarBadge
            // 
            progressBarBadge.Location = new Point(532, 139);
            progressBarBadge.Margin = new Padding(4, 5, 4, 5);
            progressBarBadge.Name = "progressBarBadge";
            progressBarBadge.Size = new Size(353, 48);
            progressBarBadge.TabIndex = 22;
            progressBarBadge.Visible = false;
            progressBarBadge.Click += progressBar3_Click;
            // 
            // cbCalls_Badge
            // 
            cbCalls_Badge.Font = new Font("Segoe UI", 10F);
            cbCalls_Badge.Location = new Point(17, 101);
            cbCalls_Badge.Margin = new Padding(4);
            cbCalls_Badge.Name = "cbCalls_Badge";
            cbCalls_Badge.Size = new Size(100, 56);
            cbCalls_Badge.TabIndex = 23;
            cbCalls_Badge.Text = "Calls";
            // 
            // cbCases_Badge
            // 
            cbCases_Badge.Font = new Font("Segoe UI", 10F);
            cbCases_Badge.Location = new Point(271, 37);
            cbCases_Badge.Margin = new Padding(4);
            cbCases_Badge.Name = "cbCases_Badge";
            cbCases_Badge.Size = new Size(100, 56);
            cbCases_Badge.TabIndex = 21;
            cbCases_Badge.Text = "Cases";
            // 
            // cbCitations_Badge
            // 
            cbCitations_Badge.Font = new Font("Segoe UI", 10F);
            cbCitations_Badge.Location = new Point(17, 37);
            cbCitations_Badge.Margin = new Padding(4);
            cbCitations_Badge.Name = "cbCitations_Badge";
            cbCitations_Badge.Size = new Size(122, 56);
            cbCitations_Badge.TabIndex = 11;
            cbCitations_Badge.Text = "Citations";
            // 
            // cbWarrants_Badge
            // 
            cbWarrants_Badge.Font = new Font("Segoe UI", 10F);
            cbWarrants_Badge.Location = new Point(147, 48);
            cbWarrants_Badge.Margin = new Padding(4);
            cbWarrants_Badge.Name = "cbWarrants_Badge";
            cbWarrants_Badge.Size = new Size(116, 35);
            cbWarrants_Badge.TabIndex = 12;
            cbWarrants_Badge.Text = "Warrants";
            // 
            // btnBadge_Migration
            // 
            btnBadge_Migration.Font = new Font("Segoe UI", 10F);
            btnBadge_Migration.Location = new Point(650, 90);
            btnBadge_Migration.Name = "btnBadge_Migration";
            btnBadge_Migration.Size = new Size(125, 41);
            btnBadge_Migration.TabIndex = 13;
            btnBadge_Migration.Text = "Migrate";
            btnBadge_Migration.UseVisualStyleBackColor = true;
            btnBadge_Migration.Click += BtnBadge_Migration_Click;
            // 
            // txtAgencyKey
            // 
            txtAgencyKey.Font = new Font("Segoe UI", 10F);
            txtAgencyKey.Location = new Point(25, 50);
            txtAgencyKey.Margin = new Padding(4);
            txtAgencyKey.Name = "txtAgencyKey";
            txtAgencyKey.PlaceholderText = "Agency Key";
            txtAgencyKey.Size = new Size(453, 34);
            txtAgencyKey.TabIndex = 1;
            // 
            // txtAgencyAPIKey
            // 
            txtAgencyAPIKey.Font = new Font("Segoe UI", 10F);
            txtAgencyAPIKey.Location = new Point(25, 112);
            txtAgencyAPIKey.Margin = new Padding(4);
            txtAgencyAPIKey.Name = "txtAgencyAPIKey";
            txtAgencyAPIKey.PlaceholderText = "Agency API Key";
            txtAgencyAPIKey.Size = new Size(453, 34);
            txtAgencyAPIKey.TabIndex = 2;
            // 
            // btnGenerateToken
            // 
            btnGenerateToken.Font = new Font("Segoe UI", 10F);
            btnGenerateToken.Location = new Point(500, 28);
            btnGenerateToken.Margin = new Padding(4);
            btnGenerateToken.Name = "btnGenerateToken";
            btnGenerateToken.Size = new Size(196, 55);
            btnGenerateToken.TabIndex = 3;
            btnGenerateToken.Text = "Generate Token";
            btnGenerateToken.Click += BtnGenerateToken_Click;
            // 
            // lblApiStatus
            // 
            lblApiStatus.Font = new Font("Segoe UI", 10F);
            lblApiStatus.Location = new Point(500, 100);
            lblApiStatus.Margin = new Padding(4, 0, 4, 0);
            lblApiStatus.Name = "lblApiStatus";
            lblApiStatus.Size = new Size(341, 80);
            lblApiStatus.TabIndex = 4;
            lblApiStatus.Click += lblApiStatus_Click_1;
            // 
            // grpApiAuth
            // 
            grpApiAuth.Controls.Add(btnGenerateToken);
            grpApiAuth.Controls.Add(lblApiStatus);
            grpApiAuth.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            grpApiAuth.Location = new Point(12, 12);
            grpApiAuth.Margin = new Padding(4);
            grpApiAuth.Name = "grpApiAuth";
            grpApiAuth.Padding = new Padding(4);
            grpApiAuth.Size = new Size(951, 195);
            grpApiAuth.TabIndex = 5;
            grpApiAuth.TabStop = false;
            grpApiAuth.Text = "API Authentication";
            grpApiAuth.Enter += grpApiAuth_Enter;            
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1045, 1115);
            Controls.Add(tabControl);
            Controls.Add(txtAgencyKey);
            Controls.Add(txtAgencyAPIKey);
            Controls.Add(grpApiAuth);
            Margin = new Padding(4);
            Name = "Form1";
            Text = "Dataport Migration Tool";
            Load += Form1_Load;
            tabControl.ResumeLayout(false);
            tabLawTrak.ResumeLayout(false);
            gbLawTrak_Folder.ResumeLayout(false);
            gbLawTrak_Folder.PerformLayout();
            gbLawTrak_Modules.ResumeLayout(false);
            gbLawTrak_SQL_Conn.ResumeLayout(false);
            gbLawTrak_SQL_Conn.PerformLayout();
            tabInSynch.ResumeLayout(false);
            gbInSynch_Modules.ResumeLayout(false);
            gbInSynch_SQL_Conn.ResumeLayout(false);
            gbInSynch_SQL_Conn.PerformLayout();
            tabBadge.ResumeLayout(false);
            gbBadge_Folder.ResumeLayout(false);
            gbBadge_Folder.PerformLayout();
            gbBadge_SQL_Conn.ResumeLayout(false);
            gbBadge_SQL_Conn.PerformLayout();
            gbBadge_Modules.ResumeLayout(false);
            grpApiAuth.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        // Controls
        private TabControl tabControl;
        private TextBox txtAgencyKey, txtAgencyAPIKey;
        private Button btnGenerateToken;
        private Label lblApiStatus;
        private GroupBox grpApiAuth;

        // LawTrak Controls
        private TabPage tabLawTrak;
        private GroupBox gbLawTrak_Folder;
        private TextBox txtLTFolderPath;
        private Button btnLawTrakBrowse;
        private GroupBox gbLawTrak_SQL_Conn;
        private TextBox txtSqlServer_LawTrak;
        private TextBox txtSqlDb_LawTrak;
        private TextBox txtSqlUser_LawTrak;
        private TextBox txtSqlPass_LawTrak;
        private ComboBox cbSqlAuth_LawTrak;
        private Button btnSqlConnect_LawTrak;
        private Label lblSqlStatus_LawTrak;
        private Button btnLawTrak_Reset;
        private GroupBox gbLawTrak_Modules;
        private Button btnLawTrak_Migration;
        private CheckBox cbCitations_LawTrak;
        private CheckBox cbWarrants_LawTrak;
        private CheckBox cbCases_LawTrak;
        private CheckBox cbAffidavits_LawTrak;
        private CheckBox cbEvidences_LawTrak;
        private CheckBox cbAccounting_LawTrak;
        private CheckBox cbJuvenile_LawTrak;
        private ProgressBar progressBarLawTrak;

        // In-Synch Controls
        private TabPage tabInSynch;
        private GroupBox gbInSynch_SQL_Conn;
        private TextBox txtSqlServer_InSynch;
        private TextBox txtSqlDb_InSynch;
        private TextBox txtSqlUser_InSynch;
        private TextBox txtSqlPass_InSynch;
        private Button btnSqlConnect_InSynch;
        private Label lblSqlStatus_InSynch;
        private Button btnInSynch_Reset;
        private GroupBox gbInSynch_Modules;
        private Button btnInSynch_Migration;
        private CheckBox cbCitations_InSynch;
        private CheckBox cbCases_InSynch;
        private ProgressBar progressBarInSynch;


        // Badge Controls
        private TabPage tabBadge;
        private GroupBox gbBadge_Folder;
        private TextBox txtBadgeFolderPath;
        private Button btnBadgeBrowse;
        private GroupBox gbBadge_SQL_Conn;
        private TextBox txtSqlServer_Badge;
        private TextBox txtSqlDb_Badge;
        private TextBox txtSqlUser_Badge;
        private TextBox txtSqlPass_Badge;
        private ComboBox cbSqlAuth_Badge;
        private Button btnSqlConnect_Badge;
        private Label lblSqlStatus_Badge;
        private Button btnBadge_Reset;
        private GroupBox gbBadge_Modules;
        private Button btnBadge_Migration;
        private CheckBox cbCitations_Badge;
        private CheckBox cbWarrants_Badge;
        private CheckBox cbCases_Badge;
        private CheckBox cbCalls_Badge;
        private ProgressBar progressBarBadge;

        private FolderBrowserDialog folderBrowserDialog1;
        private CheckBox cbBookings_LawTrak;
    }
}
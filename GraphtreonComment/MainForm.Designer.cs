namespace MGBot
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.percentLabel = new System.Windows.Forms.Label();
            this.progBar = new System.Windows.Forms.ProgressBar();
            this.statusLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.resultList = new System.Windows.Forms.ListView();
            this.No = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Email = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Status = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.URL = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.UA = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.lstLog = new System.Windows.Forms.ListView();
            this.Log = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.radRegister = new System.Windows.Forms.RadioButton();
            this.radLogin = new System.Windows.Forms.RadioButton();
            this.tabControl3 = new System.Windows.Forms.TabControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtTimeout = new System.Windows.Forms.TextBox();
            this.lblTo = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTo = new System.Windows.Forms.TextBox();
            this.txtFrom = new System.Windows.Forms.TextBox();
            this.tblAccounts = new System.Windows.Forms.DataGridView();
            this.Username = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Password = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnLoad1 = new System.Windows.Forms.Button();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.tblUA = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnComStartStop = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabControl2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabControl3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tblAccounts)).BeginInit();
            this.tabPage6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tblUA)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "#";
            this.columnHeader1.Width = 50;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Creator";
            this.columnHeader2.Width = 250;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Patrons";
            this.columnHeader3.Width = 100;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Earnings";
            this.columnHeader4.Width = 150;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Avg support per patron";
            this.columnHeader5.Width = 150;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "";
            // 
            // percentLabel
            // 
            this.percentLabel.AutoSize = true;
            this.percentLabel.Location = new System.Drawing.Point(339, 630);
            this.percentLabel.Name = "percentLabel";
            this.percentLabel.Size = new System.Drawing.Size(33, 13);
            this.percentLabel.TabIndex = 46;
            this.percentLabel.Text = "100%";
            this.percentLabel.Visible = false;
            // 
            // progBar
            // 
            this.progBar.Location = new System.Drawing.Point(12, 646);
            this.progBar.Name = "progBar";
            this.progBar.Size = new System.Drawing.Size(654, 19);
            this.progBar.TabIndex = 45;
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(13, 630);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(38, 13);
            this.statusLabel.TabIndex = 37;
            this.statusLabel.Text = "Ready";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(293, -21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 15);
            this.label1.TabIndex = 36;
            this.label1.Text = "Search Result:";
            // 
            // resultList
            // 
            this.resultList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.No,
            this.Email,
            this.Status,
            this.URL,
            this.UA});
            this.resultList.GridLines = true;
            this.resultList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.resultList.HideSelection = false;
            this.resultList.Location = new System.Drawing.Point(6, 6);
            this.resultList.Name = "resultList";
            this.resultList.Size = new System.Drawing.Size(634, 148);
            this.resultList.TabIndex = 34;
            this.resultList.UseCompatibleStateImageBehavior = false;
            this.resultList.View = System.Windows.Forms.View.Details;
            // 
            // No
            // 
            this.No.Text = "#";
            this.No.Width = 43;
            // 
            // Email
            // 
            this.Email.Text = "Email";
            this.Email.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Email.Width = 134;
            // 
            // Status
            // 
            this.Status.Text = "Status";
            this.Status.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Status.Width = 75;
            // 
            // URL
            // 
            this.URL.Text = "URL";
            this.URL.Width = 186;
            // 
            // UA
            // 
            this.UA.Text = "User-Agent";
            this.UA.Width = 256;
            // 
            // tabMain
            // 
            this.tabMain.Location = new System.Drawing.Point(2, 2);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(664, 431);
            this.tabMain.TabIndex = 35;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Controls.Add(this.tabPage1);
            this.tabControl2.Location = new System.Drawing.Point(12, 439);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(654, 186);
            this.tabControl2.TabIndex = 49;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.resultList);
            this.tabPage3.Controls.Add(this.label1);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(646, 160);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "Inspection Result";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.lstLog);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(646, 160);
            this.tabPage1.TabIndex = 1;
            this.tabPage1.Text = "Log";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // lstLog
            // 
            this.lstLog.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Log});
            this.lstLog.GridLines = true;
            this.lstLog.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lstLog.HideSelection = false;
            this.lstLog.Location = new System.Drawing.Point(6, 6);
            this.lstLog.Name = "lstLog";
            this.lstLog.Size = new System.Drawing.Size(634, 148);
            this.lstLog.TabIndex = 35;
            this.lstLog.UseCompatibleStateImageBehavior = false;
            this.lstLog.View = System.Windows.Forms.View.Details;
            // 
            // Log
            // 
            this.Log.Text = "Log";
            this.Log.Width = 628;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.radRegister);
            this.tabPage2.Controls.Add(this.radLogin);
            this.tabPage2.Controls.Add(this.tabControl3);
            this.tabPage2.Controls.Add(this.btnComStartStop);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(316, 615);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Control Panel";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // radRegister
            // 
            this.radRegister.AutoSize = true;
            this.radRegister.Location = new System.Drawing.Point(107, 584);
            this.radRegister.Name = "radRegister";
            this.radRegister.Size = new System.Drawing.Size(64, 17);
            this.radRegister.TabIndex = 62;
            this.radRegister.TabStop = true;
            this.radRegister.Text = "Register";
            this.radRegister.UseVisualStyleBackColor = true;
            this.radRegister.Visible = false;
            // 
            // radLogin
            // 
            this.radLogin.AutoSize = true;
            this.radLogin.Location = new System.Drawing.Point(16, 582);
            this.radLogin.Name = "radLogin";
            this.radLogin.Size = new System.Drawing.Size(51, 17);
            this.radLogin.TabIndex = 61;
            this.radLogin.TabStop = true;
            this.radLogin.Text = "Login";
            this.radLogin.UseVisualStyleBackColor = true;
            this.radLogin.Visible = false;
            // 
            // tabControl3
            // 
            this.tabControl3.Controls.Add(this.tabPage4);
            this.tabControl3.Controls.Add(this.tabPage6);
            this.tabControl3.Location = new System.Drawing.Point(6, 3);
            this.tabControl3.Name = "tabControl3";
            this.tabControl3.SelectedIndex = 0;
            this.tabControl3.Size = new System.Drawing.Size(307, 568);
            this.tabControl3.TabIndex = 60;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.label6);
            this.tabPage4.Controls.Add(this.label5);
            this.tabPage4.Controls.Add(this.txtTimeout);
            this.tabPage4.Controls.Add(this.lblTo);
            this.tabPage4.Controls.Add(this.label2);
            this.tabPage4.Controls.Add(this.txtTo);
            this.tabPage4.Controls.Add(this.txtFrom);
            this.tabPage4.Controls.Add(this.tblAccounts);
            this.tabPage4.Controls.Add(this.btnLoad1);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(299, 542);
            this.tabPage4.TabIndex = 0;
            this.tabPage4.Text = "Emails";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(135, 48);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(44, 13);
            this.label6.TabIndex = 66;
            this.label6.Text = "Minutes";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 48);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 13);
            this.label5.TabIndex = 65;
            this.label5.Text = "SessionTime";
            // 
            // txtTimeout
            // 
            this.txtTimeout.Location = new System.Drawing.Point(81, 45);
            this.txtTimeout.Name = "txtTimeout";
            this.txtTimeout.Size = new System.Drawing.Size(53, 20);
            this.txtTimeout.TabIndex = 64;
            this.txtTimeout.Text = "30";
            this.txtTimeout.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblTo
            // 
            this.lblTo.AutoSize = true;
            this.lblTo.Location = new System.Drawing.Point(167, 18);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(20, 13);
            this.lblTo.TabIndex = 63;
            this.lblTo.Text = "To";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 13);
            this.label2.TabIndex = 62;
            this.label2.Text = "From";
            // 
            // txtTo
            // 
            this.txtTo.Location = new System.Drawing.Point(193, 15);
            this.txtTo.Name = "txtTo";
            this.txtTo.Size = new System.Drawing.Size(100, 20);
            this.txtTo.TabIndex = 61;
            this.txtTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtFrom
            // 
            this.txtFrom.Location = new System.Drawing.Point(54, 15);
            this.txtFrom.Name = "txtFrom";
            this.txtFrom.Size = new System.Drawing.Size(80, 20);
            this.txtFrom.TabIndex = 60;
            this.txtFrom.Text = "1";
            this.txtFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tblAccounts
            // 
            this.tblAccounts.AllowUserToOrderColumns = true;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.tblAccounts.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.tblAccounts.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.tblAccounts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tblAccounts.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Username,
            this.Password});
            this.tblAccounts.Location = new System.Drawing.Point(6, 71);
            this.tblAccounts.Name = "tblAccounts";
            this.tblAccounts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.tblAccounts.Size = new System.Drawing.Size(287, 465);
            this.tblAccounts.TabIndex = 0;
            this.tblAccounts.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.tblAccounts_CellFormatting);
            this.tblAccounts.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.tblAccounts_EditingControlShowing);
            this.tblAccounts.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.tblAccounts_RowsAdded);
            // 
            // Username
            // 
            this.Username.HeaderText = "Username";
            this.Username.Name = "Username";
            // 
            // Password
            // 
            this.Password.HeaderText = "Password";
            this.Password.Name = "Password";
            // 
            // btnLoad1
            // 
            this.btnLoad1.Location = new System.Drawing.Point(219, 41);
            this.btnLoad1.Name = "btnLoad1";
            this.btnLoad1.Size = new System.Drawing.Size(74, 24);
            this.btnLoad1.TabIndex = 59;
            this.btnLoad1.Text = "From File...";
            this.btnLoad1.UseVisualStyleBackColor = true;
            this.btnLoad1.Click += new System.EventHandler(this.btnLoad1_Click);
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.tblUA);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(299, 542);
            this.tabPage6.TabIndex = 2;
            this.tabPage6.Text = "User Agents";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // tblUA
            // 
            this.tblUA.AllowUserToOrderColumns = true;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.tblUA.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
            this.tblUA.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.tblUA.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tblUA.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn3});
            this.tblUA.Location = new System.Drawing.Point(4, 6);
            this.tblUA.Name = "tblUA";
            this.tblUA.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.tblUA.Size = new System.Drawing.Size(290, 530);
            this.tblUA.TabIndex = 2;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.FillWeight = 80F;
            this.dataGridViewTextBoxColumn3.HeaderText = "User Agents";
            this.dataGridViewTextBoxColumn3.MinimumWidth = 70;
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // btnComStartStop
            // 
            this.btnComStartStop.Location = new System.Drawing.Point(203, 577);
            this.btnComStartStop.Name = "btnComStartStop";
            this.btnComStartStop.Size = new System.Drawing.Size(103, 26);
            this.btnComStartStop.TabIndex = 56;
            this.btnComStartStop.Text = "Start";
            this.btnComStartStop.UseVisualStyleBackColor = true;
            this.btnComStartStop.Click += new System.EventHandler(this.btnComStartStop_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(672, 24);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(324, 641);
            this.tabControl1.TabIndex = 33;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1001, 675);
            this.Controls.Add(this.tabControl2);
            this.Controls.Add(this.percentLabel);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.tabMain);
            this.Controls.Add(this.progBar);
            this.Controls.Add(this.statusLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "eBay Validator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabControl3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tblAccounts)).EndInit();
            this.tabPage6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tblUA)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.Label percentLabel;
        private System.Windows.Forms.ProgressBar progBar;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView resultList;
        private System.Windows.Forms.ColumnHeader No;
        private System.Windows.Forms.ColumnHeader Email;
        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnLoad1;
        private System.Windows.Forms.Button btnComStartStop;
        private System.Windows.Forms.DataGridView tblAccounts;
        private System.Windows.Forms.DataGridViewTextBoxColumn Username;
        private System.Windows.Forms.DataGridViewTextBoxColumn Password;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.ColumnHeader Status;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ListView lstLog;
        private System.Windows.Forms.ColumnHeader Log;
        private System.Windows.Forms.TabControl tabControl3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.DataGridView tblUA;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTo;
        private System.Windows.Forms.TextBox txtFrom;
        private System.Windows.Forms.ColumnHeader URL;
        private System.Windows.Forms.ColumnHeader UA;
        private System.Windows.Forms.RadioButton radRegister;
        private System.Windows.Forms.RadioButton radLogin;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtTimeout;
        private System.Windows.Forms.Label label6;
    }
}

